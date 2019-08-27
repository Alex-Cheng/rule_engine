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
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Linq;

using Autodesk.IM.Rule;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// This class is responsible for translation of rule activities.
    /// </summary>
    public class ActivityTranslator
    {
        /// <summary>
        /// A delegate for translation of activity instance.
        /// </summary>
        /// <param name="translator">The current translator for recusively translation.</param>
        /// <param name="item">The model item representing the activity item.</param>
        /// <returns>The result of translation.</returns>
        public delegate string ActivityTranslateItem(ActivityTranslator translator, ModelItem item);

        private Dictionary<Type, ActivityTranslateItem> _activityTranslateItem;


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.ActivityTranslator class.
        /// </summary>
        public ActivityTranslator()
        {
            _activityTranslateItem = new Dictionary<Type, ActivityTranslateItem>();
            CommonActivityTranslateItems.AddCommonTranslateItems(this);
        }


        /// <summary>
        /// Add a translation item for a specified type of activity. The translation item is simply
        /// a human-readable text.
        /// </summary>
        /// <param name="type">The specified System.Type object representing the type of activity.</param>
        /// <param name="displayText">The translation item.</param>
        public void AddTranslateItem(Type type, string displayText)
        {
            _activityTranslateItem.Add(type, (t, x) => displayText);
        }


        /// <summary>
        /// Add a translation item for a specified type of activity.
        /// </summary>
        /// <param name="type">The specified System.Type object representing the type of activity.</param>
        /// <param name="translateItem">The translation item.</param>
        public void AddTranslateItem(Type type, ActivityTranslateItem translateItem)
        {
            _activityTranslateItem.Add(type, translateItem);
        }


        /// <summary>
        /// Translates an activity instance.
        /// </summary>
        /// <param name="modelItem">The ModelItem object representing the activity instance.</param>
        /// <returns>The translated result.</returns>
        public string Translate(ModelItem modelItem)
        {
            if (modelItem == null)
            {
                return Constants.EmptyTextDisplay;
            }

            ModelItem activityItem = modelItem.GetActivityItem();
            if (null == activityItem)
            {
                return Constants.EmptyTextDisplay;
            }

            Type genericType = activityItem.GetGenericType();
            ActivityTranslateItem m = (from a in _activityTranslateItem
                                    where a.Key == genericType
                                    select a.Value).FirstOrDefault();
            if (m == null)
            {
                // Use fallback translation item to prevent the app from crashing.
                m = FallbackTranslateItem;
            }
            return m(this, activityItem);
        }


        private static string FallbackTranslateItem(ActivityTranslator translator, ModelItem item)
        {
            ActivityEntry entry = item.GetActivityEntry();
            if (null != entry)
                return entry.DisplayName;
            return item.GetCurrentValue().ToString();
        }
    }
}
