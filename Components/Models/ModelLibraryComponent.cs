using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Primitives.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components.Models
{
   public class ModelLibraryComponent : GenericBase
   {
      public ModelLibraryComponent() : base(
         nameof(ModelLibrary),
         Docs.ModelLibrary,
         Naming.Tab,
         Naming.Primitives,
         addTabToName: true)
      { }

      public override Guid ComponentGuid =>
            new("9b2e4c71-5d0a-4f38-a6e9-1c7d3b8f0e52");

      protected override void AddInputFields()
      {
         InGenerics(nameof(ModelLibrary.ModelAssets),
            Docs.Model);
      }

      protected override void AddOutputFields()
      {
         OutGeneric(nameof(ModelLibrary),
            Docs.ModelLibrary);
      }

      protected override void Solve(
         IGH_DataAccess da)
      {
         List<IGH_Goo> goos = new();

         da.GetDataList(0,
            goos);

         List<ModelAsset> models = goos
                                  .Select(x =>
                                         x?.ScriptVariable()
                                               as ModelAsset)
                                  .Where(x => x != null)
                                  .ToList();

         if (models.Count < goos.Count)
         {
            AddRuntimeMessage(
               GH_RuntimeMessageLevel.Warning,
               "Some inputs are not ModelAsset outputs and were skipped.");
         }

         List<string> duplicates = models.GroupBy(
                  x => x.Name,
                  StringComparer.OrdinalIgnoreCase)
              .Where(x => x.Count() > 1)
              .Select(x => x.Key)
              .ToList();

         if (duplicates.Any())
         {
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error,
               $"Model names must be unique: {string.Join(", ", duplicates)}.");

            return;
         }

         ModelLibrary library = new(models);

         da.SetData(0,
            new GH_ObjectWrapper(library));

         Message = $"{models.Count} models";
      }
   }
}
