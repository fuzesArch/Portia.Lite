// ReSharper disable InconsistentNaming

namespace Portia.Lite.Core.Primitives
{
   public enum EdgeRuleMode
   {
      Composite,

      IndexRule,

      TypeRule,

      AllEdges,

      CurveLength,

      StartEndDistance,

      StartDegree,

      EndDegree,

      StartIndex,

      StartType,

      EndIndex,

      EndType,

      IsLinear,

      VectorSimilarity,

      IsInBrep,

      Intersection,

      IsLeafEdge,

      #if INTERNAL
      HasFeature,
      #endif
   }
}
