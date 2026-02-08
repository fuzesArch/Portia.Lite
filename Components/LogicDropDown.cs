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

namespace Portia.Lite.Components
{
    public class LogicDropDown : AbsDropDownComponent<LogicType>
    {
        public LogicDropDown()
            : base(
                nameof(LogicDropDown)
                    .Substring(
                        0,
                        5)
                    .AddDropDownMark(),
                Docs.Logic,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("3216fe43-7667-4b78-9c13-b5e550d9c28d");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.ColoredLogo;

        private Gate _gate;
        private string name;
        protected IGraphLogic Logic;

        protected override void AddInputFields()
        {
            InString(
                    nameof(IGraphLogic.Name),
                    Docs.Name)
                .InEnum(
                    nameof(Gate),
                    typeof(Gate).ToEnumString(),
                    nameof(Gate.And))
                .InJsons(
                    nameof(AbsRule<double, DoubleCondition>.Conditions),
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
                nameof(Docs.Logic),
                Docs.Logic);
        }

        protected override void CommonInputSetting(
            IGH_DataAccess da)
        {
            if (!da.GetItem(
                    0,
                    out name))
            {
                exit = true;
            }
        }

        protected override void CommonOutputSetting(
            IGH_DataAccess da)
        {
            da.SetData(
                0,
                Logic.ToJson());
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
                        AbsCollectionRule<string, StringCondition>.DefMatchAll)
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

        protected static ParameterStrategy NumericRuleStrategy(
            Action<IGH_DataAccess> action,
            string description)
        {
            return new ParameterStrategy(
                new List<ParameterConfig>
                {
                    NameParameter(),
                    GateParameter(),
                    DoubleConditionsParameter(),
                },
                action,
                description);
        }

        protected static ParameterStrategy StringRuleStrategy(
            Action<IGH_DataAccess> action,
            string description)
        {
            return new ParameterStrategy(
                new List<ParameterConfig>
                {
                    NameParameter(),
                    GateParameter(),
                    StringConditionsParameter()
                },
                action,
                description);
        }

        protected ParameterStrategy BooleanStrategyFor<TRule>(
            string description)
            where TRule : IGraphLogic, IBooleanLogic, new()
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

                    var rule = new TRule { Name = name };
                    rule.SetNegatableBaseCondition(condition);

                    Logic = rule;
                    Logic.Guard();
                },
                description);
        }

        protected ParameterStrategy NumericStrategyFor<TRule>(
            string description)
            where TRule : AbsNumericRule, new()
        {
            return NumericRuleStrategy(
                da =>
                {
                    int gateInt = da.GetOptionalItem(
                        1,
                        (int)Gate.And);

                    gateInt.ValidateEnum<Gate>();

                    if (!da.GetItems(
                            2,
                            out List<string> jsons))
                    {
                        return;
                    }

                    Logic = new TRule
                    {
                        Name = name,
                        Gate = (Gate)gateInt,
                        Conditions = jsons
                            .FromJsonByTypeCheck<DoubleCondition>()
                    };

                    Logic.Guard();
                },
                description);
        }

        protected ParameterStrategy StringStrategyFor<TRule>(
            string description)
            where TRule : AbsStringRule, new()
        {
            return StringRuleStrategy(
                da =>
                {
                    int gateInt = da.GetOptionalItem(
                        1,
                        (int)Gate.And);

                    gateInt.ValidateEnum<Gate>();

                    if (!da.GetItems(
                            2,
                            out List<string> jsons))
                    {
                        return;
                    }

                    Logic = new TRule
                    {
                        Name = name,
                        Gate = (Gate)gateInt,
                        Conditions = jsons
                            .FromJsonByTypeCheck<StringCondition>()
                    };

                    Logic.Guard();
                },
                description);
        }

        protected ParameterStrategy VectorStrategyFor<TRule>(
            string description)
            where TRule : AbsVectorRule, new()
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
                    int gateInt = da.GetOptionalItem(
                        1,
                        (int)Gate.And);

                    gateInt.ValidateEnum<Gate>();

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
                        Name = name,
                        Gate = (Gate)gateInt,
                        Conditions = conditions
                    };

                    Logic = rule;
                    Logic.Guard();
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
                    int gateInt = da.GetOptionalItem(
                        1,
                        (int)Gate.And);


                    gateInt.ValidateEnum<Gate>();

                    bool matchAll = da.GetOptionalItem(
                        2,
                        AbsCollectionRule<TValue, TCondition>.DefMatchAll);

                    if (!da.GetItems(
                            3,
                            out List<string> jsons))
                    {
                        return;
                    }

                    Logic = new TRule
                    {
                        Name = name,
                        Gate = (Gate)gateInt,
                        MatchAll = matchAll,
                        Conditions = jsons.FromJsonByTypeCheck<TCondition>()
                    };

                    Logic.Guard();
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

        protected override Dictionary<LogicType, ParameterStrategy>
            DefineParameterStrategy()
        {
            return new Dictionary<LogicType, ParameterStrategy>
            {
                {
                    LogicType.Index,
                    NumericStrategyFor<NodeAdjacencyRule>(Docs.IndexLogic)
                },
                { LogicType.Type, StringStrategyFor<TypeRule>(Docs.TypeLogic) },
                {
                    LogicType.NodeAdjacency,
                    NumericStrategyFor<NodeAdjacencyRule>(Docs.NodeAdjacency)
                },
                {
                    LogicType.NodeAdjacentEdgeType,
                    StringCollectionStrategyFor<NodeAdjacentEdgeTypeRule>(
                        Docs.NodeAdjacentEdgeType)
                },
                {
                    LogicType.NodeProximity,
                    NumericStrategyFor<NodeProximityRule>(Docs.NodeProximity)
                },
                {
                    LogicType.NodeVectorSum,
                    NumericStrategyFor<NodeVectorSumRule>(Docs.NodeVectorSum)
                },
                {
                    LogicType.NodeIsLeaf,
                    BooleanStrategyFor<IsLeafNodeRule>(Docs.IsLeafNode)
                },
                {
                    LogicType.NodeAdjacentVectors,
                    VectorCollectionStrategyFor<NodeAdjacentVectorsRule>(
                        Docs.NodeAdjacentVectors)
                },
                {
                    LogicType.EdgeLength,
                    NumericStrategyFor<EdgeLengthRule>(Docs.EdgeLength)
                },
                {
                    LogicType.EdgeSourceAdjacency,
                    NumericStrategyFor<SourceAdjacencyRule>(
                        Docs.SourceAdjacency)
                },
                {
                    LogicType.EdgeTargetAdjacency,
                    NumericStrategyFor<TargetAdjacencyRule>(
                        Docs.TargetAdjacency)
                },
                {
                    LogicType.EdgeSourceIndex,
                    NumericStrategyFor<EdgeSourceIndexRule>(
                        Docs.EdgeSourceIndex)
                },
                {
                    LogicType.EdgeSourceType,
                    StringStrategyFor<EdgeSourceTypeRule>(Docs.EdgeSourceType)
                },
                {
                    LogicType.EdgeTargetIndex,
                    NumericStrategyFor<EdgeTargetIndexRule>(
                        Docs.EdgeTargetIndex)
                },
                {
                    LogicType.EdgeTargetType,
                    StringStrategyFor<EdgeTargetTypeRule>(Docs.EdgeTargetType)
                },
                {
                    LogicType.EdgeIsLinear,
                    BooleanStrategyFor<IsLinearEdgeRule>(Docs.IsLinearRule)
                },
                {
                    LogicType.EdgeVectorSimilarity,
                    VectorStrategyFor<EdgeVectorRule>(Docs.EdgeSimilarity)
                },
            };
        }
    }
}