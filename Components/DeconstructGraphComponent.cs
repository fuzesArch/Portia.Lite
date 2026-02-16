using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Portia.Helps;
using Portia.Infrastructure.Core.Portia.Main;
using Portia.Infrastructure.Core.Portia.Natives;
using System;

namespace Portia.Lite.Components
{
    public class DeconstructGraphComponent : GenericBase
    {
        public DeconstructGraphComponent()
            : base(
                nameof(DeconstructGraphComponent)
                    .Substring(
                        0,
                        16),
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
                nameof(Graph),
                nameof(Graph),
                Docs.PortiaGraph,
                GH_ParamAccess.item);
        }

        protected override void AddOutputFields()
        {
            OutGenerics(
                    nameof(Graph.Nodes),
                    Docs.GraphItem)
                .OutGenerics(
                    nameof(Graph.Edges),
                    Docs.GraphItem)
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