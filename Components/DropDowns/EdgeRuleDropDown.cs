using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Rules.BooleanBased;
using Portia.Infrastructure.Rules.BoundaryBased;
using Portia.Infrastructure.Rules.Composite;
using Portia.Infrastructure.Rules.IntersectionBased;
using Portia.Infrastructure.Rules.Numeric;
using Portia.Infrastructure.Rules.StringBased;
using Portia.Infrastructure.Rules.StringCollectionBased;
using Portia.Infrastructure.Rules.VectorBased;
using Portia.Lite.Core.Primitives;
using System;
using System.Collections.Generic;

namespace Portia.Lite.Components.DropDowns
{
   public class
         EdgeRuleDropDown : AbsRuleDropDown<EdgeRuleMode>
   {
      public EdgeRuleDropDown() : base(
         nameof(EdgeRuleDropDown)
              .Substring(0,
                  8))
      { }

      public override Guid ComponentGuid =>
            new("f4e8b2a1-6c39-4d57-8a1e-2b9f0c3d5e71");

      protected override
            Dictionary<EdgeRuleMode, ParameterSetup>
            DefineSetup()
      {
         return new Dictionary<EdgeRuleMode, ParameterSetup>
         {
            {
               EdgeRuleMode.IndexRule,
               NumericSetup<IndexRule>(Docs.IndexRule)
            },
            {
               EdgeRuleMode.TypeRule,
               StringSetup<TypeRule>(Docs.TypeRule)
            },
            {
               EdgeRuleMode.Composite,
               CompositeSetup<CompositeRule>(
                  Docs.CompositeRule)
            },
            {
               EdgeRuleMode.AllEdges,
               BooleanSetup<AllEdgesRule>(Docs.AllEdgesRule)
            },
            {
               EdgeRuleMode.CurveLength,
               NumericSetup<EdgeCurveLengthRule>(
                  Docs.EdgeCurveLength)
            },
            {
               EdgeRuleMode.StartEndDistance,
               NumericSetup<EdgeStartEndDistanceRule>(
                  Docs.EdgeStartEndDistance)
            },
            {
               EdgeRuleMode.StartDegree,
               NumericSetup<EdgeStartDegreeRule>(
                  Docs.StartDegree)
            },
            {
               EdgeRuleMode.EndDegree,
               NumericSetup<EdgeEndDegreeRule>(
                  Docs.EndDegree)
            },
            {
               EdgeRuleMode.StartIndex,
               NumericSetup<EdgeStartIndexRule>(
                  Docs.EdgeStartIndex)
            },
            {
               EdgeRuleMode.StartType,
               StringSetup<EdgeStartTypeRule>(
                  Docs.EdgeStartType)
            },
            {
               EdgeRuleMode.EndIndex,
               NumericSetup<EdgeEndIndexRule>(
                  Docs.EdgeEndIndex)
            },
            {
               EdgeRuleMode.EndType,
               StringSetup<EdgeEndTypeRule>(
                  Docs.EdgeEndType)
            },
            {
               EdgeRuleMode.IsLinear,
               BooleanSetup<IsLinearEdgeRule>(
                  Docs.IsLinearRule)
            },
            {
               EdgeRuleMode.IsLeafEdge,
               BooleanSetup<IsLeafEdgeRule>(Docs.IsLeafEdge)
            },
            {
               EdgeRuleMode.VectorSimilarity,
               VectorSetup<EdgeVectorSimilarityRule>(
                  Docs.EdgeSimilarity)
            },
            {
               EdgeRuleMode.IsInBrep,
               BoundarySetup<EdgeInBoundaryRule>(
                  Docs.EdgeInBoundary)
            },
            {
               EdgeRuleMode.HasIntersection,
               IntersectionSetup<EdgeIntersectionRule>(
                  Docs.EdgeIntersection)
            },

            #if INTERNAL
            {
               EdgeRuleMode.HasFeature,
               StringCollectionSetup<HasFeatureRule>(
                  Docs.HasFeatureRule)
            },
            #endif
         };
      }
   }
}
