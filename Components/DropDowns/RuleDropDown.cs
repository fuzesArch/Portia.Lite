using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Conditions.Base;
using Portia.Infrastructure.Conditions.Implementations;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Primitives.Enums;
using Portia.Infrastructure.Rules.Base;
using Portia.Infrastructure.Rules.BoundaryBased;
using Portia.Infrastructure.Rules.Composite;
using Portia.Infrastructure.Rules.Numeric;
using Portia.Infrastructure.Rules.StringBased;
using Portia.Infrastructure.Rules.StringCollectionBased;
using Portia.Infrastructure.Rules.VectorBased;
using Portia.Infrastructure.Rules.VectorCollectionBased;
using Portia.Infrastructure.Validators;
using Portia.Lite.Core.Primitives;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components.DropDowns
{
    public class RuleDropDown : AbsDropDownComponent<RuleMode>
    {
        public RuleDropDown()
            : base(
                nameof(RuleDropDown)
                    .Substring(
                        0,
                        4),
                Docs.Rule,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("3216fe43-7667-4b78-9c13-b5e550d9c28d");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        private Gate _gate;
        private string _name;
        private IRule _rule;

        public override void AddedToDocument(
            GH_Document document)
        {
            base.AddedToDocument(document);

            new GateValueList().AddAsSource(
                this,
                1);
        }

        protected override void AddOutputFields()
        {
            OutJson(
                nameof(Docs.Rule),
                Docs.Rule);
        }

        protected override void CommonInputSetting(
            IGH_DataAccess da)
        {
            if (!da.GetItem(
                    0,
                    out _name))
            {
                _exit = true;
            }
        }

        protected void SetGate(
            IGH_DataAccess da)
        {
            int gateInt = da.GetOptionalItem(
                1,
                (int)Gate.And);

            gateInt.ValidateEnum<Gate>();

            _gate = (Gate)gateInt;
        }

        protected override void CommonOutputSetting(
            IGH_DataAccess da)
        {
            Message = _rule.ComponentMessage();
            da.SetData(
                0,
                _rule.ToJson());
        }

        protected static ParameterConfig NameParam() =>
            new(
                () => new Param_String(),
                nameof(AbsNumericRule.Name),
                Docs.Name.Add(Prefix.String),
                GH_ParamAccess.item);

        protected static ParameterConfig BooleanParameter() =>
            new(
                () => new Param_Boolean(),
                nameof(Docs.BooleanCondition),
                Docs.BooleanCondition.Add(Prefix.Boolean),
                GH_ParamAccess.item);


        protected static ParameterConfig GateParam() =>
            new(
                () => new Param_Integer(),
                nameof(Gate),
                Docs.Gate.ByDefault(AbsNumericRule.DefGate).Add(Prefix.Integer),
                GH_ParamAccess.item,
                isOptional: true);

        protected static ParameterConfig MatchAllParam() =>
            new(
                () => new Param_Boolean(),
                nameof(Docs.MatchAll),
                Docs
                    .MatchAll
                    .ByDefault(
                        AbsCollectionRule<string, StringCondition>.DefMatchAll)
                    .Add(Prefix.Boolean),
                GH_ParamAccess.item,
                isOptional: true);

        protected static ParameterConfig DoubleConditionsParam() =>
            new(
                () => new Param_String(),
                nameof(NumericCondition) + "s",
                Docs.DoubleCondition.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig StringConditionsParam() =>
            new(
                () => new Param_String(),
                nameof(StringCondition) + "s",
                Docs.StringCondition.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig VectorConditionsParam() =>
            new(
                () => new Param_String(),
                nameof(VectorCondition) + "s",
                Docs.VectorCondition.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig BoundaryConditionsParam() =>
            new(
                () => new Param_String(),
                nameof(BoundaryCondition) + "s",
                Docs.BoundaryCondition.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig RuleParam() =>
            new(
                () => new Param_String(),
                nameof(CompositeRule.Rules),
                Docs.CompositeRule + Prefix.JsonList,
                GH_ParamAccess.list);

        protected ParameterSetup BooleanSetup<TRule>(
            string description)
            where TRule : IRule, IBooleanLogic, new()
        {
            return new ParameterSetup(
                new List<ParameterConfig> { NameParam(), BooleanParameter() },
                da =>
                {
                    if (!da.GetItem(
                            1,
                            out bool condition))
                    {
                        return;
                    }

                    var rule = new TRule { Name = _name };
                    rule.SetNegatableBaseCondition(condition);

                    _rule = rule;
                    _rule.Guard();
                },
                description);
        }

        protected ParameterSetup GenericSetup<TRule, TValue, TCondition>(
            ParameterConfig conditionsConfig,
            string description)
            where TRule : AbsBaseRule<TValue, TCondition>, new()
            where TCondition : AbsCondition<TValue>
        {
            return new ParameterSetup(
                new List<ParameterConfig>
                {
                    NameParam(), GateParam(), conditionsConfig
                },
                da =>
                {
                    SetGate(da);

                    if (!da.GetItems(
                            2,
                            out List<string> jsons))
                    {
                        return;
                    }

                    _rule = new TRule
                    {
                        Name = _name,
                        Gate = _gate,
                        Conditions =
                            jsons.FromJsonByConditionTypeCheck<TCondition>()
                    };

                    _rule.Guard();
                },
                description);
        }

        protected ParameterSetup NumericSetup<TRule>(
            string description)
            where TRule : AbsNumericRule, new()
        {
            return GenericSetup<TRule, double, NumericCondition>(
                DoubleConditionsParam(),
                description);
        }

        protected ParameterSetup StringSetup<TRule>(
            string description)
            where TRule : AbsStringRule, new()
        {
            return GenericSetup<TRule, string, StringCondition>(
                StringConditionsParam(),
                description);
        }

        protected ParameterSetup VectorSetup<TRule>(
            string description)
            where TRule : AbsVectorRule, new()
        {
            return GenericSetup<TRule, Vector3d, VectorCondition>(
                VectorConditionsParam(),
                description);
        }

        protected ParameterSetup BoundarySetup<TRule>(
            string description)
            where TRule : AbsBoundaryRule, new()
        {
            return GenericSetup<TRule, GeometryBase, BoundaryCondition>(
                BoundaryConditionsParam(),
                description);
        }

        protected ParameterSetup CompositeSetup<TRule>(
            string description)
            where TRule : CompositeRule, new()
        {
            return new ParameterSetup(
                new List<ParameterConfig>
                {
                    NameParam(), GateParam(), RuleParam()
                },
                da =>
                {
                    SetGate(da);

                    if (!da.GetItems(
                            2,
                            out List<string> jsons))
                    {
                        return;
                    }

                    var nested = jsons.FromJson<IRule>().ToList();

                    _rule = new TRule
                    {
                        Name = _name, Gate = _gate, Rules = nested
                    };

                    _rule.Guard();
                },
                description);
        }

        protected ParameterSetup CollectionSetup<TRule, TValue, TCondition>(
            ParameterConfig conditionsParameterConfig,
            string description)
            where TRule : AbsCollectionRule<TValue, TCondition>, new()
            where TCondition : AbsCondition<TValue>
        {
            return new ParameterSetup(
                new List<ParameterConfig>
                {
                    NameParam(),
                    GateParam(),
                    MatchAllParam(),
                    conditionsParameterConfig,
                },
                da =>
                {
                    SetGate(da);

                    bool matchAll = da.GetOptionalItem(
                        2,
                        AbsCollectionRule<TValue, TCondition>.DefMatchAll);

                    if (!da.GetItems(
                            3,
                            out List<string> jsons))
                    {
                        return;
                    }

                    _rule = new TRule
                    {
                        Name = _name,
                        Gate = _gate,
                        MatchAll = matchAll,
                        Conditions =
                            jsons.FromJsonByConditionTypeCheck<TCondition>()
                    };

                    _rule.Guard();
                },
                description);
        }

        protected ParameterSetup StringCollectionSetup<TRule>(
            string description)
            where TRule : AbsStringCollectionRule, new()
        {
            return CollectionSetup<TRule, string, StringCondition>(
                StringConditionsParam(),
                description);
        }

        protected ParameterSetup VectorCollectionSetup<TRule>(
            string description)
            where TRule : AbsVectorCollectionRule, new()
        {
            return CollectionSetup<TRule, Vector3d, VectorCondition>(
                VectorConditionsParam(),
                description);
        }

        protected override Dictionary<RuleMode, ParameterSetup> DefineSetup()
        {
            return new Dictionary<RuleMode, ParameterSetup>
            {
                {
                    RuleMode.Composite,
                    CompositeSetup<CompositeRule>(Docs.CompositeRule)
                },
                { RuleMode.IndexRule, NumericSetup<IndexRule>(Docs.IndexRule) },
                { RuleMode.TypeRule, StringSetup<TypeRule>(Docs.TypeRule) },
                {
                    RuleMode.Node_Degree,
                    NumericSetup<NodeDegreeRule>(Docs.NodeAdjacency)
                },
                {
                    RuleMode.Node_AdjacentEdgeType,
                    StringCollectionSetup<NodeAdjacentEdgeTypeRule>(
                        Docs.NodeAdjacentEdgeType)
                },
                {
                    RuleMode.Node_Proximity,
                    NumericSetup<NodeProximityRule>(Docs.NodeProximity)
                },
                {
                    RuleMode.Node_VectorSum,
                    NumericSetup<NodeVectorScalarSumRule>(
                        Docs.NodeVectorScalarSum)
                },
                {
                    RuleMode.Node_IsLeaf,
                    BooleanSetup<IsLeafNodeRule>(Docs.IsLeafNode)
                },
                {
                    RuleMode.Node_AdjacentEdgeVectorSimilarity,
                    VectorCollectionSetup<NodeAdjacentEdgeVectorSimilarityRule>(
                        Docs.NodeAdjacentEdgeVectorSimilarity)
                },
                {
                    RuleMode.Node_InBoundary,
                    BoundarySetup<NodeInBoundaryRule>(Docs.NodeInBoundary)
                },
                {
                    RuleMode.Edge_CurveLength,
                    NumericSetup<EdgeCurveLengthRule>(Docs.EdgeCurveLength)
                },
                {
                    RuleMode.Edge_StartEndDistance,
                    NumericSetup<EdgeStartEndDistanceRule>(
                        Docs.EdgeStartEndDistance)
                },
                {
                    RuleMode.Edge_StartDegree,
                    NumericSetup<EdgeStartDegreeRule>(Docs.StartDegree)
                },
                {
                    RuleMode.Edge_EndDegree,
                    NumericSetup<EdgeEndDegreeRule>(Docs.EndDegree)
                },
                {
                    RuleMode.Edge_StartIndex,
                    NumericSetup<EdgeStartIndexRule>(Docs.EdgeStartIndex)
                },
                {
                    RuleMode.Edge_StartType,
                    StringSetup<EdgeStartTypeRule>(Docs.EdgeStartType)
                },
                {
                    RuleMode.Edge_EndIndex,
                    NumericSetup<EdgeEndIndexRule>(Docs.EdgeEndIndex)
                },
                {
                    RuleMode.Edge_EndType,
                    StringSetup<EdgeEndTypeRule>(Docs.EdgeEndType)
                },
                {
                    RuleMode.Edge_IsLinear,
                    BooleanSetup<IsLinearEdgeRule>(Docs.IsLinearRule)
                },
                {
                    RuleMode.Edge_VectorSimilarity,
                    VectorSetup<EdgeVectorSimilarityRule>(Docs.EdgeSimilarity)
                },
                {
                    RuleMode.Edge_InBoundary,
                    BoundarySetup<EdgeInBoundaryRule>(Docs.EdgeInBoundary)
                },
            };
        }
    }
}