using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.DocStrings;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Portia.Main;
using Portia.Infrastructure.Core.Portia.Natives;
using Portia.Infrastructure.Core.Portia.Primitives;
using Portia.Infrastructure.Core.Portia.Rules;
using Portia.Infrastructure.Core.Portia.Tasks;
using Portia.Infrastructure.Core.Validators;
using Portia.Lite.Core.Primitives;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components
{
    public class TaskDropDown : AbsDropDownComponent<TaskType>
    {
        public TaskDropDown()
            : base(
                nameof(TaskDropDown)
                    .Substring(
                        0,
                        4),
                Docs.Task,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("700c752f-a51e-4a2e-944a-a8941d1fc518");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        private AbsTask _task;
        private List<IRule> _rules;

        protected override void AddInputFields()
        {
            InGeometries(
                    nameof(SetGraphByCurves.Curves),
                    Docs.Curves)
                .InStrings(
                    nameof(SetGraphByCurves.Types),
                    Docs.InitialEdgeTypes);

            SetInputParameterOptionality(1);
        }

        protected override void AddOutputFields()
        {
            OutString(
                nameof(AbsTask).Substring(3),
                Docs.Task);
        }

        protected override void CommonOutputSetting(
            IGH_DataAccess da)
        {
            _task.Guard();
            Message = _task.ComponentMessage();

            da.SetData(
                0,
                _task.ToJson());
        }

        protected void SetRules(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    0,
                    out List<string> jsons))
            {
                return;
            }

            _rules = jsons.FromJson<IRule>().ToList();
        }

        protected void BySetGraphByCurves(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    0,
                    out List<Curve> curves))
            {
                return;
            }

            var tags = da
                .GetOptionalItems<string>(1)
                .ByDefault(Identity.DefType)
                .BoostTo(curves.Count);

            _task = new SetGraphByCurves(
                curves,
                tags);
        }

        protected void ByLoadGraph(
            IGH_DataAccess da)
        {
            if (!da.GetItem(
                    0,
                    out GraphGoo goo) || goo?.Value == null)
            {
                return;
            }

            _task = new LoadGraph(goo.Value);
        }


        protected void BySetIndices<T>(
            IGH_DataAccess da)
            where T : AbsSetIndices, new()
        {
            SetRules(da);

            if (_rules == null) { return; }

            if (!da.GetItems(
                    1,
                    out List<int> indices))
            {
                return;
            }

            _task = new T { Rules = _rules, Indices = indices };
        }


        protected void BySetTypes<T>(
            IGH_DataAccess da)
            where T : AbsSetTypes, new()
        {
            SetRules(da);

            if (_rules == null) { return; }

            if (!da.GetItems(
                    1,
                    out List<string> types))
            {
                return;
            }

            _task = new T { Rules = _rules, Types = types };
        }

        protected void ByFilter<T>(
            IGH_DataAccess da)
            where T : AbsFilterTask, new()
        {
            SetRules(da);

            if (_rules == null) { return; }

            _task = new T { Rules = _rules };
        }

        protected void ByVerify<T>(
            IGH_DataAccess da)
            where T : AbsVerifyTask, new()
        {
            SetRules(da);

            if (_rules == null) { return; }

            if (!da.GetItems(
                    1,
                    out List<string> rulesToVerifyJsons))
            {
                return;
            }

            _task = new T
            {
                Rules = _rules,
                RulesToVerify =
                    rulesToVerifyJsons.FromJson<IRule>().ToList()
            };
        }

        protected void ByAmalgamation(
            IGH_DataAccess da)
        {
            SetRules(da);

            if (_rules == null) { return; }

            if (!da.GetItems(
                    1,
                    out List<string> anchorRuleJsons))
            {
                return;
            }

            if (!da.GetItem(
                    2,
                    out GraphGoo goo) || goo?.Value == null)
            {
                return;
            }

            _task = new Amalgamate(
                _rules,
                anchorRuleJsons.FromJson<IRule>().ToList(),
                goo.Value);
        }

        protected static ParameterConfig NodeRulesParameter() =>
            new(
                () => new Param_String(),
                nameof(Docs.NodeRules),
                Docs.NodeRules.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig NodeRulesParameter(
            string name,
            string description) =>
            new(
                () => new Param_String(),
                name,
                description.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig EdgeRulesParameter() =>
            new(
                () => new Param_String(),
                nameof(Docs.EdgeRules),
                Docs.EdgeRules.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig EdgeRulesParameter(
            string docsString,
            string description) =>
            new(
                () => new Param_String(),
                docsString,
                description.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig IndicesParameter() =>
            new(
                () => new Param_Integer(),
                nameof(AbsSetIndices.Indices),
                Docs.Indices.Add(Prefix.IntegerList),
                GH_ParamAccess.list);

        protected static ParameterConfig TypesParameter() =>
            new(
                () => new Param_String(),
                nameof(AbsSetTypes.Types),
                Docs.Types.Add(Prefix.StringList),
                GH_ParamAccess.list);

        protected static ParameterConfig GraphParameter() =>
            new(
                () => new GraphParameter(),
                nameof(Graph),
                Docs.GrapGoo,
                GH_ParamAccess.item);

        protected override Dictionary<TaskType, ParameterStrategy>
            DefineParameterStrategy()
        {
            return new Dictionary<TaskType, ParameterStrategy>
            {
                {
                    TaskType.SetGraphByCurves, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_Curve(),
                                nameof(SetGraphByCurves.Curves),
                                Docs.Curves.Add(Prefix.GeometryList),
                                GH_ParamAccess.list),
                            new(
                                () => new Param_String(),
                                nameof(SetGraphByCurves.Types),
                                Docs
                                    .InitialEdgeTypes
                                    .ByDefault(Identity.DefType)
                                    .Extend(
                                        CoreDocStrings.Boost(
                                            nameof(SetGraphByCurves.Curves)))
                                    .Add(Prefix.StringList),
                                GH_ParamAccess.list,
                                isOptional: true)
                        },
                        BySetGraphByCurves,
                        Docs.SetGraphByCurves)
                },
                {
                    TaskType.LoadGraph, new ParameterStrategy(
                        new List<ParameterConfig> { GraphParameter() },
                        ByLoadGraph,
                        Docs.LoadGraph)
                },
                {
                    TaskType.SetNodeIndices, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NodeRulesParameter(), IndicesParameter()
                        },
                        BySetIndices<SetNodeIndices>,
                        Docs.SetNodeIndices)
                },
                {
                    TaskType.SetEdgeIndices, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            EdgeRulesParameter(), IndicesParameter()
                        },
                        BySetIndices<SetEdgeIndices>,
                        Docs.SetEdgeIndices)
                },
                {
                    TaskType.SetNodeTypes, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NodeRulesParameter(), TypesParameter()
                        },
                        BySetTypes<SetNodeTypes>,
                        Docs.SetNodeTypes)
                },
                {
                    TaskType.SetEdgeTypes, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            EdgeRulesParameter(), TypesParameter()
                        },
                        BySetTypes<SetEdgeTypes>,
                        Docs.SetEdgeTypes)
                },
                {
                    TaskType.FilterNodes, new ParameterStrategy(
                        new List<ParameterConfig> { NodeRulesParameter(), },
                        ByFilter<FilterNodes>,
                        Docs.FilterNodes)
                },
                {
                    TaskType.FilterEdges, new ParameterStrategy(
                        new List<ParameterConfig> { EdgeRulesParameter(), },
                        ByFilter<FilterEdges>,
                        Docs.FilterEdges)
                },
                {
                    TaskType.VerifyNodes, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NodeRulesParameter(),
                            NodeRulesParameter(
                                nameof(Docs.NodeRulesToVerify),
                                Docs.NodeRulesToVerify),
                        },
                        ByVerify<VerifyNodes>,
                        Docs.VerifyNodes)
                },
                {
                    TaskType.VerifyEdges, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            EdgeRulesParameter(),
                            EdgeRulesParameter(
                                nameof(Docs.EdgeRulesToVerify),
                                Docs.EdgeRulesToVerify)
                        },
                        ByVerify<VerifyEdges>,
                        Docs.VerifyEdges)
                },
                {
                    TaskType.Amalgamate, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NodeRulesParameter(
                                nameof(Docs.TargetNodeRules),
                                Docs.TargetNodeRules),
                            NodeRulesParameter(
                                nameof(Docs.AnchorNodeRules),
                                Docs.AnchorNodeRules),
                            GraphParameter()
                        },
                        ByAmalgamation,
                        Docs.Amalgamate)
                }
            };
        }
    }
}