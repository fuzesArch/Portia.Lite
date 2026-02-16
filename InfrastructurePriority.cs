using Grasshopper;
using Grasshopper.Kernel;
using Portia.Infrastructure.Core.Portia.Helps;

namespace Portia.Lite
{
    public class InfrastructurePriority : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.DocumentServer.DocumentAdded += OnDocumentAdded;

            foreach (GH_Document doc in Instances.DocumentServer)
            {
                doc.SolutionStart -= OnSolutionStart;
                doc.SolutionStart += OnSolutionStart;
            }

            return GH_LoadingInstruction.Proceed;
        }

        private void OnDocumentAdded(
            GH_DocumentServer sender,
            GH_Document doc)
        {
            doc.SolutionStart -= OnSolutionStart;
            doc.SolutionStart += OnSolutionStart;
        }

        private void OnSolutionStart(
            object sender,
            GH_SolutionEventArgs e)
        {
            GraphMemoryCache.Clear();
        }
    }
}