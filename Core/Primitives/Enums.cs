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
        IndexLogic,
        TypeLogic,
        NodeAdjacencyLogic,
        NodeProximityLogic,
        NodeVectorSumLogic,
        NodeIsLeafLogic,
        NodeConstellationLogic,
        EdgeLengthLogic,
        EdgeSourceAdjacencyLogic,
        EdgeTargetAdjacencyLogic,
        EdgeIsBridgeLogic,
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