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

using System.Collections.Generic;
using System.Linq;

using Autodesk.IM.Rule;
using Autodesk.IM.UI.Rule;


namespace RuleConfiguration
{
    /// <summary>
    /// Selector UI for values composing an expression.
    /// </summary>
    public class ValueSelector : ItemSelector
    {
        public ValueSelector()
        {
        }

        protected override void UpdateContexts(IEnumerable<DynamicValue> validValueItemHint)
        {
            Contexts.Clear();

            RuleEditingContext context = RuleEditingContext;
            if (context == null)
                return;

            // TODO: further refactoring is still needed.
            //if (null != validValueItemHint && validValueItemHint.Any())
            //{
            //    Contexts.Add(new ValidValueSelectContext(this, validValueItemHint));
            //}
            //else if (null != this.Item)
            //{
            //    List<DynamicValue> validValues = this.Item.GetValidValues().ToList<DynamicValue>();
            //    if (validValues.Any())
            //    {
            //        Contexts.Add(new ValidValueSelectContext(this, validValues));
            //    }
            //}

            List<DynamicValue> validValues = this.Item.GetValidValues().ToList<DynamicValue>();

            if (validValues.Any())
            {
                Contexts.Add(new ValidValueSelectContext(this, validValues));
            }

            Contexts.Add(new LiteralSelectContext(this));

            foreach (var item in context.GetExpressionItemProviders())
            {
                Contexts.Add(new ExpressionItemSelectContext(item, context, this));
            }

            Contexts.Add(new OperatorSelectContext(this));
            Contexts.Add(new FunctionSelectContext(this));
            //Contexts.Add(new CatalogSelectContext(this));
        }
    }
}
