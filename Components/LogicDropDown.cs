using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Primitives;
using Portia.Infrastructure.Core.Projects.Portia.Primitives;
using Portia.Infrastructure.Core.Projects.Portia.Strategies;
using Portia.Infrastructure.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components
{
    public abstract class AbsLogicDropDown<TMode> : DropDownComponent<TMode>
        where TMode : Enum
    {
        protected AbsLogicDropDown(
            string name)
            : base(
                name.AddDropDownMark(),
                Naming.Tab,
                Naming.Tab)
        {
        }

        private LogicGate logicGate;
        private string name;
        protected IGraphLogic Logic;

        protected override void AddInputFields()
        {
            InString(
                    nameof(IGraphLogic.Name),
                    "")
                .InEnum(
                    nameof(LogicGate),
                    typeof(LogicGate).ToEnumString(),
                    AbsRule.DefLogicGate.ToString())
                .InStrings(
                    nameof(AbsRule.Conditions),
                    "");

            SetInputParameterOptionality(1);
            SetEnumDropDown<LogicGate>(1);
        }

        public override void AddedToDocument(
            GH_Document document)
        {
            base.AddedToDocument(document);

            new LogicGateValueList().AddAsSource(
                this,
                1);
        }

        protected override void AddOutputFields()
        {
            OutString(
                nameof(IGraphLogic).Substring(1),
                "");
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
                "",
                GH_ParamAccess.item);

        protected static ParameterStrategy ConditionalRuleStrategy(
            Action<IGH_DataAccess> action)
        {
            return new ParameterStrategy(
                new List<ParameterConfig>
                {
                    NameParameter(),
                    new(
                        () => new Param_Integer(),
                        nameof(LogicGate),
                        "",
                        GH_ParamAccess.item),
                    new(
                        () => new Param_String(),
                        nameof(AbsCondition).Substring(3) + "s",
                        "",
                        GH_ParamAccess.list)
                },
                action);
        }

        protected ParameterStrategy GetNonConditionalStrategyFor<TRule>()
            where TRule : IGraphLogic, new()
        {
            return new ParameterStrategy(
                new List<ParameterConfig> { NameParameter() },
                da =>
                {
                    Logic = new TRule { Name = name };
                    Logic.Guard();
                });
        }

        protected ParameterStrategy GetConditionalStrategyFor<TRule>()
            where TRule : AbsRule, new()
        {
            return ConditionalRuleStrategy(da =>
            {
                int gateInt = da.GetOptionalItem(
                    1,
                    (int)AbsRule.DefLogicGate);

                gateInt.ValidateEnum<LogicGate>();
                logicGate = (LogicGate)gateInt;

                if (!da.GetItems(
                        2,
                        out List<string> conditionJsons))
                {
                    return;
                }

                Logic = new TRule
                {
                    Conditions =
                        conditionJsons.FromJson<AbsCondition>().ToList(),
                    LogicGate = logicGate,
                    Name = name,
                };

                Logic.Guard();
            });
        }
    }


    public class NodeLogicDropDown : AbsLogicDropDown<NodeLogicType>
    {
        public NodeLogicDropDown()
            : base(
                nameof(NodeLogicDropDown)
                    .Substring(
                        0,
                        9))
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
                    NodeLogicType.NodeAdjacencyRule,
                    GetConditionalStrategyFor<NodeAdjacencyRule>()
                },
                {
                    NodeLogicType.NodeProximityRule,
                    GetConditionalStrategyFor<NodeProximityRule>()
                },
                {
                    NodeLogicType.NodeVectorSumRule,
                    GetConditionalStrategyFor<NodeVectorSumRule>()
                },
                {
                    NodeLogicType.IsLeafNodeRule,
                    GetNonConditionalStrategyFor<IsLeafNodeRule>()
                },
                {
                    NodeLogicType.JointConstellation, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NameParameter(),
                            new(
                                () => new Param_String(),
                                nameof(NodeVector) + "s",
                                "",
                                GH_ParamAccess.list),
                            new(
                                () => new Param_Boolean(),
                                nameof(JointConstellation.StrictMatch),
                                "",
                                GH_ParamAccess.item)
                        },
                        SolveByJointConstellation)
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
                        9))
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
                    EdgeLogicType.EdgeLengthRule,
                    GetConditionalStrategyFor<EdgeLengthRule>()
                },
                {
                    EdgeLogicType.SourceAdjacencyRule,
                    GetConditionalStrategyFor<SourceAdjacencyRule>()
                },
                {
                    EdgeLogicType.TargetAdjacencyRule,
                    GetConditionalStrategyFor<TargetAdjacencyRule>()
                },
                {
                    EdgeLogicType.IsBridgeEdgeRule,
                    GetNonConditionalStrategyFor<IsBridgeEdgeRule>()
                },
                {
                    EdgeLogicType.LinkConstellation, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NameParameter(),
                            new(
                                () => new Param_String(),
                                nameof(LinkConstellation.AllowedSourceTags),
                                "",
                                GH_ParamAccess.list),
                            new(
                                () => new Param_String(),
                                nameof(LinkConstellation.AllowedTargetTags),
                                "",
                                GH_ParamAccess.list),
                            new(
                                () => new Param_Boolean(),
                                nameof(LinkConstellation.Bidirectional),
                                "",
                                GH_ParamAccess.item)
                        },
                        SolveByLinkConstellation)
                }
            };
        }
    }
}