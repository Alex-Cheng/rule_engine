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
using System.Activities.Statements;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Represents a rule signature.
    /// </summary>
    public class RuleSignature : ActivitySignature
    {
        private List<IRuleSignatureExtension> _extensions = new List<IRuleSignatureExtension>();

        private RuleManager _owner;


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.RuleSignature with specified owner.
        /// </summary>
        /// <param name="owner">The owner which manages this rule signature.</param>
        public RuleSignature(RuleManager owner)
        {
            _owner = owner;
        }


        /// <summary>
        /// Gets its owner, an instance of RuleManager.
        /// </summary>
        [XmlIgnore]
        public RuleManager Owner
        {
            get
            {
                return _owner;
            }
        }


        /// <summary>
        /// Returns the list of extensions added to this rule signature.
        /// </summary>
        /// <returns>The rule signature extensions</returns>
        public IEnumerable<IRuleSignatureExtension> Extensions
        {
            get
            {
                return _extensions;
            }
        }


        /// <summary>
        /// Adds a rule signature extension to the rule signature.
        /// </summary>
        /// <param name="extension">The rule signature extension to be added</param>
        public void AddExtension(IRuleSignatureExtension extension)
        {
            if (_extensions.Contains(extension))
                return;
            _extensions.Add(extension);
        }


        /// <summary>
        /// Creates a default routine.
        /// </summary>
        /// <returns>Default routine</returns>
        public virtual DynamicActivity CreateDefaultRoutine()
        {
            DynamicActivity da = new DynamicActivity()
            {
                Implementation = () => new Sequence { }
            };

            // add in arguments
            foreach (var item in SystemInArguments)
            {
                da.Properties.Add(new DynamicActivityProperty
                {
                    Name = item.Name,
                    Type = typeof(InArgument<>).MakeGenericType(item.ArgumentType)
                });
            }

            // add out arguments
            foreach (var item in SystemOutArguments)
            {
                da.Properties.Add(new DynamicActivityProperty
                {
                    Name = item.Name,
                    Type = typeof(OutArgument<>).MakeGenericType(item.ArgumentType)
                });
            }

            return da;
        }


        /// <summary>
        /// Checks if given rule can be used in rules defined
        /// by this rule signature.
        /// </summary>
        /// <param name="rule">Rule</param>
        /// <returns>Return true if it can be used, otherwise return false.</returns>
        public virtual bool IsRuleAvailable(RuleBase rule)
        {
            if (rule.Signature == null)
            {
                return false;
            }
            return Match(rule.Signature);
        }


        /// <summary>
        /// Checks if the given activity can be used in rules defined
        /// by this rule signature
        /// </summary>
        /// <param name="activityEntry">Activity</param>
        /// <returns>Return true if it can be used, otherwise return false.</returns>
        public virtual bool IsActivityAvailable(ActivityEntry activityEntry)
        {
            return Match(activityEntry.Signature);
        }


        /// <summary>
        /// Checks if the given signature matches to this signature.
        /// </summary>
        /// <param name="signature">The given signature</param>
        /// <returns>Return true if match, otherwise return false.</returns>
        public bool Match(ActivitySignature signature)
        {
            // Check if input arguments are covered.
            foreach (var inArg in signature.SystemInArguments)
            {
                if (this.SystemInArguments.Find((arg) => arg.Id == inArg.Id) == null)
                {
                    return false;
                }
            }

            return this.CanModifyData || !signature.CanModifyData;
        }


        /// <summary>
        /// Returns providers of expression items.
        /// </summary>
        /// <returns>A collection of providers of expression items</returns>
        public virtual IEnumerable<IExpressionItemProvider> GetExpressionItemProviders()
        {
            yield return new NamedRuleExpressionItemProvider(
                this,
                Properties.Resources.NamedRuleProviderName,
                Properties.Resources.NamedRuleProviderDisplayName);
            foreach (IRuleSignatureExtension extension in _extensions)
                foreach (IExpressionItemProvider provider in extension.GetExpressionItemProviders())
                    yield return provider;
        }


        /// <summary>
        /// Adds used extended arguments to DynamicActivity.Properties and remove unused extended
        /// arguments from DynamicActivity.Properties.
        /// </summary>
        /// <param name="da">The DynamicActivity to which we apply extended arguments</param>
        public void ApplyExtendedArguments(DynamicActivity da)
        {
            // Get serialized XAML
            ActivitySerializer serializer = Owner.Storage.Serializer;
            string xaml = serializer.Serialize(da);

            foreach (IRuleSignatureExtension extension in Extensions)
            {
                foreach (RuleArgument arg in extension.GetSystemInArguments())
                {
                    string argumentName = arg.Name;
                    string keyword = String.Format("[{0}]", argumentName);
                    bool isUsed = xaml.Contains(keyword);
                    if (isUsed)
                    {
                        // Make sure the corresponding property exists.
                        if (!da.Properties.Contains(argumentName))
                        {
                            da.Properties.Add(new DynamicActivityProperty
                            {
                                Name = argumentName,
                                Type = typeof(InArgument<>).MakeGenericType(arg.ArgumentType)
                            });
                        }
                    }
                    else
                    {
                        // Make sure the property is removed.
                        if (da.Properties.Contains(argumentName))
                        {
                            da.Properties.Remove(argumentName);
                        }
                    }
                }
            }
        }
    }
}
