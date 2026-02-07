using Portia.Infrastructure.Core.Primitives;

namespace Portia.Lite.Core.Primitives
{
    public enum SelectionMode
    {
        ByLogicSelection,
        ByWrapSelection,
        ByIntersectionSelection,
        ByCompositeSelection,
    }

    public enum ConditionMode
    {
        DoubleCondition,
        StringCondition
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
        NodeProximity,

        [Category("Nodes")]
        NodeVectorSum,

        [Category("Nodes")]
        NodeIsLeaf,

        [Category("Nodes")]
        NodeSimilarity,

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
        EdgeIsBridge,

        [Category("Edges")]
        EdgeLinkConstellationLogic
    }

    public enum TaskType
    {
        Task_SetCurves,
        Task_SetNodeTypes,
        Task_SetEdgeTypes,
        Task_GetNodes,
        Task_GetEdges,
        Task_VerifyNodes,
        Task_VerifyEdges,
    }
}