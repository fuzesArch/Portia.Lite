using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Helps;
using System;

#if INTERNAL
using Portia.Infrastructure.Solvers.ZoneSolving;
#endif

namespace Portia.Lite.Components.SolverRelated
{
    #if INTERNAL
    public class ZoneDemandComponent : GenericBase
    {
        public override Guid ComponentGuid =>
            new("da7104f0-942d-41d4-906e-d8d0b4554daf");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        public ZoneDemandComponent()
            : base(
                nameof(ZoneDemand),
                Docs.ZoneDemand,
                Naming.Tab,
                Naming.SolverPrimitives)
        {
        }

        protected override void AddInputFields()
        {
            InInteger(
                    nameof(ZoneDemand.Category),
                    Docs.ZoneCategory)
                .InDouble(
                    nameof(ZoneDemand.TargetArea),
                    Docs.ZoneTargetArea)
                .InDouble(
                    nameof(ZoneDemand.Tolerance),
                    Docs.ZoneTolerance,
                    ZoneDemand.DefTolerance);

            SetInputParameterOptionality(2);
        }

        protected override void AddOutputFields()
        {
            OutString(
                nameof(ZoneDemand),
                Docs.ZoneDemand);
        }

        protected override void Solve(
            IGH_DataAccess da)
        {
            int category = 0;
            double targetArea = 0.0;

            if (!da.GetData(
                    0,
                    ref category))
            {
                return;
            }

            if (!da.GetData(
                    1,
                    ref targetArea))
            {
                return;
            }

            double tolerance = da.GetOptionalItem(
                2,
                ZoneDemand.DefTolerance);

            var demand = new ZoneDemand(
                category,
                targetArea,
                tolerance);

            da.SetData(
                0,
                demand.ToJson());
        }
    }
    #endif
}