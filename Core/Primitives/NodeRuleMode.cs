// ReSharper disable InconsistentNaming

namespace Portia.Lite.Core.Primitives
{
   public enum NodeRuleMode
   {
      Composite,

      IndexRule,

      TypeRule,

      AllNodes,

      Degree,

      AdjacentEdgeType,

      Proximity,

      AdjacentEdgeVectorSimilarity,

      InBrep,

      #if INTERNAL
      HasFeature,
      #endif
   }
}
