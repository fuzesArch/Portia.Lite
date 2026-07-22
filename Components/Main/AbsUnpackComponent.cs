using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Goo;
using Portia.Infrastructure.GraphItems;
using Portia.Infrastructure.Graphs;
using Portia.Infrastructure.Helps;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Portia.Lite.Components.Main
{
   // Accepts EITHER a whole Graph (unpacks all its items of this kind) OR the item goos straight
   // from a Filter task. One generic input, sorted out at runtime — the same union trick
   // PortiaComponent uses for Origin. A GH parameter can only BE one type, so the union lives in
   // Collect, not in the registration.
   public abstract class
         AbsUnpackComponent<TItem> : GenericBase
         where TItem : class, IGraphItem
   {
      protected AbsUnpackComponent(
         string name,
         string description) : base(name,
         description,
         Naming.Tab,
         Naming.Logic)
      { }

      public override GH_Exposure Exposure =>
            GH_Exposure.quinary;

      protected override Bitmap Icon =>
            Properties.Resources.BaseLogo;

      protected abstract IEnumerable<TItem> GraphItems(
         Graph graph);

      protected abstract string ItemLabel { get; }

      protected override void AddInputFields()
      {
         InGenerics("Goo",
            $"A Graph (unpacks all {ItemLabel}) or the {ItemLabel} goos from a Filter.");

         SetInputParameterOptionality(0);
      }

      protected override void AddOutputFields()
      {
         OutGeometries(nameof(Docs.Geometries),
                  Docs.Geometries)
              .OutIntegers(nameof(Docs.Index) + "es",
                  Docs.Identity)
              .OutStrings(nameof(Docs.Type) + "s",
                  Docs.Type);

         SimplifyOutputs();
      }

      protected override void Solve(
         IGH_DataAccess da)
      {
         List<IGH_Goo> goos = new();

         if (!da.GetDataList(0,
            goos) || goos.Count == 0)
         {
            return;
         }

         List<TItem> items = Collect(goos);

         if (items.Count == 0)
         {
            AddRuntimeMessage(
               GH_RuntimeMessageLevel.Warning,
               $"No {ItemLabel} in the input — did you connect the other kind of goo?");

            return;
         }

         da.SetDataList(0,
            items.Select(i =>
                  new GraphItemGoo(i).GetGeometries()));

         da.SetDataList(1,
            items.Select(i => i.GraphIdentity.Index));

         da.SetDataList(2,
            items.Select(i => i.GraphIdentity.Type));
      }

      // GraphGoo → all its items of this kind; GraphItemGoo → itself, if it IS this kind
      // (edge goos wired into a nodes unpacker are simply skipped, then warned about above)
      private List<TItem> Collect(
         IEnumerable<IGH_Goo> goos)
      {
         List<TItem> items = new();

         foreach (IGH_Goo goo in goos)
         {
            if (goo is GraphGoo graphGoo &&
               graphGoo.Value != null)
            {
               items.AddRange(GraphItems(graphGoo.Value));
            }
            else if (goo is GraphItemGoo itemGoo &&
               itemGoo.Value is TItem item)
            {
               items.Add(item);
            }
         }

         return items;
      }
   }
}
