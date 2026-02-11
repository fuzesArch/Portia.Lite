using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.DocStrings;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Portia.Main;
using Portia.Infrastructure.Core.Portia.Primitives;
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
                        4)
                    .AddDropDownMark(),
                Docs.Task.AddDropDownNote(),
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("700c752f-a51e-4a2e-944a-a8941d1fc518");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.ColoredLogo;

        private AbsTask _task;
        private List<IRule> _rules;

        protected override void AddInputFields()
        {
            InGeometries(
                    nameof(SetCurves.Curves),
                    Docs.Curves)
                .InStrings(
                    nameof(SetCurves.Types),
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

        protected void BySetCurves(
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

            _task = new SetCurves(
                curves,
                tags);
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
            _task.Guard();
        }

        protected void ByGet<T>(
            IGH_DataAccess da)
            where T : AbsGetTask, new()
        {
            SetRules(da);

            if (_rules == null) { return; }

            _task = new T { Rules = _rules };
            _task.Guard();
        }

        protected void ByVerify<T>(
            IGH_DataAccess da)
            where T : AbsVerifyTask, new()
        {
            SetRules(da);

            if (_rules == null) { return; }

            if (!da.GetItems(
                    1,
                    out List<string> logicJsons))
            {
                return;
            }

            _task = new T
            {
                Rules = _rules,
                Logics = logicJsons.FromJson<IRule>().ToList()
            };
            _task.Guard();
        }

        protected static ParameterConfig NodeRuleParameter() =>
            new(
                () => new Param_String(),
                nameof(Docs.NodeRule) + "s",
                Docs.NodeRule,
                GH_ParamAccess.list);

        protected static ParameterConfig EdgeRuleParameter() =>
            new(
                () => new Param_String(),
                nameof(Docs.EdgeRule) + "s",
                Docs.EdgeRule,
                GH_ParamAccess.list);

        protected static ParameterConfig NodeLogicsParameter() =>
            new(
                () => new Param_String(),
                nameof(Docs.NodeLogics),
                Docs.NodeLogics.Add(Prefix.JsonList),
                GH_ParamAccess.list);


        protected static ParameterConfig EdgeLogicsParameter() =>
            new(
                () => new Param_String(),
                nameof(Docs.EdgeLogics),
                Docs.EdgeLogics.Add(Prefix.JsonList),
                GH_ParamAccess.list);

        protected static ParameterConfig TypesParameter() =>
            new(
                () => new Param_String(),
                nameof(AbsSetTypes.Types),
                Docs.Types.Add(Prefix.StringList),
                GH_ParamAccess.list);

        protected override Dictionary<TaskType, ParameterStrategy>
            DefineParameterStrategy()
        {
            return new Dictionary<TaskType, ParameterStrategy>
            {
                {
                    TaskType.SetCurves, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_Curve(),
                                nameof(SetCurves.Curves),
                                Docs.Curves.Add(Prefix.GeometryList),
                                GH_ParamAccess.list),
                            new(
                                () => new Param_String(),
                                nameof(SetCurves.Types),
                                Docs
                                    .InitialEdgeTypes
                                    .ByDefault(Identity.DefType)
                                    .Extend(
                                        CoreDocStrings.Boost(
                                            nameof(SetCurves.Curves)))
                                    .Add(Prefix.StringList),
                                GH_ParamAccess.list,
                                isOptional: true)
                        },
                        BySetCurves,
                        Docs.SetCurves)
                },
                {
                    TaskType.SetNodeTypes, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NodeRuleParameter(), TypesParameter()
                        },
                        BySetTypes<SetNodeTypes>,
                        Docs.SetNodeTypes)
                },
                {
                    TaskType.SetEdgeTypes, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            EdgeRuleParameter(), TypesParameter()
                        },
                        BySetTypes<SetEdgeTypes>,
                        Docs.SetEdgeTypes)
                },
                {
                    TaskType.GetNodes, new ParameterStrategy(
                        new List<ParameterConfig> { NodeRuleParameter(), },
                        ByGet<GetNodes>,
                        Docs.GetNodes)
                },
                {
                    TaskType.GetEdges, new ParameterStrategy(
                        new List<ParameterConfig> { EdgeRuleParameter(), },
                        ByGet<GetEdges>,
                        Docs.GetEdges)
                },
                {
                    TaskType.VerifyNodes, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NodeRuleParameter(), NodeLogicsParameter()
                        },
                        ByVerify<VerifyNodes>,
                        Docs.VerifyNodes)
                },
                {
                    TaskType.VerifyEdges, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            EdgeRuleParameter(), EdgeLogicsParameter()
                        },
                        ByVerify<VerifyEdges>,
                        Docs.VerifyEdges)
                }
            };
        }
    }
}