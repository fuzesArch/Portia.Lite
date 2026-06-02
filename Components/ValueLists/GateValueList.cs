using Portia.Infrastructure.Components.ValueLists;
using System;

namespace Portia.Lite.Components.ValueLists
{
    public class GateValueList : AbsGateValueList
    {
        public GateValueList()
            : base(
                Naming.Tab,
                Naming.Primitives)
        {
        }

        public override Guid ComponentGuid =>
            new("825ca3c1-851c-4e8a-a566-38f9e082c1ba");

        public static GateValueList Create() => new();
    }
}