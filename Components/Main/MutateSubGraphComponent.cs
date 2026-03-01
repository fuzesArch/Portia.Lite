using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphHelps;
using Portia.Infrastructure.GraphItems;
using Portia.Infrastructure.Graphs;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Tasks.Base;
using Portia.Infrastructure.Tasks.Isomorphism;
using Portia.Infrastructure.Validators;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

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
                nameof(MutateSubGraph.SubGraph) + "Goo",
                Docs.GraphGoo);


            InGenerics(
                nameof(MutateSubGraph.SubGraphPorts) + "Goos",
                Docs.GraphItemGoo);

            InGeneric(
                nameof(MutateSubGraph.NewGraph) + "Goo",
                Docs.GraphGoo);

            InGeometries(
                nameof(MutateSubGraph.NewGraphPorts),
                Docs.ReplacementPortPoints.ByDefault(Prefix.PointList));
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
                    out GraphGoo subGraphGoo) || subGraphGoo?.Value == null)
            {
                return;
            }

            if (!da.GetItems(
                    2,
                    out List<GraphItemGoo> subGraphPortGoos))
            {
                return;
            }


            if (!da.GetItem(
                    3,
                    out GraphGoo newGoo) || newGoo?.Value == null)
            {
                return;
            }

            if (!da.GetItems(
                    4,
                    out List<Point3d> points))
            {
                return;
            }

            var subGraph = subGraphGoo.Value.Clone();
            var subGraphPorts = subGraphPortGoos
                .Select(x => x.Value.As<GraphNode>())
                .ToList();

            var newGraph = newGoo.Value.Clone();
            var newGraphPorts = newGraph.GetNodesByPoints(points);

            var task = new MutateSubGraph
            {
                SubGraph = subGraph,
                SubGraphPorts = subGraphPorts,
                NewGraph = newGraph,
                NewGraphPorts = newGraphPorts,
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