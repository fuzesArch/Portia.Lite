using Portia.Infrastructure.Components;
using Portia.Infrastructure.Primitives;
using System;

namespace Portia.Lite.Components
{
    public class DoubleRelationValueList : AbsValueList<NumericRelation>
    {
        public DoubleRelationValueList()
            : base(
                nameof(NumericRelation),
                Docs.NumericRelation,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new Guid("7b4666d4-b72e-46ec-8f55-94dc196b5c07");

        public static DoubleRelationValueList Create() =>
            new DoubleRelationValueList();
    }

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