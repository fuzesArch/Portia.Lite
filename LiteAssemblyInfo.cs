using Grasshopper.Kernel;
using Grasshopper;
using System;
using System.Drawing;

namespace Portia.Lite
{
    public class LiteAssemblyInfo : GH_AssemblyInfo
    {
        public override string Name => "Portia.Lite";

        public override Bitmap Icon => null;

        public override string Description =>
            "Sovereign Graph & Logic Infrastructure - Public Edition";

        public override Guid Id =>
            new Guid("4e038c5a-14d7-44fe-9805-c28be2c84e3a");

        public override string AuthorName => "Bálint Péter Füzes";

        public override string AuthorContact =>
            "https://fuzesarch.com/ and balint@fuzesarch.com";
    }

    public class LiteCategoryIcon : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.ComponentServer.AddCategoryShortName(
                "Portia",
                "P");
            Instances.ComponentServer.AddCategorySymbolName(
                "Portia",
                'P');
            return GH_LoadingInstruction.Proceed;
        }
    }
}