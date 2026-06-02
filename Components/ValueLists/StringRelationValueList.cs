using Portia.Infrastructure.Components.ValueLists;
using System;

namespace Portia.Lite.Components.ValueLists
{
    public class StringRelationValueList : AbsStringRelationValueList
    {
        public StringRelationValueList()
            : base(
                Naming.Tab,
                Naming.Primitives)
        {
        }

        public static StringRelationValueList Create() => new();

        public override Guid ComponentGuid =>
            new("ee8ce3e3-013d-41c8-829e-8a160d8fb07a");
    }
}