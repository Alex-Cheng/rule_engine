using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.IM.Rule;

namespace RuleConfiguration
{
    public class PipePropertyExprItemProvider : IExpressionItemProvider
    {
        public string Name
        {
            get
            {
                return "Pipe Properties";
            }
        }

        public string DisplayName
        {
            get
            {
                return Name;
            }
        }

        public IEnumerable<IExpressionItem> ExpressionItems
        {
            get 
            {
                yield return new PipePropertyExprItem() { Name = PipePropertyActivity.NominalDiameterPropName };
                yield return new PipePropertyExprItem() { Name = PipePropertyActivity.MaterialPropName};
                //yield return new PipePropertyExprItem() { Name = "Pipe Type" };

                yield return new PipePropertyExprItem() { Name = PipePropertyActivity.MaxCoverPropName };
                yield return new PipePropertyExprItem() { Name = PipePropertyActivity.MinCoverPropName };
            }
        }
    }
}
