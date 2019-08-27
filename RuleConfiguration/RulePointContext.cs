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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

using Autodesk.IM.Rule;
using Autodesk.IM.UI.Rule;
using RuleConfiguration;


namespace RuleConfiguration
{
    public class RulePointContext : RuleBaseContext
    {
        internal const string RulePointDisplayFormat = "{0} >> {1}"; //NOXLATE

        private RuleSignature _signature;

        public RulePointContext(RulePoint rulePoint, RulePointContext parentContext, RuleConfigContext ruleConfigContext)
            : base(rulePoint.Name, parentContext, ruleConfigContext)
        {
            if (rulePoint == null)
            {
                throw new ArgumentNullException("rulePoint");   // NOXLATE
            }

            _displayName = rulePoint.DisplayName;
            UpdateDisplayTexts();

            _signature = rulePoint.Signature;

            if (_signature != null)
            {
                DeleteContentOperation = new OperationContext(
                    Properties.Resources.DeleteRulePointDisplayName,
                    new RelayCommand(obj => DeleteContent()));
                CreateContentOperation = new OperationContext(
                    Properties.Resources.CreateRuleContentDisplayName,
                    new RelayCommand(obj => CreateContent()));
                CreateNamedRuleOperation = new OperationContext(
                    Properties.Resources.CreateNewSubruleDisplayName,
                    new RelayCommand(obj => CreateNewNamedRule(Properties.Resources.DefaultNewSubruleName)));
                CopyOperation = new OperationContext(
                    Properties.Resources.CopyRulePointDisplayName,
                    new RelayCommand(obj => CopyToClipboard(), obj => HasContent));
                PasteNamedRuleOperation = new OperationContext(
                    Properties.Resources.PasteNamedRuleDisplayName,
                    new RelayCommand(obj => PasteAsNamedRule(), obj => CanPasteNamedRule()));
            }
        }


        private string _displayName;
        public override string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                if (_displayName == value)
                {
                    return;
                }

                _displayName = value;
                UpdateDisplayTexts();

