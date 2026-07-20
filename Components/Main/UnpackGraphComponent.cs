using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Components.Goo;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphHelps;
using Portia.Infrastructure.Graphs;
using Portia.Infrastructure.Helps;
using System;
using System.Drawing;

namespace Portia.Lite.Components.Main
{
   public class UnpackGraphComponent : GenericBase
   {
      public UnpackGraphComponent() : base(
         nameof(Docs.UnpackGraph),
         Docs.UnpackGraph,
         Naming.Tab,
         Naming.Logic)
      { }

      public override Guid ComponentGuid =>
            new("c2a5e1f0-8b34-4d6e-9a17-3f5b2c8d4e60");

      public override GH_Exposure Exposure =>
            GH_Exposure.quinary;

      protected override Bitmap Icon =>
            Properties.Resources.BaseLogo;

      protected override void AddInputFields()
      {
         InManager.AddParameter(new GraphGooParameter(),
            nameof(GraphGoo),
            nameof(GraphGoo),
            Docs.GraphGoo.Add(Prefix.GraphGoo),
            GH_ParamAccess.item);
      }

      protected override void AddOutputFields()
      {
         OutGenerics(nameof(Docs.GraphNodeGoo),
                  Docs.GraphNodeGoo)
              .OutGenerics(nameof(Docs.GraphEdgeGoo),
                  Docs.GraphEdgeGoo);

         SimplifyOutputs();
      }

      protected override void Solve(
         IGH_DataAccess da)
      {
         if (!da.GetItem(0,
               out GraphGoo graphGoo) ||
            graphGoo?.Value == null)
         {
            return;
         }

         Graph graph = graphGoo.Value;

         da.SetDataList(0,
            graph.Nodes.ToGoo());

         da.SetDataList(1,
            graph.Edges.ToGoo());
      }
   }
}
