using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Features.Base;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphHelps;
using Portia.Infrastructure.Helps;
using Portia.Lite.Components.Goo;
using Portia.Lite.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components.DropDowns
{
    public class UnpackDropDown : AbsDropDownComponent<UnpackMode>
    {
        public UnpackDropDown()
            : base(
                nameof(Docs.Unpack),
                Docs.Unpack,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("3f8622b2-9671-4164-a69b-cf0ea482b774");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        private GraphGoo _graphGoo;
        private GraphItemGoo _itemGoo;
        private FeatureGoo _featureGoo;

        protected override void AddOutputFields()
        {
            switch (CurrentMode)
            {
                case UnpackMode.Graph:
                    OutGenerics(
                            nameof(Docs.GraphNodeGoo),
                            Docs.GraphNodeGoo)
                        .OutGenerics(
                            nameof(Docs.GraphEdgeGoo),
                            Docs.GraphEdgeGoo);
                    break;

                case UnpackMode.Item:
                    OutGenerics(
                            nameof(Docs.Geometries),
                            Docs.Geometries)
                        .OutIntegers(
                            nameof(Docs.Index) + "es",
                            Docs.Identity)
                        .OutStrings(
                            nameof(Docs.Type) + "s",
                            Docs.Type)
                        .OutGenerics(
                            nameof(Docs.FeatureGoo) + "s",
                            Docs.FeatureGoo);
                    break;

                case UnpackMode.Feature:
                    OutStrings(
                            nameof(IFeature.Name),
                            Docs.Name)
                        .OutGenerics(
                            nameof(AbsFeature<double>.Value),
                            Docs.FeatureValue);
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
            if (_itemGoo?.Value == null)
            {
                return;
            }

            da.SetData(
                0,
                _itemGoo.GetGeometries());

            da.SetData(
                1,
                _itemGoo.Value.GraphIdentity.Index);

            da.SetData(
                2,
                _itemGoo.Value.GraphIdentity.Type);

            var features = _itemGoo.Value.FeatureSet.GetAll();

            da.SetDataList(
                3,
                features.Select(f => new FeatureGoo(f)));
        }

        private void SetFeatureOutputs(
            IGH_DataAccess da)
        {
            if (_featureGoo?.Value == null)
            {
                return;
            }

            da.SetData(
                0,
                _featureGoo.Value.Name);

            dynamic dynFeature = _featureGoo.Value;
            var val = dynFeature.Value;

            if (val is System.Collections.IEnumerable list && val is not string)
            {
                da.SetDataList(
                    1,
                    list);
            }
            else
            {
                da.SetData(
                    1,
                    val);
            }
        }

        protected override void CommonOutputSetting(
            IGH_DataAccess da)
        {
            switch (CurrentMode)
            {
                case UnpackMode.Graph: SetGraphOutputs(da); break;
                case UnpackMode.Item: SetItemOutputs(da); break;
                case UnpackMode.Feature: SetFeatureOutputs(da); break;
            }
        }

        protected override Dictionary<UnpackMode, ParameterSetup> DefineSetup()
        {
            return new Dictionary<UnpackMode, ParameterSetup>
            {
                {
                    UnpackMode.Graph, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new GraphParameter(),
                                nameof(GraphGoo),
                                Docs.GraphGoo.Add(Prefix.GraphGoo),
                                GH_ParamAccess.item)
                        },
                        da => da.GetItem(
                            0,
                            out _graphGoo),
                        Docs.UnpackGraph)
                },
                {
                    UnpackMode.Item, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new GraphItemGooParameter(),
                                nameof(Docs.GraphItemGoo),
                                Docs.GraphItemGoo.Add(Prefix.GraphItemGoo),
                                GH_ParamAccess.item)
                        },
                        da => da.GetItem(
                            0,
                            out _itemGoo),
                        Docs.UnpackItem)
                },
                {
                    UnpackMode.Feature, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new FeatureGooParameter(),
                                nameof(Docs.FeatureGoo),
                                Docs.FeatureGoo.Add(Prefix.FeatureGoo),
                                GH_ParamAccess.item)
                        },
                        da => da.GetItem(
                            0,
                            out _featureGoo),
                        Docs.UnpackFeature)
                }
            };
        }
    }
}