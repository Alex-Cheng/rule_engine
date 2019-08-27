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
using System.Diagnostics;
using System.IO;
using System.Linq;

using Autodesk.IM.Rule;
using Autodesk.IM.UI.Rule;
using RuleConfiguration;


namespace RuleConfiguration
{
    public class NamedRuleContext : RuleBaseContext
    {
        const string IllegalChars = "'"; // NOXLATE


        public NamedRuleContext(NamedRule namedRule, RulePointContext parentContext, RuleConfigContext ruleConfigContext)
            : this(namedRule.Name, parentContext, ruleConfigContext)
        {
        }

        public NamedRuleContext(string name, RulePointContext parentContext, RuleConfigContext ruleConfigContext)
            : base(name, parentContext, ruleConfigContext)
        {
            DeleteOperation = new OperationContext(
                Properties.Resources.DeleteNamedRuleDisplayName,
                new RelayCommand(obj => Delete()));
            CopyOperation = new OperationContext(
                Properties.Resources.CopyNamedRuleDisplayName,
                new RelayCommand(obj => CopyToClipboard()));
            CutOperation = new OperationContext(
                Properties.Resources.CutNamedRuleDisplayName,
                new RelayCommand(obj => Cut()));
            UpdateDisplayTexts();
        }

        public override string DisplayName
        {
            // TODO: decouple name and display name for named rule.
            get
            {
                return Name;
            }
            set
            {
                if (Name == value)
                {
                    return;
                }

                if (String.IsNullOrEmpty(value))
                {
                    throw new InvalidOperationException(Properties.Resources.NameCannotBeEmpty);
                }
                else if (ContainIllegalChar(value))
                {
                    throw new ArgumentException(String.Format(Properties.Resources.RuleNameContainsIllegalChar, value));
                }
                else if (IsDuplicateName(value))
                {
                    throw new InvalidOperationException(String.Format(Properties.Resources.DuplicateNameExists, value));
                }

                Name = value;
                UpdateDisplayTexts();
                Parent.RefreshSortedChildren();

                OnPropertyChanged("DisplayName"); // NOXLATE
            }
        }

        public override bool CanRename
        {
            get
            {
                return true;
            }
        }

        public override IEnumerable<OperationContext> Operations
        {
            get
            {
                foreach (var item in base.Operations)
                {
                    yield return item;
                }

                yield return CopyOperation;
                yield return CutOperation;
                yield return DeleteOperation;
            }
        }


        public override bool IsExpanded
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotSupportedException();
            }
        }


        public OperationContext CopyOperation
        {
            get;
            private set;
        }


        public OperationContext CutOperation
        {
            get;
            private set;
        }


        public OperationContext DeleteOperation
        {
            get;
            private set;
        }


        public override RuleSignature Signature
        {
            get
            {
                return Parent.Signature;
            }
        }


        public override RuleEditingContext GetEditingContext()
        {
            return _parentContext.GetEditingContext();
        }


        public override void Commit()
        {
            AudActivitySerializer serializer = new AudActivitySerializer();
            using (StringReader sr = new StringReader(Text))
            {
                DynamicActivity da = serializer.Deserialize(sr, typeof(DynamicActivity)) as DynamicActivity;
                var ruleManager = RuleAppExtension.RuleManagerInstance;
                var namedRule = ruleManager.GetNamedRule(Path);
                if (namedRule != null)
                {
                    namedRule.Save(da);
                }
                else
                {
                    var rulePoint = ruleManager.GetRulePoint(_parentContext.Path);
                    var newNamedRule = rulePoint.CreateNamedRule(Name);
                    newNamedRule.Save(da);
                }
            }
            IsDirty = false;
        }


        private void Delete()
        {
            // If the named rule has siblings, move to next named rule.
            int index = _parentContext.Children.IndexOf(this);
            Debug.Assert(index != -1, "Its parent must contain it."); //NOXLATE

            RuleBaseContext nextRule = null;
            if (index + 1 < _parentContext.Children.Count) // Try to get next sibling
            {
                nextRule = _parentContext.Children.ElementAt(index + 1);
            }
            else if(index - 1 >= 0) // Try to get previous sibling
            {
                nextRule = _parentContext.Children.ElementAt(index - 1);
            }

            _parentContext.Children.Remove(this);
            int idx = _ruleConfigContext.SearchResult.IndexOf(this);
            if (idx != -1)
            {
                _ruleConfigContext.SearchResult.RemoveAt(idx);
            }
            _ruleConfigContext.IsDirty = true;

            if (nextRule != null)
            {
                nextRule.IsSelected = true;
            }
        }


        private void Cut()
        {
            CopyToClipboard();
            _ruleConfigContext.CuttedRule = this;
        }


        public override bool IsNamedRule
        {
            get
            {
                return true;
            }
        }

        public override string DisplayPath
        {
            get
            {
                // the RulePointDisplayName for a rule point does NOT include the rule point's name
                // so for a named rule we need to append the rule point's display name, as that is
                // the direct 'parent' of the named rule.
                if (_parentContext != null)
                {
                    return String.Format(RulePointContext.RulePointDisplayFormat,
                        _parentContext.DisplayPath, _parentContext.DisplayName);
                }
                return String.Empty;
            }
        }

        public override RuleBase GetRule()
        {
            return GetNamedRule();
        }

        private NamedRule GetNamedRule()
        {
            RuleManager ruleManager = RuleAppExtension.RuleManagerInstance;
            return ruleManager.GetNamedRule(Path);
        }

        private bool IsDuplicateName(string value)
        {
            // TODO: if we adopt GUID as named rule's name, we will not need to compare the name property.
            return _parentContext.Children.Count(
                (n) =>
                    n != this &&
                    (String.Compare(n.DisplayName, value, true) == 0 ||
                    String.Compare(n.Name, value, false) == 0))
                    > 0;
        }

        private bool ContainIllegalChar(string value)
        {
            foreach (var c in IllegalChars.ToCharArray())
            {
                if (value.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
