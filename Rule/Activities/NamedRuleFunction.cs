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

using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;


namespace Autodesk.IM.Rule.Activities
{
    /// <summary>
    /// Invokes a named rule specified by the RulePath property and return its result.
    /// </summary>
    public class NamedRuleFunction : CodeActivity<DynamicValue>
    {
        public static NamedRuleFunction Create()
        {
            return new NamedRuleFunction();
        }


        /// <summary>
        /// Gets or sets the rule path of the named rule.
        /// </summary>
        public string RulePath
        {
            get;
            set;
        }


        protected override DynamicValue Execute(CodeActivityContext context)
        {
            RuleExecutingContext executingContext = context.GetExtension<RuleExecutingContext>();
            Debug.Assert(executingContext != null);

            RuleManager ruleManager = executingContext.RuleManager;
            NamedRule rule = ruleManager.GetNamedRule(RulePath);
            if (rule == null)
            {
                throw new ArgumentException(String.Format(Properties.Resources.NamedRuleNotExist, RulePath));
            }

            IDictionary<string, object> parameters = executingContext.GetWorkflowInvokerArguments();
            return ruleManager.InvokeRule<DynamicValue>(rule, parameters);
        }
    }
}
