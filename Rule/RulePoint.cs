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
using System.Collections.ObjectModel;
using System.Linq;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Represents a locations where a specific kind of rule
    /// could be invoked.  It associates with a RuleSignature instance which defines
    /// the rule for this rule point.  It also includes sub rule points so that all rule
    /// points compose a rule point hierarchy.
    /// </summary>
    public class RulePoint : RuleBase
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.RulePoint with specified owner,
        /// name, signature, display name.
        /// </summary>
        /// <param name="owner">An object of RuleManager which owns this rule point.</param>
        /// <param name="name">The specified rule point's name.</param>
        /// <param name="signature">The specified rule point's signature.</param>
        /// <param name="displayName">The rule point's display name.</param>
        public RulePoint(RuleManager owner, string name = "", RuleSignature signature = null, string displayName = null)
        {
            // Fix defect 20970 <https://fogbugzaec.autodesk.com/default.asp?20970> - When a Catalog 
            // Category contains slash "/", adding a material rule causes crash.
            // Because the name will be used for Autodesk.UtilityDesign.UI.Rule.RuleBaseContext.Path,
            // so if it contains the character "/", that will result in an invalid path.
            // Solution: Replace character "/" with its url encoding "%2F". 
            // Note that we don't need to decode this change. The Name property is only used for identifying 
            // the RulePoint, and the DisplayName property is what actually gets displayed in the UI.
            if (name.Contains("/"))
            {
                name = name.Replace("/", "%2F");
            }

            _owner = owner;
            _name = name;
            _displayName = displayName;
            _signature = signature;
        }


        private RuleManager _owner;
        public override RuleManager Owner
        {
            get
            {
                return _owner;
            }
        }


        private string _name;
        public override string Name
        {
            get
            {
                return _name;
            }
        }


        private string _displayName = null;
        public override string DisplayName
        {
            get
            {
                return _displayName == null ? Name : _displayName;
            }
        }


        private RulePoint _parent;
        public override RulePoint Parent
        {
            get
            {
                return _parent;
            }
        }


        private RuleSignature _signature;
        public override RuleSignature Signature
        {
            get
            {
                return _signature;
            }
        }


        public override string ActivityID
        {
            get
            {
                return String.Format("{0};", this.Path); // NOXLATE
            }
        }


        private ObservableCollection<RulePoint> subRulePoints = new ObservableCollection<RulePoint>();
        /// <summary>
        /// Gets its children.
        ///
        /// Note: the property is slower than normal property.
        /// </summary>
        public IEnumerable<RuleBase> Children
        {
            get
            {
                foreach (var item in SubRulePoints)
                {
                    yield return item;
                }

                foreach (var namedRule in GetNamedRules())
                {
                    yield return namedRule;
                }
            }
        }


        /// <summary>
        /// Returns all sub-rule points of this rule point.
        /// </summary>
        public IEnumerable<RulePoint> SubRulePoints
        {
            get
            {
                return subRulePoints;
            }
        }


        /// <summary>
        /// Adds a sub rule point.
        /// </summary>
        /// <param name="item">sub rule point to add</param>
        public void AddRulePoint(RulePoint item)
        {
            if (HasChild(item.Name))
            {
                throw new ArgumentException(String.Format(Properties.Resources.NamedRuleAlreadyExists, item.Name));
            }

            item._parent = this;
            subRulePoints.Add(item);
        }


        /// <summary>
        /// Creates named rule as child of this rule point.
        /// </summary>
        /// <param name="name">Name of the new one.</param>
        /// <returns>The newly-created named rule.</returns>
        public NamedRule CreateNamedRule(string name)
        {
            if (HasChild(name))
                throw new ArgumentException(String.Format(Properties.Resources.NamedRuleAlreadyExists, name));

            NamedRule newOne = new NamedRule(Owner, name, this);
            try
            {
                var defaultWorkflow = newOne.Signature.CreateDefaultRoutine();
                defaultWorkflow.Name = RuleClassName;
                Owner.Storage.SetNamedRule(newOne, defaultWorkflow);
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.FailToAddSubRule, ex);
            }

            return newOne;
        }


        /// <summary>
        /// Creates default workflow for the rule point.
        /// </summary>
        public void CreateDefaultWorkflow()
        {
            if (Signature != null)
            {
                DynamicActivity da = Signature.CreateDefaultRoutine();
                da.Name = RuleClassName;
                Owner.Storage.SetRulePointWorkflow(this, da);
            }
            else
            {
                throw new InvalidOperationException("The signature is null."); // NOXLATE
            }
        }


        /// <summary>
        /// Removes named rule by given name.
        /// </summary>
        /// <param name="name">The name of named rule to remove</param>
        public void RemoveNamedRule(string name)
        {
            NamedRule theOneToRemove = GetNamedRule(name);

            if (theOneToRemove != null)
            {
                Owner.Storage.RemoveNamedRule(theOneToRemove);
            }
            else
            {
                throw new ArgumentException(String.Format(Properties.Resources.NamedRuleNotExist, name));
            }
        }


        /// <summary>
        /// Gets sub rule point by given name.
        /// </summary>
        public RulePoint GetSubRulePoint(string name)
        {
            return SubRulePoints.FirstOrDefault((rp) => String.Compare(rp.Name, name, true) == 0);
        }


        /// <summary>
        /// Gets the specified named rule among its children.
        /// </summary>
        /// <param name="name">The specified name to find named rule.</param>
        /// <returns>A named rule if it exists; otherwise, null.</returns>
        public NamedRule GetNamedRule(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name"); // NOXLATE
            }
            return GetNamedRules().FirstOrDefault((nr) => String.Compare(nr.Name, name, true) == 0);
        }


        /// <summary>
        /// Gets named rules among its children.
        /// </summary>
        /// <returns>An emunerable object to access named rules.</returns>
        public IEnumerable<NamedRule> GetNamedRules()
        {
            return Owner.Storage.GetNamedRules(this.Path);
        }


        /// <summary>
        /// Returns true if the rule point has named rule or sub rule point; otherwise, false.
        /// </summary>
        public bool HasChild(string childName)
        {
            return Children.Any((child) => String.Compare(child.Name, childName, true) == 0);
        }


        public override void Save(DynamicActivity da)
        {
            Signature.ApplyExtendedArguments(da);
            Owner.Storage.SetRulePointWorkflow(this, da);
        }


        public override void Delete()
        {
            Owner.Storage.RemoveRulePointWorkflow(this);
        }
    }
}
