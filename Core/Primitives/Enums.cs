using Portia.Infrastructure.Core.Primitives;

// ReSharper disable InconsistentNaming

namespace Portia.Lite.Core.Primitives
{
    public enum ConditionMode
    {
        Double,
        String,
        Vector,
        Boundary
    }

    public enum ConstraintMode
    {
        [Category("Nodes / Edges")]
        Composite,

        [Category("Nodes / Edges")]
        IndexConstraint,

        [Category("Nodes / Edges")]
        TypeConstraint,

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
        Node_AdjacentVectors,

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