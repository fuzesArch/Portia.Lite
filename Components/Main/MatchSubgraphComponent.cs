using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphHelps;
using Portia.Infrastructure.Graphs;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Rules.Base;
using Portia.Infrastructure.Tasks.Base;
using Portia.Infrastructure.Tasks.Isomorphism;
using Portia.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components.Main
{
    public class MatchSubgraphComponent : GenericBase
    {
        public MatchSubgraphComponent()
            : base(
                nameof(MatchSubGraph),
                Docs.MatchSubGraph,
                Naming.Tab,
                Naming.Graph)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        public override Guid ComponentGuid =>
            new("1dee4058-4ff8-4fb8-a9ac-a7264fadd024");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        protected override void AddInputFields()
        {
            InGeneric(
                nameof(Docs.GraphGoo),
                Docs.GraphGoo);

            InGeneric(
                nameof(MatchSubGraph.PatternGraph) + "Goo",
                Docs.GraphGoo);

            InBoolean(
                nameof(MatchSubGraph.MatchTypes),
                Docs.SubGraphMatchTypes,
                MatchSubGraph.DefMatchTypes);

            InBoolean(
                nameof(MatchSubGraph.MatchIndices),
                Docs.SubGraphMatchIndices,
                MatchSubGraph.DefMatchIndices);

            InStrings(
                nameof(MatchSubGraph.NodeFilters),
                Docs.SubGraphMatchNodeFilters);

            InStrings(
                nameof(MatchSubGraph.NodeRules),
                Docs.SubGraphMatchNodeRules);

            InStrings(
                nameof(MatchSubGraph.EdgeFilters),
                Docs.SubGraphMatchEdgeFilters);

            InStrings(
                nameof(MatchSubGraph.EdgeRules),
                Docs.SubGraphMatchEdgeRules);

            Params.Input[2].Optional = true;
            Params.Input[3].Optional = true;
            Params.Input[4].Optional = true;
            Params.Input[5].Optional = true;
            Params.Input[6].Optional = true;
            Params.Input[7].Optional = true;
        }

        protected override void AddOutputFields()
        {
            new MatchSubGraph().RegisterOutputs(Params);
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

            if (!da.GetItem(
                    1,
                    out GraphGoo patternGoo) || patternGoo?.Value == null)
            {
                return;
            }

            var task = new MatchSubGraph
            {
                PatternGraph = patternGoo.Value,
                MatchTypes = da.GetOptionalItem(
                    2,
                    MatchSubGraph.DefMatchTypes),
                MatchIndices = da.GetOptionalItem(
                    3,
                    MatchSubGraph.DefMatchIndices),
                NodeFilters =
                    da.GetOptionalItems<string>(4).FromJson<IRule>().ToList(),
                NodeRules =
                    da.GetOptionalItems<string>(5).FromJson<IRule>().ToList(),
                EdgeFilters =
                    da.GetOptionalItems<string>(6).FromJson<IRule>().ToList(),
                EdgeRules =
                    da.GetOptionalItems<string>(7).FromJson<IRule>().ToList()
            };

            task.Guard();

            GraphPipeline pipeline = new(new List<AbsTask> { task })
            {
                Graph = mainGoo.Value.Clone()
            };

            pipeline.Execute(
                da,
                this,
                task.Queries);

            Message = pipeline.Graph.ComponentMessage() ??
                      task.ComponentMessage();
        }
    }
}