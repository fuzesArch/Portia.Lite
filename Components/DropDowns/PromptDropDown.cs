using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.Graphs;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Primitives.Models;
using Portia.Infrastructure.Tasks.AI.Prompts;
using Portia.Lite.Core.Primitives;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portia.Lite.Components.DropDowns
{
   // The text to paste next to a photograph for a vision model — the grid it should read, the
   // pattern it should paint, or the models it should map. The answer goes into SetGraph
   // (FromImage) or Task (BuildPattern / BuildModels).
   public class
         PromptDropDown : AbsDropDownComponent<PromptMode>
   {
      private string _prompt;

      public PromptDropDown() : base("Prompt",
         Docs.Prompt,
         Naming.Tab,
         Naming.Logic,
         addTabToName: true)
      { }

      public override Guid ComponentGuid =>
            new("6d3a8f17-4c2e-4b95-a0d1-8e7f2c5b9a36");

      // every mode outputs the prompt, so a mode switch keeps the wire
      protected override bool KeepOutputAcrossModes => true;

      protected override void AddOutputFields()
      {
         OutString(nameof(Docs.Prompt),
            Docs.Prompt);
      }

      protected override void CommonOutputSetting(
         IGH_DataAccess da)
      {
         if (_prompt == null) return;

         da.SetData(0,
            _prompt);
      }

      private void ByGrid(
         IGH_DataAccess da)
      {
         Brep host = null;

         _prompt = da.GetData(0,
            ref host) && host != null
               ? GridPrompt.Build(host)
               : null;

         Message = _prompt == null ? null : "grid";
      }

      private void ByPattern(
         IGH_DataAccess da)
      {
         _prompt = null;
         Message = null;

         if (!da.GetItem(0,
               out GraphGoo graphGoo) ||
            graphGoo?.Value == null)
         {
            return;
         }

         int count = graphGoo.Value.Nodes.Count();

         _prompt = PatternPrompt.Build(count) +
               WithGraph(graphGoo);

         Message = $"{count} nodes";
      }

      private void ByModels(
         IGH_DataAccess da)
      {
         _prompt = null;
         Message = null;
         IGH_Goo libraryGoo = null;

         if (!da.GetItem(0,
               out GraphGoo graphGoo) ||
            graphGoo?.Value == null || !da.GetData(1,
               ref libraryGoo))
         {
            return;
         }

         if (libraryGoo?.ScriptVariable() is not
                  ModelLibrary library ||
            !library.ModelAssets.Any())
         {
            AddRuntimeMessage(
               GH_RuntimeMessageLevel.Warning,
               "Connect a ModelLibrary with at least one model.");

            return;
         }

         int count = graphGoo.Value.Nodes.Count();

         _prompt = ModelPrompt.Build(library,
            count) + WithGraph(graphGoo);

         Message =
               $"{library.ModelAssets.Count} models · {count} nodes";
      }

      private static string WithGraph(
         GraphGoo graphGoo) =>
            Environment.NewLine + Environment.NewLine +
            "GRAPH:" + Environment.NewLine +
            GraphExtract.Build(graphGoo.Value);

      protected override
            Dictionary<PromptMode, ParameterSetup>
            DefineSetup()
      {
         ParameterConfig graph = new(
            () => new Param_GenericObject(),
            nameof(GraphGoo),
            Docs.GraphInput,
            GH_ParamAccess.item);

         return new Dictionary<PromptMode, ParameterSetup>
         {
            {
               PromptMode.Grid,
               new ParameterSetup(new List<ParameterConfig>
                  {
                     new(() => new Param_Brep(),
                        nameof(Docs.HostSurface),
                        Docs.HostSurface,
                        GH_ParamAccess.item)
                  },
                  ByGrid,
                  Docs.Prompt)
            },
            {
               PromptMode.Pattern,
               new ParameterSetup(new List<ParameterConfig>
                        { graph },
                  ByPattern,
                  Docs.Prompt)
            },
            {
               PromptMode.Models,
               new ParameterSetup(new List<ParameterConfig>
                  {
                     graph,
                     new(() => new Param_GenericObject(),
                        nameof(ModelLibrary),
                        Docs.ModelLibrary,
                        GH_ParamAccess.item)
                  },
                  ByModels,
                  Docs.Prompt)
            }
         };
      }
   }
}
