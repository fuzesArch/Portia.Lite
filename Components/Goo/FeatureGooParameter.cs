using Grasshopper.Kernel;
using Portia.Infrastructure.Features.Base;
using Portia.Infrastructure.Goo;
using System;

namespace Portia.Lite.Components.Goo
{
    public class FeatureGooParameter : GH_Param<FeatureGoo>
    {
        public FeatureGooParameter()
            : base(
                nameof(IFeature).Substring(1),
                nameof(IFeature).Substring(1),
                Docs.FeatureGoo,
                Naming.Tab,
                Naming.Tab,
                GH_ParamAccess.item)
        {
        }

        public override Guid ComponentGuid =>
            new("57dbc0ed-1e03-4e83-8b4b-e33d4cf65ab3");

        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}