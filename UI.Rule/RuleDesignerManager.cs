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
using System.Activities.Presentation.Metadata;
using System.Collections.Generic;
using System.ComponentModel;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Rule Designer Manager is responsible for registration of designers for Rule Activities,
    /// Rule Operators and Rule Functions.
    /// </summary>
    /// <remarks>
    /// All of designers are derived from UIControl base class.
    /// </remarks>
    public class RuleDesignerManager
    {
        private bool needRefresh = false;

        private List<ActivityDesignerEntry> designers = new List<ActivityDesignerEntry>();


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.RuleDesignerManager class.
        /// </summary>
        public RuleDesignerManager()
        {
        }


        /// <summary>
        /// Registers designer UI for an activity.
        /// </summary>
        /// <param name="activityType">The type of WF4 activity</param>
        /// <param name="designerType">The type of WF4 activity designer</param>
        public void RegisterDesigner(Type activityType, Type designerType)
        {
            designers.Add(new ActivityDesignerEntry(activityType, designerType));
            needRefresh = true;
        }


        /// <summary>
        /// Updates attributes with registered designer. WF4 Workflow Designer or our Custom Workflow Designer
        /// both get designers by activity's DesignerAttribute. This method should be called before display
        /// a workflow.
        /// </summary>
        /// <returns></returns>
        public void UpdateDesignerAttributes()
        {
            if (needRefresh)
            {
                AttributeTableBuilder builder = new AttributeTableBuilder();
                foreach (var designer in designers)
                {
                    builder.AddCustomAttributes(designer.ActivityType, new DesignerAttribute(designer.DesignerType));
                }
                MetadataStore.AddAttributeTable(builder.CreateTable());
                needRefresh = false;
            }
        }
    }
}
