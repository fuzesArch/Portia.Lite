using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphHelps;
using Portia.Infrastructure.Graphs;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Rules.Base;
using Portia.Infrastructure.Tasks.Base;
using Portia.Infrastructure.Tasks.GraphSetting;
using Portia.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components.Main
{
    public class AmalgamateComponent : GenericBase
    {
        public AmalgamateComponent()
            : base(
                nameof(Docs.Amalgamate),
                Docs.Amalgamate,
                Naming.Tab,
                Naming.Graph)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override Guid ComponentGuid =>
            new("2304d560-440a-46a0-851f-d149be62a048");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        protected override void AddInputFields()
        {
            InGeneric(
                nameof(Docs.GraphGoo),
                Docs.GraphGoo);

            InStrings(
                nameof(Docs.TargetNodeRules),
                Docs.TargetNodeRules);

            InGeneric(
                nameof(Docs.PayloadGraphGoo),
                Docs.PayloadGraphGoo);

            InStrings(
                nameof(Docs.AnchorNodeRules),
                Docs.AnchorNodeRules);
        }

        protected override void AddOutputFields()
        {
            new AmalgamateGraph().RegisterOutputs(Params);
        }

        protected override void Solve(
            IGH_DataAccess da)
        {
            if (!da.GetItem(
                    0,
                    out GraphGoo mainGoo) || mainGoo?.Value == null)
            {
                return;
            }

            if (!da.GetItems(
                    1,
                    out List<string> targetJsons))
            {
                return;
            }


            if (!da.GetItem(
                    2,
                    out GraphGoo payloadGoo) || payloadGoo?.Value == null)
            {
                return;
            }

            if (!da.GetItems(
                    3,
                    out List<string> anchorJsons))
            {
                return;
            }


            var targetRules = targetJsons.FromJson<IRule>().ToList();
            var anchorRules = anchorJsons.FromJson<IRule>().ToList();

            var task = new AmalgamateGraph(
                targetRules,
                anchorRules,
                payloadGoo.Value);

            task.Guard();

            GraphPipeline pipeline = new(new List<AbsTask> { task })
            {
                Graph = mainGoo.Value.Clone()
            };

            pipeline.Execute(
                da,
                this,
                task.Queries);

            Message = pipeline.Graph.ComponentMessage();
        }
    }
}