using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Portia.Main;
using Portia.Infrastructure.Core.Portia.Primitives;
using Portia.Infrastructure.Core.Portia.Strategies;
using Portia.Infrastructure.Core.Primitives;
using Portia.Lite.Core.Primitives;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components
{
    public class SelectionDropDown : AbsDropDownComponent<SelectionType>
    {
        public SelectionDropDown()
            : base(
                nameof(Selection).AddDropDownMark(),
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("ac2c40f6-c85b-4400-ada2-a0f630590589");

        private string name;

        protected AbsSelection Selection;

        protected override void AddInputFields()
        {
            InString(
                    nameof(AbsSelection.Name),
                    "")
                .InStrings(
                    nameof(GraphIdentity) + "s",
                    "");
        }

        protected override void AddOutputFields()
        {
            OutString(
                nameof(Selection),
                "");
        }


        protected override void CommonInputSetting(
            IGH_DataAccess da)
        {
            if (!da.GetItem(
                    0,
                    out name))
            {
                exit = true;
            }
        }

        protected override void CommonOutputSetting(
            IGH_DataAccess da)
        {
            da.SetData(
                0,
                Selection.ToJson());
        }

        private void SolveByGraphIdentity(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    1,
                    out List<string> jsons))
            {
                return;
            }

            Selection = new GraphIdentitySelection
            {
                Name = name,
                GraphIdentities = jsons.FromJson<GraphIdentity>().ToList()
            };
        }

        private void SolveByRule(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    1,
                    out List<string> logicJsons))
            {
                return;
            }

            int gateInt = da.GetOptionalItem(
                2,
                (int)LogicSelection.DefLogicGate);

            gateInt.ValidateEnum<LogicGate>();

            Selection = new LogicSelection
            {
                Name = name,
                Logics = logicJsons.FromJson<IGraphLogic>().ToList(),
                LogicGate = (LogicGate)gateInt
            };
        }

        private void SolveByWrap(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    1,
                    out List<Brep> breps))
            {
                return;
            }

            bool strictlyIn = da.GetOptionalItem(
                2,
                WrapSelection.DefStrictlyIn);

            Selection = new WrapSelection
            {
                Name = name, Wrappers = breps, StrictlyIn = strictlyIn
            };
        }

        private void SolveByIntersection(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    1,
                    out List<Brep> breps))
            {
                return;
            }

            Selection =
                new IntersectionSelection { Name = name, Breps = breps, };
        }

        private void SolveByComposite(
            IGH_DataAccess da)
        {
            if (!da.GetItems(
                    1,
                    out List<string> selectionJsons))
            {
                return;
            }

            int gateInt = da.GetOptionalItem(
                2,
                (int)CompositeSelection.DefLogicGate);

            gateInt.ValidateEnum<LogicGate>();

            Selection = new CompositeSelection
            {
                Name = name,
                Selections = selectionJsons
                    .FromJson<AbsSelection>()
                    .ToList(),
                LogicGate = (LogicGate)gateInt
            };
        }


        private static ParameterConfig NameParameter() =>
            new(
                () => new Param_String(),
                nameof(AbsSelection.Name),
                "",
                GH_ParamAccess.item);

        protected override Dictionary<SelectionType, ParameterStrategy>
            DefineParameterStrategy()
        {
            return new Dictionary<SelectionType, ParameterStrategy>
            {
                {
                    SelectionType.GraphIdentitySelection, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NameParameter(),
                            new(
                                () => new Param_String(),
                                nameof(GraphIdentity) + "s",
                                "",
                                GH_ParamAccess.list)
                        },
                        SolveByGraphIdentity)
                },
                {
                    SelectionType.LogicSelection, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NameParameter(),
                            new(
                                () => new Param_String(),
                                nameof(LogicSelection.Logics),
                                "",
                                GH_ParamAccess.list),
                            new(
                                () => new Param_Integer(),
                                nameof(LogicSelection.LogicGate),
                                "",
                                GH_ParamAccess.item,
                                LogicGateValueList.Create)
                        },
                        SolveByRule)
                },
                {
                    SelectionType.WrapSelection, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NameParameter(),
                            new(
                                () => new Param_Geometry(),
                                nameof(WrapSelection.Wrappers),
                                "",
                                GH_ParamAccess.list),
                            new(
                                () => new Param_Boolean(),
                                nameof(WrapSelection.StrictlyIn),
                                "",
                                GH_ParamAccess.item)
                        },
                        SolveByWrap)
                },
                {
                    SelectionType.IntersectionSelection, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NameParameter(),
                            new(
                                () => new Param_Geometry(),
                                nameof(IntersectionSelection.Breps),
                                "",
                                GH_ParamAccess.list)
                        },
                        SolveByIntersection)
                },
                {
                    SelectionType.CompositeSelection, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            NameParameter(),
                            new(
                                () => new Param_String(),
                                nameof(CompositeSelection.Selections),
                                "",
                                GH_ParamAccess.list),
                            new(
                                () => new Param_Integer(),
                                nameof(CompositeSelection.LogicGate),
                                "",
                                GH_ParamAccess.item,
                                LogicGateValueList.Create)
                        },
                        SolveByComposite)
                },
            };
        }
    }
}