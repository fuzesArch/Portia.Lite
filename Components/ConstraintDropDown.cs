using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.DocStrings;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Portia.Primitives;
using Portia.Infrastructure.Core.Portia.Strategies;
using Portia.Infrastructure.Core.Primitives;
using Portia.Infrastructure.Core.Validators;
using Portia.Lite.Core.Primitives;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components
{
    public class ConstraintDropDown : AbsDropDownComponent<LogicType>
    {
        public ConstraintDropDown()
            : base(
                nameof(ConstraintDropDown)
                    .Substring(
                        0,
                        10)
                    .AddDropDownMark(),
                Docs.Constraint,
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
        protected IConstraint Constraint;

        protected override void AddInputFields()
        {
            InString(
                    nameof(IConstraint.Name),
                    Docs.Name)
                .InEnum(
                    nameof(Gate),
                    typeof(Gate).ToEnumString(),
                    nameof(Gate.And))
                .InJsons(
                    nameof(AbsConstraint<double, DoubleCondition>.Conditions),
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
                nameof(Docs.Constraint),
                Docs.Constraint);
        }

        protected override void CommonInputSetting(
            IGH_DataAccess da)
        {
            if (!da.GetItem(
                    0,
                    out _name))
            {
                exit = true;
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
            da.SetData(
                0,
                Constraint.ToJson());
        }

        protected static ParameterConfig NameParameter() =>
            new(
                () => new Param_String(),
                nameof(AbsSelection.Name),
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
                Docs.Gate.ByDefault(AbsSelection.DefGate).Add(Prefix.Integer),
                GH_ParamAccess.item,
                isOptional: true,
                listFactory: GateValueList.Create);

        protected static ParameterConfig MatchAllParameter() =>
            new(
                () => new Param_Boolean(),
                nameof(Docs.MatchAll),
                Docs
                    .MatchAll
                    .ByDefault(
                        AbsCollectionConstraint<string, StringCondition>
                            .DefMatchAll)
                    .Add(Prefix.Boolean),
                GH_ParamAccess.item,
                isOptional: true);

        protected static ParameterConfig DoubleConditionsParameter() =>
            new(
                () => new Param_String(),
                nameof(DoubleCondition) + "s",
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

        protected static ParameterConfig WrapConditionsParameter() =>
            new(
                () => new Param_String(),
                nameof(WrapCondition) + "s",
                Docs.WrapCondition.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig ConstraintsParameter() =>
            new(
                () => new Param_String(),
                nameof(CompositeConstraint.Constraints),
                Docs.CompositeConstraint + Prefix.JsonList,
                GH_ParamAccess.list);

        protected ParameterStrategy BooleanStrategyFor<TRule>(
            string description)
            where TRule : IConstraint, IBooleanLogic, new()
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

                    Constraint = rule;
                    Constraint.Guard();
                },
                description);
        }

        protected ParameterStrategy NumericStrategyFor<TRule>(
            string description)
            where TRule : AbsNumericConstraint, new()
        {
            return new ParameterStrategy(
                new List<ParameterConfig>
                {
                    NameParameter(),
                    GateParameter(),
                    DoubleConditionsParameter(),
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

                    Constraint = new TRule
                    {
                        Name = _name,
                        Gate = _gate,
                        Conditions = jsons
                            .FromJsonByTypeCheck<DoubleCondition>()
                    };

                    Constraint.Guard();
                },
                description);
        }

        protected ParameterStrategy StringStrategyFor<TRule>(
            string description)
            where TRule : AbsStringConstraint, new()
        {
            return new ParameterStrategy(
                new List<ParameterConfig>
                {
                    NameParameter(),
                    GateParameter(),
                    StringConditionsParameter()
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

                    Constraint = new TRule
                    {
                        Name = _name,
                        Gate = _gate,
                        Conditions = jsons
                            .FromJsonByTypeCheck<StringCondition>()
                    };

                    Constraint.Guard();
                },
                description);
        }

        protected ParameterStrategy VectorStrategyFor<TRule>(
            string description)
            where TRule : AbsVectorConstraint, new()
        {
            return new ParameterStrategy(
                new List<ParameterConfig>
                {
                    NameParameter(),
                    GateParameter(),
                    VectorConditionsParameter()
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

                    var conditions =
                        jsons.FromJsonByTypeCheck<VectorCondition>();

                    var rule = new TRule
                    {
                        Name = _name, Gate = _gate, Conditions = conditions
                    };

                    Constraint = rule;
                    Constraint.Guard();
                },
                description);
        }

        protected ParameterStrategy WrapStrategyFor<TRule>(
            string description)
            where TRule : AbsWrapConstraint, new()
        {
            return new ParameterStrategy(
                new List<ParameterConfig>
                {
                    NameParameter(),
                    GateParameter(),
                    WrapConditionsParameter()
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

                    var conditions = jsons.FromJsonByTypeCheck<WrapCondition>();

                    var rule = new TRule
                    {
                        Name = _name, Gate = _gate, Conditions = conditions
                    };

                    Constraint = rule;
                    Constraint.Guard();
                },
                description);
        }

        protected ParameterStrategy CompositeStrategyFor<TRule>(
            string description)
            where TRule : CompositeConstraint, new()
        {
            return new ParameterStrategy(
                new List<ParameterConfig>
                {
                    NameParameter(), GateParameter(), ConstraintsParameter()
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

                    var nested = jsons.FromJson<IConstraint>().ToList();

                    Constraint = new TRule
                    {
                        Name = _name, Gate = _gate, Constraints = nested
                    };

                    Constraint.Guard();
                },
                description);
        }

        protected ParameterStrategy CollectionStrategyFor<TRule, TValue,
            TCondition>(
            ParameterConfig conditionsParameterConfig,
            string description)
            where TRule : AbsCollectionConstraint<TValue, TCondition>, new()
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
                        AbsCollectionConstraint<TValue, TCondition>
                            .DefMatchAll);

                    if (!da.GetItems(
                            3,
                            out List<string> jsons))
                    {
                        return;
                    }

                    Constraint = new TRule
                    {
                        Name = _name,
                        Gate = _gate,
                        MatchAll = matchAll,
                        Conditions = jsons.FromJsonByTypeCheck<TCondition>()
                    };

                    Constraint.Guard();
                },
                description);
        }

        protected ParameterStrategy StringCollectionStrategyFor<TRule>(
            string description)
            where TRule : AbsStringCollectionConstraint, new()
        {
            return CollectionStrategyFor<TRule, string, StringCondition>(
                StringConditionsParameter(),
                description);
        }

        protected ParameterStrategy VectorCollectionStrategyFor<TRule>(
            string description)
            where TRule : AbsVectorCollectionConstraint, new()
        {
            return CollectionStrategyFor<TRule, Vector3d, VectorCondition>(
                VectorConditionsParameter(),
                description);
        }

        protected override Dictionary<LogicType, ParameterStrategy>
            DefineParameterStrategy()
        {
            return new Dictionary<LogicType, ParameterStrategy>
            {
                {
                    LogicType.Composite,
                    CompositeStrategyFor<CompositeConstraint>(
                        Docs.CompositeConstraint)
                },
                {
                    LogicType.Index,
                    NumericStrategyFor<NodeAdjacencyConstraint>(
                        Docs.IndexConstraint)
                },
                {
                    LogicType.Type,
                    StringStrategyFor<TypeConstraint>(Docs.TypeConstraint)
                },
                {
                    LogicType.NodeAdjacency,
                    NumericStrategyFor<NodeAdjacencyConstraint>(
                        Docs.NodeAdjacency)
                },
                {
                    LogicType.NodeAdjacentEdgeType,
                    StringCollectionStrategyFor<NodeAdjacentEdgeTypeConstraint>(
                        Docs.NodeAdjacentEdgeType)
                },
                {
                    LogicType.NodeProximity,
                    NumericStrategyFor<NodeProximityConstraint>(
                        Docs.NodeProximity)
                },
                {
                    LogicType.NodeVectorSum,
                    NumericStrategyFor<NodeVectorSumConstraint>(
                        Docs.NodeVectorSum)
                },
                {
                    LogicType.NodeIsLeaf,
                    BooleanStrategyFor<IsLeafNodeConstraint>(Docs.IsLeafNode)
                },
                {
                    LogicType.NodeAdjacentVectors,
                    VectorCollectionStrategyFor<NodeAdjacentVectorsConstraint>(
                        Docs.NodeAdjacentVectors)
                },
                {
                    LogicType.NodeInWrap,
                    WrapStrategyFor<NodeInWrapConstraint>(Docs.NodeInWrap)
                },
                {
                    LogicType.EdgeLength,
                    NumericStrategyFor<EdgeLengthConstraint>(Docs.EdgeLength)
                },
                {
                    LogicType.EdgeSourceAdjacency,
                    NumericStrategyFor<SourceAdjacencyConstraint>(
                        Docs.SourceAdjacency)
                },
                {
                    LogicType.EdgeTargetAdjacency,
                    NumericStrategyFor<TargetAdjacencyConstraint>(
                        Docs.TargetAdjacency)
                },
                {
                    LogicType.EdgeSourceIndex,
                    NumericStrategyFor<EdgeSourceIndexConstraint>(
                        Docs.EdgeSourceIndex)
                },
                {
                    LogicType.EdgeSourceType,
                    StringStrategyFor<EdgeSourceTypeConstraint>(
                        Docs.EdgeSourceType)
                },
                {
                    LogicType.EdgeTargetIndex,
                    NumericStrategyFor<EdgeTargetIndexConstraint>(
                        Docs.EdgeTargetIndex)
                },
                {
                    LogicType.EdgeTargetType,
                    StringStrategyFor<EdgeTargetTypeConstraint>(
                        Docs.EdgeTargetType)
                },
                {
                    LogicType.EdgeIsLinear,
                    BooleanStrategyFor<IsLinearEdgeConstraint>(
                        Docs.IsLinearRule)
                },
                {
                    LogicType.EdgeVectorSimilarity,
                    VectorStrategyFor<EdgeVectorConstraint>(Docs.EdgeSimilarity)
                },
            };
        }
    }
}