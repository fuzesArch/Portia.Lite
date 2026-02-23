using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphHelps;
using Portia.Infrastructure.GraphItems;
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
                Naming.Tab)
        {
        }

        public override Guid ComponentGuid =>
            new("2304d560-440a-46a0-851f-d149be62a048");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        protected override void AddInputFields()
        {
            InGeneric(
                "MainGraph",
                "The base Portia Graph to amalgamate into.");

            InStrings(
                "TargetRules",
                "Rules defining which Nodes in the MainGraph receive the Payload.");

            InStrings(
                "AnchorRules",
                "Rules defining the Anchor Node in the PayloadGraph.");

            InGeneric(
                "PayloadGraph",
                "The extra Portia Graph to append.");
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

            if (!da.GetItems(
                    2,
                    out List<string> anchorJsons))
            {
                return;
            }

            if (!da.GetItem(
                    3,
                    out GraphGoo payloadGoo) || payloadGoo?.Value == null)
            {
                return;
            }

            var targetRules = targetJsons.FromJson<IRule>().ToList();
            var anchorRules = anchorJsons.FromJson<IRule>().ToList();

            var amalgamateTask = new AmalgamateGraph(
                targetRules,
                anchorRules,
                payloadGoo.Value);

            amalgamateTask.Guard();

            GraphPipeline pipeline = new(new List<AbsTask> { amalgamateTask })
            {
                Graph = mainGoo.Value.Clone()
            };

            pipeline.Execute(
                da,
                this,
                amalgamateTask.Queries);

            Message = pipeline.Graph.ComponentMessage();
        }
    }
}