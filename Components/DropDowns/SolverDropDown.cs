using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Solvers;
using Portia.Infrastructure.Validators;
using Portia.Lite.Core.Primitives;
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
                        Docs.Solve)
                }
                #endif
            };
        }
    }
}