using Portia.Infrastructure.Primitives;

// ReSharper disable InconsistentNaming

namespace Portia.Lite.Core.Primitives
{
    public enum RuleMode
    {
        [Category("Universal: Nodes / Edges")]
        AllItems,

        [Category("Universal: Nodes / Edges")]
        IndexRule,

        [Category("Universal: Nodes / Edges")]
        TypeRule,

        [Category("Universal: Nodes / Edges")]
        Composite,

        [Category("Universal: Nodes / Edges")]
        HasFeature,

        [Category("Nodes")]
        AllNodes,

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
        AllEdges,

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
}