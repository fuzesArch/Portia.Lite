using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphHelps;
using Portia.Infrastructure.GraphItems;
using Portia.Infrastructure.Graphs;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Tasks.Isomorphism;
using Portia.Lite.Components.Goo;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components.Main
{
    #if INTERNAL
    public class MutationSetComponent : GenericBase
    {
        public MutationSetComponent()
            : base(
                nameof(MutationSet),
                Docs.MutationSets,
                Naming.Tab,
                Naming.Graph)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        public override Guid ComponentGuid =>
            new("8bdf4607-fbf8-488a-aea1-7ecf884d9ed0");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        protected override void AddInputFields()
        {
            InGenerics(
                nameof(MutationSet.Targets),
                "The list of isolated Match SubGraphs.");

            Params.RegisterInputParam(
                new GraphItemGooParameter
                {
                    Name =
                        nameof(MutationSet.Targets) + " / " +
                        nameof(PortGraph.Ports),
                    NickName =
                        nameof(MutationSet.Targets) + " / " +
                        nameof(PortGraph.Ports),
                    Description =
                        "DataTree of Port Nodes matching the TargetSubGraphs.",
                    Access = GH_ParamAccess.tree
                });

            InGeneric(
                nameof(MutationSet.Replacement),
                "The new Graph template to inject.");

            InGeometries(
                nameof(Docs.ReplacementPortPoints),
                Docs.ReplacementPortPoints);
        }

        protected override void AddOutputFields()
        {
            Params.RegisterOutputParam(
                new MutationSetParameter
                {
                    Name = nameof(MutationSet),
                    NickName = nameof(MutationSet),
                    Description = Docs.MutationSets
                });
        }

        protected override void Solve(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    0,
                    out List<GraphGoo> targetGoos))
            {
                return;
            }

            if (!da.GetDataTree(
                    1,
                    out GH_Structure<GraphItemGoo> targetPortTree))
            {
                return;
            }

            if (!da.GetItem(
                    2,
                    out GraphGoo newGoo) || newGoo?.Value == null)
            {
                return;
            }

            if (!da.GetItems(
                    3,
                    out List<Point3d> points))
            {
                return;
            }

            Graph newGraph = newGoo.Value.Clone();
            List<GraphNode> newPorts = newGraph.GetNodesBy(points);

            MutationSet mutationSet = new()
            {
                Replacement = new PortGraph(
                    newGraph,
                    newPorts),
            };

            if (targetGoos.Count != targetPortTree.Branches.Count)
            {
                AddRuntimeMessage(
                    GH_RuntimeMessageLevel.Error,
                    "Mismatch! The number of Target SubGraphs must exactly match " +
                    "the number of branches in the Target Ports tree.");
                return;
            }

            for (int i = 0; i < targetGoos.Count; i++)
            {
                var graph = targetGoos[i].Value.Clone();
                var ports = targetPortTree
                    .Branches[i]
                    .Select(item => item.Value.As<GraphNode>())
                    .ToList();

                mutationSet.Targets.Add(
                    new PortGraph(
                        graph,
                        ports));
            }

            da.SetData(
                0,
                new MutationSetGoo(mutationSet));
        }
    }
    #endif
}