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

        protected void SolveByJointConstellation(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    1,
                    out List<string> jsons))
            {
                return;
            }

            bool strictMatch = da.GetOptionalItem(
                2,
                JointConstellation.DefStrictMatch);

            Logic = new JointConstellation(
                jsons.FromJson<NodeVector>().ToList(),
                strictMatch,
                name);
        }

        protected void SolveByLinkConstellation(
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
                LinkConstellation.DefBidirectional);

            Logic = new LinkConstellation(
                allowedSourceTags,
                allowedTargetTags,
                bidirectional,
                name);
        }

        protected static ParameterConfig NameParameter() =>
            new(
                () => new Param_String(),
                nameof(AbsSelection.Name),
                Docs.Name,
                GH_ParamAccess.item);

        protected static ParameterConfig BooleanParameter() =>
            new(
                () => new Param_Boolean(),
                nameof(Docs.BooleanCondition),
                Docs.BooleanCondition,
                GH_ParamAccess.item);

        protected static ParameterStrategy NumericRuleStrategy(
            Action<IGH_DataAccess> action,
            string description)
        {
            return new ParameterStrategy(
                new List<ParameterConfig>
                {
                    NameParameter(),
                    new(
                        () => new Param_Integer(),
                        nameof(Gate),
                        Docs.Gate,
                        GH_ParamAccess.item),
                    new(
                        () => new Param_String(),
                        nameof(DoubleCondition) + "s",
                        Docs.Condition,
                        GH_ParamAccess.list)
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
                    new(
                        () => new Param_Integer(),
                        nameof(Gate),
                        Docs.Gate,
                        GH_ParamAccess.item),
                    new(
                        () => new Param_String(),
                        nameof(StringCondition) + "s",
                        Docs.Condition,
                        GH_ParamAccess.list)
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
                            out bool boolCondition))
                    {
                        return;
                    }

                    var rule = new TRule { Name = name };
                    rule.SetNegatableBaseCondition(boolCondition);

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
                            out List<string> conditionJsons))
                    {
                        return;
                    }

                    var deserializedConditions = conditionJsons
                        .Select(json => json.FromJson<IConditionAnchor>())
                        .ToList();

                    if (!deserializedConditions.All(c => c is DoubleCondition))
                    {
                        throw new Exception(DocStrings.TypeErrorNumbersOnly);
                    }

                    Logic = new TRule
                    {
                        Name = name,
                        Gate = _gate,
                        Conditions = conditionJsons
                            .FromJson<DoubleCondition>()
                            .ToList()
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
                            out List<string> conditionJsons))
                    {
                        return;
                    }

                    var deserializedConditions = conditionJsons
                        .Select(json => json.FromJson<IConditionAnchor>())
                        .ToList();

                    if (!deserializedConditions.All(c => c is StringCondition))
                    {
                        throw new Exception(DocStrings.TypeErrorNumbersOnly);
                    }

                    Logic = new TRule
                    {
                        Name = name,
                        Gate = _gate,
                        Conditions = conditionJsons
                            .FromJson<StringCondition>()
                            .ToList()
                    };

                    Logic.Guard();
                },
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
                    LogicType.NodeConstellation, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NameParameter(),
                            new(
                                () => new Param_String(),
                                nameof(NodeVector) + "s",
                                Docs.NodeVector,
                                GH_ParamAccess.list),
                            new(
                                () => new Param_Boolean(),
                                nameof(JointConstellation.StrictMatch),
                                Docs.StrictMatch,
                                GH_ParamAccess.item)
                        },
                        SolveByJointConstellation,
                        Docs.JointConstellation)
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
                                nameof(LinkConstellation.AllowedSourceTypes),
                                Docs.AllowedSourceTypes,
                                GH_ParamAccess.list),
                            new(
                                () => new Param_String(),
                                nameof(LinkConstellation.AllowedTargetTypes),
                                Docs.AllowedTargetTypes,
                                GH_ParamAccess.list),
                            new(
                                () => new Param_Boolean(),
                                nameof(LinkConstellation.Bidirectional),
                                Docs.Bidirectional,
                                GH_ParamAccess.item)
                        },
                        SolveByLinkConstellation,
                        Docs.LinkConstellation)
                }
            };
        }
    }
}