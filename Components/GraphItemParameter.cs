using Grasshopper.Kernel;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.Main;
using System;

namespace Portia.Lite.Components
{
    public class GraphItemParameter : GH_Param<GraphItemGoo>
    {
        public GraphItemParameter()
            : base(
                nameof(GraphItem),
                nameof(GraphItem),
                Docs.Rule,
                Naming.Tab,
                Naming.Tab,
                GH_ParamAccess.item)
        {
        }

        public override Guid ComponentGuid =>
            new("78e9f1a2-4c5d-4b3a-9e2f-1d2c3b4a5e6f");

        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}