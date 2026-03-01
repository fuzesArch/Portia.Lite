using Portia.Infrastructure.Primitives;

namespace Portia.Lite.Core.Primitives
{
    public enum TaskMode
    {
        [Category("Nodes")]
        SetNodeIndices,

        [Category("Edges")]
        SetEdgeIndices,

        [Category("Nodes")]
        SetNodeTypes,

        [Category("Edges")]
        SetEdgeTypes,

        [Category("Nodes")]
        SetNodeFeatures,

        [Category("Edges")]
        SetEdgeFeatures,

        [Category("Nodes")]
        FilterNodes,

        [Category("Edges")]
        FilterEdges,

        [Category("Nodes")]
        VerifyNodes,

        [Category("Edges")]
        VerifyEdges,

        #if INTERNAL
        [Category("Graph")]
        Solve,
        #endif
    }
}