using Grasshopper.Kernel;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.Main;
using System;

namespace Portia.Lite.Components.Goo
{
    public class GraphParameter : GH_Param<GraphGoo>
    {
        public GraphParameter()
            : base(
                nameof(Graph),
                nameof(Graph),
                "A sovereign Portia Graph.",
                Naming.Tab,
                Naming.Tab,
                GH_ParamAccess.item)
        {
        }

        public override Guid ComponentGuid =>
            new("65fbe9a6-94d5-4f38-a072-ae1e44c050bf");

        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}