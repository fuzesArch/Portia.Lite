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
    public class ConstraintDropDown : AbsDropDownComponent<ConstraintMode>
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
        private IConstraint _constraint;

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
                    nameof(AbsConstraint<double, NumericCondition>.Conditions),
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
                _constraint.ToJson());
        }

        protected static ParameterConfig NameParameter() =>
            new(
                () => new Param_String(),
                nameof(AbsNumericConstraint.Name),
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
                Docs
                    .Gate.ByDefault(AbsNumericConstraint.DefGate)
                    .Add(Prefix.Integer),
                GH_ParamAccess.item,
                isOptional: true);

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

                    _constraint = rule;
                    _constraint.Guard();
                },
                description);
        }

        protected ParameterStrategy GenericStrategyFor<TRule, TValue,
            TCondition>(
            ParameterConfig conditionsConfig,
            string description)
            where TRule : AbsBaseConstraint<TValue, TCondition>, new()
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

                    _constraint = new TRule
                    {
                        Name = _name,
                        Gate = _gate,
                        Conditions = jsons.FromJsonByTypeCheck<TCondition>()
                    };

                    _constraint.Guard();
                },
                description);
        }

        protected ParameterStrategy NumericStrategyFor<TRule>(
            string description)
            where TRule : AbsNumericConstraint, new()
        {
            return GenericStrategyFor<TRule, double, NumericCondition>(
                DoubleConditionsParameter(),
                description);
        }

        protected ParameterStrategy StringStrategyFor<TRule>(
            string description)
            where TRule : AbsStringConstraint, new()
        {
            return GenericStrategyFor<TRule, string, StringCondition>(
                StringConditionsParameter(),
                description);
        }

        protected ParameterStrategy VectorStrategyFor<TRule>(
            string description)
            where TRule : AbsVectorConstraint, new()
        {
            return GenericStrategyFor<TRule, Vector3d, VectorCondition>(
                VectorConditionsParameter(),
                description);
        }

        protected ParameterStrategy BoundaryStrategyFor<TRule>(
            string description)
            where TRule : AbsBoundaryConstraint, new()
        {
            return GenericStrategyFor<TRule, Point3d, BoundaryCondition>(
                BoundaryConditionsParameter(),
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

                    _constraint = new TRule
                    {
                        Name = _name, Gate = _gate, Constraints = nested
                    };

                    _constraint.Guard();
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

                    _constraint = new TRule
                    {
                        Name = _name,
                        Gate = _gate,
                        MatchAll = matchAll,
                        Conditions = jsons.FromJsonByTypeCheck<TCondition>()
                    };

                    _constraint.Guard();
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

        protected override Dictionary<ConstraintMode, ParameterStrategy>
            DefineParameterStrategy()
        {
            return new Dictionary<ConstraintMode, ParameterStrategy>
            {
                {
                    ConstraintMode.Composite,
                    CompositeStrategyFor<CompositeConstraint>(
                        Docs.CompositeConstraint)
                },
                {
                    ConstraintMode.IndexConstraint,
                    NumericStrategyFor<NodeAdjacencyConstraint>(
                        Docs.IndexConstraint)
                },
                {
                    ConstraintMode.TypeConstraint,
                    StringStrategyFor<TypeConstraint>(Docs.TypeConstraint)
                },
                {
                    ConstraintMode.Node_Adjacency,
                    NumericStrategyFor<NodeAdjacencyConstraint>(
                        Docs.NodeAdjacency)
                },
                {
                    ConstraintMode.Node_AdjacentEdgeType,
                    StringCollectionStrategyFor<NodeAdjacentEdgeTypeConstraint>(
                        Docs.NodeAdjacentEdgeType)
                },
                {
                    ConstraintMode.Node_Proximity,
                    NumericStrategyFor<NodeProximityConstraint>(
                        Docs.NodeProximity)
                },
                {
                    ConstraintMode.Node_VectorSum,
                    NumericStrategyFor<NodeVectorSumConstraint>(
                        Docs.NodeVectorSum)
                },
                {
                    ConstraintMode.Node_IsLeaf,
                    BooleanStrategyFor<IsLeafNodeConstraint>(Docs.IsLeafNode)
                },
                {
                    ConstraintMode.Node_AdjacentVectorSimilarity,
                    VectorCollectionStrategyFor<
                        NodeAdjacentVectorSimilarityConstraint>(
                        Docs.NodeAdjacentVectorSimilarity)
                },
                {
                    ConstraintMode.Node_InBoundary,
                    BoundaryStrategyFor<NodeInBoundaryConstraint>(
                        Docs.NodeInBoundary)
                },
                {
                    ConstraintMode.Edge_Length,
                    NumericStrategyFor<EdgeLengthConstraint>(Docs.EdgeLength)
                },
                {
                    ConstraintMode.Edge_StartAdjacency,
                    NumericStrategyFor<EdgeStartAdjacencyConstraint>(
                        Docs.StartAdjacency)
                },
                {
                    ConstraintMode.Edge_EndAdjacency,
                    NumericStrategyFor<EndAdjacencyConstraint>(
                        Docs.EndAdjacency)
                },
                {
                    ConstraintMode.Edge_StartIndex,
                    NumericStrategyFor<EdgeStartIndexConstraint>(
                        Docs.EdgeStartIndex)
                },
                {
                    ConstraintMode.Edge_StartType,
                    StringStrategyFor<EdgeStartTypeConstraint>(
                        Docs.EdgeStartType)
                },
                {
                    ConstraintMode.Edge_EndIndex,
                    NumericStrategyFor<EdgeEndIndexConstraint>(
                        Docs.EdgeEndIndex)
                },
                {
                    ConstraintMode.Edge_EndType,
                    StringStrategyFor<EdgeEndTypeConstraint>(Docs.EdgeEndType)
                },
                {
                    ConstraintMode.Edge_IsLinear,
                    BooleanStrategyFor<IsLinearEdgeConstraint>(
                        Docs.IsLinearRule)
                },
                {
                    ConstraintMode.Edge_VectorSimilarity,
                    VectorStrategyFor<EdgeVectorSimilarityConstraint>(
                        Docs.EdgeSimilarity)
                },
            };
        }
    }
}