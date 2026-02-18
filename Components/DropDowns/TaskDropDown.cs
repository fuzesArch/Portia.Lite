using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Features;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Main;
using Portia.Infrastructure.Rules.Base;
using Portia.Infrastructure.Solvers;
using Portia.Infrastructure.Tasks.Base;
using Portia.Infrastructure.Tasks.GraphSetting;
using Portia.Infrastructure.Tasks.RuleBased.Filtering;
using Portia.Infrastructure.Tasks.RuleBased.Setting.FeatureSetting;
using Portia.Infrastructure.Tasks.RuleBased.Setting.IndexSetting;
using Portia.Infrastructure.Tasks.RuleBased.Setting.TypeSetting;
using Portia.Infrastructure.Tasks.RuleBased.Verification;
using Portia.Infrastructure.Tasks.Solving;
using Portia.Infrastructure.Validators;
using Portia.Lite.Components.Goo;
using Portia.Lite.Core.Primitives;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components.DropDowns
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
                .ByDefault(GraphIdentity.DefType)
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
            where T : AbsSetIndicesTask, new()
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
            where T : AbsSetTypesTask, new()
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

        protected void BySetFeatures<T>(
            IGH_DataAccess da)
            where T : AbsSetFeaturesTask, new()
        {
            SetRules(da);

            if (_rules == null) { return; }

            if (!da.GetItems(
                    1,
                    out List<string> jsons))
            {
                return;
            }

            _task = new T
            {
                Rules = _rules,
                Features = jsons.FromJson<IFeature>().ToList()
            };
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

            _task = new AmalgamateGraph(
                _rules,
                anchorRuleJsons.FromJson<IRule>().ToList(),
                goo.Value);
        }

        protected void BySolve(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    0,
                    out List<string> solverJsons))
            {
                return;
            }

            _task = new Solve
            {
                Solvers = solverJsons.FromJson<ISolver>().ToList()
            };
        }

        protected static ParameterConfig JsonsParam(
            string name,
            string description) =>
            new(
                () => new Param_String(),
                name,
                description.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig NodeRulesParam() =>
            JsonsParam(
                nameof(Docs.NodeRules),
                Docs.NodeRules);

        protected static ParameterConfig EdgeRulesParam() =>
            JsonsParam(
                nameof(Docs.EdgeRules),
                Docs.EdgeRules);

        protected static ParameterConfig IndicesParam() =>
            new(
                () => new Param_Integer(),
                nameof(AbsSetIndicesTask.Indices),
                Docs.Indices.Add(Prefix.IntegerList),
                GH_ParamAccess.list);

        protected static ParameterConfig TypesParam() =>
            new(
                () => new Param_String(),
                nameof(AbsSetTypesTask.Types),
                Docs.Types.Add(Prefix.StringList),
                GH_ParamAccess.list);

        protected static ParameterConfig GraphParam() =>
            new(
                () => new GraphParameter(),
                nameof(Graph),
                Docs.GrapGoo,
                GH_ParamAccess.item);

        protected override Dictionary<TaskType, ParameterSetup> DefineSetup()
        {
            return new Dictionary<TaskType, ParameterSetup>
            {
                {
                    TaskType.SetGraphByCurves, new ParameterSetup(
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
                                    .ByDefault(GraphIdentity.DefType)
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
                    TaskType.LoadGraph, new ParameterSetup(
                        new List<ParameterConfig> { GraphParam() },
                        ByLoadGraph,
                        Docs.LoadGraph)
                },
                {
                    TaskType.SetNodeIndices, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            NodeRulesParam(), IndicesParam()
                        },
                        BySetIndices<SetNodeIndices>,
                        Docs.SetNodeIndices)
                },
                {
                    TaskType.SetEdgeIndices, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            EdgeRulesParam(), IndicesParam()
                        },
                        BySetIndices<SetEdgeIndices>,
                        Docs.SetEdgeIndices)
                },
                {
                    TaskType.SetNodeTypes, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            NodeRulesParam(), TypesParam()
                        },
                        BySetTypes<SetNodeTypes>,
                        Docs.SetNodeTypes)
                },
                {
                    TaskType.SetEdgeTypes, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            EdgeRulesParam(),
                            JsonsParam(
                                nameof(Docs.NodeFeatures),
                                Docs.SetNodeFeatures)
                        },
                        BySetTypes<SetEdgeTypes>,
                        Docs.SetEdgeTypes)
                },
                {
                    TaskType.SetNodeFeatures, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            NodeRulesParam(),
                            JsonsParam(
                                nameof(Docs.NodeFeatures),
                                Docs.SetNodeFeatures)
                        },
                        BySetFeatures<SetNodeFeatures>,
                        Docs.SetNodeFeatures)
                },
                {
                    TaskType.SetEdgeFeatures, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            EdgeRulesParam(),
                            JsonsParam(
                                nameof(Docs.EdgeFeatures),
                                Docs.SetEdgeFeatures)
                        },
                        BySetFeatures<SetEdgeFeatures>,
                        Docs.SetEdgeFeatures)
                },
                {
                    TaskType.FilterNodes, new ParameterSetup(
                        new List<ParameterConfig> { NodeRulesParam(), },
                        ByFilter<FilterNodes>,
                        Docs.FilterNodes)
                },
                {
                    TaskType.FilterEdges, new ParameterSetup(
                        new List<ParameterConfig> { EdgeRulesParam(), },
                        ByFilter<FilterEdges>,
                        Docs.FilterEdges)
                },
                {
                    TaskType.VerifyNodes, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            NodeRulesParam(),
                            JsonsParam(
                                nameof(Docs.NodeRulesToVerify),
                                Docs.NodeRulesToVerify),
                        },
                        ByVerify<VerifyNodes>,
                        Docs.VerifyNodes)
                },
                {
                    TaskType.VerifyEdges, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            EdgeRulesParam(),
                            JsonsParam(
                                nameof(Docs.EdgeRulesToVerify),
                                Docs.EdgeRulesToVerify)
                        },
                        ByVerify<VerifyEdges>,
                        Docs.VerifyEdges)
                },
                {
                    TaskType.AmalgamateGraph, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            JsonsParam(
                                nameof(Docs.TargetNodeRules),
                                Docs.TargetNodeRules),
                            JsonsParam(
                                nameof(Docs.AnchorNodeRules),
                                Docs.AnchorNodeRules),
                            GraphParam()
                        },
                        ByAmalgamation,
                        Docs.Amalgamate)
                },
                {
                    TaskType.Solve, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            JsonsParam(
                                nameof(Docs.Solvers),
                                Docs.Solvers),
                        },
                        BySolve,
                        Docs.Solve)
                }
            };
        }
    }
}