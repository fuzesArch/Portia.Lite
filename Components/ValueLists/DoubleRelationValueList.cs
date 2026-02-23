using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Primitives.Enums;
using System;

namespace Portia.Lite.Components.ValueLists
{
    public class DoubleRelationValueList : AbsValueList<NumericRelation>
    {
        public DoubleRelationValueList()
            : base(
                nameof(NumericRelation),
                Docs.NumericRelation,
                Naming.Tab,
                Naming.Primitives)
        {
        }

        public override Guid ComponentGuid =>
            new("7b4666d4-b72e-46ec-8f55-94dc196b5c07");

        public static DoubleRelationValueList Create() => new();

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}