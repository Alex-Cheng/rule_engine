using System;
using System.Activities;

using Microsoft.VisualBasic.Activities;

using Autodesk.IM.Rule;
using Autodesk.Civil.DatabaseServices;


namespace RuleConfiguration
{
    public class PipePropertyActivity : CodeActivity<DynamicValue>
    {
        public const string NominalDiameterPropName = "Nominal Diameter";
        public const string MaterialPropName = "Material";
        public const string MaxCoverPropName = "Max Cover";
        public const string MinCoverPropName = "Min Cover";

        public InArgument<object> Pipe = new InArgument<object>((Activity<object>)(new VisualBasicValue<object>(PipeRuleSignature.PipeArgument.Name)));

        public string PropertyName
        {
            get;
            set;
        }


        protected override DynamicValue Execute(CodeActivityContext context)
        {
            PressurePipe pipe = context.GetValue(Pipe) as PressurePipe;
            
            DynamicValue result = new DynamicValue();
            if (pipe != null)
            {               
                switch (PropertyName)
                {
                    case NominalDiameterPropName:
                        result = new DynamicValue(pipe.InnerDiameter);
                        break;
                    case MaterialPropName:
                        result = new DynamicValue(pipe.PipeMaterial);
                        break;
                    case MaxCoverPropName:
                        result = new DynamicValue(pipe.MaximumCover);
                        break;
                    case MinCoverPropName:
                        result = new DynamicValue(pipe.MinimumCover);
                        break;
                }
            }

            return result;
        }
    }
}
