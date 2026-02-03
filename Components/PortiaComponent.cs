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

namespace Portia.Lite.Components
{
    public class PortiaComponent : GenericBase, IGH_VariableParameterComponent
    {
        public PortiaComponent()
            : base(
                Naming.Tab.ToUpper(),
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("ee1888f9-45c2-4c58-9d9e-b5eece9f5e94");

        private const int FixedInputCount = 1;
        private static int LastFixedInputIndex => FixedInputCount - 1;

        private List<IGraphQuery> queries = new();

        protected override void AddInputFields()
        {
            InString(
                nameof(SetCurves),
                "");
        }

        protected override void AddOutputFields()
        {
            new SetCurves().RegisterOutputs(Params);
        }

        protected override void SolveInstance(
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

                if (LastFixedInputIndex >= index)
                {
                    continue;
                }

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
            var requiredQueries =
                pipeline.Tasks.SelectMany(x => x.Queries).ToList();

            if (OutputsMismatch(requiredQueries))
            {
                queries = requiredQueries;

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
            Params.ClearOutputs();
            queries.RegisterOutputs(Params);
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
            return side == GH_ParameterSide.Input &&
                   LastFixedInputIndex < index;
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
            for (int i = 1; i < Params.Input.Count; i++)
            {
                var param = Params.Input[i];

                if (param.SourceCount != 0)
                {
                    continue;
                }

                param.Name = $"Task_{i}";
                param.NickName = $"Task_{i}";
                param.Description = "Connect a JSON Task here";
            }
        }
    }
}