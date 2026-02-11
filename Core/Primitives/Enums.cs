using Portia.Infrastructure.Core.Primitives;

// ReSharper disable InconsistentNaming

namespace Portia.Lite.Core.Primitives
{
    public enum ConditionMode
    {
        Numeric,
        String,
        Vector,
        Boundary
    }

    public enum RuleMode
    {
        [Category("Nodes / Edges")]
        Composite,

        [Category("Nodes / Edges")]
        IndexRule,

        [Category("Nodes / Edges")]
        TypeRule,

        [Category("Nodes")]
        Node_Adjacency,

        [Category("Nodes")]
        Node_AdjacentEdgeType,

        [Category("Nodes")]
        Node_Proximity,

        [Category("Nodes")]
        Node_VectorSum,

        [Category("Nodes")]
        Node_IsLeaf,

        [Category("Nodes")]
        Node_AdjacentVectorSimilarity,

        [Category("Nodes")]
        Node_InBoundary,

        [Category("Edges")]
        Edge_Length,

        [Category("Edges")]
        Edge_StartAdjacency,

        [Category("Edges")]
        Edge_EndAdjacency,

        [Category("Edges")]
        Edge_StartIndex,

        [Category("Edges")]
        Edge_StartType,

        [Category("Edges")]
        Edge_EndIndex,

        [Category("Edges")]
        Edge_EndType,

        [Category("Edges")]
        Edge_IsLinear,

        [Category("Edges")]
        Edge_VectorSimilarity
    }

    public enum TaskType
    {
        SetCurves,
        SetNodeTypes,
        SetEdgeTypes,
        GetNodes,
        GetEdges,
        VerifyNodes,
        VerifyEdges,
    }
}