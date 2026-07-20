using Portia.Infrastructure.GraphItems;
using Portia.Infrastructure.Graphs;
using System;
using System.Collections.Generic;

namespace Portia.Lite.Components.Main
{
   public class
         UnpackEdgesComponent : AbsUnpackComponent<
      GraphEdge>
   {
      public UnpackEdgesComponent() : base(
         nameof(UnpackEdgesComponent)
              .Substring(0,
                  11),
         "A graph's edge curves, each with its index and type — from a Graph or filtered edge goos.")
      { }

      public override Guid ComponentGuid =>
            new("e8c2d4f3-6b59-4a2e-9d37-2fa05b8c3e61");

      protected override string ItemLabel => "edges";

      protected override IEnumerable<GraphEdge> GraphItems(
         Graph graph) =>
            graph.Edges;
   }
}
