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


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Represents a provider of expression items representing named rules.
    /// </summary>
    public class NamedRuleExpressionItemProvider : IExpressionItemProvider
    {
        private RuleSignature _subruleSignature;

        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.NamedRuleExpressionItemProvider class
        /// with specified signature, name and display name.
        /// </summary>
        /// <param name="subruleSignature">The specified rule signature for determining which named rules are selected as expression items.</param>
        /// <param name="name">The specified name of the new instance.</param>
        /// <param name="displayName">The specified display name of the new instance.</param>
        public NamedRuleExpressionItemProvider(
            RuleSignature subruleSignature,
            string name,
            string displayName)
        {
            Name = name;
            DisplayName = displayName;
            _subruleSignature = subruleSignature;
        }


        /// <summary>
        /// Gets the provider's name.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets the provider's display name.
        /// </summary>
        public string DisplayName
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets expression items provided by this provider.
        /// </summary>
        public IEnumerable<IExpressionItem> ExpressionItems
        {
            get
            {
                var ruleManager = _subruleSignature.Owner;
                foreach (var item in ruleManager.FindNamedRulesBySignature(_subruleSignature))
                {
                    // Rules without output arguments cannot be used in an expression.
                    if (item.Signature.SystemOutArguments.Count > 0)
                    {
                        yield return new NamedRuleExpressionItem(item);
                    }
                }
            }
        }
    }
}
