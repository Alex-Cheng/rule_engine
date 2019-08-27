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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.VisualBasic.Activities;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Provides functions of registration, organization, finding and invocation of rules and
    /// has rule activity manager and rule library. RuleManager is center of Rule System.
    /// </summary>
    public class RuleManager
    {
        private const string ResultParameterName = "Result";  // NOXLATE

        public const string VARIABLE_RULELOGGING = "RULELOGGING"; //NOXLATE

        private RulePoint _rootRulePoint;


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.RuleManager.
        /// </summary>
        public RuleManager()
        {
            _rootRulePoint = new RulePoint(this, String.Empty, null, String.Empty);
            this.ActivityManager = new RuleActivityManager();
        }


        /// <summary>
        /// Root node of rule point tree.
        /// </summary>
        public RulePoint RootRulePoint
        {
            get
            {
                return _rootRulePoint;
            }
        }


        /// <summary>
        /// Get rule activity manager.
        /// </summary>
        public RuleActivityManager ActivityManager
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets or sets whether rule logging is enabled.
        /// </summary>
        public bool LoggingEnabled
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets a RuleLogger instance for logging.
        /// </summary>
        public RuleLogger Logger
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets rule library object for storage.
        /// </summary>
        /// <remarks></remarks>
        public IRuleLibrary Storage
        {
            get;
            set;
        }


        /// <summary>
        /// Event fired after rule execution completes and logging is enabled.
        /// </summary>
        public event EventHandler<LoggingCompletedEventArgs> LoggingCompleted;


        public event EventHandler<InvokingRuleEventArgs> InvokingRule;


        public event EventHandler<InvokedRuleEventArgs> InvokedRule;


        /// <summary>
        /// Adds the rule signature extension to a particular rule point's signature.
        /// </summary>
        /// <param name="path">The rule point path for which the rule signature extension should be applied.</param>
        /// <param name="signatureExtension">The rule signature extension to register.</param>
        public void AddRuleSignatureExtension(string path, IRuleSignatureExtension signatureExtension)
        {
            RulePoint rulePoint = this.GetRulePoint(path);
            if (null != rulePoint)
                AddRuleSignatureExtension(rulePoint, signatureExtension);
        }


        private void AddRuleSignatureExtension(RulePoint rulePoint, IRuleSignatureExtension signatureExtension)
        {
            RuleSignature signature = rulePoint.Signature;
            if (null != signature)
                signature.AddExtension(signatureExtension);
            foreach (RuleBase rule in rulePoint.Children)
            {
                if (rule is RulePoint)
                {
                    AddRuleSignatureExtension((rule as RulePoint), signatureExtension);
                }
            }
        }


        /// <summary>
        /// Registers a new rule point with specified position.
        /// </summary>
        /// <param name="parentPath">A string representing the position where the rule point goes.</param>
        /// <param name="name">The name of rule point.</param>
        /// <param name="displayName">The display name of rule point.</param>
        /// <param name="signature">The signature of rule point.</param>
        public void RegisterRulePoint(string parentPath, string name, string displayName, RuleSignature signature)
        {
            RulePoint rulePoint = new RulePoint(this, name, signature, displayName);

            // Create rule points recursively.
            RulePoint iter = RootRulePoint;
            string[] pathItems = RulePathHelper.GetRulePathComponents(parentPath);
            foreach (string item in pathItems)
            {
                if (!iter.HasChild(item))
                {
                    iter.AddRulePoint(new RulePoint(this, item));
                }
                iter = iter.GetSubRulePoint(item);
                if (iter == null)
                {
                    throw new InvalidOperationException(Properties.Resources.CannotRegisterRulePoint);
                }
            }

            iter.AddRulePoint(rulePoint);
        }


        /// <summary>
        /// Gets a rule point by a given path.
        /// </summary>
        /// <param name="rulePointPath">The path of the rule point you want to get</param>
        /// <returns>The rule point if it exists; otherwise, null.</returns>
        public RulePoint GetRulePoint(string rulePointPath)
        {
            RulePoint iter = RootRulePoint;
            string[] pathItems = RulePathHelper.GetRulePathComponents(rulePointPath);
            foreach (string item in pathItems)
            {
                iter = iter.GetSubRulePoint(item);
                if (iter == null)
                    return null;
            }
            return iter;
        }


        /// <summary>
        /// Gets a rule point by a given path. Returns the lowest level sub rule point in
        /// the heirarchy which has a valid activity defined.
        /// </summary>
        /// <param name="rulePointPath">The path of the rule point you want to get</param>
        /// <returns>The lowest level rule point in the path that has an activity; otherwise, null.</returns>
        public RulePoint GetRulePointWithActivity(string rulePointPath)
        {
            RulePoint activityPoint = null;
            RulePoint iter = RootRulePoint;
            string[] pathItems = RulePathHelper.GetRulePathComponents(rulePointPath);
            foreach (string item in pathItems)
            {
                iter = iter.GetSubRulePoint(item);
                if (iter == null)
                {
                    return activityPoint;
                }
                else if (iter.HasActivity)
                {
                    activityPoint = iter;
                }
            }
            return activityPoint;
        }


        /// <summary>
        /// Gets a named rule by a given path.
        /// </summary>
        /// <param name="rulePath">The path of the named rule you want to get</param>
        /// <returns>The named rule if it exists; otherwise, null.</returns>
        public NamedRule GetNamedRule(string rulePath)
        {
            if (String.IsNullOrEmpty(rulePath))
            {
                throw new ArgumentNullException("rulePath"); // NOXLATE
            }

            string rulePointPath;
            string ruleName;
            if (RulePathHelper.ExtractNamedRulePath(rulePath, out rulePointPath, out ruleName))
            {
                RulePoint theRulePoint = GetRulePoint(rulePointPath);
                if (theRulePoint != null)
                {
                    return theRulePoint.GetNamedRule(ruleName);
                }
            }
            return null;
        }


        /// <summary>
        /// Invokes a Rule Point, run its routine with given arguments.
        /// </summary>
        /// <param name="rulePointPath">Path of Rule Point</param>
        /// <param name="arguments">Arguments required to run the Rule Point</param>
        /// <returns>Result of rule execution, or null if the rule does not exist.</returns>
        public IDictionary<string, object> InvokeRulePoint(string rulePointPath, Dictionary<string, object> arguments)
        {
            RulePoint theRulePoint = GetRulePoint(rulePointPath);
            // loop until get to the root or rule point has no signature
            // (which means cannot create rule for it and its parents)
            while (theRulePoint != null && theRulePoint.Signature != null)
            {
                if (theRulePoint.HasActivity)
                {
                    return InvokeRule(theRulePoint, arguments);
                }
                else
                {
                    theRulePoint = theRulePoint.Parent;
                }
            }
            return null;
        }


        /// <summary>
        /// Invokes named rule in a specified rulePointPath.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="rulePointPath">The path of rule point which contains the named rule.</param>
        /// <param name="ruleName">The name of the rule</param>
        /// <param name="arguments">The arguments required by this rule</param>
        /// <returns>The execution result of the named rule, or default(T) if the rule does not exist"/></returns>
        public T InvokeNamedRule<T>(string rulePointPath, string ruleName, Dictionary<string, object> arguments)
        {
            if (String.IsNullOrEmpty(rulePointPath))
            {
                throw new ArgumentNullException("rulePointPath"); // NOXLATE
            }
            if (String.IsNullOrEmpty(ruleName))
            {
                throw new ArgumentNullException("ruleName"); // NOXLATE
            }
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments"); // NOXLATE
            }

            RulePoint parentRulePoint = GetRulePoint(rulePointPath);
            if (parentRulePoint == null)
            {
                return default(T);
            }
            NamedRule namedRule = parentRulePoint.GetNamedRule(ruleName);
            if (namedRule == null)
            {
                return default(T);
            }
            return InvokeRule<T>(namedRule, arguments);
        }


        /// <summary>
        /// Invokes named rule specified by rule path.
        /// </summary>
        /// <typeparam name="T">Result type</typeparam>
        /// <param name="rulePath">Path of the named rule</param>
        /// <param name="arguments">Arguments required by the named rule</param>
        /// <returns>Execution result, or default(T) if the rule does not exist</returns>
        public T InvokeNamedRule<T>(string rulePath, Dictionary<string, object> arguments)
        {
            string rulePointPath;
            string ruleName;
            if (RulePathHelper.ExtractNamedRulePath(rulePath, out rulePointPath, out ruleName))
            {
                return InvokeNamedRule<T>(rulePointPath, ruleName, arguments);
            }
            else
            {
                return default(T);
            }
        }


        /// <summary>
        /// Executes a rule and return the exection result.
        /// </summary>
        /// <typeparam name="T">Type of the result</typeparam>
        /// <param name="rule">Rule to execute</param>
        /// <param name="arguments">Arguments for executing the rule</param>
        /// <returns>The execution result</returns>
        public T InvokeRule<T>(IRule rule, IDictionary<string, object> arguments)
        {
            if (rule == null)
            {
                throw new ArgumentNullException("rule"); // NOXLATE
            }
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments"); // NOXLATE
            }

            IDictionary<string, object> results = InvokeRule(rule, arguments);

            if (results.ContainsKey(ResultParameterName))
            {
                object result = results[ResultParameterName];
                return DynamicValueConvert.ConvertTo<T>(result);
            }
            else
            {
                return default(T);
            }
        }


        // Below two members are only used for InvokeRule() method. So I put them together for
        // better readability.
        private int _invokeDepth = 0;
        private const int MaxInvokeDepth = 20;
        private RuleLogger _rootRuleLogger = null;
        /// <summary>
        /// Executes a rule with arguments and return all output arguments by a Dictionary object.
        /// </summary>
        /// <param name="rule">The rule to execute</param>
        /// <param name="arguments">Arguments for executing the rule</param>
        /// <returns>The execution result</returns>
        public IDictionary<string, object> InvokeRule(IRule rule, IDictionary<string, object> arguments)
        {
            DynamicActivity activity = rule.Activity;
            if (activity == null)
            {
                throw new ArgumentException("The rule does not have content (activity)."); // NOXLATE
            }

            RuleExecutingContext executingContext = new RuleExecutingContext(this, rule, arguments);
            VisualBasicSettings vbs = executingContext.GetVisualBasicSettings();
            Microsoft.VisualBasic.Activities.VisualBasic.SetSettings(activity, vbs);


            _invokeDepth++;
            IDictionary<string, object> results = null;
            try
            {
                if (_invokeDepth > MaxInvokeDepth)
                {
                    throw new RuleException(rule.FullName, Properties.Resources.ExceedsMaxInvokeDepth);
                }

                OnInvokingRule(rule, arguments);

                WorkflowInvoker invoker = new WorkflowInvoker(activity);
                RuleLogger logger = null;
                if (this.LoggingEnabled)
                {
                    if (_invokeDepth == 1)
                    {
                        Logger.BeginLogging(rule);
                        _rootRuleLogger = logger;
                    }
                    else
                    {
                        Debug.Assert(rule is NamedRule);
                        _rootRuleLogger.OnInvokingNamedRule(rule as NamedRule);
                        logger = _rootRuleLogger;
                    }

                    invoker.Extensions.Add(logger);
                }

                // Set original input argument to execution context.
                invoker.Extensions.Add(executingContext);

                IDictionary<string, object> workflowArgs = executingContext.GetWorkflowInvokerArguments();
                results = invoker.Invoke(workflowArgs);

                if (null != logger && _invokeDepth == 1)
                {
                    this.LoggingCompleted(this, new LoggingCompletedEventArgs(logger.Log));
                }
                return results;
            }
            catch (RuleException)
            {
                throw;
            }
            catch (InvalidWorkflowException wfex)
            {
                throw new RuleException(rule.FullName, string.Format(Properties.Resources.CommonRuleExceptionUserMessage, wfex.Message));
            }
            catch (Exception ex)
            {
                throw new RuleException(rule.FullName, ex);
            }
            finally
            {
                _invokeDepth--;

                OnInvokedRule(rule, arguments, results);
            }
        }


        /// <summary>
        /// Trigger the event InvokingRule when invoking a rule.
        /// </summary>
        /// <param name="rule">The rule being invoked.</param>
        /// <param name="arguments">The arguments passed to rule.</param>
        protected virtual void OnInvokingRule(IRule rule, IDictionary<string, object> arguments)
        {
            if (InvokingRule != null)
            {
                InvokingRule(this, new InvokingRuleEventArgs(rule, arguments));
            }
        }


        /// <summary>
        /// Trigger the event InvokedRule when a rule has been invoked.
        /// </summary>
        /// <param name="rule">The invoked rule.</param>
        /// <param name="arguments">The argument passed to rule.</param>
        /// <param name="results">The results returned by rule.</param>
        protected virtual void OnInvokedRule(IRule rule, IDictionary<string, object> arguments, IDictionary<string, object> results)
        {
            if (InvokedRule != null)
            {
                InvokedRule(this, new InvokedRuleEventArgs(rule, arguments, results));
            }
        }


        /// <summary>
        /// Finds named rules which can be accepted by given rule signature
        /// </summary>
        /// <param name="signature">The rule signature</param>
        /// <returns>Named rules which can be accepted by given rule signature</returns>
        public IEnumerable<NamedRule> FindNamedRulesBySignature(RuleSignature signature)
        {
            if (signature == null)
            {
                throw new ArgumentNullException("signature");  // NOXLATE
            }
            Queue<RulePoint> workingQueue = new Queue<RulePoint>();
            workingQueue.Enqueue(RootRulePoint);
            while (workingQueue.Count > 0)
            {
                RulePoint theRulePoint = workingQueue.Dequeue();

                if (signature.IsRuleAvailable(theRulePoint))
                {
                    // The named rules of this rule point are all availabe for this signature.
                    // Also add sub-rule points into queue
                    foreach (var namedRule in theRulePoint.GetNamedRules())
                    {
                        yield return namedRule;
                    }
                }
                // Ignore all its named rules(subrules) and add its sub-rule points
                // to the working queue.
                foreach (var rulePoint in theRulePoint.SubRulePoints)
                {
                    workingQueue.Enqueue(rulePoint);
                }
            }
        }


        /// <summary>
        /// Gets all registered rule points
        /// </summary>
        /// <returns>Rule points</returns>
        public IEnumerable<RulePoint> GetAllRulePoints()
        {
            Queue<RulePoint> workingQueue = new Queue<RulePoint>();
            workingQueue.Enqueue(RootRulePoint);
            while (workingQueue.Count > 0)
            {
                RulePoint theRulePoint = workingQueue.Dequeue();
                yield return theRulePoint;

                foreach (var rulePoint in theRulePoint.SubRulePoints)
                {
                    workingQueue.Enqueue(rulePoint);
                }
            }
        }
    }
}
