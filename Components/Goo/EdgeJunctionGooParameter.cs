using Grasshopper.Kernel;
using Portia.Infrastructure.Goo;
using System;

namespace Portia.Lite.Components.Goo
{
    public class EdgeJunctionGooParameter : GH_Param<EdgeJunctionGoo>
    {
        public EdgeJunctionGooParameter()
            : base(
                nameof(EdgeJunctionGoo),
                nameof(EdgeJunctionGoo),
                Docs.EdgeJunctionGoo,
                Naming.Tab,
                Naming.Tab,
                GH_ParamAccess.item)
        {
        }

        public override Guid ComponentGuid =>
            new("57105d98-f4e3-47ea-9761-8d4f0f545a94");

        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}