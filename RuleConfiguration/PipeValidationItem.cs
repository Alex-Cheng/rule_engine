using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RuleConfiguration
{
    class PipeValidationItem : IValidationItem
    {
        public long FID
        {
            get;
            set;
        }

        public object FeatureItem
        {
            get;
            set;
        }

        public string Category
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Subtitle
        {
            get;
            set;
        }

        public ValidationType ResultType
        {
            get;
            set;
        }

        public string RulePointPath
        {
            get;
            set;
        }

        public string DeviceType
        {
            get;
            set;
        }
    }
}
