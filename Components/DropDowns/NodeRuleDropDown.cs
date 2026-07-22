using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Rules.BooleanBased;
using Portia.Infrastructure.Rules.BoundaryBased;
using Portia.Infrastructure.Rules.Composite;
using Portia.Infrastructure.Rules.Numeric;
using Portia.Infrastructure.Rules.StringBased;
using Portia.Infrastructure.Rules.StringCollectionBased;
using Portia.Infrastructure.Rules.VectorCollectionBased;
using Portia.Lite.Core.Primitives;
using System;
using System.Collections.Generic;

namespace Portia.Lite.Components.DropDowns
{
   public class
         NodeRuleDropDown : AbsRuleDropDown<NodeRuleMode>
   {
      public NodeRuleDropDown() : base(
         nameof(NodeRuleDropDown)
              .Substring(0,
                  8))
      { }

      public override Guid ComponentGuid =>
            new("a1c47d9e-3b62-4f18-9d05-7e2c6b1a4f30");

      protected override
            Dictionary<NodeRuleMode, ParameterSetup>
            DefineSetup()
      {
         return new Dictionary<NodeRuleMode, ParameterSetup>
         {
            {
               NodeRuleMode.IndexRule,
               NumericSetup<IndexRule>(Docs.IndexRule)
            },
            {
               NodeRuleMode.TypeRule,
               StringSetup<TypeRule>(Docs.TypeRule)
            },
            {
               NodeRuleMode.Composite,
               CompositeSetup<CompositeRule>(
                  Docs.CompositeRule)
            },
            {
               NodeRuleMode.AllNodes,
               BooleanSetup<AllNodesRule>(Docs.AllNodesRule)
            },
            {
               NodeRuleMode.Degree,
               NumericSetup<NodeDegreeRule>(
                  Docs.NodeAdjacency)
            },
            {
               NodeRuleMode.AdjacentEdgeType,
               StringCollectionSetup<
                  NodeAdjacentEdgeTypeRule>(
                  Docs.NodeAdjacentEdgeType)
            },
            {
               NodeRuleMode.Proximity,
               NumericSetup<NodeProximityRule>(
                  Docs.NodeProximity)
            },
            {
               NodeRuleMode.AdjacentEdgeVectorSimilarity,
               VectorCollectionSetup<
                  NodeAdjacentEdgeVectorSimilarityRule>(Docs
                    .NodeAdjacentEdgeVectorSimilarity)
            },
            {
               NodeRuleMode.InBrep,
               BoundarySetup<NodeInBoundaryRule>(
                  Docs.NodeInBoundary)
            },
            #if INTERNAL
            {
               NodeRuleMode.HasFeature,
               StringCollectionSetup<HasFeatureRule>(
                  Docs.HasFeatureRule)
            },
            #endif
         };
      }
   }
}
