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
using System.Collections.Generic;


namespace Autodesk.IM.Rule
{
    // TODO: implement a default simple rule library.
    /// <summary>
    /// Represents a rule library which finds, loads and saves rules.
    /// </summary>
    public class RuleLibrary : IRuleLibrary
    {
        public ActivitySerializer Serializer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HasActivity(string activityId)
        {
            throw new NotImplementedException();
        }

        public System.Activities.DynamicActivity GetActivity(string activityId)
        {
            throw new NotImplementedException();
        }

        public System.Activities.DynamicActivity GetOriginalActivity(string activityId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NamedRule> GetNamedRules()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NamedRule> GetNamedRules(string path)
        {
            throw new NotImplementedException();
        }

        public void SetNamedRule(NamedRule namedRule, System.Activities.DynamicActivity workflow)
        {
            throw new NotImplementedException();
        }

        public void SetRulePointWorkflow(RulePoint rulePoint, System.Activities.DynamicActivity workflow)
        {
            throw new NotImplementedException();
        }

        public void RemoveNamedRule(NamedRule theOneToRemove)
        {
            throw new NotImplementedException();
        }

        public void RemoveRulePointWorkflow(RulePoint rulePoint)
        {
            throw new NotImplementedException();
        }
    }
}
