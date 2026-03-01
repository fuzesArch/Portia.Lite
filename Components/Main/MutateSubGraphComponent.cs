using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphHelps;
using Portia.Infrastructure.Graphs;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Tasks.Base;
using Portia.Infrastructure.Tasks.Isomorphism;
using Portia.Infrastructure.Validators;
using System;
using System.Collections.Generic;

namespace Portia.Lite.Components.Main
{
    public class MutateSubGraphComponent : GenericBase
    {
        public MutateSubGraphComponent()
            : base(
                nameof(MutateSubGraph),
                Docs.MutateSubGraph,
                Naming.Tab,
                Naming.Graph)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        public override Guid ComponentGuid =>
            new("04263052-5cc3-47ba-96df-f8b91d274acd");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        protected override void AddInputFields()
        {
            InGeneric(
                nameof(Docs.GraphGoo),
                Docs.GraphGoo);

            InGeneric(
                nameof(MutateSubGraph.TargetMatchGraph) + "Goo",
                Docs.GraphGoo);

            InGeneric(
                nameof(MutateSubGraph.ReplacementGraph) + "Goo",
                Docs.GraphGoo);
        }

        protected override void AddOutputFields()
        {
            new MutateSubGraph().RegisterOutputs(Params);
        }

        protected override void Solve(
            IGH_DataAccess da)
        {
            if (!da.GetItem(
                    0,
                    out GraphGoo mainGoo) || mainGoo?.Value == null)
            {
                return;
            }

            if (!da.GetItem(
                    1,
                    out GraphGoo targetGoo) || targetGoo?.Value == null)
            {
                return;
            }

            if (!da.GetItem(
                    2,
                    out GraphGoo replacementGoo) ||
                replacementGoo?.Value == null)
            {
                return;
            }

            var task = new MutateSubGraph
            {
                TargetMatchGraph = targetGoo.Value,
                ReplacementGraph = replacementGoo.Value,
            };

            task.Guard();

            GraphPipeline pipeline = new(new List<AbsTask> { task })
            {
                Graph = mainGoo.Value.Clone()
            };

            pipeline.Execute(
                da,
                this,
                task.Queries);

            Message = pipeline.Graph.ComponentMessage() ??
                      task.ComponentMessage();
        }
    }
}