using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Primitives;
using Portia.Infrastructure.Core.Projects.Portia.Main;
using Portia.Infrastructure.Core.Projects.Portia.Primitives;
using Portia.Infrastructure.Core.Projects.Portia.Strategies;
using Portia.Infrastructure.Core.Projects.Portia.Tasks;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components
{
    public class TaskDropDown : DropDownComponent<TaskType>
    {
        public TaskDropDown()
            : base(
                nameof(TaskDropDown)
                    .Substring(
                        0,
                        4)
                    .AddDropDownMark(),
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("700c752f-a51e-4a2e-944a-a8941d1fc518");

        private AbsTask task;
        private List<AbsSelection> selections;

        protected override void AddInputFields()
        {
            InGeometries(
                    nameof(SetCurves.Curves),
                    "")
                .InStrings(
                    nameof(SetCurves.InitialEdgeTypes),
                    "");

            SetInputParameterOptionality(1);
        }

        protected override void AddOutputFields()
        {
            OutString(
                nameof(AbsTask).Substring(3),
                "");
        }

        protected override void CommonOutputSetting(
            IGH_DataAccess da)
        {
            da.SetData(
                0,
                task.ToJson());
        }

        protected void SetSelectionsInput(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    0,
                    out List<string> selectionStrings))
            {
                return;
            }

            selections = selectionStrings.FromJson<AbsSelection>().ToList();
        }

        protected void SolveBySetCurves(
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

            task = new SetCurves(
                curves,
                tags);
        }

        protected void SolveBySetNodeTypes(
            IGH_DataAccess da)
        {
            SetSelectionsInput(da);

            if (selections == null) { return; }

            if (!da.GetItems(
                    1,
                    out List<string> tags))
            {
                return;
            }

            task = new SetNodeTypes(
                selections,
                tags);
        }

        protected void SolveBySetEdgeTypes(
            IGH_DataAccess da)
        {
            SetSelectionsInput(da);

            if (selections == null) { return; }

            if (!da.GetItems(
                    1,
                    out List<string> tags))
            {
                return;
            }

            task = new SetEdgeTypes(
                selections,
                tags);
        }

        protected void SolveByGetNodes(
            IGH_DataAccess da)
        {
            SetSelectionsInput(da);

            if (selections == null) { return; }

            task = new GetNodes(selections);
        }

        protected void SolveByGetEdges(
            IGH_DataAccess da)
        {
            SetSelectionsInput(da);

            if (selections == null) { return; }

            task = new GetEdges(selections);
        }

        protected void SolveByVerifyNodes(
            IGH_DataAccess da)
        {
            SetSelectionsInput(da);

            if (selections == null) { return; }

            if (!da.GetItems(
                    1,
                    out List<string> logicJsons))
            {
                return;
            }

            task = new VerifyNodes(
                selections,
                logicJsons.FromJson<IGraphLogic>().ToList());
        }

        protected void SolveByVerifyEdges(
            IGH_DataAccess da)
        {
            SetSelectionsInput(da);

            if (selections == null) { return; }

            if (!da.GetItems(
                    1,
                    out List<string> logicJsons))
            {
                return;
            }

            task = new VerifyEdges(
                selections,
                logicJsons.FromJson<IGraphLogic>().ToList());
        }

        protected static ParameterConfig SelectionsParameter() =>
            new(
                () => new Param_String(),
                nameof(selections).ToUpperFirst(),
                "",
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
                                "",
                                GH_ParamAccess.list),
                            new(
                                () => new Param_String(),
                                nameof(SetCurves.InitialEdgeTypes),
                                "",
                                GH_ParamAccess.list)
                        },
                        SolveBySetCurves)
                },
                {
                    TaskType.SetNodeTypes, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            SelectionsParameter(),
                            new(
                                () => new Param_String(),
                                nameof(SetNodeTypes.Tags),
                                "",
                                GH_ParamAccess.list)
                        },
                        SolveBySetNodeTypes)
                },
                {
                    TaskType.SetEdgeTypes, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            SelectionsParameter(),
                            new(
                                () => new Param_String(),
                                nameof(SetEdgeTypes.Tags),
                                "",
                                GH_ParamAccess.list)
                        },
                        SolveBySetEdgeTypes)
                },
                {
                    TaskType.GetNodes, new ParameterStrategy(
                        new List<ParameterConfig> { SelectionsParameter(), },
                        SolveByGetNodes)
                },
                {
                    TaskType.GetEdges, new ParameterStrategy(
                        new List<ParameterConfig> { SelectionsParameter(), },
                        SolveByGetEdges)
                },
                {
                    TaskType.VerifyNodes, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            SelectionsParameter(),
                            new(
                                () => new Param_String(),
                                nameof(VerifyNodes.Logics),
                                "",
                                GH_ParamAccess.list)
                        },
                        SolveByVerifyNodes)
                },
                {
                    TaskType.VerifyEdges, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            SelectionsParameter(),
                            new(
                                () => new Param_String(),
                                nameof(VerifyEdges.Logics),
                                "",
                                GH_ParamAccess.list)
                        },
                        SolveByVerifyEdges)
                }
            };
        }
    }
}