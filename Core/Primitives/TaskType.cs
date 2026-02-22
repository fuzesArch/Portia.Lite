using Portia.Infrastructure.Primitives;

namespace Portia.Lite.Core.Primitives
{
    public enum TaskType
    {
        [Category("Setup Graph")]
        SetGraphByCurves,

        [Category("Setup Graph")]
        LoadGraph,

        [Category("Setup Graph")]
        AmalgamateGraph,

        [Category("Setup")]
        SetNodeIndices,

        [Category("Setup")]
        SetEdgeIndices,

        [Category("Setup")]
        SetNodeTypes,

        [Category("Setup")]
        SetEdgeTypes,

        [Category("Setup")]
        SetNodeFeatures,

        [Category("Setup")]
        SetEdgeFeatures,

        [Category("Filter")]
        FilterNodes,

        [Category("Filter")]
        FilterEdges,

        [Category("Validate")]
        VerifyNodes,

        [Category("Validate")]
        VerifyEdges,

        [Category("Manipulate")]
        Solve,
    }
}