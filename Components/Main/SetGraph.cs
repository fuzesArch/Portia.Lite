using Portia.Infrastructure.Components;

namespace Portia.Lite.Components.Main
{
   public class SetGraph : SetGraphDropDown
   {
      public SetGraph() : base(nameof(SetGraph),
         Naming.Tab,
         Naming.Graph,
         addTabToName: true)
      { }
   }
}
