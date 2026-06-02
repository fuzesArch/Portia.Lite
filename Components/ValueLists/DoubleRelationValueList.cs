using Portia.Infrastructure.Components.ValueLists;
using System;

namespace Portia.Lite.Components.ValueLists
{
    public class DoubleRelationValueList : AbsDoubleRelationValueList
    {
        public DoubleRelationValueList()
            : base(
                Naming.Tab,
                Naming.Primitives)
        {
        }

        public override Guid ComponentGuid =>
            new("7b4666d4-b72e-46ec-8f55-94dc196b5c07");

        public static DoubleRelationValueList Create() => new();
    }
}