using Portia.Infrastructure.Primitives;

namespace Portia.Lite.Core.Primitives
{
   public enum TaskMode
   {
      SetNodeTypes,
      SetEdgeTypes,
      SetEdgeTypesInOrder,
      FilterNodes,
      FilterEdges,

      #if INTERNAL
      [Category("Nodes")] SetNodeIndices,

      [Category("Edges")] SetEdgeIndices,

      [Category("Nodes")] SetNodeFeatures,

      [Category("Edges")] SetEdgeFeatures,

      [Category("Nodes")] VerifyNodes,

      [Category("Edges")] VerifyEdges,

      [Category("Nodes")] RemoveNodes,

      [Category("Edges")] RemoveEdges,

      [Category("Graph")] Solve,

      [Category("Blossom")] Blossom,

      [Category("Nodes")] AddNodesToEdges,

      [Category("Edges")] AddEdges,

      [Category("AI")] AiResponse,

      #endif

      // fixed numbers: after the INTERNAL block their position differs between builds, and the
      // saved mode is a number
      [Category("Layers")] BuildGridFeatures = 100,

      [Category("Layers")] BuildPattern = 101,

      [Category("Layers")] BuildModels = 102,
   }
}
