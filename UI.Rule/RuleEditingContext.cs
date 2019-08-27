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

using Autodesk.IM.Rule;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Represents the context for editing rules. It is often used by activity designers
    /// to interact with underlying rule system.
    /// </summary>
    public class RuleEditingContext
    {
        private RuleSignature _ruleSignature;

        private Dictionary<Type, object> _services = new Dictionary<Type, object>();


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.RuleEditingContext class
        /// with specified rule signature and activity factory.
        /// </summary>
        /// <param name="ruleSignature">The specified rule signature used to initialize the new instance.</param>
        /// <param name="activityFactory">The sepcified activity factory.</param>
        public RuleEditingContext(RuleSignature ruleSignature)
        {
            _ruleSignature = ruleSignature;
        }


        /// <summary>
        /// Gets available activities.
        /// </summary>
        /// <returns>Available activities.</returns>
        public IEnumerable<ActivityEntry> GetAvailableActivities()
        {
            RuleManager ruleManager = _ruleSignature.Owner;
            foreach (var activityEntry in ruleManager.ActivityManager.GetActivityRegistries())
            {
                if (_ruleSignature.IsActivityAvailable(activityEntry))
                {
                    yield return activityEntry;
                }
            }
        }


        /// <summary>
        /// Gets expression item providers.
        /// </summary>
        public IEnumerable<IExpressionItemProvider> GetExpressionItemProviders()
        {
            return _ruleSignature.GetExpressionItemProviders();
        }


        /// <summary>
        /// Gets functions by specified output type.
        /// </summary>
        /// <param name="outputType">The output type of functions.</param>
        public IEnumerable<FunctionEntry> GetFunctions(Type outputType)
        {
            RuleManager ruleManager = _ruleSignature.Owner;
            foreach (var function in ruleManager.ActivityManager.GetFunctionRegistries())
            {
                if (!_ruleSignature.IsActivityAvailable(function))
                {
                    continue;
                }

                if (outputType == null ||
                    function.ReturnType == outputType ||
                    function.ReturnType.IsSubclassOf(outputType))
                {
                    yield return function;
                }
            }
        }


        /// <summary>
        /// Gets operators by specified output type.
        /// </summary>
        /// <param name="outputType">The output type of operators.</param>
        public IEnumerable<OperatorEntry> GetOperators(Type outputType)
        {
            RuleManager ruleManager = _ruleSignature.Owner;
            foreach (var _operator in ruleManager.ActivityManager.GetOperatorRegistries())
            {
                if (outputType == null ||
                    _operator.ReturnType == outputType ||
                    _operator.ReturnType.IsSubclassOf(outputType))
                {
                    yield return _operator;
                }
            }
        }


        /// <summary>
        /// Gets rule signature of current rule.
        /// </summary>
        public RuleSignature GetRuleSignature()
        {
            return _ruleSignature;
        }


        /// <summary>
        /// Gets named rules.
        /// </summary>
        public IEnumerable<NamedRule> GetNamedRuleActivities()
        {
            var ruleManager = _ruleSignature.Owner;
            foreach (var namedRule in ruleManager.FindNamedRulesBySignature(_ruleSignature))
            {
                if (namedRule.Signature.SystemOutArguments.Count == 0)
                {
                    // Named rules with output arguments are treated as functions which are used to
                    // compose an expression. So we ignore them, and only get those without output arguments.
                    yield return namedRule;
                }
            }
        }
    }
}
