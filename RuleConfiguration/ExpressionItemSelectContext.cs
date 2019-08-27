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
using System.Diagnostics;

using Autodesk.IM.Rule;
using Autodesk.IM.UI.Rule;


namespace RuleConfiguration
{
    /// <summary>
    /// Select context for expression item given by rule signature.
    /// </summary>
    internal class ExpressionItemSelectContext : SelectContext
    {
        IExpressionItemProvider _provider;

        public ExpressionItemSelectContext(IExpressionItemProvider provider, RuleEditingContext context, ItemSelector parent)
            : base(parent)
        {
            _provider = provider;
        }

        public override string SelectContextName
        {
            get { return _provider.DisplayName; }
        }

        public override object CreateNewInstance(SelectItem item)
        {
            IExpressionItem expressionItem = item.Value as IExpressionItem;
            Debug.Assert(expressionItem != null);

            ActivityFactory factory = Parent.EditingContext.Services.GetService<ActivityFactory>();
            Debug.Assert(factory != null);
            object instance = factory.CreateActivity(expressionItem.CreateActivity, Parent.GetModelProperty());
            Type argumentType = typeof(InArgument<>).MakeGenericType(expressionItem.ValueType);
            InArgument arg = Activator.CreateInstance(argumentType, instance) as InArgument;
            return arg;
        }

        public override void UpdateSelectItems()
        {
            SelectItems.Clear();
            Type requiredType = this.GetOutputType();
            foreach (var item in _provider.ExpressionItems)
            {
                if (requiredType == null ||
                    requiredType == item.ValueType ||
                    item.ValueType.IsSubclassOf(requiredType) ||
                    requiredType == typeof(DynamicValue))
                {
                    SelectItems.Add(new SelectItem(item.Name, item.DisplayName, item));
                }
            }
        }
    }
}
