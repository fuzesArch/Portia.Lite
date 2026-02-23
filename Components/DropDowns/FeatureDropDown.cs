using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Features;
using Portia.Infrastructure.Features.Base;
using Portia.Infrastructure.Features.Implementations.Items;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Validators;
using Portia.Lite.Core.Primitives;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace Portia.Lite.Components.DropDowns
{
    public class FeatureDropDown : AbsDropDownComponent<FeatureMode>
    {
        public FeatureDropDown()
            : base(
                nameof(IFeature).Substring(1),
                Docs.Feature,
                Naming.Tab,
                Naming.Logic)
        {
        }

        public override Guid ComponentGuid =>
            new("c66e5e25-615b-45b9-8b0c-f0c89f0c4216");

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        private IFeature _feature;

        protected override void AddOutputFields()
        {
            OutJson(
                nameof(IFeature).Substring(1),
                Docs.Feature);
        }

        protected override void CommonOutputSetting(
            IGH_DataAccess da)
        {
            _feature.Guard();

            if (FeatureName.Outputs.Contains(_feature.Name))
            {
                AddRuntimeMessage(
                    GH_RuntimeMessageLevel.Error,
                    $"Access Denied: '{_feature.Name}' is a reserved Solver Output " +
                    $"and cannot be injected manually.");
                return;
            }

            da.SetData(
                0,
                _feature.ToJson());

            Message = GetFeatureDoc(CurrentMode);
        }

        protected static ParameterConfig NameParam() =>
            new(
                () => new Param_String(),
                nameof(IFeature.Name),
                Docs.Name.ByDefault(FeatureName.EdgeWidth).Add(Prefix.String),
                GH_ParamAccess.item,
                isOptional: true);

        protected ParameterSetup GenericSetup<TFeature, TValue>(
            ParameterConfig valueParameter,
            string description)
            where TFeature : AbsFeature<TValue>, new()
        {
            return new ParameterSetup(
                new List<ParameterConfig> { NameParam(), valueParameter },
                da =>
                {
                    if (!da.GetItem(
                            0,
                            out string name))
                    {
                        return;
                    }

                    if (!da.GetItem(
                            1,
                            out TValue value))
                    {
                        return;
                    }

                    _feature = new TFeature { Name = name, Value = value };
                },
                description);
        }

        protected override Dictionary<FeatureMode, ParameterSetup> DefineSetup()
        {
            return new Dictionary<FeatureMode, ParameterSetup>
            {
                {
                    FeatureMode.Numeric, GenericSetup<NumericFeature, double>(
                        new ParameterConfig(
                            () => new Param_Number(),
                            nameof(NumericFeature.Value),
                            nameof(Prefix.Double),
                            GH_ParamAccess.item),
                        Docs.NumericFeature)
                },
                {
                    FeatureMode.String, GenericSetup<StringFeature, string>(
                        new ParameterConfig(
                            () => new Param_String(),
                            nameof(StringFeature.Value),
                            nameof(Prefix.String),
                            GH_ParamAccess.item),
                        Docs.StringFeature)
                },
                {
                    FeatureMode.Boolean, GenericSetup<BooleanFeature, bool>(
                        new ParameterConfig(
                            () => new Param_Boolean(),
                            nameof(BooleanFeature.Value),
                            nameof(Prefix.Boolean),
                            GH_ParamAccess.item),
                        Docs.BooleanFeature)
                },
                {
                    FeatureMode.Geometry,
                    GenericSetup<GeometryFeature, GeometryBase>(
                        new ParameterConfig(
                            () => new Param_Geometry(),
                            nameof(GeometryFeature.Value),
                            nameof(Prefix.Geometry),
                            GH_ParamAccess.item),
                        Docs.GeometryFeature)
                }
            };
        }

        private static string GetFeatureDoc(
            FeatureMode mode)
        {
            return mode switch
            {
                FeatureMode.Numeric => Docs.NumericFeature,
                FeatureMode.String => Docs.StringFeature,
                FeatureMode.Boolean => Docs.BooleanFeature,
                FeatureMode.Geometry => Docs.GeometryFeature,
                _ => Docs.FeatureName
            };
        }
    }
}