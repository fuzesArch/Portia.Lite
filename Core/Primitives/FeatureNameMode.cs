using Portia.Infrastructure.Primitives;

namespace Portia.Lite.Core.Primitives
{
    public enum FeatureNameMode
    {
        [Category("Input")]
        EdgeWidth,

        [Category("Input")]
        EdgeRank,

        [Category("Input")]
        EdgeGrid,

        [Category("Output")]
        EdgeStartCap,

        [Category("Output")]
        EdgeEndCap,

        [Category("Output")]
        EdgeBoundary,

        [Category("Output")]
        StartRankState,

        [Category("Output")]
        EndRankState,

        [Category("Output")]
        SpotPoints,

        [Category("Output")]
        SpotParameters,

        [Category("Output")]
        SpotTypes,

        [Category("Output")]
        SectorBoundaries,

        [Category("Output")]
        SectorLines,

        [Category("Output")]
        ZoneCategories,
    }
}