using Portia.Infrastructure.GraphItems;
using Portia.Infrastructure.Graphs;
using System;
using System.Collections.Generic;

namespace Portia.Lite.Components.Main
{
   public class
         UnpackNodesComponent : AbsUnpackComponent<
      GraphNode>
   {
      public UnpackNodesComponent() : base(
         nameof(UnpackNodesComponent)
              .Substring(0,
                  11),
         "A graph's node points, each with its index and type — from a Graph or filtered node goos.")
      { }

      public override Guid ComponentGuid =>
            new("b7d1c3e2-5a48-4f19-8c26-1e9f4a7b2d50");

      protected override string ItemLabel => "nodes";

      protected override IEnumerable<GraphNode> GraphItems(
         Graph graph) =>
            graph.Nodes;
   }
}
