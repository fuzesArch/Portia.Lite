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
        [Category("Composite")]
        Composite,

        [Category("Universal: Nodes / Edges")]
        IndexRule,

        [Category("Universal: Nodes / Edges")]
        TypeRule,

        [Category("Nodes")]
        Node_Degree,

        [Category("Nodes")]
        Node_AdjacentEdgeType,

        [Category("Nodes")]
        Node_Proximity,

        [Category("Nodes")]
        Node_VectorSum,

        [Category("Nodes")]
        Node_IsLeaf,

        [Category("Nodes")]
        Node_AdjacentEdgeVectorSimilarity,

        [Category("Nodes")]
        Node_InBoundary,

        [Category("Edges")]
        Edge_CurveLength,

        [Category("Edges")]
        Edge_StartEndDistance,

        [Category("Edges")]
        Edge_StartDegree,

        [Category("Edges")]
        Edge_EndDegree,

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
        Edge_VectorSimilarity,

        [Category("Edges")]
        Edge_InBoundary,
    }

    public enum TaskType
    {
        [Category("Set graph")]
        SetGraphByCurves,

        [Category("Set graph")]
        LoadGraph,

        [Category("Set indices")]
        SetNodeIndices,

        [Category("Set indices")]
        SetEdgeIndices,

        [Category("Set types")]
        SetNodeTypes,

        [Category("Set types")]
        SetEdgeTypes,

        [Category("Amalgamate")]
        AmalgamateGraph,

        [Category("Filter")]
        FilterNodes,

        [Category("Filter")]
        FilterEdges,

        [Category("Verify")]
        VerifyNodes,

        [Category("Verify")]
        VerifyEdges,

        [Category("Adapt")]
        Adapt,
    }
}