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
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Diagnostics;

using Autodesk.IM.Rule;
using Autodesk.IM.UI.Rule;


namespace RuleConfiguration
{
    /// <summary>
    /// Select context for rule activities.
    /// </summary>
    public sealed class ActivitySelectContext : SelectContext
    {
        public ActivitySelectContext(ItemSelector parent)
            : base(parent)
        {
            UpdateSelectItems();
        }

        public override object CreateNewInstance(SelectItem item)
        {
            Debug.Assert(item.Value is ActivityEntry);
            ActivityEntry activityEntry = item.Value as ActivityEntry;

            // Get old item, parent item and parent item property
            Debug.Assert(Parent.OwnerItem != null && Parent.ItemName != null);
            ModelProperty parentProperty = Parent.OwnerItem.Properties[Parent.ItemName];
            ActivityFactory activityFactory = Parent.EditingContext.Services.GetService<ActivityFactory>();
            Activity newActivity = activityFactory.CreateActivity(activityEntry.Create, Parent.GetModelProperty());
            return newActivity;
        }

        public override void UpdateSelectItems()
        {
            SelectItems.Clear();
            RuleEditingContext context = Parent.RuleEditingContext;
            if (context == null)
            {
                return;
            }

            IEnumerable<ActivityEntry> activities = context.GetAvailableActivities();
            foreach (ActivityEntry item in activities)
            {
                SelectItems.Add(new SelectItem(item.Name, item.DisplayName, item));
            }
        }

        public override string SelectContextName
        {
            get
            {
                return Properties.Resources.ActivitySelectContext;
            }
        }
    }
}
