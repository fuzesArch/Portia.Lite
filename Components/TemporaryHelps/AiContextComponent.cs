using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using System;

#if INTERNAL
using Portia.Infrastructure.Tasks.AI.AiContext;
#endif

namespace Portia.Lite.Components.TemporaryHelps
{
    #if INTERNAL
    public class AiContextComponent : GenericBase
    {
        public override Guid ComponentGuid =>
            new("0c6ca144-0151-48bc-a3db-abced876443b");

        protected override System.Drawing.Bitmap Icon =>
            Properties.Resources.BaseLogo;

        public AiContextComponent()
            : base(
                nameof(AiContextComponent),
                Docs.AiContext,
                Naming.Tab,
                Naming.Primitives)
        {
        }

        protected override void AddOutputFields()
        {
            OutString(
                nameof(AiInstructions),
                Docs.AiContext);
        }

        protected override void Solve(
            IGH_DataAccess da)
        {
            da.SetData(
                0,
                AiInstructions.Context);
        }
    }
    #endif
}