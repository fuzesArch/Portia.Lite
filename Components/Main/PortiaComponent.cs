using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphHelps;
using Portia.Infrastructure.Graphs;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Tasks.Base;
using Portia.Infrastructure.Tasks.GraphSetting;
using Portia.Infrastructure.Validators;
using Rhino.Geometry;
using System.Drawing;

namespace Portia.Lite.Components.Main
{
    public class PortiaComponent : GenericBase, IGH_VariableParameterComponent
    {
        public PortiaComponent()
            : base(
                Naming.Tab.ToUpper(),
                Docs.PortiaComponent,
                Naming.Tab,
                Naming.Graph)
        {
        }

        public override Guid ComponentGuid =>
            new("ee1888f9-45c2-4c58-9d9e-b5eece9f5e94");

        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override void CreateAttributes()
        {
            m_attributes = new PortiaComponentAttributes(this);
        }

        protected override Bitmap Icon => Properties.Resources.WhiteLogo;

        private const int FixedInputCount = 1;
        private static int LastFixedInputIndex => FixedInputCount - 1;

        private List<IGraphQuery> _queries = new();

        protected override void AddInputFields()
        {
            InGenerics(
                nameof(Docs.Origin),
                Docs.Origin);

            InString(
                nameof(AbsSetGraphTask).Substring(3),
                "");

            Params.Input[0].Optional = true;
            Params.Input[1].Optional = true;
        }

        protected override void AddOutputFields()
        {
            new SetGraphByCurves().RegisterOutputs(Params);
        }

        protected override void Solve(
            IGH_DataAccess da)
        {
            var tasks = new List<AbsTask>();
            bool redrawNeeded = false;

            var origin = new List<IGH_Goo>();

            if (da.GetDataList(
                    0,
                    origin) && origin.Any())
            {
                if (origin.First() is GraphGoo { Value: not null } graphGoo)
                {
                    tasks.Add(new LoadGraph(graphGoo.Value));
                }
                else
                {
                    var curves = new List<Curve>();
                    foreach (var goo in origin)
                    {
                        if (goo.CastTo(out Curve crv))
                        {
                            curves.Add(crv);
                        }
                    }

                    if (curves.Any())
                    {
                        tasks.Add(new SetGraphByCurves(curves));
                    }
                }
            }
            else
            {
                AddRuntimeMessage(
                    GH_RuntimeMessageLevel.Warning,
                    "Connect a Graph or Curves to initialize.");
                return;
            }

            for (int index = FixedInputCount;
                 index < Params.Input.Count;
                 index++)
            {
                if (!da.GetItem(
                        index,
                        out string json) || string.IsNullOrWhiteSpace(json))
                {
                    continue;
                }

                var task = json.FromJson<AbsTask>();
                tasks.Add(task);

                string name = task.GetType().Name;
                var param = Params.Input[index];

                if (param.Name == name)
                {
                    continue;
                }

                param.Name = name;
                param.NickName = name;
                param.Description = task.Description;
                redrawNeeded = true;
            }

            if (redrawNeeded)
            {
                OnDisplayExpired(true);
            }

            var pipeline = new GraphPipeline(tasks);
            pipeline.Guard();

            foreach (var task in tasks)
            {
                task.Guard();
            }

            var requiredQueries =
                pipeline.Tasks.SelectMany(x => x.Queries).ToList();

            if (OutputsMismatch(requiredQueries))
            {
                _queries = requiredQueries;

                OnPingDocument()
                    .ScheduleSolution(
                        2,
                        UpdateOutputsCallback);
                return;
            }

            pipeline.Execute(
                da,
                this,
                requiredQueries);

            pipeline.Graph.Log.ExposeToComponent(this);
            Message = pipeline.Graph.ComponentMessage();
        }

        private bool OutputsMismatch(
            List<IGraphQuery> required)
        {
            if (Params.Output.Count != required.Count)
            {
                return true;
            }

            for (int i = 0; i < required.Count; i++)
            {
                if (Params.Output[i].Name != required[i].Name)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateOutputsCallback(
            GH_Document doc)
        {
            while (_queries.Count < Params.Output.Count)
            {
                Params.UnregisterOutputParameter(
                    Params.Output[Params.Output.Count - 1]);
            }

            for (int i = 0; i < _queries.Count; i++)
            {
                var query = _queries[i];

                if (i < Params.Output.Count)
                {
                    var existingParam = Params.Output[i];

                    if (existingParam.Name != query.Name)
                    {
                        existingParam.Name = query.Name;
                        existingParam.NickName = query.Name;
                        existingParam.Description = "Portia Query Output";
                    }
                }
                else
                {
                    query.RegisterOutput(
                        Params,
                        i);
                }
            }

            Params.OnParametersChanged();
            ExpireSolution(true);
        }

        public bool CanInsertParameter(
            GH_ParameterSide side,
            int index)
        {
            return side == GH_ParameterSide.Input &&
                   index == Params.Input.Count;
        }

        public bool CanRemoveParameter(
            GH_ParameterSide side,
            int index)
        {
            return side == GH_ParameterSide.Input &&
                   index == Params.Input.Count - 1 &&
                   index > LastFixedInputIndex;
        }

        //public bool CanInsertParameter(
        //    GH_ParameterSide side,
        //    int index)
        //{
        //    return side == GH_ParameterSide.Input &&
        //           LastFixedInputIndex < index;
        //}

        //public bool CanRemoveParameter(
        //    GH_ParameterSide side,
        //    int index)
        //{
        //    return side == GH_ParameterSide.Input &&
        //           LastFixedInputIndex < index;
        //}

        public IGH_Param CreateParameter(
            GH_ParameterSide side,
            int index)
        {
            return new Param_String
            {
                Name = $"Task_{index}",
                NickName = $"Task_{index}",
                Description = "Additional Task",
                Access = GH_ParamAccess.item
            };
        }

        public bool DestroyParameter(
            GH_ParameterSide side,
            int index)
        {
            return true;
        }

        public void VariableParameterMaintenance()
        {
            for (int i = 0; i < Params.Input.Count; i++)
            {
                var param = Params.Input[i];

                if (param.SourceCount != 0)
                {
                    continue;
                }

                if (i == 0)
                {
                    param.Name = nameof(Docs.Origin);
                    param.NickName = nameof(Docs.Origin);
                    param.Description = Docs.Origin;
                }
                else
                {
                    param.Name = $"Task_{i}";
                    param.NickName = $"Task_{i}";
                    param.Description = "Connect a JSON Task here.";
                }
            }
        }
    }

    public class PortiaComponentAttributes : GH_ComponentAttributes
    {
        public PortiaComponentAttributes(
            IGH_Component component)
            : base(component)
        {
        }

        protected override void Render(
            GH_Canvas canvas,
            Graphics graphics,
            GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Objects)
            {
                GH_PaletteStyle style = GH_Skin.palette_normal_standard;

                GH_Skin.palette_normal_standard = new GH_PaletteStyle(
                    Color.Black,
                    Color.Black,
                    Color.PapayaWhip);

                base.Render(
                    canvas,
                    graphics,
                    channel);

                GH_Skin.palette_normal_standard = style;
            }
            else
            {
                base.Render(
                    canvas,
                    graphics,
                    channel);
            }
        }
    }
}