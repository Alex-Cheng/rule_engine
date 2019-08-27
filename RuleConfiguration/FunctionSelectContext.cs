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
using System.Linq;

using Autodesk.IM.Rule;
using Autodesk.IM.UI.Rule;


namespace RuleConfiguration
{
    /// <summary>
    /// Context for selecting functions.
    /// </summary>
    internal sealed class FunctionSelectContext : SelectContext
    {
        public FunctionSelectContext(ItemSelector parent)
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

            foreach (var function in context.GetFunctions(outputType).OrderBy(f=>f.DisplayName))
            {
                SelectItems.Add(new SelectItem(function.Name, function.DisplayName, function));
            }
        }

        public override object CreateNewInstance(SelectItem item)
        {
            FunctionEntry functionEntry = item.Value as FunctionEntry;
            Debug.Assert(functionEntry != null);

            ActivityFactory factory = Parent.EditingContext.Services.GetService<ActivityFactory>();
            Activity function = factory.CreateActivity(functionEntry.Create, Parent.GetModelProperty());

            // Create InArgument instance wrapping the activity
            Type genericType = typeof(InArgument<>).MakeGenericType(functionEntry.ReturnType);
            InArgument argument = Activator.CreateInstance(genericType, function) as InArgument;
            return argument;
        }

        public override string SelectContextName
        {
            get
            {
                return Properties.Resources.FunctionSelectContext;
            }
        }
    }
}
