using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.Helps;
using System.Linq;
using Portia.Infrastructure.Main;

namespace Portia.Lite.Components.Goo
{
    public class DeconstructItemComponent : GenericBase
    {
        public DeconstructItemComponent()
            : base(
                nameof(DeconstructItemComponent)
                    .Substring(
                        0,
                        11),
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
                new GraphItemParameter(),
                nameof(GraphItem),
                nameof(GraphItem),
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

            da.SetDataList(
                3,
                goos.Select(x => x.Value.ToJson()));
        }
    }
}