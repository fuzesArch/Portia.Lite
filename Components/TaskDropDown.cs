using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.DocStrings;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Portia.Main;
using Portia.Infrastructure.Core.Portia.Primitives;
using Portia.Infrastructure.Core.Portia.Strategies;
using Portia.Infrastructure.Core.Portia.Tasks;
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

        private AbsTask task;
        private List<AbsSelection> selections;

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
                task.ToJson());
        }

        protected void SetSelections(
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

            task = new SetCurves(
                curves,
                tags);
        }

        protected void BySetNodeTypes(
            IGH_DataAccess da)
        {
            SetSelections(da);

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

        protected void BySetEdgeTypes(
            IGH_DataAccess da)
        {
            SetSelections(da);

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

        protected void ByGetNodes(
            IGH_DataAccess da)
        {
            SetSelections(da);

            if (selections == null) { return; }

            task = new GetNodes(selections);
        }

        protected void ByGetEdges(
            IGH_DataAccess da)
        {
            SetSelections(da);

            if (selections == null) { return; }

            task = new GetEdges(selections);
        }

        protected void ByVerifyNodes(
            IGH_DataAccess da)
        {
            SetSelections(da);

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

        protected void ByVerifyEdges(
            IGH_DataAccess da)
        {
            SetSelections(da);

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
                nameof(Docs.Selections),
                Docs.Selections,
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
                            SelectionsParameter(),
                            new(
                                () => new Param_String(),
                                nameof(SetNodeTypes.Types),
                                Docs.Types.Add(Prefix.StringList),
                                GH_ParamAccess.list)
                        },
                        BySetNodeTypes,
                        Docs.SetNodeTypes)
                },
                {
                    TaskType.SetEdgeTypes, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            SelectionsParameter(),
                            new(
                                () => new Param_String(),
                                nameof(SetEdgeTypes.Types),
                                Docs.Types.Add(Prefix.StringList),
                                GH_ParamAccess.list)
                        },
                        BySetEdgeTypes,
                        Docs.SetEdgeTypes)
                },
                {
                    TaskType.GetNodes, new ParameterStrategy(
                        new List<ParameterConfig> { SelectionsParameter(), },
                        ByGetNodes,
                        Docs.GetNodes)
                },
                {
                    TaskType.GetEdges, new ParameterStrategy(
                        new List<ParameterConfig> { SelectionsParameter(), },
                        ByGetEdges,
                        Docs.GetEdges)
                },
                {
                    TaskType.VerifyNodes, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            SelectionsParameter(),
                            new(
                                () => new Param_String(),
                                nameof(VerifyNodes.Logics),
                                Docs.Logics.Add(Prefix.JsonList),
                                GH_ParamAccess.list)
                        },
                        ByVerifyNodes,
                        Docs.VerifyNodes)
                },
                {
                    TaskType.VerifyEdges, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            SelectionsParameter(),
                            new(
                                () => new Param_String(),
                                nameof(VerifyEdges.Logics),
                                Docs.Logics.Add(Prefix.JsonList),
                                GH_ParamAccess.list)
                        },
                        ByVerifyEdges,
                        Docs.VerifyEdges)
                }
            };
        }
    }
}