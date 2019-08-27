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
using System.Collections.Generic;
using System.Activities;
using System.Activities.Presentation.Model;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Create WF4 Activioty instance(rule activity, function, operator and etc.) in UI layer.
    /// It customizes created Activity instance by calling creation callbacks. Customization on
    /// created WF4 Activity instance improves user experience, e.g. when users change '+' to '-' on
    /// expression '1 + 2', creation callback will assign operands to newly-created Subtract activity
    /// instance, so that the expression will be '1 - 2' that users don't need to set left and right
    /// operands again.
    /// </summary>
    public class ActivityFactory
    {
        public delegate void ActivityCreationCallback(Activity activity, ModelProperty itemProperty);

        public delegate Activity FunctionObjectFactory();

        Dictionary<Type, ActivityCreationCallback> _creationCallbacks;


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.ActivityFactory class.
        /// </summary>
        public ActivityFactory()
        {
            _creationCallbacks = new Dictionary<Type, ActivityCreationCallback>();
            CommonActivityCreationCallbacks.RegisterCommonActivityCreationCallbacks(this);
        }


        /// <summary>
        /// Registers creation callback for a specified type of activities.
        /// </summary>
        /// <param name="type">The specified type of activity.</param>
        /// <param name="callback">The specified callback.</param>
        public void RegisterCreationCallback(Type type, ActivityCreationCallback callback)
        {
            _creationCallbacks[type] = callback;
        }


        /// <summary>
        /// Creates a new instance by the given delegate and invoke corresponding callbacks against it.
        /// </summary>
        /// <param name="createFunc">The specified delegate used to create a new activity instance.</param>
        /// <param name="itemProperty">The corresponding item property for which the activity is created.</param>
        /// <returns>The new instance having callbacks performed against it.</returns>
        public Activity CreateActivity(Func<Activity> createFunc, ModelProperty itemProperty)
        {
            Activity newActivity = createFunc();

            // First, try the passed type, which might have generics specified
            InvokeActivityCreationCallbacks(newActivity.GetType(), newActivity, itemProperty);
            return newActivity;
        }


        private bool InvokeActivityCreationCallbacks(Type type, Activity newActivity, ModelProperty itemProperty)
        {
            if (_creationCallbacks.ContainsKey(type))
            {
                _creationCallbacks[type](newActivity, itemProperty);
                return true;
            }
            else if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                Type genericType = type.GetGenericTypeDefinition();
                return InvokeActivityCreationCallbacks(genericType, newActivity, itemProperty);
            }
            else if (type.BaseType != null)
            {
                InvokeActivityCreationCallbacks(type.BaseType, newActivity, itemProperty);
            }
            return false;
        }
    }
}
