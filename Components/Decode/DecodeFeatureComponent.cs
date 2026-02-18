using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Features.Base;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.Helps;
using Portia.Lite.Components.Goo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components.Decode
{
    public class DecodeFeatureComponent : GenericBase
    {
        public DecodeFeatureComponent()
            : base(
                nameof(DecodeFeatureComponent)
                    .Substring(
                        0,
                        13),
                "Deconstructs a Portia Feature into its Name and Value.",
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("463f82b3-c234-4b5c-b162-c2f2911a100a");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        protected override void RegisterInputParams(
            GH_InputParamManager pManager)
        {
            pManager.AddParameter(
                new FeatureGooParameter(),
                nameof(Docs.FeatureGoo),
                nameof(Docs.FeatureGoo),
                Docs.FeatureGoo,
                GH_ParamAccess.list);
        }

        protected override void AddOutputFields()
        {
            OutStrings(
                    nameof(IFeature.Name),
                    Docs.Name)
                .OutGenerics(
                    nameof(AbsFeature<double>.Value),
                    Docs.FeatureValue);
        }

        protected override void SolveInstance(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    0,
                    out List<FeatureGoo> goos))
            {
                return;
            }

            da.SetDataList(
                0,
                goos.Select(x => x?.Value?.Name));

            da.SetDataList(
                1,
                goos.Select(x =>
                {
                    dynamic dynFeature = x.Value;
                    return dynFeature.Value;
                }));
        }
    }
}