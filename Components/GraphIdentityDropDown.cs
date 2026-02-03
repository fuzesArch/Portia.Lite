using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Core.Helps;
using Portia.Infrastructure.Core.Portia.Main;
using Portia.Lite.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components
{
    public class GraphIdentityDropDown
        : AbsDropDownComponent<GraphIdentityCreationMode>
    {
        public GraphIdentityDropDown()
            : base(
                nameof(GraphIdentities).AddDropDownMark(),
                Docs.GraphIdentity.AddDropDownNote(),
                Naming.Tab,
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("fb60709c-48fb-43f9-85d2-f49dac289551");

        protected List<GraphIdentity> GraphIdentities;

        protected override void AddInputFields()
        {
            InIntegers(
                nameof(GraphIdentity.Index) + "es",
                Docs.Index);
        }

        protected override void AddOutputFields()
        {
            OutJsons(
                nameof(GraphIdentities),
                Docs.GraphIdentity);
        }

        protected override void CommonOutputSetting(
            IGH_DataAccess da)
        {
            da.SetDataList(
                0,
                GraphIdentities.Select(x => x.ToJson()));
        }

        private void SolveBy<T>(
            IGH_DataAccess da,
            Func<List<T>, IEnumerable<GraphIdentity>> factory)
        {
            if (da.GetItems(
                    0,
                    out List<T> values))
            {
                GraphIdentities = factory(values).ToList();
            }
        }

        protected override
            Dictionary<GraphIdentityCreationMode, ParameterStrategy>
            DefineParameterStrategy()
        {
            return new Dictionary<GraphIdentityCreationMode, ParameterStrategy>
            {
                {
                    GraphIdentityCreationMode.Index, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_Integer(),
                                nameof(GraphIdentity.Index) + "es",
                                Docs.Index,
                                GH_ParamAccess.list)
                        },
                        da => SolveBy<int>(
                            da,
                            indexes => indexes.Select(GraphIdentity.ByIndex)),
                        Docs.GraphIdentityByIndex)
                },
                {
                    GraphIdentityCreationMode.Type, new ParameterStrategy(
                        new List<ParameterConfig>
                        {
                            new(
                                () => new Param_String(),
                                nameof(GraphIdentity.Type) + "s",
                                Docs.Type,
                                GH_ParamAccess.list)
                        },
                        da => SolveBy<string>(
                            da,
                            tags => tags.Select(GraphIdentity.ByTag)),
                        Docs.GraphIdentityByTag)
                }
            };
        }
    }
}