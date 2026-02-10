using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.DocStrings;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Primitives;
using Portia.Lite.Core.Primitives;
using Rhino.Geometry;
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
            Properties.Resources.ColoredLogo;

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

        private void ByDouble(
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

        private void ByVectorAngle(
            IGH_DataAccess da)
        {
            if (!da.GetItem(
                    0,
                    out Vector3d vector))
            {
                return;
            }

            double tolerance = da.GetOptionalItem(
                1,
                VectorCondition.DefAngleTolerance);

            bool bidirectional = da.GetOptionalItem(
                2,
                VectorCondition.DefBidirectional);

            conditionJson = new VectorCondition(
                vector,
                tolerance,
                bidirectional).ToJson();
        }

        private void ByString(
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

        private void ByBoundary(
            IGH_DataAccess da)
        {
            if (!da.GetItem(
                    0,
                    out Brep boundary))
            {
                return;
            }

            bool strictlyIn = da.GetOptionalItem(
                1,
                BoundaryCondition.DefStrictlyIn);

            conditionJson = new BoundaryCondition(
                boundary,
                strictlyIn).ToJson();
        }

        protected override Dictionary<ConditionMode, ParameterStrategy>
            DefineParameterStrategy()
        {
            return new Dictionary<ConditionMode, ParameterStrategy>
            {
                {
                    ConditionMode.Double, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_Integer(),
                                nameof(DoubleRelation),
                                Docs.DoubleRelation.Add(Prefix.Integer),
                                GH_ParamAccess.item,
                                listFactory: DoubleRelationValueList.Create),
                            new(
                                () => new Param_Number(),
                                nameof(Docs.DoubleValue),
                                Docs.DoubleValue.Add(Prefix.Double),
                                GH_ParamAccess.item)
                        },
                        ByDouble,
                        Docs.DoubleCondition)
                },
                {
                    ConditionMode.String, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_Integer(),
                                nameof(StringRelation),
                                Docs.StringRelation.Add(Prefix.Integer),
                                GH_ParamAccess.item,
                                listFactory: StringRelationValueList.Create),
                            new(
                                () => new Param_String(),
                                nameof(Docs.StringValue),
                                Docs.StringValue.Add(Prefix.String),
                                GH_ParamAccess.item)
                        },
                        ByString,
                        Docs.StringCondition)
                },
                {
                    ConditionMode.Vector, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_Vector(),
                                nameof(VectorCondition.Vector),
                                Docs.Vector.Add(Prefix.Vector),
                                GH_ParamAccess.item),
                            new(
                                () => new Param_Number(),
                                nameof(VectorCondition.AngleTolerance),
                                Docs
                                    .AngleTolerance.ByDefault(
                                        VectorCondition.DefAngleTolerance)
                                    .Add(Prefix.Double),
                                GH_ParamAccess.item,
                                isOptional: true),
                            new(
                                () => new Param_Boolean(),
                                nameof(Docs.Bidirectional),
                                Docs
                                    .Bidirectional.ByDefault(
                                        VectorCondition.DefBidirectional)
                                    .Add(Prefix.Boolean),
                                GH_ParamAccess.item,
                                isOptional: true)
                        },
                        ByVectorAngle,
                        Docs.VectorCondition)
                },
                {
                    ConditionMode.Boundary, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_Brep(),
                                nameof(BoundaryCondition.Boundary),
                                Docs.Boundary.Add(Prefix.Brep),
                                GH_ParamAccess.item),
                            new(
                                () => new Param_Boolean(),
                                nameof(BoundaryCondition.StrictlyIn),
                                Docs
                                    .StrictlyIn.ByDefault(
                                        BoundaryCondition.DefStrictlyIn)
                                    .Add(Prefix.Boolean),
                                GH_ParamAccess.item,
                                isOptional: true)
                        },
                        ByBoundary,
                        Docs.BoundaryCondition)
                },
            };
        }
    }
}