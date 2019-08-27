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


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Represents a registration entry of an activity designer for a specified activity type.
    /// </summary>
    public class ActivityDesignerEntry
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.ActivityDesignerEntry class.
        /// </summary>
        /// <param name="activityType">The type of activity.</param>
        /// <param name="designerType">The corresponding designer type.</param>
        public ActivityDesignerEntry(Type activityType, Type designerType)
        {
            ActivityType = activityType;
            DesignerType = designerType;
        }


        /// <summary>
        /// Gets type of activity.
        /// </summary>
        public Type ActivityType
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets type of activity designer.
        /// </summary>
        public Type DesignerType
        {
            get;
            private set;
        }
    }
}
