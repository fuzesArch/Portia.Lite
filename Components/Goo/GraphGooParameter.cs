using Grasshopper.Kernel;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.Graphs;
using System;

namespace Portia.Lite.Components.Goo
{
    public class GraphGooParameter : GH_Param<GraphGoo>
    {
        public GraphGooParameter()
            : base(
                nameof(Graph),
                nameof(Graph),
                Docs.GraphGoo,
                Naming.Tab,
                Naming.Primitives,
                GH_ParamAccess.item)
        {
        }

        public override Guid ComponentGuid =>
            new("65fbe9a6-94d5-4f38-a072-ae1e44c050bf");

        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}