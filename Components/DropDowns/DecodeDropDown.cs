using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Features.Base;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphHelps;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Solvers;
using Portia.Lite.Components.Goo;
using Portia.Lite.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components.DropDowns
{
    public class DecodeDropDown : AbsDropDownComponent<DecodeMode>
    {
        public DecodeDropDown()
            : base(
                nameof(Docs.Decode),
                Docs.Decode,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("3f8622b2-9671-4164-a69b-cf0ea482b774");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        private GraphGoo _graphGoo;
        private List<GraphItemGoo> _itemGoos;
        private List<FeatureGoo> _featureGoos;
        private List<EdgeJunctionGoo> _edgeJunctionGoos;

        protected override void AddOutputFields()
        {
            switch (CurrentMode)
            {
                case DecodeMode.Graph:
                    OutGenerics(
                            nameof(Docs.GraphNodeGoo),
                            Docs.GraphNodeGoo)
                        .OutGenerics(
                            nameof(Docs.GraphEdgeGoo),
                            Docs.GraphEdgeGoo);
                    break;

                case DecodeMode.Node:
                case DecodeMode.Edge:
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
                            Docs.FeatureGoo);
                    break;

                case DecodeMode.Feature:
                    OutStrings(
                            nameof(IFeature.Name),
                            Docs.Name)
                        .OutGenerics(
                            nameof(AbsFeature<double>.Value),
                            Docs.FeatureValue);
                    break;

                case DecodeMode.EdgeJunction:
                    OutGenerics(
                            nameof(EdgeJunction.StartCap),
                            Docs.StartCap)
                        .OutGenerics(
                            nameof(EdgeJunction.EndCap),
                            Docs.EndCap)
                        .OutGenerics(
                            nameof(EdgeJunction.LeftSweep),
                            Docs.LeftSweep)
                        .OutGenerics(
                            nameof(EdgeJunction.RightSweep),
                            Docs.RightSweep)
                        .OutStrings(
                            nameof(EdgeJunction.StartRankState),
                            Docs.StartState)
                        .OutStrings(
                            nameof(EdgeJunction.EndRankState),
                            Docs.EndState)
                        .OutGenerics(
                            nameof(EdgeJunction.Boundary),
                            Docs.EdgeJunctionBoundary);
                    break;
            }
        }

        private void SetGraphOutputs(
            IGH_DataAccess da)
        {
            if (_graphGoo?.Value == null)
            {
                return;
            }

            var graph = _graphGoo.Value;

            da.SetDataList(
                0,
                graph.Nodes.ToGoo());

            da.SetDataList(
                1,
                graph.Edges.ToGoo());
        }

        private void SetItemOutputs(
            IGH_DataAccess da)
        {
            if (_itemGoos == null || !_itemGoos.Any())
            {
                return;
            }

            da.SetDataList(
                0,
                _itemGoos.Select(x => x?.GetGeometries()));

            da.SetDataList(
                1,
                _itemGoos.Select(x => x?.Value?.GraphIdentity.Index));

            da.SetDataList(
                2,
                _itemGoos.Select(x => x?.Value?.GraphIdentity.Type));

            var featureTree = new DataTree<FeatureGoo>();

            for (int i = 0; i < _itemGoos.Count; i++)
            {
                var path = new GH_Path(
                    da.Iteration,
                    i);
                if (_itemGoos[i]?.Value?.FeatureSet == null)
                {
                    continue;
                }

                var features = _itemGoos[i].Value.FeatureSet.GetAll();
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
        }

        private void SetFeatureOutputs(
            IGH_DataAccess da)
        {
            if (_featureGoos == null || !_featureGoos.Any())
            {
                return;
            }

            da.SetDataList(
                0,
                _featureGoos.Select(x => x?.Value?.Name));

            da.SetDataList(
                1,
                _featureGoos.Select(x =>
                {
                    if (x?.Value == null)
                    {
                        return null;
                    }

                    dynamic dynFeature = x.Value;
                    return dynFeature.Value;
                }));
        }

        private void SetEdgeJunctionOutputs(
            IGH_DataAccess da)
        {
            if (_edgeJunctionGoos == null || !_edgeJunctionGoos.Any())
            {
                return;
            }

            da.SetDataList(
                0,
                _edgeJunctionGoos.Select(x => x?.Value?.StartCap));

            da.SetDataList(
                1,
                _edgeJunctionGoos.Select(x => x?.Value?.EndCap));

            da.SetDataList(
                2,
                _edgeJunctionGoos.Select(x => x?.Value?.LeftSweep));

            da.SetDataList(
                3,
                _edgeJunctionGoos.Select(x => x?.Value?.RightSweep));

            da.SetDataList(
                4,
                _edgeJunctionGoos.Select(x => x?.Value?.StartRankState));

            da.SetDataList(
                5,
                _edgeJunctionGoos.Select(x => x?.Value?.EndRankState));

            da.SetDataList(
                6,
                _edgeJunctionGoos.Select(x => x?.Value?.Boundary));
        }

        protected override void CommonOutputSetting(
            IGH_DataAccess da)
        {
            switch (CurrentMode)
            {
                case DecodeMode.Graph:
                    SetGraphOutputs(da);
                    break;

                case DecodeMode.Node:
                case DecodeMode.Edge:
                    SetItemOutputs(da);
                    break;

                case DecodeMode.Feature:
                    SetFeatureOutputs(da);
                    break;

                case DecodeMode.EdgeJunction:
                    SetEdgeJunctionOutputs(da);
                    break;
            }
        }


        private void ByGraph(
            IGH_DataAccess da)
        {
            da.GetItem(
                0,
                out _graphGoo);
        }

        private void ByItem(
            IGH_DataAccess da)
        {
            da.GetItems(
                0,
                out _itemGoos);
        }

        private void ByFeature(
            IGH_DataAccess da)
        {
            da.GetItems(
                0,
                out _featureGoos);
        }

        private void ByEdgeJunction(
            IGH_DataAccess da)
        {
            da.GetItems(
                0,
                out _edgeJunctionGoos);
        }

        protected override Dictionary<DecodeMode, ParameterSetup> DefineSetup()
        {
            return new Dictionary<DecodeMode, ParameterSetup>
            {
                {
                    DecodeMode.Graph, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new GraphParameter(),
                                nameof(GraphGoo),
                                Docs.GraphGoo.Add(Prefix.GraphGoo),
                                GH_ParamAccess.item)
                        },
                        ByGraph,
                        Docs.DeconstructGraph)
                },
                {
                    DecodeMode.Node, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new GraphItemGooParameter(),
                                nameof(Docs.GraphNodeGoo),
                                Docs.GraphNodeGoo.Add(Prefix.GraphNodeGoo),
                                GH_ParamAccess.list)
                        },
                        ByItem,
                        Docs.DeconstructItem)
                },
                {
                    DecodeMode.Edge, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new GraphItemGooParameter(),
                                nameof(Docs.GraphEdgeGoo),
                                Docs.GraphEdgeGoo.Add(Prefix.GraphEdgeGoo),
                                GH_ParamAccess.list)
                        },
                        ByItem,
                        Docs.DeconstructItem)
                },
                {
                    DecodeMode.Feature, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new FeatureGooParameter(),
                                nameof(Docs.FeatureGoo),
                                Docs.FeatureGoo.Add(Prefix.FeatureGoo),
                                GH_ParamAccess.list)
                        },
                        ByFeature,
                        Docs.DeconstructFeature)
                },
                {
                    DecodeMode.EdgeJunction, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new EdgeJunctionGooParameter(),
                                nameof(Docs.EdgeJunctionGoo),
                                Docs.EdgeJunctionGoo.Add(
                                    Prefix.EdgeJunctionGoo),
                                GH_ParamAccess.list)
                        },
                        ByEdgeJunction,
                        Docs.EdgeJunctionGoo)
                }
            };
        }
    }
}