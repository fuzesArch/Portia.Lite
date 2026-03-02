using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphHelps;
using Portia.Infrastructure.Graphs;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Tasks.Base;
using Portia.Infrastructure.Tasks.Isomorphism;
using Portia.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components.Main
{
    public class MutateSubGraphComponent : GenericBase
    {
        public MutateSubGraphComponent()
            : base(
                nameof(MutateSubGraphs),
                Docs.MutateSubGraphs,
                Naming.Tab,
                Naming.Graph)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        public override Guid ComponentGuid =>
            new("04263052-5cc3-47ba-96df-f8b91d274acd");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        protected override void AddInputFields()
        {
            InGeneric(
                "Host" + nameof(Docs.GraphGoo),
                Docs.GraphGoo);

            Params.RegisterInputParam(
                new Goo.MutationSetParameter
                {
                    Name = nameof(MutateSubGraphs.MutationSets),
                    NickName = nameof(MutateSubGraphs.MutationSets),
                    Description = Docs.MutationSetGoo,
                    Access = GH_ParamAccess.list
                });
        }

        protected override void AddOutputFields()
        {
            new MutateSubGraphs().RegisterOutputs(Params);
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
                    out List<MutationSetGoo> setGoos))
            {
                return;
            }

            var task = new MutateSubGraphs
            {
                MutationSets = setGoos.Select(g => g.Value).ToList()
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