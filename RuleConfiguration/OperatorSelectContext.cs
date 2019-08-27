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

using Autodesk.IM.Rule;
using Autodesk.IM.UI.Rule;


namespace RuleConfiguration
{
    /// <summary>
    /// Context for selecting operators.
    /// </summary>
    public class OperatorSelectContext : SelectContext
    {
        public OperatorSelectContext(ItemSelector parent)
            : base(parent)
        {
        }

        public override void UpdateSelectItems()
        {
            SelectItems.Clear();
            Type outputType = GetOutputType();
            if (outputType == null)
            {
                return;
            }

            RuleEditingContext context = Parent.RuleEditingContext;
            if (context == null)
            {
                return;
            }

            foreach (var _operator in context.GetOperators(outputType))
            {
                SelectItems.Add(new SelectItem(_operator.Name, _operator.DisplayName, _operator));
            }
        }

        public override object CreateNewInstance(SelectItem item)
        {
            OperatorEntry operatorEntry = item.Value as OperatorEntry;
            ActivityFactory factory = Parent.EditingContext.Services.GetService<ActivityFactory>();
            Activity _operator = factory.CreateActivity(operatorEntry.Create, Parent.GetModelProperty());

            // Create InArgument instance wrapping the activity
            Type genericType = typeof(InArgument<>).MakeGenericType(operatorEntry.ReturnType);
            InArgument argument = Activator.CreateInstance(genericType, _operator) as InArgument;
            return argument;
        }

        public override string SelectContextName
        {
            get
            {
                return Properties.Resources.OperatorSelectContext;
            }
        }
    }
}
