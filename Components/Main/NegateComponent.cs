using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Grasshopper.Kernel;
using Portia.Infrastructure.Components;
using Portia.Infrastructure.DocStrings;
using Portia.Infrastructure.Helps;
using Portia.Infrastructure.Rules.Base;

namespace Portia.Lite.Components.Main
{
   public class NegateComponent : GenericBase
   {
      public NegateComponent() : base("NegateRule",
         Docs.NegateComponent,
         Naming.Tab,
         Naming.Graph)
      { }

      public override Guid ComponentGuid =>
            new("f2524dee-26f9-4127-a448-ab5a69aa76e8");

      protected override Bitmap Icon =>
            Properties.Resources.BaseLogo;

      protected override void AddInputFields()
      {
         InStrings("Rules",
                  Docs.Rule)
              .InBoolean(nameof(RuleBase.IsNegated),
                  Docs.NegateComponent,
                  RuleBase.DefIsNegated);

         SetInputParameterOptionality(1);
      }

      protected override void AddOutputFields()
      {
         OutStrings("Rules",
            Docs.Rule);
      }

      protected override void Solve(
         IGH_DataAccess da)
      {
         if (!da.GetItems(0,
            out List<string> ruleJsons))
         {
            return;
         }

         bool isNegated = da.GetOptionalItem(1,
            RuleBase.DefIsNegated);

         List<IRule> rules = ruleJsons.FromJson<IRule>()
                                      .ToList();

         if (isNegated)
         {
            foreach (IRule rule in rules)
            {
               rule.IsNegated = true;
            }
         }

         da.SetDataList(0,
            rules.Select(r => r.ToJson()));
      }
   }
}
