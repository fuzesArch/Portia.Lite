using Portia.Infrastructure.Components;
using Portia.Infrastructure.Primitives.Enums;
using System;

namespace Portia.Lite.Components.ValueLists
{
    public class GateValueList : AbsValueList<Gate>
    {
        public GateValueList()
            : base(
                nameof(Gate),
                Docs.Gate,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new Guid("825ca3c1-851c-4e8a-a566-38f9e082c1ba");

        public static GateValueList Create() => new GateValueList();
    }
}