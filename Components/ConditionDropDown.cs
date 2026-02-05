using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Primitives;
using Portia.Lite.Core.Primitives;
using System;
using System.Collections.Generic;

namespace Portia.Lite.Components
{
    public class ConditionDropDown : AbsDropDownComponent<ConditionMode>
    {
        public ConditionDropDown()
            : base(
                nameof(DoubleCondition).Substring(6).AddDropDownMark(),
                Docs.Condition.AddDropDownNote(),
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("b5e22506-0e6f-4987-9600-bd21e962ab52");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        private string conditionJson;

        protected override void AddInputFields()
        {
            InEnum(
                    nameof(DoubleRelation),
                    Docs.DoubleRelation + Environment.NewLine +
                    typeof(DoubleRelation).ToEnumString(),
                    DoubleCondition.DefRelation.ToString())
                .InDouble(
                    nameof(DoubleCondition.Value),
                    Docs.DoubleValue);

            SetInputParameterOptionality(0);
            SetEnumDropDown<DoubleRelation>(0);
        }

        protected override void AddOutputFields()
        {
            OutJson(
                nameof(DoubleCondition),
                Docs.Condition);
        }

        protected override void CommonOutputSetting(
            IGH_DataAccess da)
        {
            da.SetData(
                0,
                conditionJson);
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

            conditionJson = new DoubleCondition(
                (DoubleRelation)relationInteger,
                value).ToJson();
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

            conditionJson = new StringCondition(
                (StringRelation)relationInteger,
                value).ToJson();
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
                                Docs.DoubleRelation,
                                GH_ParamAccess.item,
                                DoubleRelationValueList.Create),
                            new(
                                () => new Param_Number(),
                                nameof(Docs.DoubleValue),
                                Docs.DoubleValue,
                                GH_ParamAccess.item)
                        },
                        SolveByDouble,
                        Docs.DoubleCondition)
                },
                {
                    ConditionMode.StringCondition, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_Integer(),
                                nameof(StringRelation),
                                Docs.StringRelation,
                                GH_ParamAccess.item,
                                StringRelationValueList.Create),
                            new(
                                () => new Param_String(),
                                nameof(Docs.StringValue),
                                Docs.StringValue,
                                GH_ParamAccess.item)
                        },
                        SolveByString,
                        Docs.StringCondition)
                }
            };
        }
    }
}