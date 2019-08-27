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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using Autodesk.IM.Rule;
using Autodesk.IM.UI.Rule;
using IMConstants = Autodesk.IM.UI.Rule.Constants;
using RuleConfiguration;



namespace RuleConfiguration
{
    public class RuleConfigContext : ContextBase
    {
        public RuleConfigContext()
        {
            RootRulePoint = new RulePointContext(RuleAppExtension.RuleManagerInstance.RootRulePoint, null, this);
            ApplyCommand = new RelayCommand(new Action<object>(Apply), new Predicate<object>(ApplyCanExecute));
        }

        private RuleBaseContext _selectedRule = null;
        public RuleBaseContext SelectedRule
        {
            get
            {
                return _selectedRule;
            }
            set
            {
                if (_selectedRule != value)
                {
                    _selectedRule = value;
                    if (_selectedRule != null)
                    {
                        var ruleIter = _selectedRule.Parent;
                        while (ruleIter != null)
                        {
                            if (!ruleIter.IsExpanded)
                            {
                                ruleIter.IsExpanded = true;
                            }
                            ruleIter = ruleIter.Parent;
                        }
                    }
                    OnPropertyChanged("SelectedRule"); // NOXLATE
                }

                // See if the selected node can be edit.
                if (_selectedRule != null && _selectedRule.HasContent)
                {
                    if (EditingRule != SelectedRule)
                    {
                        EditingRule = _selectedRule;
                    }
                }
                else
                {
                    EditingRule = null;
                }
            }
        }

        private RuleBaseContext _editingRule = null;
        public RuleBaseContext EditingRule
        {
            get
            {
                return _editingRule;
            }
            private set
            {
                _editingRule = value;
                OnPropertyChanged("EditingRule"); // NOXLATE
            }
        }

        private bool _isDirty = false;
        public bool IsDirty
        {
            get
            {
                return _isDirty;
            }
            set
            {
                _isDirty = value;

                OnPropertyChanged("IsDirty"); // NOXLATE
                OnPropertyChanged("ApplyCommand"); // NOXLATE
            }
        }

        public RulePointContext RootRulePoint
        {
            get;
            set;
        }

        private string _searchKeyword = String.Empty;
        public string SearchKeyword
        {
            get
            {
                return _searchKeyword;
            }
            set
            {
                if (_searchKeyword == value)
                {
                    return;
                }

                _searchKeyword = value;
                StartSearching();

                OnPropertyChanged("SearchKeyword"); // NOXLATE
                OnPropertyChanged("IsSearching"); // NOXLATE
            }
        }

        private ObservableCollection<RuleBaseContext> _searchResult = new ObservableCollection<RuleBaseContext>();
        public ObservableCollection<RuleBaseContext> SearchResult
        {
            get
            {
                return _searchResult;
            }
        }

        public List<OperationContext> Operations
        {
            get;
            set;
        }

        public ICommand ApplyCommand
        {
            get;
            private set;
        }

        private bool _needRerunRules = false;
        public bool NeedRerunRules
        {
            get
            {
                return _needRerunRules;
            }
            set
            {
                _needRerunRules = value;
            }
        }


        private RuleBaseContext _cuttedRule;
        public RuleBaseContext CuttedRule
        {
            get
            {
                return _cuttedRule;
            }
            set
            {
                if (_cuttedRule != value)
                {
                    if (_cuttedRule != null)
                    {
                        _cuttedRule.IsCutted = false;
                    }

                    _cuttedRule = value;
                    if (_cuttedRule != null)
                    {
                        _cuttedRule.IsCutted = true;
                    }

                    OnPropertyChanged("CuttedRule"); // NOXLATE
                }
            }
        }


        public bool IsSearching
        {
            get
            {
                return SearchKeyword.Length != 0;
            }
        }