                OnPropertyChanged("DisplayName"); // NOXLATE
            }
        }


        public override bool CanRename
        {
            get
            {
                return false;
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

                if (_signature != null)
                {
                    if (HasContent)
                    {
                        yield return DeleteContentOperation;
                    }
                    else
                    {
                        yield return CreateContentOperation;
                    }

                    yield return CreateNamedRuleOperation;
                    yield return CopyOperation;
                    yield return PasteNamedRuleOperation;
                }
            }
        }


        private ObservableCollection<RuleBaseContext> _children = null;
        public ObservableCollection<RuleBaseContext> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new ObservableCollection<RuleBaseContext>();
                    RulePoint rulePoint = GetRulePoint();
                    Debug.Assert(rulePoint != null);

                    foreach (var item in rulePoint.Children)
                    {
                        if (item is NamedRule)
                        {
                            _children.Add(new NamedRuleContext(item as NamedRule, this, _ruleConfigContext));
                        }
                        else if (item is RulePoint)
                        {
                            _children.Add(new RulePointContext(item as RulePoint, this, _ruleConfigContext));
                        }
                        else
                        {
                            Debug.Assert(false, "Should not go here.");  // NOXLATE
                        }
                    }

                    _children.CollectionChanged += (s, e) =>
                        {
                            RefreshSortedChildren();
                        };
                }
                return _children;
            }
        }


        private CollectionViewSource _sortedChildren = null;
        public ICollectionView SortedChildrenView
        {
            get
            {
                if (_sortedChildren == null)
                {
                    _sortedChildren = new CollectionViewSource();
                    _sortedChildren.Source = Children;
                    _sortedChildren.SortDescriptions.Add(
                        new SortDescription("IsNamedRule", System.ComponentModel.ListSortDirection.Descending)); //NOXLATE
                    _sortedChildren.SortDescriptions.Add(
                        new SortDescription("DisplayName", System.ComponentModel.ListSortDirection.Ascending)); //NOXLATE
                }
                return _sortedChildren.View;
            }
        }


        public OperationContext CreateContentOperation
        {
            get;
            private set;
        }


        public OperationContext DeleteContentOperation
        {
            get;
            private set;
        }


        public OperationContext CreateNamedRuleOperation
        {
            get;
            private set;
        }


        public OperationContext CopyOperation
        {
            get;
            private set;
        }


        public OperationContext PasteNamedRuleOperation
        {
            get;
            private set;
        }


        public override bool IsNamedRule
        {
            get
            {
                return false;
            }
        }


        public override RuleSignature Signature
        {
            get
            {
                return _signature;
            }
        }


        public override string DisplayPath
        {
            get
            {
                // NOTE: the RulePointDisplayName of the rule point context does NOT include the display
                // name of the rule point itself, but only the parent names, without including any which
                // have valid signatures. This is to filter out the many feature class hierarchy points.
                if (_parentContext != null)
                {
                    string parentDisplayPath = _parentContext.DisplayPath;
                    if (!String.IsNullOrEmpty(parentDisplayPath))
                    {
                        RulePoint thisRulePoint = this.GetRulePoint();
                        RulePoint parentRulePoint = _parentContext.GetRulePoint();
                        if (null != thisRulePoint && null != parentRulePoint)
                        {
                            if (null == thisRulePoint.Signature && null == parentRulePoint.Signature)
                                return String.Format(RulePointDisplayFormat, parentDisplayPath, this.DisplayName);
                        }
                        return parentDisplayPath;
                    }
                }
                return this.DisplayName;
            }
        }


        public override RuleEditingContext GetEditingContext()
        {
            var ruleManager = RuleAppExtension.RuleManagerInstance;
            RulePoint rulePoint = ruleManager.GetRulePoint(Path);
            return new RuleEditingContext(rulePoint.Signature);
        }


        public override RuleBase GetRule()
        {
            return GetRulePoint();
        }


        public override void Commit()
        {
            if (String.IsNullOrEmpty(Text))
            {
                // If the content is empty, we should remove content from rule storage.
                RulePoint rulePoint = GetRulePoint();
                Debug.Assert(rulePoint != null);
                rulePoint.Delete();
            }
            else
            {
                // Save the content.
                AudActivitySerializer serializer = new AudActivitySerializer();
                using (StringReader sr = new StringReader(Text))
                {
                    DynamicActivity da = serializer.Deserialize(sr, typeof(DynamicActivity)) as DynamicActivity;
                    var ruleManager = RuleAppExtension.RuleManagerInstance;
                    var rulePoint = ruleManager.GetRulePoint(Path);
                    rulePoint.Save(da);
                }
            }
            IsDirty = false;
        }

        // Make sure this function is called in UI thread.
        public void RefreshSortedChildren()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(
                new Action(() =>
                {
                    SortedChildrenView.Refresh();
                }),
                DispatcherPriority.ApplicationIdle,
                new object[] { });
        }


        private RuleBaseContext CreateNewNamedRule(string initialRuleName, string defaultContent = null)
        {
            string ruleName = GenerateNamedRuleName(initialRuleName);
            try
            {
                NamedRuleContext newNamedRule = new NamedRuleContext(ruleName, this, _ruleConfigContext);
                newNamedRule.Text = defaultContent ?? CreateDefaultContent();
                Children.Add(newNamedRule);
                IsExpanded = true;
                newNamedRule.IsSelected = true;
                _ruleConfigContext.IsDirty = true;
                return newNamedRule;
            }
            catch (Exception)
            {
                MessageBox.Show(
                    Properties.Resources.CreateNewSubruleFailed,
                    Properties.Resources.Warning,
                    MessageBoxButton.OK);
                return null;
            }
        }


        private void DeleteContent()
        {
            try
            {
                Text = String.Empty;
                // re-select this rule to refresh rule designer
                _ruleConfigContext.SelectedRule = null;
                _ruleConfigContext.SelectedRule = this;
            }
            catch (Exception)
            {
                MessageBox.Show(
                    String.Format(Properties.Resources.FailedToDelete, Path),
                    Properties.Resources.Error,
                    MessageBoxButton.OK);
            }

            OnPropertyChanged("Operations"); // NOXLATE
        }


        private void CreateContent()
        {
            try
            {
                Text = CreateDefaultContent();

                // Re-select this rule to edit it.
                _ruleConfigContext.SelectedRule = null;
                _ruleConfigContext.SelectedRule = this;
                OnPropertyChanged("Operations"); // NOXLATE
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    String.Format(Properties.Resources.FailedToCreateRule, ex.Message),
                    Properties.Resources.Error,
                    MessageBoxButton.OK);
            }
        }


        private void PasteAsNamedRule()
        {
            var ruleToPaste = _ruleConfigContext.GetRuleFromClipboard();
            Debug.Assert(ruleToPaste != null);

            // Paste as a named rule
            var newRule = CreateNewNamedRule(ruleToPaste.DisplayName, ruleToPaste.Text);

            // Delete original rule if it is cut.
            if (ruleToPaste.IsCutted)
            {
                ruleToPaste.Parent.Children.Remove(ruleToPaste);
                _ruleConfigContext.CuttedRule = null;
                try
                {
                    Clipboard.Clear();
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // Ignore known COMExcpetion.
                }
            }
        }


        private bool CanPasteNamedRule()
        {
            var rule = _ruleConfigContext.GetRuleFromClipboard();

            if (rule != _ruleConfigContext.CuttedRule)
            {
                _ruleConfigContext.CuttedRule = null;
            }

            // The copied subrule must be available to this rule point.
            return rule != null && GetRule().Signature.Match(rule.Signature);
        }


        private string CreateDefaultContent()
        {
            RulePoint rulePoint = GetRulePoint();
            Debug.Assert(rulePoint != null && rulePoint.Signature != null);
            DynamicActivity activity = rulePoint.Signature.CreateDefaultRoutine();
            activity.Name = RuleBase.RuleClassName;
            ActivitySerializer serializer = new ActivitySerializer();
            return serializer.Serialize(activity);
        }


        private RulePoint GetRulePoint()
        {
            RuleManager ruleManager = RuleAppExtension.RuleManagerInstance;
            return ruleManager.GetRulePoint(Path);
        }


        /// <summary>
        /// Get a proper name for new named rule according to initial name.
        /// </summary>
        /// <param name="initialName">The initial candidate name</param>
        /// <returns>The proper name</returns>
        private string GenerateNamedRuleName(string initialName)
        {
            string ruleName = initialName;
            int tryCount = 0;
            while (true)
            {
                var finding = Children.FirstOrDefault((r) => String.Compare(r.Name, ruleName, true) == 0);
                if (finding == null)
                {
                    break;
                }

                ruleName = String.Format("{0}_{1}", initialName, ++tryCount);  // NOXLATE
            }

            return ruleName;
        }
    }
}
