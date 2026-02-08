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

        protected void ByNodeSimilarity(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    1,
                    out List<Line> directionLines))
            {
                return;
            }

            bool strictMatch = da.GetOptionalItem(
                2,
                NodeSimilarity.DefStrictMatch);


            double tolerance = da.GetOptionalItem(
                3,
                NodeSimilarity.DefAngleTolerance);

            Logic = new NodeSimilarity(
                directionLines,
                strictMatch,
                tolerance,
                name);
        }

        protected void ByLinkSimilarity(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    1,
                    out List<string> allowedSourceTags))
            {
                return;
            }

            if (!da.GetItems(
                    2,
                    out List<string> allowedTargetTags))
            {
                return;
            }

            bool bidirectional = da.GetOptionalItem(
                3,
                LinkSimilarity.DefBidirectional);

            Logic = new LinkSimilarity(
                allowedSourceTags,
                allowedTargetTags,
                bidirectional,
                name);
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
                Docs.Condition.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig StringConditionsParameter() =>
            new(
                () => new Param_String(),
                nameof(StringCondition) + "s",
                Docs.Condition.Add(Prefix.StringList),
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
            where TRule : AbsRule<double, DoubleCondition>, new()
        {
            return NumericRuleStrategy(
                da =>
                {
                    int gateInt = da.GetOptionalItem(
                        1,
                        (int)Gate.And);

                    gateInt.ValidateEnum<Gate>();
                    _gate = (Gate)gateInt;

                    if (!da.GetItems(
                            2,
                            out List<string> jsons))
                    {
                        return;
                    }

                    Logic = new TRule
                    {
                        Name = name,
                        Gate = _gate,
                        Conditions = jsons
                            .FromJsonByTypeCheck<DoubleCondition>()
                    };

                    Logic.Guard();
                },
                description);
        }

        protected ParameterStrategy StringStrategyFor<TRule>(
            string description)
            where TRule : AbsRule<string, StringCondition>, new()
        {
            return StringRuleStrategy(
                da =>
                {
                    int gateInt = da.GetOptionalItem(
                        1,
                        (int)Gate.And);

                    gateInt.ValidateEnum<Gate>();
                    _gate = (Gate)gateInt;

                    if (!da.GetItems(
                            2,
                            out List<string> jsons))
                    {
                        return;
                    }

                    Logic = new TRule
                    {
                        Name = name,
                        Gate = _gate,
                        Conditions = jsons
                            .FromJsonByTypeCheck<StringCondition>()
                    };

                    Logic.Guard();
                },
                description);
        }

        protected ParameterStrategy CollectionStrategyFor<TRule, TValue,
            TCondition>(
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
                    StringConditionsParameter(),
                },
                da =>
                {
                    int gateInt = da.GetOptionalItem(
                        1,
                        (int)Gate.And);

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
            where TRule : AbsCollectionRule<string, StringCondition>, new()
        {
            return CollectionStrategyFor<TRule, string, StringCondition>(
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
                    LogicType.NodeSimilarity, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NameParameter(),
                            new(
                                () => new Param_Line(),
                                nameof(NodeSimilarity.DirectionLines),
                                Docs.DirectionLines.Add(Prefix.LineList),
                                GH_ParamAccess.list),
                            new(
                                () => new Param_Boolean(),
                                nameof(NodeSimilarity.StrictMatch),
                                Docs
                                    .StrictMatch
                                    .ByDefault(NodeSimilarity.DefStrictMatch)
                                    .Add(Prefix.Boolean),
                                GH_ParamAccess.item,
                                isOptional: true),
                            new(
                                () => new Param_Number(),
                                nameof(NodeSimilarity.AngleTolerance),
                                Docs
                                    .AngleTolerance
                                    .ByDefault(NodeSimilarity.DefAngleTolerance)
                                    .Add(Prefix.Double),
                                GH_ParamAccess.item,
                                isOptional: true)
                        },
                        ByNodeSimilarity,
                        Docs.LinkSimilarity)
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
                    LogicType.EdgeIsBridge,
                    BooleanStrategyFor<IsBridgeEdgeRule>(Docs.IsBridgeEdge)
                },
                {
                    LogicType.EdgeLinkConstellationLogic, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NameParameter(),
                            new(
                                () => new Param_String(),
                                nameof(LinkSimilarity.AllowedSourceTypes),
                                Docs.AllowedSourceTypes.Add(Prefix.StringList),
                                GH_ParamAccess.list),
                            new(
                                () => new Param_String(),
                                nameof(LinkSimilarity.AllowedTargetTypes),
                                Docs.AllowedTargetTypes.Add(Prefix.StringList),
                                GH_ParamAccess.list),
                            new(
                                () => new Param_Boolean(),
                                nameof(LinkSimilarity.Bidirectional),
                                Docs.Bidirectional.Add(Prefix.Boolean),
                                GH_ParamAccess.item)
                        },
                        ByLinkSimilarity,
                        Docs.LinkConstellation)
                }
            };
        }
    }
}