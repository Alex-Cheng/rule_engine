using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.IM.Rule;

namespace RuleConfiguration
{
    public class PipeRuleSignature : RuleSignature
    {
        public static RuleArgument PipeArgument = new RuleArgument(new Guid("C1143844-028A-4E84-9144-D98877FD67B6"), "Pipe", typeof(object));
        public static RuleArgument ResultArgument = new RuleArgument(new Guid("C1143844-028A-4E84-9144-D98877FD67B7"), "ValidationResult", typeof(ValidationRuleResult));

        public PipeRuleSignature(RuleManager owner)
            : base(owner)
        {
            SystemInArguments.Add(PipeArgument);
            SystemInArguments.Add(ResultArgument);
        }
        
        public override IEnumerable<IExpressionItemProvider> GetExpressionItemProviders()
        {
            foreach (var item in base.GetExpressionItemProviders())
            {
                yield return item;
            }

            yield return new PipePropertyExprItemProvider();
        }
    }
}
