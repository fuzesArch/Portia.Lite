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
        SetNodeTypesByIndex,

        [Category("Edges")]
        SetEdgeTypesByIndex,

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

        [Category("Nodes")]
        RemoveNodes,

        [Category("Edges")]
        RemoveEdges,

        [Category("Blossom")]
        Blossom,

        #if INTERNAL
        [Category("Graph")]
        Solve,

        [Category("Nodes")]
        AddNodesToEdges,

        [Category("Edges")]
        AddEdges,
        #endif

        [Category("AI")]
        AiResponse,
    }
}