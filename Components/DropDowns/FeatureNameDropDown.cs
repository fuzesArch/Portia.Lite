using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Features;
using Portia.Lite.Core.Primitives;
using System;
using System.Collections.Generic;

namespace Portia.Lite.Components.DropDowns
{
    #if INTERNAL
    public class FeatureNameDropDown : AbsDropDownComponent<FeatureNameMode>
    {
        public FeatureNameDropDown()
            : base(
                nameof(FeatureName),
                Docs.FeatureName,
                Naming.Tab,
                Naming.Logic)
        {
        }

        public override Guid ComponentGuid =>
            new("7dffeaf1-c72d-4f9d-b40a-be3fcfa7c3b0");

        public override GH_Exposure Exposure => GH_Exposure.primary;

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
            da.SetData(
                0,
                CurrentMode.ToString());

            Message = GetFeatureDoc(CurrentMode);
        }

        protected override Dictionary<FeatureNameMode, ParameterSetup>
            DefineSetup()
        {
            var setups = new Dictionary<FeatureNameMode, ParameterSetup>();

            foreach (FeatureNameMode mode in Enum.GetValues(
                         typeof(FeatureNameMode)))
            {
                setups.Add(
                    mode,
                    new ParameterSetup(
                        new List<ParameterConfig>(),
                        _ =>
                        {
                        },
                        GetFeatureDoc(mode)));
            }

            return setups;
        }

        private static string GetFeatureDoc(
            FeatureNameMode mode)
        {
            return mode switch
            {
                FeatureNameMode.EdgeWidth => Docs.EdgeWidth,
                FeatureNameMode.EdgeRank => Docs.EdgeRank,
                FeatureNameMode.EdgeGrid => Docs.EdgeGrid,
                FeatureNameMode.EdgeStartCap => Docs.EdgeStartCap,
                FeatureNameMode.EdgeEndCap => Docs.EdgeEndCap,
                FeatureNameMode.EdgeBoundary => Docs.EdgeBoundary,
                FeatureNameMode.StartRankState => Docs.StartRankState,
                FeatureNameMode.EndRankState => Docs.EndRankState,
                FeatureNameMode.SpotPoints => Docs.SpotPoints,
                FeatureNameMode.SpotParameters => Docs.SpotParameters,
                FeatureNameMode.SpotTypes => Docs.SpotTypes,
                FeatureNameMode.SectorBoundaries => Docs.SectorBoundaries,
                FeatureNameMode.SectorLines => Docs.SectorLines,
                FeatureNameMode.ZoneCategories => Docs.ZoneCategories,

                _ => Docs.FeatureName
            };
        }
    }
    #endif
}