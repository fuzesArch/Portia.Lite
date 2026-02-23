using Portia.Infrastructure.Components;
using Portia.Infrastructure.Primitives.Enums;
using System;

namespace Portia.Lite.Components.ValueLists
{
    public class StringRelationValueList : AbsValueList<StringRelation>
    {
        public StringRelationValueList()
            : base(
                nameof(StringRelation),
                Docs.StringRelation,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new Guid("ee8ce3e3-013d-41c8-829e-8a160d8fb07a");

        public static StringRelationValueList Create() =>
            new StringRelationValueList();
    }
}