using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Features;
using Portia.Infrastructure.Features.Base;
using Portia.Infrastructure.Features.Implementations;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Validators;
using Portia.Lite.Core.Primitives;
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
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("c66e5e25-615b-45b9-8b0c-f0c89f0c4216");

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
            da.SetData(
                0,
                _feature.ToJson());
        }

        protected static ParameterConfig NameParam() =>
            new(
                () => new Param_String(),
                nameof(IFeature.Name),
                Docs.Name.ByDefault(FeatureName.Width).Add(Prefix.String),
                GH_ParamAccess.item,
                isOptional: true);

        protected ParameterSetup GenericSetup<TFeature, TValue>(
            ParameterConfig valueParameter,
            string defName)
            where TFeature : AbsFeature<TValue>, new()
        {
            return new ParameterSetup(
                new List<ParameterConfig> { NameParam(), valueParameter },
                da =>
                {
                    string name = da.GetOptionalItem(
                        0,
                        defName);

                    if (!da.GetItem(
                            1,
                            out TValue value))
                    {
                        return;
                    }

                    _feature = new TFeature { Name = name, Value = value };
                },
                Docs.Feature);
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
                        FeatureName.Width)
                },
                {
                    FeatureMode.String, GenericSetup<StringFeature, string>(
                        new ParameterConfig(
                            () => new Param_String(),
                            nameof(StringFeature.Value),
                            nameof(Prefix.String),
                            GH_ParamAccess.item),
                        FeatureName.GroupType)
                },
                {
                    FeatureMode.Boolean, GenericSetup<BooleanFeature, bool>(
                        new ParameterConfig(
                            () => new Param_Boolean(),
                            nameof(BooleanFeature.Value),
                            nameof(Prefix.Boolean),
                            GH_ParamAccess.item),
                        FeatureName.Active)
                },
                //{
                //    FeatureMode.Curve, new ParameterStrategy(
                //        new List<ParameterConfig>
                //        {
                //            new(
                //                () => new Param_String(),
                //                nameof(IFeature.Name),
                //                "Feature Key".Add(Prefix.String),
                //                GH_ParamAccess.item,
                //                isOptional: true),
                //            new(
                //                () => new Param_Curve(),
                //                nameof(CurveFeature.Value),
                //                "Boundary Curve".Add(Prefix.Curve),
                //                GH_ParamAccess.item)
                //        },
                //        ByCurve,
                //        "Curve Feature")
                //},
                //{
                //    FeatureMode.Profile, new ParameterStrategy(
                //        new List<ParameterConfig>
                //        {
                //            new(
                //                () => new Param_String(),
                //                nameof(IFeature.Name),
                //                "Feature Key".Add(Prefix.String),
                //                GH_ParamAccess.item,
                //                isOptional: true),
                //            new(
                //                () => new Param_Curve(),
                //                nameof(ProfileFeature.Value),
                //                "Profile Curve (1x1 Domain)".Add(Prefix.Curve),
                //                GH_ParamAccess.item)
                //        },
                //        ByProfile,
                //        "Profile Feature")
                //},
                //{
                //    FeatureMode.Constraint, new ParameterStrategy(
                //        new List<ParameterConfig>
                //        {
                //            new(
                //                () => new Param_String(),
                //                nameof(IFeature.Name),
                //                "Feature Key".Add(Prefix.String),
                //                GH_ParamAccess.item,
                //                isOptional: true),
                //            new(
                //                () => new Param_String(),
                //                nameof(ConstraintFeature.Value),
                //                "Target ID / Name".Add(Prefix.String),
                //                GH_ParamAccess.item),
                //            new(
                //                () => new Param_Number(),
                //                nameof(ConstraintFeature.TargetDistance),
                //                "Target Distance".Add(Prefix.Double),
                //                GH_ParamAccess.item)
                //        },
                //        ByConstraint,
                //        "Constraint Feature")
                //}
            };
        }
    }
}