using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Features.Base;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Rules.Base;
using Portia.Infrastructure.Tasks.Base;
using Portia.Infrastructure.Tasks.RuleBased.Filtering;
using Portia.Infrastructure.Tasks.RuleBased.Setting.FeatureSetting;
using Portia.Infrastructure.Tasks.RuleBased.Setting.IndexSetting;
using Portia.Infrastructure.Tasks.RuleBased.Setting.TypeSetting;
using Portia.Infrastructure.Tasks.RuleBased.Verification;
using Portia.Infrastructure.Validators;
using Portia.Lite.Components.Goo;
using Portia.Lite.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

#if INTERNAL
using Portia.Infrastructure.Tasks.Solving;
using Portia.Infrastructure.Solvers.Base;
#endif

namespace Portia.Lite.Components.DropDowns
{
    public class TaskDropDown : AbsDropDownComponent<TaskMode>
    {
        public TaskDropDown()
            : base(
                nameof(TaskDropDown)
                    .Substring(
                        0,
                        4),
                Docs.Task,
                Naming.Tab,
                Naming.Logic)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        public override Guid ComponentGuid =>
            new("700c752f-a51e-4a2e-944a-a8941d1fc518");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        private AbsTask _task;
        private List<IRule> _rules;

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

        #if INTERNAL
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
        #endif

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
                nameof(GraphGoo),
                Docs.GraphGoo,
                GH_ParamAccess.item);

        protected override Dictionary<TaskMode, ParameterSetup> DefineSetup()
        {
            return new Dictionary<TaskMode, ParameterSetup>
            {
                {
                    TaskMode.SetNodeIndices, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            NodeRulesParam(), IndicesParam()
                        },
                        BySetIndices<SetNodeIndices>,
                        Docs.SetNodeIndices)
                },
                {
                    TaskMode.SetNodeTypes, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            NodeRulesParam(), TypesParam()
                        },
                        BySetTypes<SetNodeTypes>,
                        Docs.SetNodeTypes)
                },
                {
                    TaskMode.SetNodeFeatures, new ParameterSetup(
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
                    TaskMode.FilterNodes, new ParameterSetup(
                        new List<ParameterConfig> { NodeRulesParam(), },
                        ByFilter<FilterNodes>,
                        Docs.FilterNodes)
                },
                {
                    TaskMode.VerifyNodes, new ParameterSetup(
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
                    TaskMode.SetEdgeIndices, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            EdgeRulesParam(), IndicesParam()
                        },
                        BySetIndices<SetEdgeIndices>,
                        Docs.SetEdgeIndices)
                },
                {
                    TaskMode.SetEdgeTypes, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            EdgeRulesParam(),
                            JsonsParam(
                                nameof(Docs.SetEdgeTypes),
                                Docs.SetEdgeTypes)
                        },
                        BySetTypes<SetEdgeTypes>,
                        Docs.SetEdgeTypes)
                },
                {
                    TaskMode.SetEdgeFeatures, new ParameterSetup(
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
                    TaskMode.FilterEdges, new ParameterSetup(
                        new List<ParameterConfig> { EdgeRulesParam(), },
                        ByFilter<FilterEdges>,
                        Docs.FilterEdges)
                },
                {
                    TaskMode.VerifyEdges, new ParameterSetup(
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
                #if INTERNAL
                {
                    TaskMode.Solve, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            JsonsParam(
                                nameof(Docs.Solvers),
                                Docs.Solvers),
                        },
                        BySolve,
                        Docs.Solve)
                }
                #endif
            };
        }
    }
}