using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphHelps;
using Portia.Infrastructure.Helps;
using Portia.Lite.Components.Goo;
using System;

namespace Portia.Lite.Components.Decode
{
    public class DecodeGraphComponent : GenericBase
    {
        public DecodeGraphComponent()
            : base(
                nameof(DecodeGraphComponent)
                    .Substring(
                        0,
                        11),
                Docs.DeconstructGraph,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("b646255d-cd59-4d64-a0e3-5f26f365ffbe");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        protected override void RegisterInputParams(
            GH_InputParamManager pManager)
        {
            pManager.AddParameter(
                new GraphParameter(),
                nameof(GraphGoo),
                nameof(GraphGoo),
                Docs.GraphGoo,
                GH_ParamAccess.item);
        }

        protected override void AddOutputFields()
        {
            OutGenerics(
                    nameof(Docs.GraphNodeGoo),
                    Docs.GraphNodeGoo)
                .OutGenerics(
                    nameof(Docs.GraphEdgeGoo),
                    Docs.GraphEdgeGoo)
                .OutJson(
                    nameof(Docs.Json),
                    Docs.Json);
        }

        protected override void SolveInstance(
            IGH_DataAccess da)
        {
            if (!da.GetItem(
                    0,
                    out GraphGoo goo) || goo?.Value == null)
            {
                return;
            }

            var graph = goo.Value;

            da.SetDataList(
                0,
                graph.Nodes.ToGoo());

            da.SetDataList(
                1,
                graph.Edges.ToGoo());

            da.SetData(
                2,
                graph.ToJson());
        }
    }
}