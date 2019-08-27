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
    /// <summary>
    /// Event arguments for invoking rule event.
    /// </summary>
    public class InvokingRuleEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.InvokingRuleEventArgs class with the rule
        /// being called and arguments.
        /// </summary>
        /// <param name="rule">The rule being called.</param>
        /// <param name="arguments">The arguments passed to the rule.</param>
        public InvokingRuleEventArgs(IRule rule, IDictionary<string, object> arguments)
        {
            Rule = rule;
            Arguments = arguments;
        }


        /// <summary>
        /// Gets the rule being called.
        /// </summary>
        public IRule Rule
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets arguments passed to the rule being called.
        /// </summary>
        public IDictionary<string, object> Arguments
        {
            get;
            private set;
        }
    }
}
