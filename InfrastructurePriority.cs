using Grasshopper.Kernel;

namespace Portia.Lite
{
    public class InfrastructurePriority : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            return GH_LoadingInstruction.Proceed;
        }
    }
}