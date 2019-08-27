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
using System.Activities;
using System.Collections.Generic;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Represents a named rule which is invoked by other rules. Named rule is called subrule in UI.
    /// </summary>
    [Serializable]
    public class NamedRule : RuleBase
    {
        /// <summary>
        /// Constructor used for deserialization. Must invoke its Initialize(RuleManager) method
        /// before use the instance.
        /// </summary>
        public NamedRule()
        { }


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.NamedRule class with specified owner, name and parent Rule Point.
        /// </summary>
        /// <param name="owner">The specified RuleManager object as its owner.</param>
        /// <param name="name">The specified name.</param>
        /// <param name="parent">The Rule Point object as its parent.</param>
        public NamedRule(RuleManager owner, string name, RulePoint parent)
        {
            RuleName = name;
            if (parent != null)
            {
                ParentRulePointPath = parent.Path;
            }

            Initialize(owner);
        }


        /// <summary>
        /// Gets owner of this named rule. The owner is an instance of Autodesk.IM.RuleManager class.
        /// </summary>
        private RuleManager _owner;
        public override RuleManager Owner
        {
            get
            {
                return _owner;
            }
        }


        /// <summary>
        /// Gets or sets the path of rule point which is parent of the named rule.
        /// </summary>
        public string ParentRulePointPath
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the rule name.
        /// </summary>
        public string RuleName
        {
            get;
            set;
        }


        public override string Name
        {
            get
            {
                return RuleName;
            }
        }


        public override string DisplayName
        {
            get
            {
                return Name;
            }
        }


        public override RulePoint Parent
        {
            get
            {
                return Owner.GetRulePoint(ParentRulePointPath);
            }
        }


        public override RuleSignature Signature
        {
            get
            {
                return Parent.Signature;
            }
        }


        public override string ActivityID
        {
            get
            {
                return String.Format("{0};{1}", this.ParentRulePointPath, this.Name); // NOXLATE
            }
        }


        /// <summary>
        /// Initializes the instance with specified owner.
        /// </summary>
        /// <param name="owner">The specified owner.</param>
        public void Initialize(RuleManager owner)
        {
            _owner = owner;
        }


        public override void Save(DynamicActivity da)
        {
            Signature.ApplyExtendedArguments(da);
            Owner.Storage.SetNamedRule(this, da);
        }


        public override void Delete()
        {
            Owner.Storage.RemoveNamedRule(this);
        }
    }
}
