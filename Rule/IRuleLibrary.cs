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

using System.Activities;
using System.Collections.Generic;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Represents a rule library providing functions of saving and loading rules.
    /// </summary>
    public interface IRuleLibrary
    {
        /// <summary>
        /// Gets an instance implementing IActivitySerializer interface. This instance
        /// is for serialization and deserialization of activities in rules.
        /// </summary>
        ActivitySerializer Serializer
        {
            get;
        }


        /// <summary>
        /// Determines if a rule path has an activity routine defined.
        /// </summary>
        /// <param name="activityId">ID to retrieve Rule's routine</param>
        /// <returns>An activity representing the routine</returns>
        bool HasActivity(string activityId);


        /// <summary>
        /// Gets Rule's activity from cache.
        /// </summary>
        /// <param name="activityId">The ID to retrieve Rule's activity</param>
        DynamicActivity GetActivity(string activityId);


        /// <summary>
        /// Gets a copy of original activity from storage(not cached).
        /// </summary>
        /// <param name="activityId">The ID to retrieve Rule's activity</param>
        DynamicActivity GetOriginalActivity(string activityId);


        /// <summary>
        /// Returns Named Rules stored in the library.
        /// </summary>
        /// <returns>Named Rules</returns>
        IEnumerable<NamedRule> GetNamedRules();


        /// <summary>
        /// Returns Named Rules under a certain path.
        /// </summary>
        /// <returns>Named Rules</returns>
        IEnumerable<NamedRule> GetNamedRules(string path);


        /// <summary>
        /// Saves a named rule.
        /// </summary>
        /// <param name="newOne">The named rule to save.</param>
        /// <param name="namedRule">The workflow to be saved as the named rule's content</param>
        void SetNamedRule(NamedRule namedRule, DynamicActivity workflow);


        /// <summary>
        /// Updates Rule Point's workflow which consists of WF4 activities.
        /// </summary>
        /// <param name="workflow"></param>
        void SetRulePointWorkflow(RulePoint rulePoint, DynamicActivity workflow);


        /// <summary>
        /// Removes the named rule as well as its activity from library.
        /// </summary>
        /// <param name="theOneToRemove">The rule to remove</param>
        void RemoveNamedRule(NamedRule theOneToRemove);


        /// <summary>
        /// Removes content of the rule point. That makes the rule point's content is undefined,
        /// and calling to this rule point will be transfered to its parent.
        /// </summary>
        /// <param name="rulePoint"></param>
        void RemoveRulePointWorkflow(RulePoint rulePoint);
    }
}
