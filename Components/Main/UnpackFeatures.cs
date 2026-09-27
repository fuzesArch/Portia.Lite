using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using System;
using System.Drawing;

namespace Portia.Lite.Components.Main
{
   // UnpackFeatures on the Portia tab, next to UnpackNodes and UnpackEdges — all its logic lives
   // in Infrastructure
   public class UnpackFeatures : UnpackFeaturesComponent
   {
      public UnpackFeatures() : base(nameof(UnpackFeatures),
         Naming.Tab,
         Naming.Logic,
         addTabToName: true)
      { }

      // the Guid UnpackLayers had, so its canvases still find it
      public override Guid ComponentGuid =>
            new("c9d46e07-d1e6-4c6f-95bb-c1c647cc05e9");

      public override GH_Exposure Exposure =>
            GH_Exposure.quinary;

      protected override Bitmap Icon =>
            Properties.Resources.BaseLogo;
   }
}
