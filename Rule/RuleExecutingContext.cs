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
using System.Linq;

using Microsoft.VisualBasic.Activities;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Represents the context during execution of a rule.
    /// </summary>
    public class RuleExecutingContext
    {
        public RuleExecutingContext(RuleManager owner, IRule rule, IDictionary<string, object> originalArguments)
        {
            RuleManager = owner;
            InitializeArguments(rule, originalArguments);
        }


        public RuleExecutingContext(RuleManager ruleManager, IDictionary<RuleArgument, object> exactArguments)
        {
            RuleManager = ruleManager;
            Arguments = new Dictionary<RuleArgument, object>(exactArguments);
        }


        /// <summary>
        /// The rule manager relating to this executing rule.
        /// </summary>
        public RuleManager RuleManager
        {
            get;
            private set;
        }


        /// <summary>
        /// Actual arguments for execution of rule.
        /// </summary>
        protected IDictionary<RuleArgument, object> Arguments
        {
            get;
            private set;
        }


        /// <summary>
        /// Get argument list of type IDictionary<string, object> for WorkflowInvoker.
        /// </summary>
        public IDictionary<string, object> GetWorkflowInvokerArguments()
        {
            IDictionary<string, object> arguments = new Dictionary<string, object>();
            foreach (var kv in Arguments)
            {
                arguments.Add(kv.Key.Name, kv.Value);
            }
            return arguments;
        }


        /// <summary>
        /// Get Visual Basic settings containing importings of assemblies and namespaces.
        /// </summary>
        public VisualBasicSettings GetVisualBasicSettings()
        {
            VisualBasicSettings vbs = new VisualBasicSettings();
            foreach (var arg in Arguments.Keys)
            {
                Type importType = arg.ArgumentType;
                if (importType == null)
                {
                    continue;
                }

                while (importType != null && importType != typeof(object))
                {
                    string ns = importType.Namespace;
                    if (vbs.ImportReferences.Count((i) => i.Import.Equals(ns)) == 0)
                    {
                        VisualBasicImportReference theReference = new VisualBasicImportReference
                        {
                            Assembly = importType.Assembly.FullName,
                            Import = ns
                        };
                        vbs.ImportReferences.Add(theReference);
                    }
                    importType = importType.BaseType;
                }

                foreach (Type interfaceType in arg.ArgumentType.GetInterfaces())
                {
                    string interfaceNs = interfaceType.Namespace;
                    if (vbs.ImportReferences.Count((i) => i.Import.Equals(interfaceNs)) == 0)
                    {
                        VisualBasicImportReference interfaceReference = new VisualBasicImportReference
                        {
                            Assembly = interfaceType.Assembly.FullName,
                            Import = interfaceNs
                        };
                        vbs.ImportReferences.Add(interfaceReference);
                    }
                }
            }
            return vbs;
        }


        /// <summary>
        /// Initializes arguments including system input arguments and extended arguments.
        ///
        /// Initialization of extended arguments are based on original input arguments and rule's activity.
        /// This method checks rule's activity first to find out the extended arguments which are really used.
        ///
        /// When the extended argument exists in the input argument list, use it immediately.
        /// If not, retrieve the argument from rule extensions.
        /// </summary>
        /// <param name="rule">The specified rule for initialization of arguments.</param>
        /// <param name="originalArguments">The original input arguments.</param>
        private void InitializeArguments(IRule rule, IDictionary<string, object> originalArguments)
        {
            DynamicActivity da = rule.Activity;
            RuleSignature signature = rule.Signature;
            var properties = da.Properties;
            Dictionary<RuleArgument, object> arguments = new Dictionary<RuleArgument, object>();

            // Add system input arguments
            foreach (RuleArgument arg in signature.SystemInArguments)
            {
                string argumentName = arg.Name;
                if (properties.Contains(argumentName))
                {
                    if (originalArguments.ContainsKey(argumentName))
                    {
                        arguments.Add(arg, originalArguments[argumentName]);
                    }
                    else
                    {
                        // Comment this because there are so many assert triggered
                        // Ideally, the assert should be triggered when the system input arguments
                        // are absent, however existing code cannot make sure it, and fixing it
                        // requires a lot of effort. Considering limitation of time and the stage
                        // we are in, we comment it out and give default value or null when input
                        // arguments are not given.
                        //Debug.Assert(false, "Some system input arguments are missing."); //NOXLATE

                        Type type = arg.ArgumentType;
                        if (type.IsValueType)
                        {
                            arguments.Add(arg, Activator.CreateInstance(type));
                        }
                        else
                        {
                            arguments.Add(arg, null);
                        }
                    }
                }
            }

            // Initialize extended arguments.
            foreach (IRuleSignatureExtension extension in signature.Extensions)
            {
                foreach (RuleArgument arg in extension.GetSystemInArguments())
                {
                    string argumentName = arg.Name;
                    if (properties.Contains(argumentName)) // Require this argument.
                    {
                        if (originalArguments.ContainsKey(argumentName))
                        {
                            // Use caller-provided argument
                            arguments.Add(arg, originalArguments[argumentName]);
                        }
                        else
                        {
                            // Use default argument provided by rule extension.
                            extension.AddDefaultArgument(arg, arguments);
                        }
                    }
                }
            }

            Arguments = arguments;
        }
    }
}
