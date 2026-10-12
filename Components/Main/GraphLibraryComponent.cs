using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.Primitives.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components.Main
{
   // Names a list of graphs so a live session can attach them by name.
   public class GraphLibraryComponent : GenericBase
   {
      public GraphLibraryComponent() : base(
         nameof(GraphLibrary),
         Docs.GraphLibrary,
         Naming.Tab,
         Naming.Graph,
         addTabToName: true)
      { }

      public override Guid ComponentGuid =>
            new("4f8a2d6b-9c31-4e7a-b5d0-2a6e8f1c7b93");

      protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

      protected override void AddInputFields()
      {
         InStrings(nameof(Docs.GraphAssetNames)
                 .Substring(10),
               Docs.GraphAssetNames)
              .InGenerics(nameof(Docs.GraphAssetGraphs)
                    .Substring(10),
                  Docs.GraphAssetGraphs);
      }

      protected override void AddOutputFields()
      {
         OutGeneric(nameof(GraphLibrary),
            Docs.GraphLibrary);
      }

      protected override void Solve(
         IGH_DataAccess da)
      {
         List<string> names = new();
         List<IGH_Goo> goos = new();

         if (!da.GetDataList(0,
                names) ||
            !da.GetDataList(1,
               goos))
         {
            return;
         }

         if (names.Count != goos.Count)
         {
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error,
               $"{names.Count} names for {goos.Count} graphs — one name per graph.");

            return;
         }

         List<GraphAsset> assets = new();

         for (int i = 0; i < names.Count; i++)
         {
            if (goos[i] is not GraphGoo { Value: { } graph })
            {
               AddRuntimeMessage(
                  GH_RuntimeMessageLevel.Warning,
                  $"Input {i} is not a graph and was skipped.");

               continue;
            }

            if (string.IsNullOrWhiteSpace(names[i]))
            {
               AddRuntimeMessage(
                  GH_RuntimeMessageLevel.Warning,
                  $"Graph {i} has no name and was skipped.");

               continue;
            }

            assets.Add(new GraphAsset(names[i].Trim(),
               graph));
         }

         List<string> duplicates = assets.GroupBy(
                  a => a.Name,
                  StringComparer.OrdinalIgnoreCase)
              .Where(g => g.Count() > 1)
              .Select(g => g.Key)
              .ToList();

         if (duplicates.Any())
         {
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error,
               $"Asset names must be unique: {string.Join(", ", duplicates)}.");

            return;
         }

         da.SetData(0,
            new GH_ObjectWrapper(new GraphLibrary(assets)));

         Message = $"{assets.Count} assets";
      }
   }
}
