using Grasshopper.Kernel;
using Portia.Infrastructure.Core.Portia.Main;
using Portia.Infrastructure.Core.Portia.Natives;
using System;

namespace Portia.Lite.Components
{
    public class GraphItemParameter : GH_Param<GraphItemGoo>
    {
        public GraphItemParameter()
            : base(
                nameof(GraphItem),
                nameof(GraphItem),
                Docs.Logic,
                Naming.Tab,
                Naming.Tab,
                GH_ParamAccess.item)
        {
        }

        public override Guid ComponentGuid =>
            new Guid("78e9f1a2-4c5d-4b3a-9e2f-1d2c3b4a5e6f");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.ColoredLogo;
    }
}