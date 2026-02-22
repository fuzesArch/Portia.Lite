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
        StartCap,

        [Category("Output")]
        EndCap,

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
    }
}