namespace Portia.Lite.Core.Primitives
{
    public enum SelectionType
    {
        GraphIdentitySelection,
        LogicSelection,
        WrapSelection,
        IntersectionSelection,
        CompositeSelection,
    }

    public enum GraphIdentityCreationMode
    {
        Index,
        Type
    }

    public enum ConditionMode
    {
        DoubleCondition,
        StringCondition
    }

    public enum NodeLogicType
    {
        NodeAdjacencyLogic,
        NodeProximityLogic,
        NodeVectorSumLogic,
        IsLeafNodeLogic,
        JointConstellationLogic,
    }

    public enum EdgeLogicType
    {
        EdgeLengthLogic,
        SourceAdjacencyLogic,
        TargetAdjacencyLogic,
        IsBridgeEdgeLogic,
        LinkConstellationLogic
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