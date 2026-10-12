using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using System;
using System.Drawing;

namespace Portia.Lite.Components.Main
{
   public class GraphComponent : AbsPortiaGraphComponent
   {
      public GraphComponent() : base(nameof(GraphComponent)
              .Substring(0,
                  5),
         Docs.PortiaComponent,
         Naming.Tab,
         Naming.Graph,
         addTabToName: true)
      { }

      public override Guid ComponentGuid =>
            new("ee1888f9-45c2-4c58-9d9e-b5eece9f5e94");

      protected override Bitmap Icon =>
            Properties.Resources.WhiteLogo;

      protected override Color Colour => Color.PapayaWhip;
   }
}
