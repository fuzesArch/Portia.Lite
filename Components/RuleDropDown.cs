using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.DocStrings;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Portia.Primitives;
using Portia.Infrastructure.Core.Portia.Rules;
using Portia.Infrastructure.Core.Primitives;
using Portia.Infrastructure.Core.Validators;
using Portia.Lite.Core.Primitives;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components
{
    public class RuleDropDown : AbsDropDownComponent<RuleMode>
    {
        public RuleDropDown()
            : base(
                nameof(RuleDropDown)
                    .Substring(
                        0,
                        4)
                    .AddDropDownMark(),
                Docs.Rule,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("3216fe43-7667-4b78-9c13-b5e550d9c28d");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.ColoredLogo;

        private Gate _gate;
        private string _name;
        private IRule _rule;

        protected override void AddInputFields()
        {
            InString(
                    nameof(IRule.Name),
                    Docs.Name)
                .InEnum(
                    nameof(Gate),
                    typeof(Gate).ToEnumString(),
                    nameof(Gate.And))
                .InJsons(
                    nameof(AbsRule<double, NumericCondition>.Conditions),
                    Docs.Condition);

            SetInputParameterOptionality(1);
            SetEnumDropDown<Gate>(1);
        }

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

        protected static ParameterConfig NameParameter() =>
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


        protected static ParameterConfig GateParameter() =>
            new(
                () => new Param_Integer(),
                nameof(Gate),
                Docs.Gate.ByDefault(AbsNumericRule.DefGate).Add(Prefix.Integer),
                GH_ParamAccess.item,
                isOptional: true);

        protected static ParameterConfig MatchAllParameter() =>
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

        protected static ParameterConfig DoubleConditionsParameter() =>
            new(
                () => new Param_String(),
                nameof(NumericCondition) + "s",
                Docs.DoubleCondition.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig StringConditionsParameter() =>
            new(
                () => new Param_String(),
                nameof(StringCondition) + "s",
                Docs.StringCondition.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig VectorConditionsParameter() =>
            new(
                () => new Param_String(),
                nameof(VectorCondition) + "s",
                Docs.VectorCondition.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig BoundaryConditionsParameter() =>
            new(
                () => new Param_String(),
                nameof(BoundaryCondition) + "s",
                Docs.BoundaryCondition.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig RuleParameter() =>
            new(
                () => new Param_String(),
                nameof(CompositeRule.Rules),
                Docs.CompositeRule + Prefix.JsonList,
                GH_ParamAccess.list);

        protected ParameterStrategy BooleanStrategyFor<TRule>(
            string description)
            where TRule : IRule, IBooleanLogic, new()
        {
            return new ParameterStrategy(
                new List<ParameterConfig>
                {
                    NameParameter(), BooleanParameter()
                },
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

        protected ParameterStrategy GenericStrategyFor<TRule, TValue,
            TCondition>(
            ParameterConfig conditionsConfig,
            string description)
            where TRule : AbsBaseRule<TValue, TCondition>, new()
            where TCondition : AbsCondition<TValue>
        {
            return new ParameterStrategy(
                new List<ParameterConfig>
                {
                    NameParameter(), GateParameter(), conditionsConfig
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
                        Conditions = jsons.FromJsonByTypeCheck<TCondition>()
                    };

                    _rule.Guard();
                },
                description);
        }

        protected ParameterStrategy NumericStrategyFor<TRule>(
            string description)
            where TRule : AbsNumericRule, new()
        {
            return GenericStrategyFor<TRule, double, NumericCondition>(
                DoubleConditionsParameter(),
                description);
        }

        protected ParameterStrategy StringStrategyFor<TRule>(
            string description)
            where TRule : AbsStringRule, new()
        {
            return GenericStrategyFor<TRule, string, StringCondition>(
                StringConditionsParameter(),
                description);
        }

        protected ParameterStrategy VectorStrategyFor<TRule>(
            string description)
            where TRule : AbsVectorRule, new()
        {
            return GenericStrategyFor<TRule, Vector3d, VectorCondition>(
                VectorConditionsParameter(),
                description);
        }

        protected ParameterStrategy BoundaryStrategyFor<TRule>(
            string description)
            where TRule : AbsBoundaryRule, new()
        {
            return GenericStrategyFor<TRule, GeometryBase, BoundaryCondition>(
                BoundaryConditionsParameter(),
                description);
        }

        protected ParameterStrategy CompositeStrategyFor<TRule>(
            string description)
            where TRule : CompositeRule, new()
        {
            return new ParameterStrategy(
                new List<ParameterConfig>
                {
                    NameParameter(), GateParameter(), RuleParameter()
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


        protected ParameterStrategy CollectionStrategyFor<TRule, TValue,
            TCondition>(
            ParameterConfig conditionsParameterConfig,
            string description)
            where TRule : AbsCollectionRule<TValue, TCondition>, new()
            where TCondition : AbsCondition<TValue>
        {
            return new ParameterStrategy(
                new List<ParameterConfig>
                {
                    NameParameter(),
                    GateParameter(),
                    MatchAllParameter(),
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
                        Conditions = jsons.FromJsonByTypeCheck<TCondition>()
                    };

                    _rule.Guard();
                },
                description);
        }

        protected ParameterStrategy StringCollectionStrategyFor<TRule>(
            string description)
            where TRule : AbsStringCollectionRule, new()
        {
            return CollectionStrategyFor<TRule, string, StringCondition>(
                StringConditionsParameter(),
                description);
        }

        protected ParameterStrategy VectorCollectionStrategyFor<TRule>(
            string description)
            where TRule : AbsVectorCollectionRule, new()
        {
            return CollectionStrategyFor<TRule, Vector3d, VectorCondition>(
                VectorConditionsParameter(),
                description);
        }

        protected override Dictionary<RuleMode, ParameterStrategy>
            DefineParameterStrategy()
        {
            return new Dictionary<RuleMode, ParameterStrategy>
            {
                {
                    RuleMode.Composite,
                    CompositeStrategyFor<CompositeRule>(Docs.CompositeRule)
                },
                {
                    RuleMode.IndexRule,
                    NumericStrategyFor<NodeAdjacencyRule>(Docs.IndexRule)
                },
                {
                    RuleMode.TypeRule,
                    StringStrategyFor<TypeRule>(Docs.TypeRule)
                },
                {
                    RuleMode.Node_Adjacency,
                    NumericStrategyFor<NodeAdjacencyRule>(Docs.NodeAdjacency)
                },
                {
                    RuleMode.Node_AdjacentEdgeType,
                    StringCollectionStrategyFor<NodeAdjacentEdgeTypeRule>(
                        Docs.NodeAdjacentEdgeType)
                },
                {
                    RuleMode.Node_Proximity,
                    NumericStrategyFor<NodeProximityRule>(Docs.NodeProximity)
                },
                {
                    RuleMode.Node_VectorSum,
                    NumericStrategyFor<NodeVectorSumRule>(Docs.NodeVectorSum)
                },
                {
                    RuleMode.Node_IsLeaf,
                    BooleanStrategyFor<IsLeafNodeRule>(Docs.IsLeafNode)
                },
                {
                    RuleMode.Node_AdjacentVectorSimilarity,
                    VectorCollectionStrategyFor<
                        NodeAdjacentVectorSimilarityRule>(
                        Docs.NodeAdjacentVectorSimilarity)
                },
                {
                    RuleMode.Node_InBoundary,
                    BoundaryStrategyFor<NodeInBoundaryRule>(Docs.NodeInBoundary)
                },
                {
                    RuleMode.Edge_Length,
                    NumericStrategyFor<EdgeLengthRule>(Docs.EdgeLength)
                },
                {
                    RuleMode.Edge_StartAdjacency,
                    NumericStrategyFor<EdgeStartAdjacencyRule>(
                        Docs.StartAdjacency)
                },
                {
                    RuleMode.Edge_EndAdjacency,
                    NumericStrategyFor<EndAdjacencyRule>(Docs.EndAdjacency)
                },
                {
                    RuleMode.Edge_StartIndex,
                    NumericStrategyFor<EdgeStartIndexRule>(Docs.EdgeStartIndex)
                },
                {
                    RuleMode.Edge_StartType,
                    StringStrategyFor<EdgeStartTypeRule>(Docs.EdgeStartType)
                },
                {
                    RuleMode.Edge_EndIndex,
                    NumericStrategyFor<EdgeEndIndexRule>(Docs.EdgeEndIndex)
                },
                {
                    RuleMode.Edge_EndType,
                    StringStrategyFor<EdgeEndTypeRule>(Docs.EdgeEndType)
                },
                {
                    RuleMode.Edge_IsLinear,
                    BooleanStrategyFor<IsLinearEdgeRule>(Docs.IsLinearRule)
                },
                {
                    RuleMode.Edge_VectorSimilarity,
                    VectorStrategyFor<EdgeVectorSimilarityRule>(
                        Docs.EdgeSimilarity)
                },
                {
                    RuleMode.Edge_InBoundary,
                    BoundaryStrategyFor<EdgeInBoundaryRule>(Docs.EdgeInBoundary)
                },
            };
        }
    }
}