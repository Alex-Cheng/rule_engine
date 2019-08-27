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
using System.Collections.Generic;

using Autodesk.IM.Rule;
using Autodesk.IM.Rule.Activities;
using Autodesk.IM.UI.Rule;


namespace RuleConfiguration
{
    /// <summary>
    /// The context of selecting valid values,
    /// usually for domain attributes or properties of enum type
    /// </summary>
    internal sealed class ValidValueSelectContext : SelectContext
    {
        public ValidValueSelectContext(ItemSelector parent, IEnumerable<DynamicValue> validValues)
            : base(parent)
        {
            this.ValidValues = validValues;
        }

        public IEnumerable<DynamicValue> ValidValues
        {
            get;
            private set;
        }

        public override string SelectContextName
        {
            get
            {
                return Properties.Resources.ValidValueSelectContext;
            }
        }

        public override void UpdateSelectItems()
        {
            this.SelectItems.Clear();

            foreach (DynamicValue value in this.ValidValues)
            {
                SelectItems.Add(new SelectItem(value.ToString(), value.ToString(), value.Value));
            }
        }

        public override object CreateNewInstance(SelectItem item)
        {
            object value = item.Value;
            return new InArgument<DynamicValue>(DynamicLiteral.Create(value));
        }
    }
}
