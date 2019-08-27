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
using System.Text;

using Autodesk.IM.Rule.Activities;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Represents expression item of named rules.
    /// </summary>
    public class NamedRuleExpressionItem : IExpressionItem
    {
        private NamedRule _item;


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.NamedRuleExpressionItem class with
        /// specified named rule.
        /// </summary>
        /// <param name="item">The specified named rule which will used in an expression.</param>
        public NamedRuleExpressionItem(NamedRule item)
        {
            this._item = item;
        }


        public string Name
        {
            get
            {
                return _item.Name;
            }
        }


        public string DisplayName
        {
            get
            {
                string seperator = " > "; // NOXLATE
                StringBuilder stringBuilder = new StringBuilder(_item.DisplayName);
                RulePoint parent = _item.Parent;
                while (parent != null)
                {
                    stringBuilder.Insert(0, parent.DisplayName + seperator); // NOXLATE
                    parent = parent.Parent;
                }
                return stringBuilder.ToString();
            }
        }


        public System.Activities.Activity CreateActivity()
        {
            return new NamedRuleFunction()
            {
                RulePath = _item.Path
            };
        }


        public Type ValueType
        {
            get
            {
                // We have decided to adopt loosing type. All subrules return result of type DynamicValue.
                return typeof(DynamicValue);
            }
        }
    }
}
