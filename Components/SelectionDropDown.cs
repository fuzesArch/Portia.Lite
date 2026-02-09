//using Grasshopper.Kernel;
//using Grasshopper.Kernel.Parameters;
//using Portia.Infrastructure.Components;
//using Portia.Infrastructure.Core.DocStrings;
//using Portia.Infrastructure.Core.Helps;
//using Portia.Infrastructure.Core.Portia.Primitives;
//using Portia.Infrastructure.Core.Portia.Strategies;
//using Portia.Infrastructure.Core.Primitives;
//using Portia.Infrastructure.Core.Validators;
//using Portia.Lite.Core.Primitives;
//using Rhino.Geometry;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Portia.Lite.Components
//{
//    public class SelectionDropDown : AbsDropDownComponent<SelectionMode>
//    {
//        public SelectionDropDown()
//            : base(
//                nameof(Selection).AddDropDownMark(),
//                Docs.Selection.AddDropDownNote(),
//                Naming.Tab,
//                Naming.Tab)
//        {
//        }

//        protected override System.Drawing.Bitmap Icon =>
//            Properties.Resources.ColoredLogo;

//        public override Guid ComponentGuid =>
//            new("ac2c40f6-c85b-4400-ada2-a0f630590589");

//        private string name;

//        protected AbsSelection Selection;

//        protected override void AddInputFields()
//        {
//            InString(
//                    nameof(AbsSelection.Name),
//                    Docs.Name)
//                .InEnum(
//                    nameof(Gate),
//                    typeof(Gate).ToEnumString(),
//                    nameof(Gate.And))
//                .InStrings(
//                    nameof(LogicSelection.Logics),
//                    Docs.Logics);
//        }

//        protected override void AddOutputFields()
//        {
//            OutString(
//                nameof(Selection),
//                Docs.Selection);
//        }


//        protected override void CommonInputSetting(
//            IGH_DataAccess da)
//        {
//            if (!da.GetItem(
//                    0,
//                    out name))
//            {
//                exit = true;
//            }
//        }

//        protected override void CommonOutputSetting(
//            IGH_DataAccess da)
//        {
//            da.SetData(
//                0,
//                Selection.ToJson());
//        }

//        private void ByLogic(
//            IGH_DataAccess da)
//        {
//            int gateInt = da.GetOptionalItem(
//                1,
//                (int)AbsSelection.DefGate);

//            gateInt.ValidateEnum<Gate>();

//            if (!da.GetItems(
//                    2,
//                    out List<string> logicJsons))
//            {
//                return;
//            }

//            Selection = new LogicSelection
//            {
//                Name = name,
//                Logics = logicJsons.FromJson<IConstraint>().ToList(),
//                Gate = (Gate)gateInt
//            };
//        }

//        private void ByWrap(
//            IGH_DataAccess da)
//        {
//            if (!da.GetItems(
//                    1,
//                    out List<Brep> breps))
//            {
//                return;
//            }

//            bool strictlyIn = da.GetOptionalItem(
//                2,
//                ByWrapSelection.DefStrictlyIn);

//            Selection = new ByWrapSelection
//            {
//                Name = name, Wrappers = breps, StrictlyIn = strictlyIn
//            };

//            Selection.Guard();
//        }

//        private void ByIntersection(
//            IGH_DataAccess da)
//        {
//            if (!da.GetItems(
//                    1,
//                    out List<Brep> breps))
//            {
//                return;
//            }

//            Selection =
//                new ByIntersectionSelection { Name = name, Breps = breps, };
//        }

//        private void ByComposite(
//            IGH_DataAccess da)
//        {
//            int gateInt = da.GetOptionalItem(
//                1,
//                (int)ByCompositeSelection.DefGate);

//            gateInt.ValidateEnum<Gate>();

//            if (!da.GetItems(
//                    2,
//                    out List<string> selectionJsons))
//            {
//                return;
//            }

//            Selection = new ByCompositeSelection
//            {
//                Name = name,
//                Constraints = selectionJsons
//                    .FromJson<AbsSelection>()
//                    .ToList(),
//                Gate = (Gate)gateInt
//            };
//        }

//        private static ParameterConfig NameParameter() =>
//            new(
//                () => new Param_String(),
//                nameof(AbsSelection.Name),
//                Docs.Name.Add(Prefix.String),
//                GH_ParamAccess.item);

//        protected override Dictionary<SelectionMode, ParameterStrategy>
//            DefineParameterStrategy()
//        {
//            return new Dictionary<SelectionMode, ParameterStrategy>
//            {
//                {
//                    SelectionMode.ByLogic, new ParameterStrategy(
//                        new List<ParameterConfig>
//                        {
//                            NameParameter(),
//                            new(
//                                () => new Param_Integer(),
//                                nameof(LogicSelection.Gate),
//                                Docs
//                                    .Gate.ByDefault(AbsSelection.DefGate)
//                                    .Add(Prefix.Integer),
//                                GH_ParamAccess.item,
//                                listFactory: GateValueList.Create,
//                                isOptional: true),
//                            new(
//                                () => new Param_String(),
//                                nameof(LogicSelection.Logics),
//                                Docs.Logics.Add(Prefix.StringList),
//                                GH_ParamAccess.list),
//                        },
//                        ByLogic,
//                        Docs.ByLogicSelection)
//                },
//                {
//                    SelectionMode.ByWrap, new ParameterStrategy(
//                        new List<ParameterConfig>
//                        {
//                            NameParameter(),
//                            new(
//                                () => new Param_Geometry(),
//                                nameof(ByWrapSelection.Wrappers),
//                                Docs.Wrappers.Add(Prefix.GeometryList),
//                                GH_ParamAccess.list),
//                            new(
//                                () => new Param_Boolean(),
//                                nameof(ByWrapSelection.StrictlyIn),
//                                Docs.StrictlyIn.Add(Prefix.Boolean),
//                                GH_ParamAccess.item)
//                        },
//                        ByWrap,
//                        Docs.ByWrapSelection)
//                },
//                {
//                    SelectionMode.ByIntersection, new ParameterStrategy(
//                        new List<ParameterConfig>
//                        {
//                            NameParameter(),
//                            new(
//                                () => new Param_Geometry(),
//                                nameof(ByIntersectionSelection.Breps),
//                                Docs.Breps.Add(Prefix.GeometryList),
//                                GH_ParamAccess.list)
//                        },
//                        ByIntersection,
//                        Docs.ByIntersectionSelection)
//                },
//                {
//                    SelectionMode.ByComposite, new ParameterStrategy(
//                        new List<ParameterConfig>
//                        {
//                            NameParameter(),
//                            new(
//                                () => new Param_Integer(),
//                                nameof(ByCompositeSelection.Gate),
//                                Docs
//                                    .Gate.ByDefault(AbsSelection.DefGate)
//                                    .Add(Prefix.Integer),
//                                GH_ParamAccess.item,
//                                listFactory: GateValueList.Create,
//                                isOptional: true),
//                            new(
//                                () => new Param_String(),
//                                nameof(ByCompositeSelection.Constraints),
//                                Docs.Selections.Add(Prefix.JsonList),
//                                GH_ParamAccess.list),
//                        },
//                        ByComposite,
//                        Docs.ByCompositeSelection)
//                },
//            };
//        }
//    }
//}

