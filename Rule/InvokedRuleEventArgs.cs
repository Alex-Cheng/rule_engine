/*
 * Copyright (C) 2011 by Autodesk, Inc. All Rights Reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Autodesk.IM.Rule
{
    public class InvokedRuleEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.InvokedRuleEventArgs class with the rule
        /// which has been called, arguments and the results.
        /// </summary>
        /// <param name="rule">The rule being called.</param>
        /// <param name="arguments">The arguments passed to the rule.</param>
        /// <param name="results">The results returned by the rule.</param>
        public InvokedRuleEventArgs(IRule rule, IDictionary<string, object> arguments, IDictionary<string, object> results)
        {
            Rule = rule;
            Arguments = arguments;
            Results = results;
        }


        /// <summary>
        /// Gets the rule which has been called.
        /// </summary>
        public IRule Rule
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets arguments passed to the rule.
        /// </summary>
        public IDictionary<string, object> Arguments
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets results returned by the rule.
        /// </summary>
        public IDictionary<string, object> Results
        {
            get;
            private set;
        }
    }
}
