using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Validators;
using Portia.Lite.Core.Primitives;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
#if INTERNAL
using Portia.Infrastructure.Solvers.Base;
using Portia.Infrastructure.Solvers.BoundarySolving;
using Portia.Infrastructure.Solvers.SectorSolving;
using Portia.Infrastructure.Solvers.SpotSolving;
using Portia.Infrastructure.Solvers.ZoneSolving;
#endif

namespace Portia.Lite.Components.DropDowns
{
    #if INTERNAL
    public class SolverDropDown : AbsDropDownComponent<SolverMode>
    {
        public SolverDropDown()
            : base(
                nameof(ISolver).Substring(1),
                Docs.Solve,
                Naming.Tab,
                Naming.Logic)
        {
        }

        public override Guid ComponentGuid =>
            new("443022d6-25c3-4960-9f71-9b3b68fb8cf9");

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        private ISolver _solver;

        protected override void AddOutputFields()
        {
            OutJson(
                nameof(ISolver).Substring(1),
                Docs.Solve);
        }

        protected override void CommonOutputSetting(
            IGH_DataAccess da)
        {
            _solver.Guard();

            da.SetData(
                0,
                _solver.ToJson());
        }


        private void BySpot(
            IGH_DataAccess da)
        {
            var gridLines = da.GetOptionalItems<Curve>(0);
            var voidLines = da.GetOptionalItems<Curve>(1);
            var roomLines = da.GetOptionalItems<Curve>(2);

            _solver = new SpotSolver(
                gridLines,
                voidLines,
                roomLines);
        }

        private void ByZone(
            IGH_DataAccess da)
        {
            var demands = new List<ZoneDemand>();

            if (da.GetItems(
                    0,
                    out List<string> demandJsons))
            {
                demands = demandJsons.FromJson<ZoneDemand>().ToList();
            }

            _solver = new ZoneSolver { ZoneDemands = demands };
        }


        protected override Dictionary<SolverMode, ParameterSetup> DefineSetup()
        {
            return new Dictionary<SolverMode, ParameterSetup>
            {
                {
                    SolverMode.Boundary, new ParameterSetup(
                        new List<ParameterConfig>(),
                        _ =>
                        {
                            _solver = new BoundarySolver();
                        },
                        Docs.BoundarySolver)
                },
                {
                    SolverMode.Spot, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_Curve(),
                                nameof(Docs.GridCurves),
                                Docs.GridCurves.Add(Prefix.CurveList),
                                GH_ParamAccess.list,
                                isOptional: true),
                            new(
                                () => new Param_Curve(),
                                nameof(Docs.VoidCurves),
                                Docs.VoidCurves.Add(Prefix.CurveList),
                                GH_ParamAccess.list,
                                isOptional: true),
                            new(
                                () => new Param_Curve(),
                                nameof(Docs.RoomCurves),
                                Docs.RoomCurves.Add(Prefix.CurveList),
                                GH_ParamAccess.list,
                                isOptional: true)
                        },
                        BySpot,
                        Docs.SpotSolver)
                },
                {
                    SolverMode.Sector, new ParameterSetup(
                        new List<ParameterConfig>(),
                        _ =>
                        {
                            _solver = new SectorSolver();
                        },
                        Docs.SectorSolver)
                },
                {
                    SolverMode.Zone, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_String(),
                                nameof(ZoneSolver.ZoneDemands),
                                Docs.ZoneDemand.Add(Prefix.JsonList),
                                GH_ParamAccess.list)
                        },
                        ByZone,
                        Docs.ZoneSolver)
                }
            };
        }
    }
    #endif
}