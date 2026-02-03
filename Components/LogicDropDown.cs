using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
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
    public abstract class AbsLogicDropDown<TMode> : AbsDropDownComponent<TMode>
        where TMode : Enum
    {
        protected AbsLogicDropDown(
            string name,
            string description)
            : base(
                name.AddDropDownMark(),
                description,
                Naming.Tab,
                Naming.Tab)
        {
        }

        private Gate _gate;
        private string name;
        protected IGraphLogic Logic;

        protected override void AddInputFields()
        {
            InString(
                    nameof(IGraphLogic.Name),
                    "")
                .InEnum(
                    nameof(Gate),
                    typeof(Gate).ToEnumString(),
                    AbsRule.DefGate.ToString())
                .InJsons(
                    nameof(AbsRule.Conditions),
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

        protected static ParameterStrategy ConditionalRuleStrategy(
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
                        nameof(AbsCondition).Substring(3) + "s",
                        Docs.Condition,
                        GH_ParamAccess.list)
                },
                action,
                description);
        }

        protected ParameterStrategy GetNonConditionalStrategyFor<TRule>(
            string description)
            where TRule : IGraphLogic, new()
        {
            return new ParameterStrategy(
                new List<ParameterConfig> { NameParameter() },
                da =>
                {
                    Logic = new TRule { Name = name };
                    Logic.Guard();
                },
                description);
        }

        protected ParameterStrategy GetConditionalStrategyFor<TRule>(
            string description)
            where TRule : AbsRule, new()
        {
            return ConditionalRuleStrategy(
                da =>
                {
                    int gateInt = da.GetOptionalItem(
                        1,
                        (int)AbsRule.DefGate);

                    gateInt.ValidateEnum<Gate>();
                    _gate = (Gate)gateInt;

                    if (!da.GetItems(
                            2,
                            out List<string> conditionJsons))
                    {
                        return;
                    }

                    Logic = new TRule
                    {
                        Conditions =
                            conditionJsons
                                .FromJson<AbsCondition>()
                                .ToList(),
                        Gate = _gate,
                        Name = name,
                    };

                    Logic.Guard();
                },
                description);
        }
    }

    public class NodeLogicDropDown : AbsLogicDropDown<NodeLogicType>
    {
        public NodeLogicDropDown()
            : base(
                nameof(NodeLogicDropDown)
                    .Substring(
                        0,
                        9),
                Docs.NodeLogic.AddDropDownNote())
        {
        }

        public override Guid ComponentGuid =>
            new("3216fe43-7667-4b78-9c13-b5e550d9c28d");

        protected override Dictionary<NodeLogicType, ParameterStrategy>
            DefineParameterStrategy()
        {
            return new Dictionary<NodeLogicType, ParameterStrategy>
            {
                {
                    NodeLogicType.NodeAdjacencyLogic,
                    GetConditionalStrategyFor<NodeAdjacencyRule>(
                        Docs.NodeAdjacency)
                },
                {
                    NodeLogicType.NodeProximityLogic,
                    GetConditionalStrategyFor<NodeProximityRule>(
                        Docs.NodeProximity)
                },
                {
                    NodeLogicType.NodeVectorSumLogic,
                    GetConditionalStrategyFor<NodeVectorSumRule>(
                        Docs.NodeVectorSum)
                },
                {
                    NodeLogicType.IsLeafNodeLogic,
                    GetNonConditionalStrategyFor<IsLeafNodeRule>(
                        Docs.IsLeafNode)
                },
                {
                    NodeLogicType.JointConstellationLogic,
                    new ParameterStrategy(
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
                }
            };
        }
    }

    public class EdgeLogicDropDown : AbsLogicDropDown<EdgeLogicType>
    {
        public EdgeLogicDropDown()
            : base(
                nameof(EdgeLogicDropDown)
                    .Substring(
                        0,
                        9),
                Docs.EdgeLogic.AddDropDownNote())
        {
        }

        public override Guid ComponentGuid =>
            new("8a5199a3-1bd9-449a-894e-3b1588b5c439");

        protected override Dictionary<EdgeLogicType, ParameterStrategy>
            DefineParameterStrategy()
        {
            return new Dictionary<EdgeLogicType, ParameterStrategy>
            {
                {
                    EdgeLogicType.EdgeLengthLogic,
                    GetConditionalStrategyFor<EdgeLengthRule>(Docs.EdgeLength)
                },
                {
                    EdgeLogicType.SourceAdjacencyLogic,
                    GetConditionalStrategyFor<SourceAdjacencyRule>(
                        Docs.SourceAdjacency)
                },
                {
                    EdgeLogicType.TargetAdjacencyLogic,
                    GetConditionalStrategyFor<TargetAdjacencyRule>(
                        Docs.TargetAdjacency)
                },
                {
                    EdgeLogicType.IsBridgeEdgeLogic,
                    GetNonConditionalStrategyFor<IsBridgeEdgeRule>(
                        Docs.IsBridgeEdge)
                },
                {
                    EdgeLogicType.LinkConstellationLogic, new ParameterStrategy(
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