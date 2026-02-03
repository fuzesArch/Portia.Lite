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
        Tag
    }

    public enum ConditionMode
    {
        DoubleCondition,
        StringCondition
    }

    public enum NodeLogicType
    {
        NodeAdjacencyRule,
        NodeProximityRule,
        NodeVectorSumRule,
        IsLeafNodeRule,
        JointConstellation,
    }

    public enum EdgeLogicType
    {
        EdgeLengthRule,
        SourceAdjacencyRule,
        TargetAdjacencyRule,
        IsBridgeEdgeRule,
        LinkConstellation
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