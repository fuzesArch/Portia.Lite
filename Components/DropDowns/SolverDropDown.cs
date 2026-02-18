using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Features;
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

        protected override void AddInputFields()
        {
            InString(
                nameof(FeatureKey.Width),
                Docs.Name.ByDefault(nameof(FeatureKey.Width)).Add(Prefix.Json),
                nameof(FeatureKey.Width));

            SetInputParameterOptionality(0);
        }

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

        private void ByJunction(
            IGH_DataAccess da)
        {
            string widthKey = da.GetOptionalItem(
                0,
                nameof(FeatureKey.Width));

            _solver = new JunctionSolver { WidthKey = widthKey };
        }

        protected override Dictionary<SolverMode, ParameterSetup> DefineSetup()
        {
            return new Dictionary<SolverMode, ParameterSetup>
            {
                {
                    SolverMode.Junction, new ParameterSetup(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_String(),
                                nameof(JunctionSolver.WidthKey),
                                Docs
                                    .Name.ByDefault(nameof(FeatureKey.Width))
                                    .Add(Prefix.String),
                                GH_ParamAccess.item,
                                isOptional: true)
                        },
                        ByJunction,
                        Docs.Solve)
                }
            };
        }
    }
}