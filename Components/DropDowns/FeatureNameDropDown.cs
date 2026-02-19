using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Features;
using Portia.Lite.Core.Primitives;
using System;
using System.Collections.Generic;

namespace Portia.Lite.Components.DropDowns
{
    public class FeatureNameDropDown : AbsDropDownComponent<FeatureNameMode>
    {
        public FeatureNameDropDown()
            : base(
                nameof(FeatureName),
                Docs.FeatureName,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new Guid("7dffeaf1-c72d-4f9d-b40a-be3fcfa7c3b0");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        protected override void AddOutputFields()
        {
            OutString(
                nameof(FeatureName),
                Docs.FeatureName);
        }

        protected override void CommonOutputSetting(
            IGH_DataAccess da)
        {
            string outputKey = CurrentMode switch
            {
                FeatureNameMode.Width => FeatureName.Width,
                FeatureNameMode.Rank => FeatureName.Rank,
                _ => CurrentMode.ToString()
            };

            da.SetData(
                0,
                outputKey);
        }

        protected override Dictionary<FeatureNameMode, ParameterSetup>
            DefineSetup()
        {
            var dict = new Dictionary<FeatureNameMode, ParameterSetup>();

            foreach (FeatureNameMode mode in Enum.GetValues(
                         typeof(FeatureNameMode)))
            {
                dict.Add(
                    mode,
                    new ParameterSetup(
                        new List<ParameterConfig>(),
                        _ =>
                        {
                        },
                        Docs.FeatureName));
            }

            return dict;
        }
    }
}