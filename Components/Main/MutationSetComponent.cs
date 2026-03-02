using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphHelps;
using Portia.Infrastructure.GraphItems;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Tasks.Isomorphism;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components.Main
{
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
                "TargetSubGraphs",
                "The list of isolated Match SubGraphs.");

            var portsParam = new Goo.GraphItemGooParameter
            {
                Name = "TargetPorts",
                NickName = "TP",
                Description =
                    "DataTree of Port Nodes matching the TargetSubGraphs.",
                Access = GH_ParamAccess.tree
            };
            Params.RegisterInputParam(portsParam);

            InGeneric(
                "ReplacementGraph",
                "The new Graph template to inject.");
            InGeometries(
                "ReplacementPortPoints",
                "List of points identifying the ports on the Replacement Graph.");
        }

        protected override void AddOutputFields()
        {
            var param = new Goo.MutationSetParameter();
            param.Name = "MutationSet";
            param.NickName = "MS";
            param.Description = "The packaged mutation rule.";
            Params.RegisterOutputParam(param);
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
                    out GH_Structure<GraphItemGoo> targetPortsTree))
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

            if (targetGoos.Count != targetPortsTree.Branches.Count)
            {
                AddRuntimeMessage(
                    GH_RuntimeMessageLevel.Error,
                    "The number of Target SubGraphs must equal the number " +
                    "of branches in the Target Ports tree.");
                return;
            }

            var mutationSet = new MutationSet
            {
                TargetSubGraphs =
                    targetGoos.Select(g => g.Value.Clone()).ToList(),
                ReplacementGraph = newGoo.Value.Clone(),
                ReplacementPortPoints = points
            };

            // Map the DataTree branches into the List<List<GraphNode>>
            foreach (var branch in targetPortsTree.Branches)
            {
                var portList = branch
                    .Select(item => item.Value.As<GraphNode>())
                    .ToList();
                mutationSet.TargetPortLists.Add(portList);
            }

            da.SetData(
                0,
                new MutationSetGoo(mutationSet));
        }
    }
}