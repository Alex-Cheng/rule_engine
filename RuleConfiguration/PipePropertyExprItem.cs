using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.IM.Rule;
using System.Activities;

namespace RuleConfiguration
{
    public class PipePropertyExprItem : IExpressionItem
    {
        public string Name
        {
            get;
            set;
        }

        public string DisplayName
        {
            get 
            {
                return Name;
            }
        }

        public Activity CreateActivity()
        {
            return new PipePropertyActivity { PropertyName = Name };
        }

        public Type ValueType
        {
            get 
            {
                return typeof(DynamicValue);
            }
        }
    }
}
