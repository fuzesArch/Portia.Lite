using Portia.Infrastructure.Core.Primitives;

namespace Portia.Lite.Core.Primitives
{
    public enum SelectionMode
    {
        ByLogic,
        ByWrap,
        ByIntersection,
        ByComposite,
    }

    public enum ConditionMode
    {
        DoubleCondition,
        StringCondition,
        VectorCondition
    }

    public enum LogicType
    {
        [Category("Nodes / Edges")]
        Index,

        [Category("Nodes / Edges")]
        Type,

        [Category("Nodes")]
        NodeAdjacency,

        [Category("Nodes")]
        NodeAdjacentEdgeType,

        [Category("Nodes")]
        NodeProximity,

        [Category("Nodes")]
        NodeVectorSum,

        [Category("Nodes")]
        NodeIsLeaf,

        [Category("Nodes")]
        NodeAdjacentVectors,

        [Category("Edges")]
        EdgeLength,

        [Category("Edges")]
        EdgeSourceAdjacency,

        [Category("Edges")]
        EdgeTargetAdjacency,

        [Category("Edges")]
        EdgeSourceIndex,

        [Category("Edges")]
        EdgeSourceType,

        [Category("Edges")]
        EdgeTargetIndex,

        [Category("Edges")]
        EdgeTargetType,

        [Category("Edges")]
        EdgeIsLinear,

        [Category("Edges")]
        EdgeVectorSimilarity
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