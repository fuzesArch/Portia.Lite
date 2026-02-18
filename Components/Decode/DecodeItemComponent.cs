using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.Helps;
using System.Linq;
using Portia.Lite.Components.Goo;
using Grasshopper.Kernel.Data;
using Grasshopper;

namespace Portia.Lite.Components.Decode
{
    public class DecodeItemComponent : GenericBase
    {
        public DecodeItemComponent()
            : base(
                nameof(DecodeItemComponent)
                    .Substring(
                        0,
                        10),
                Docs.DeconstructItem,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("c7ca6133-1a0f-4d40-80c6-7761eaabaa5a");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        protected override void RegisterInputParams(
            GH_InputParamManager pManager)
        {
            pManager.AddParameter(
                new GraphItemGooParameter(),
                nameof(GraphItemGoo),
                nameof(GraphItemGoo),
                Docs.GraphItemGoo,
                GH_ParamAccess.list);
        }

        protected override void AddOutputFields()
        {
            OutGenerics(
                    nameof(Docs.Geometries),
                    Docs.Geometries)
                .OutIntegers(
                    nameof(Docs.Index) + "es",
                    Docs.Identity)
                .OutStrings(
                    nameof(Docs.Type) + "s",
                    Docs.Type)
                .OutGenericTree(
                    nameof(Docs.FeatureGoo) + "s",
                    Docs.FeatureGoo)
                .OutJsons(
                    nameof(Docs.Json),
                    Docs.Json);
        }

        protected override void SolveInstance(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    0,
                    out List<GraphItemGoo> goos))
            {
                return;
            }

            da.SetDataList(
                0,
                goos.Select(x => x.GetGeometries()));

            da.SetDataList(
                1,
                goos.Select(x => x.Value.GraphIdentity.Index));

            da.SetDataList(
                2,
                goos.Select(x => x.Value.GraphIdentity.Type));

            var featureTree = new DataTree<FeatureGoo>();

            for (int i = 0; i < goos.Count; i++)
            {
                var path = new GH_Path(
                    da.Iteration,
                    i);
                var features = goos[i].Value.FeatureSet.GetAll();

                foreach (var feature in features)
                {
                    featureTree.Add(
                        new FeatureGoo(feature),
                        path);
                }
            }

            da.SetDataTree(
                3,
                featureTree);

            da.SetDataList(
                4,
                goos.Select(x => x.Value.ToJson()));
        }
    }
}