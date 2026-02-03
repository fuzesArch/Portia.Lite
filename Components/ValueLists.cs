using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.Primitives;
using System;
using static Rhino.Runtime.Notifications.Notification;

namespace Portia.Lite.Components
{
    public class DoubleRelationValueList : AbsValueList<DoubleRelation>
    {
        public DoubleRelationValueList()
            : base(
                nameof(DoubleRelation),
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
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new Guid("ee8ce3e3-013d-41c8-829e-8a160d8fb07a");

        public static StringRelationValueList Create() =>
            new StringRelationValueList();
    }

    public class LogicGateValueList : AbsValueList<LogicGate>
    {
        public LogicGateValueList()
            : base(
                nameof(LogicGate),
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new Guid("825ca3c1-851c-4e8a-a566-38f9e082c1ba");

        public static LogicGateValueList Create() => new LogicGateValueList();
    }

    public class SeverityValueList : AbsValueList<Severity>
    {
        public SeverityValueList()
            : base(
                nameof(Severity),
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new Guid("be3e5253-b902-4c2b-be91-21211a77d443");
    }
}