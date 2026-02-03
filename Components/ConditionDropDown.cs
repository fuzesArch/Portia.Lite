using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Primitives;
using System;
using System.Collections.Generic;

namespace Portia.Lite.Components
{
    public class ConditionDropDown : DropDownComponent<ConditionMode>
    {
        public ConditionDropDown()
            : base(
                nameof(AbsCondition).Substring(3).AddDropDownMark(),
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new Guid("b5e22506-0e6f-4987-9600-bd21e962ab52");

        private AbsCondition condition;

        protected override void AddInputFields()
        {
            InEnum(
                    nameof(DoubleRelation),
                    typeof(DoubleRelation).ToEnumString(),
                    DoubleCondition.DefRelation.ToString())
                .InDouble(
                    nameof(DoubleCondition.Value),
                    "");

            SetInputParameterOptionality(0);
            SetEnumDropDown<DoubleRelation>(0);
        }

        protected override void AddOutputFields()
        {
            OutString(
                nameof(AbsCondition).Substring(3),
                "");
        }

        protected override void CommonOutputSetting(
            IGH_DataAccess da)
        {
            da.SetData(
                0,
                condition.ToJson());
        }

        private void SolveByDouble(
            IGH_DataAccess da)
        {
            int relationInteger = da.GetOptionalItem(
                0,
                (int)DoubleCondition.DefRelation);

            relationInteger.ValidateEnum<DoubleRelation>();

            if (!da.GetItem(
                    1,
                    out double value))
            {
                return;
            }

            condition = new DoubleCondition(
                (DoubleRelation)relationInteger,
                value);
        }

        private void SolveByString(
            IGH_DataAccess da)
        {
            int relationInteger = da.GetOptionalItem(
                0,
                (int)StringCondition.DefRelation);

            relationInteger.ValidateEnum<StringRelation>();

            if (!da.GetItem(
                    1,
                    out string value))
            {
                return;
            }

            condition = new StringCondition(
                (StringRelation)relationInteger,
                value);
        }

        protected override Dictionary<ConditionMode, ParameterStrategy>
            DefineParameterStrategy()
        {
            return new Dictionary<ConditionMode, ParameterStrategy>
            {
                {
                    ConditionMode.DoubleCondition, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_Integer(),
                                nameof(DoubleRelation),
                                "",
                                GH_ParamAccess.item,
                                DoubleRelationValueList.Create),
                            new(
                                () => new Param_Number(),
                                nameof(DoubleCondition.Value),
                                "",
                                GH_ParamAccess.item)
                        },
                        SolveByDouble)
                },
                {
                    ConditionMode.StringCondition, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_Integer(),
                                nameof(StringRelation),
                                "",
                                GH_ParamAccess.item,
                                StringRelationValueList.Create),
                            new(
                                () => new Param_String(),
                                nameof(StringCondition.Value),
                                "",
                                GH_ParamAccess.item)
                        },
                        SolveByString)
                }
            };
        }
    }
}