using Eto.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Solvers.Base;
using Portia.Infrastructure.Solvers.JunctionSolving;
using Portia.Infrastructure.Solvers.SpotSolving;
using Portia.Infrastructure.Validators;
using Portia.Lite.Core.Primitives;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace Portia.Lite.Components.DropDowns
{
    public class SolverDropDown : AbsDropDownComponent<SolverMode>
    {
        public SolverDropDown()
            : base(
                nameof(ISolver).Substring(1),
                Docs.Solve,
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("443022d6-25c3-4960-9f71-9b3b68fb8cf9");

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

        #if INTERNAL
        private void ByJunction(
            IGH_DataAccess da)
        {
            _solver = new JunctionSolver();
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

        #endif

        protected override Dictionary<SolverMode, ParameterSetup> DefineSetup()
        {
            return new Dictionary<SolverMode, ParameterSetup>
            {
                #if INTERNAL
                {
                    SolverMode.Junction, new ParameterSetup(
                        new List<ParameterConfig>(),
                        ByJunction,
                        Docs.JunctionSolver)
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
                }

                #endif
            };
        }
    }
}