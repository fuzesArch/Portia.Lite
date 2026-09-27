using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Primitives.Models;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace Portia.Lite.Components.Models
{
   public class ModelAssetComponent : GenericBase
   {
      public ModelAssetComponent() : base(
         nameof(ModelAsset),
         Docs.Model,
         Naming.Tab,
         Naming.Primitives,
         addTabToName: true)
      { }

      public override Guid ComponentGuid =>
            new("3f6a9d12-8c4b-4e7a-b1d5-6e2c0f9a7b41");

      protected override void AddInputFields()
      {
         InString(nameof(ModelAsset.Name),
                  Docs.ModelName)
              .InGeometries(nameof(ModelAsset.Geometry),
                  Docs.ModelGeometry)
              .InString(nameof(ModelAsset.Description),
                  Docs.ModelDescription);

         SetInputParameterOptionality(2);
      }

      protected override void AddOutputFields()
      {
         OutGeneric(nameof(ModelAsset),
            Docs.Model);
      }

      protected override void Solve(
         IGH_DataAccess da)
      {
         List<GeometryBase> geometry = new();

         if (!da.GetItem(0,
               out string name) ||
            string.IsNullOrWhiteSpace(name) ||
            !da.GetDataList(
               1,
               geometry))
         {
            return;
         }

         ModelAsset model = new(name.Trim(),
            da.GetOptionalItem(2,
               ""),
            geometry);

         da.SetData(0,
            new GH_ObjectWrapper(model));

         Message = model.Size;
      }
   }
}
