using System;
using Portia.Infrastructure.Components.Capture;

namespace Portia.Lite.Components.Main
{
   // CaptureGraph on the Portia tab — all its logic lives in Infrastructure
   public class CaptureGraph : CaptureGraphComponent
   {
      public CaptureGraph() : base(nameof(CaptureGraph),
         Naming.Tab,
         Naming.Graph,
         addTabToName: true)
      { }

      // the Guid the Skins CaptureGrid had, so its canvases still find it
      public override Guid ComponentGuid =>
            new("e21f72a4-783b-4c53-b99b-4309a6fdf391");
   }
}
