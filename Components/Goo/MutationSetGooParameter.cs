using Grasshopper.Kernel;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.Tasks.Isomorphism;
using System;

namespace Portia.Lite.Components.Goo
{
    public class MutationSetParameter : GH_Param<MutationSetGoo>
    {
        public MutationSetParameter()
            : base(
                nameof(MutationSet),
                nameof(MutationSet),
                Docs.MutationSetGoo,
                Naming.Tab,
                Naming.Primitives,
                GH_ParamAccess.item)
        {
        }

        public override Guid ComponentGuid =>
            new("15cf422a-743d-459e-8f4f-24a8eb645239");

        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}