        /// <summary>
        /// Compare working set and original set, and commit the delta to rule storage.
        /// </summary>
        public void CommitWorkingSet()
        {
            var originalSet = GetOriginalSet();
            var workingSet = GetWorkingSet();
            var originalEnumerator = originalSet.GetEnumerator();
            var workingSetEnumerator = workingSet.GetEnumerator();

            bool originalSetFlag = originalEnumerator.MoveNext();
            bool workingSetFlag = workingSetEnumerator.MoveNext();
            try
            {
                while (originalSetFlag && workingSetFlag)
                {
                    var original = originalEnumerator.Current;
                    var working = workingSetEnumerator.Current;
                    int compareResult = original.Key.CompareTo(working.Key);
                    if (compareResult == 0)
                    {
                        if (working.Value.IsDirty)
                        {
                            working.Value.Commit();
                        }
                        originalSetFlag = originalEnumerator.MoveNext();
                        workingSetFlag = workingSetEnumerator.MoveNext();
                    }
                    else if (compareResult > 0)
                    {
                        working.Value.Commit();
                        workingSetFlag = workingSetEnumerator.MoveNext();
                    }
                    else if (compareResult < 0)
                    {
                        original.Value.Delete();
                        originalSetFlag = originalEnumerator.MoveNext();
                    }
                }

                while (originalSetFlag)
                {
                    var original = originalEnumerator.Current;
                    original.Value.Delete();
                    originalSetFlag = originalEnumerator.MoveNext();
                }

                while (workingSetFlag)
                {
                    var working = workingSetEnumerator.Current;
                    working.Value.Commit();
                    workingSetFlag = workingSetEnumerator.MoveNext();
                }

                IsDirty = false;
                NeedRerunRules = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    String.Format("Cannot save rules. Exception: {0}.", ex.Message),
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Get RuleBaseContext instance by given path.
        /// </summary>
        /// <param name="path">The path of rule.</param>
        /// <returns>RuleBaseContext instance</returns>
        public RuleBaseContext GetRuleContext(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path"); // NOXLATE
            }

            RuleBaseContext iter = RootRulePoint;
            string[] pathItems = RulePathHelper.GetRulePathComponents(path);
            foreach (string component in pathItems)
            {
                RulePointContext rulePoint = iter as RulePointContext;
                if (rulePoint == null)
                {
                    return null;
                }
                iter = rulePoint.Children.FirstOrDefault((r) => (r.Name == component)) as RuleBaseContext;
            }
            return iter;
        }


        /// <summary>
        /// Get rule from clipboard.
        /// </summary>
        /// <returns>The rule copied or cutted to clipboard</returns>
        public RuleBaseContext GetRuleFromClipboard()
        {
            try
            {
                //ClipboardContentForRule contentToClipboard = Clipboard.GetData(IMConstants.RuleFormatOnClipboard) as ClipboardContentForRule;
                //// Prevent from copying or cutting beyond AUD process.
                //if (contentToClipboard != null && contentToClipboard.PID == Process.GetCurrentProcess().Id)
                //{
                //    string rulePath = contentToClipboard.Path;
                //    return GetRuleContext(rulePath);
                //}
                return null;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
        }


        public void ClearClipboardIfNeed()
        {
            var rule = GetRuleFromClipboard();
            if (rule != null)
            {
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


        private Stack<RuleBaseContext> _workingBuffer;
        /// <summary>
        /// Start searching.
        /// </summary>
        private void StartSearching()
        {
            _searchResult.Clear();

            // Use stack rather than queue because I want Depth-First.
            _workingBuffer = new Stack<RuleBaseContext>();
            _workingBuffer.Push(RootRulePoint);
            Dispatcher.CurrentDispatcher.BeginInvoke(
                new Action<Stack<RuleBaseContext>>(SearchRules),
                DispatcherPriority.Normal,
                _workingBuffer);
        }


        /// <summary>
        /// Search rules including rule points and named rules, and add searching
        /// result to SearchResult property.
        /// </summary>
        private void SearchRules(Stack<RuleBaseContext> workingBuffer)
        {
            const int MaxWorkload = 10;
            const int MaxSearchResultNumber = 1000;

            if (workingBuffer != _workingBuffer)
            {
                // System has started an another searching work by a new keyword.
                return;
            }

            int count = 0;
            while (workingBuffer.Count > 0 && count < MaxWorkload)
            {
                RuleBaseContext theContext = workingBuffer.Pop();
                if (String.IsNullOrEmpty(SearchKeyword))
                {
                    _searchResult.Add(theContext);
                }
                else
                {
                    // Call ToUpper() for search in case-insensitive way.
                    if (theContext.DisplayName.ToUpper().Contains(SearchKeyword.ToUpper()))
                    {
                        _searchResult.Add(theContext);
                    }
                }

                theContext.UpdateDisplayTexts();

                // Add children into working queue if it has.
                RulePointContext rulePoint = theContext as RulePointContext;
                if (rulePoint != null)
                {
                    foreach (var child in rulePoint.Children.Reverse()) // first child, first serve
                    {
                        workingBuffer.Push(child);
                    }
                }

                ++count;
            }

            if (_workingBuffer.Count > 0 && _searchResult.Count < MaxSearchResultNumber)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(
                    new Action<Stack<RuleBaseContext>>(SearchRules),
                    DispatcherPriority.Background,
                    workingBuffer);
            }
        }


        private SortedDictionary<string, RuleBase> GetOriginalSet()
        {
            SortedDictionary<string, RuleBase> originalSet = new SortedDictionary<string, RuleBase>();

            foreach (var rulePoint in RuleAppExtension.RuleManagerInstance.GetAllRulePoints())
            {
                string rulePath = rulePoint.Path;
                if (!originalSet.ContainsKey(rulePath))
                {
                    originalSet.Add(rulePath, rulePoint);
                }
                else
                {
                    Debug.Assert(false, "There are rule points having duplicated rule path. This is not allowed. The document must have something wrong."); //NOXLATE
                }
            }

            foreach (var namedRule in RuleAppExtension.RuleManagerInstance.Storage.GetNamedRules())
            {
                string rulePath = namedRule.Path;
                if (!originalSet.ContainsKey(rulePath))
                {
                    originalSet.Add(rulePath, namedRule);
                }
                else
                {
                    Debug.Assert(false, "There are named rules having duplicated rule path. This is not allowed. The document must have something wrong."); //NOXLATE
                }
            }
            return originalSet;
        }

        private SortedDictionary<string, RuleBaseContext> GetWorkingSet()
        {
            SortedDictionary<string, RuleBaseContext> workingSet = new SortedDictionary<string, RuleBaseContext>();
            Queue<RuleBaseContext> workingQueue = new Queue<RuleBaseContext>();
            workingQueue.Enqueue(RootRulePoint);
            while (workingQueue.Count > 0)
            {
                RuleBaseContext theRuleContext = workingQueue.Dequeue();
                string rulePath = theRuleContext.Path;
                if (!workingSet.ContainsKey(rulePath))
                {
                    workingSet.Add(rulePath, theRuleContext);
                }
                else
                {
                    Debug.Assert(false, "There are rule points or named rules having duplicated rule path. This is not allowed. There must be something wrong."); //NOXLATE
                }

                RulePointContext rulePointContext = theRuleContext as RulePointContext;
                if (rulePointContext != null)
                {
                    foreach (var ruleContext in rulePointContext.Children)
                    {
                        workingQueue.Enqueue(ruleContext);
                    }
                }
            }
            return workingSet;
        }


        private bool ApplyCanExecute(object target)
        {
            return IsDirty;
        }


        private void Apply(object target)
        {
            CommitWorkingSet();
        }
    }
}
