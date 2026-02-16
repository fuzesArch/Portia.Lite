using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Portia.Helps;
using Portia.Infrastructure.Core.Portia.Main;
using Portia.Infrastructure.Core.Portia.Tasks;
using Portia.Infrastructure.Core.Validators;

namespace Portia.Lite.Components
{
    public class PortiaComponent : GenericBase, IGH_VariableParameterComponent
    {
        public PortiaComponent()
            : base(
                Naming.Tab.ToUpper(),
                Docs.PortiaComponent,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("ee1888f9-45c2-4c58-9d9e-b5eece9f5e94");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.PortiaLogo;

        private const int FixedInputCount = 0;
        private static int LastFixedInputIndex => FixedInputCount - 1;

        private List<IGraphQuery> _queries = new();

        protected override void AddInputFields()
        {
            InString(
                nameof(AbsSetGraph).Substring(3),
                "");
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

            for (int index = 0; index < Params.Input.Count; index++)
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

            var orderedTasks = tasks
                .OrderBy(GraphMaps.GetExecutionOrderOrCrash)
                .ToList();

            var pipeline = new GraphPipeline(orderedTasks);
            pipeline.Guard();

            foreach (var task in orderedTasks)
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
                   LastFixedInputIndex < index;
        }

        public bool CanRemoveParameter(
            GH_ParameterSide side,
            int index)
        {
            return side == GH_ParameterSide.Input && 0 < index;
        }

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
                    param.Name = nameof(AbsSetGraph).Substring(3);
                    param.NickName = nameof(AbsSetGraph).Substring(3);
                    param.Description =
                        $"Connect a {nameof(SetGraphByCurves)} or {nameof(LoadGraph)} Task JSON here.";
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
}