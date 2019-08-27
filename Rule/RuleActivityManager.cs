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
using System.Collections.Generic;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// RuleActivityManager is responsible for registration and management of Rule Activities,
    /// Rule Functions and Rule Operators.
    /// </summary>
    public class RuleActivityManager
    {
        private List<ActivityEntry> activityEntries = new List<ActivityEntry>();
        private List<OperatorEntry> operatorEntries = new List<OperatorEntry>();
        private List<FunctionEntry> functionEntries = new List<FunctionEntry>();
        private Dictionary<Type, ActivityEntry> typeDictionary = new Dictionary<Type, ActivityEntry>();

        internal RuleActivityManager()
        { }


        /// <summary>
        /// Gets activity registry entry by specified activity type.
        /// </summary>
        /// <param name="activityType">The specified System.Type object representing the activity's type.</param>
        /// <returns>The corresponding activity entry.</returns>
        public ActivityEntry GetEntry(Type activityType)
        {
            return typeDictionary.ContainsKey(activityType) ? typeDictionary[activityType] : null;
        }


        /// <summary>
        /// Registers activities with specified name, display name, factory function, signature and activity type.
        /// </summary>
        /// <param name="name">The activity's name.</param>
        /// <param name="displayName">The activity's display name.</param>
        /// <param name="factoryFunction">The factory function to create new instance of the activity.</param>
        /// <param name="signature">The activity's signature.</param>
        /// <param name="activityType">The specified System.Type object representing the activity's type.</param>
        public void RegisterActivity(string name, string displayName, Func<Activity> factoryFunction, ActivitySignature signature, Type activityType)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");    // NOXLATE
            }
            if (displayName == null)
            {
                throw new ArgumentNullException("displayName");    // NOXLATE
            }
            if (factoryFunction == null)
            {
                throw new ArgumentNullException("factoryFunction");    // NOXLATE
            }
            if (signature == null)
            {
                throw new ArgumentNullException("signature");    // NOXLATE
            }
            if (activityType == null)
            {
                throw new ArgumentNullException("activityType");    // NOXLATE
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(String.Format(Properties.Resources.ArgumentExceptionMessage, "name"));  // NOXLATE
            }
            if (displayName.Length == 0)
            {
                throw new ArgumentException(String.Format(Properties.Resources.ArgumentExceptionMessage, "displayName"));  // NOXLATE
            }

            ActivityEntry entry = new ActivityEntry(name, displayName, factoryFunction, signature, activityType);
            activityEntries.Add(entry);
            typeDictionary[activityType] = entry;
        }


        /// <summary>
        /// Gets registry entries of registered activities.
        /// </summary>
        /// <returns>Entries of activities.</returns>
        public IEnumerable<ActivityEntry> GetActivityRegistries()
        {
            return activityEntries;
        }


        /// <summary>
        /// Registers operators with specified name, display name, factory function, signature,
        /// operator type(operator is a kind of activity), return type and category.
        /// </summary>
        /// <param name="name">The operator's name.</param>
        /// <param name="displayName">The operator's display name.</param>
        /// <param name="factoryFunction">The factory function to create new instance of the operator.</param>
        /// <param name="signature">The operator's signature.</param>
        /// <param name="activityType">The specified System.Type object representing the operator's type.</param>
        /// <param name="returnType">The specified System.Type object representing the operator's return type.</param>
        /// <param name="category">The specified category of the operator.</param>
        public void RegisterOperator(string name, string displayName, Func<Activity> factoryFunction,
            ActivitySignature signature, Type activityType, Type returnType, OperatorEntry.OperatorCategory category)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");    // NOXLATE
            }
            if (displayName == null)
            {
                throw new ArgumentNullException("displayName");    // NOXLATE
            }
            if (factoryFunction == null)
            {
                throw new ArgumentNullException("factoryFunction");    // NOXLATE
            }
            if (signature == null)
            {
                throw new ArgumentNullException("signature");    // NOXLATE
            }
            if (activityType == null)
            {
                throw new ArgumentNullException("activityType");    // NOXLATE
            }
            if (returnType == null)
            {
                throw new ArgumentNullException("returnType");    // NOXLATE
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(String.Format(Properties.Resources.ArgumentExceptionMessage, "name"));  // NOXLATE
            }
            if (displayName.Length == 0)
            {
                throw new ArgumentException(String.Format(Properties.Resources.ArgumentExceptionMessage, "displayName"));  // NOXLATE
            }

            OperatorEntry entry = new OperatorEntry(
                name,
                displayName,
                factoryFunction,
                signature,
                activityType,
                returnType,
                category);
            operatorEntries.Add(entry);
            typeDictionary[activityType] = entry;
        }


        /// <summary>
        /// Gets registry entries of registered operators.
        /// </summary>
        /// <returns>Entries of operators.</returns>
        public IEnumerable<OperatorEntry> GetOperatorRegistries()
        {
            return operatorEntries;
        }


        /// <summary>
        /// Register functions with specified name, display name, factory function,
        /// signature, function type(function is a kind of activity) and return type.
        /// </summary>
        /// <param name="name">The function's name.</param>
        /// <param name="displayName">The function's display name.</param>
        /// <param name="factoryFunction">The factory function to create new instance of the function.</param>
        /// <param name="signature">The function's signature.</param>
        /// <param name="activityType">The specified System.Type object representing the function's type.</param>
        /// <param name="returnType">The specified System.Type object representing the function's return type.</param>
        public void RegisterFunction(string name, string displayName, Func<Activity> factoryFunction,
            ActivitySignature signature, Type activityType, Type returnType)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");    // NOXLATE
            }
            if (displayName == null)
            {
                throw new ArgumentNullException("displayName");    // NOXLATE
            }
            if (factoryFunction == null)
            {
                throw new ArgumentNullException("factoryFunction");    // NOXLATE
            }
            if (signature == null)
            {
                throw new ArgumentNullException("signature");    // NOXLATE
            }
            if (activityType == null)
            {
                throw new ArgumentNullException("activityType");    // NOXLATE
            }
            if (returnType == null)
            {
                throw new ArgumentNullException("returnType");    // NOXLATE
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(String.Format(Properties.Resources.ArgumentExceptionMessage, "name"));  // NOXLATE
            }
            if (displayName.Length == 0)
            {
                throw new ArgumentException(String.Format(Properties.Resources.ArgumentExceptionMessage, "displayName"));  // NOXLATE
            }

            FunctionEntry entry = new FunctionEntry(
                name,
                displayName,
                factoryFunction,
                signature,
                activityType,
                returnType
                );
            functionEntries.Add(entry);
            typeDictionary[activityType] = entry;
        }


        /// <summary>
        /// Gets registry entries of registered functions.
        /// </summary>
        /// <returns>Entries of functions.</returns>
        public IEnumerable<FunctionEntry> GetFunctionRegistries()
        {
            return functionEntries;
        }
    }
}
