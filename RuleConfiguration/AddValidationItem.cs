/*
 * Copyright (C) 2010-2011 by Autodesk, Inc. All Rights Reserved.
 *
 * By using this code, you are agreeing to the terms and conditions of
 * the License Agreement included in the documentation for this code.
 *
 * AUTODESK MAKES NO WARRANTIES, EXPRESS OR IMPLIED, AS TO THE
 * CORRECTNESS OF THIS CODE OR ANY DERIVATIVE WORKS WHICH INCORPORATE
 * IT. AUTODESK PROVIDES THE CODE ON AN "AS-IS" BASIS AND EXPLICITLY
 * DISCLAIMS ANY LIABILITY, INCLUDING CONSEQUENTIAL AND INCIDENTAL
 * DAMAGES FOR ERRORS, OMISSIONS, AND OTHER PROBLEMS IN THE CODE.
 *
 * Use, duplication, or disclosure by the U.S. Government is subject
 * to restrictions set forth in FAR 52.227-19 (Commercial Computer
 * Software Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
 * (Rights in Technical Data and Computer Software), as applicable.
 */

using System.Activities;

using Microsoft.VisualBasic.Activities;

using Autodesk.IM.Rule.Activities;


namespace RuleConfiguration
{
    public class AddValidationItem : CodeActivity
    {
        public static AddValidationItem Create()
        {
            AddValidationItem rule = new AddValidationItem();
            rule.InitDefault();
            return rule;
        }

        public AddValidationItem()
        {
        }

        public void InitDefault()
        {
            this.Message = new InArgument<string>(
                new StringExpression()
                {
                    Elements =
                    {
                        StringLiteral.CreateArgument("The rule is violated.")
                    }
                });
            this.Type = ValidationType.Error;
            this.Pipe = new InArgument<object>((Activity<object>)
                new VisualBasicValue<object>(PipeRuleSignature.PipeArgument.Name));
            this.Results = new InArgument<ValidationRuleResult>(
                new VisualBasicValue<ValidationRuleResult>(PipeRuleSignature.ResultArgument.Name));
        }

        // user displayed input arguments for UI
        public ValidationType Type
        {
            get;
            set;
        }
        public InArgument<string> Message
        {
            get;
            set;
        }

        // hidden input arguments, the UI should automatically set and manage
        public InArgument<object> Pipe
        {
            get;
            set;
        }
        public InArgument<ValidationRuleResult> Results
        {
            get;
            set;
        }

        protected override void Execute(CodeActivityContext context)
        {
            object pipe = this.Pipe.Get(context);

            // The internal system will use the resultSet, but also return as output for other usage
            ValidationRuleResult results = this.Results.Get(context);
            if (null != results)
            {
                results.AddResult(pipe, this.Type, "Pipe", this.Message.Get(context));
            }
            return;
        }
    }
}
