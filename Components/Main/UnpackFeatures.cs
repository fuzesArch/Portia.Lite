using Portia.Infrastructure.Components;
using System;
using System.Drawing;

namespace Portia.Lite.Components.Main
{
   public class UnpackFeatures : UnpackFeaturesComponent
   {
      public UnpackFeatures() : base(nameof(UnpackFeatures),
         Naming.Tab,
         Naming.Logic,
         addTabToName: true)
      { }

      public override Guid ComponentGuid =>
            new("c9d46e07-d1e6-4c6f-95bb-c1c647cc05e9");

      protected override Bitmap Icon =>
            Properties.Resources.BaseLogo;
   }
}
