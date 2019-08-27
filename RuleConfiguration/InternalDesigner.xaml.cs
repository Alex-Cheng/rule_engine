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
using System.Activities.Presentation;
using System.Activities.Presentation.View;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

using Autodesk.IM.Rule;
using Autodesk.IM.Rule.Activities;
using Autodesk.IM.UI.Rule;

namespace RuleConfiguration
{
    /// <summary>
    /// Interaction logic for InternalDesigner.xaml
    /// </summary>
    public partial class InternalDesigner : UserControl
    {
        WorkflowDesigner _designer;
        ScrollViewer _mainScrollViewer;
        FrameworkElement _designerPresenter;
        TextBlockComparer _textBlockComparer;
        string _prevSearchKeyword;
        List<TextBlock> _prevMatchedTextBlock;
        int _currentSearchIndex;
        DispatcherTimer _updateSearchTimer;

        public InternalDesigner()
        {
            InitializeComponent();

            _updateSearchTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(2000)
            };
            _updateSearchTimer.Tick += (sender, e) =>
            {
                UpdateSearchResults();
            };
        }

        ICommand _findPreviousCommand;
        public ICommand FindPreviousCommand
        {
            get
            {
                if (_findPreviousCommand == null)
                    _findPreviousCommand = new RelayCommand(FindPreviousCommand_Executed, FindCommand_CanExecute);
                return _findPreviousCommand;
            }
        }

        ICommand _findNextCommand;
        public ICommand FindNextCommand
        {
            get
            {
                if (_findNextCommand == null)
                    _findNextCommand = new RelayCommand(FindNextCommand_Executed, FindCommand_CanExecute);
                return _findNextCommand;
            }
        }

        public void SetDesigner(WorkflowDesigner designer)
        {
            if (_designer != null)
            {
                mSearchBox.Text = String.Empty;
                SetCommandTarget(null);
                _designer.View.PreviewKeyDown -= new KeyEventHandler(View_PreviewKeyDown);
                this.mContentControl.Content = null;
                _textBlockComparer = null;
                _designerPresenter = null;
                _mainScrollViewer = null;
            }

            _designer = designer;

            if (designer != null)
            {
                // set some view params to hide stuff and expand all items
                DesignerView designerView = _designer.Context.Services.GetService<DesignerView>();
                designerView.WorkflowShellBarItemVisibility = ShellBarItemVisibility.None;
                designerView.ShouldExpandAll = false;
                designerView.ShouldCollapseAll = true;

                RuleDesignerHelper.HideExpandCollapseControl(designerView);
                RuleDesignerHelper.ChangeRuleContentAlighment(designerView);

                _mainScrollViewer = LogicalTreeHelper.FindLogicalNode(designerView, "scrollViewer") as ScrollViewer; // NOXLATE
                _designerPresenter = LogicalTreeHelper.FindLogicalNode(_mainScrollViewer, "designerPresenter") as FrameworkElement; // NOXLATE
                _textBlockComparer = new TextBlockComparer(_designerPresenter);

                this.mContentControl.Content = designer.View;
                designer.View.PreviewKeyDown += new KeyEventHandler(View_PreviewKeyDown);
                SetCommandTarget(designerView);
            }
        }

        /// <summary>
        /// Prevent the designer from expanding to the select item when the enter key is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void View_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.None &&
                e.OriginalSource is WorkflowViewElement &&
                e.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }

        void SetCommandTarget(DesignerView view)
        {
            this.mCopyBtn.CommandTarget = view;
            this.mCutBtn.CommandTarget = view;
            this.mPasteBtn.CommandTarget = view;
            this.mRedoBtn.CommandTarget = view;
            this.mUndoBtn.CommandTarget = view;
        }

        private void PreviewDelete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;

            if (_designer != null)
            {
                e.CanExecute = CanDelete;
            }

            e.ContinueRouting = false;
            e.Handled = true;
        }

        private void PreviewCut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;

            if (_designer != null)
            {
                e.CanExecute = CanCut;
            }

            e.ContinueRouting = false;
            e.Handled = true;
        }

        private void PreviewNotExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.ContinueRouting = false;
            e.Handled = true;
        }

        bool CanDelete
        {
            get
            {
                return IsRuleSelected;
            }
        }

        bool CanCut
        {
            get
            {
                return IsRuleSelected && CanCopyOrCut();
            }
        }

        bool IsRuleSelected
        {
            get
            {
                Selection s = this._designer.Context.Items.GetValue<Selection>();
                if (s != null && s.PrimarySelection != null)
                {
                    if (s.PrimarySelection.ItemType == typeof(InvokeNamedRule))
                    {
                        return true;
                    }

                    RuleEditingContext context = s.PrimarySelection.GetRootRuleEditingContext();
                    IEnumerable<ActivityEntry> activities = context.GetAvailableActivities();
                    return activities.Count((a) => a.ActivityType == s.PrimarySelection.ItemType) > 0;
                }
                return false;
            }
        }

        private void content_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Stop further processing of this double click event.
            e.Handled = true;
        }

        private void content_PreviewDragEnter(object sender, DragEventArgs e)
        {
            // Stop further processing. Further processing will start a timer and expand an activity when time
            // elapsed. We don't want expand an activity so we block it.
            e.Handled = true;
        }

        private void PreviewCopy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanCopyOrCut();
            e.ContinueRouting = false;
            e.Handled = true;
        }

        private bool CanCopyOrCut()
        {
            if (_designer != null)
            {
                Selection s = this._designer.Context.Items.GetValue<Selection>();
                if (s != null && s.PrimarySelection != null)
                {
                    Type selectedValueType = s.PrimarySelection.GetCurrentValue().GetType();

                    Type type = selectedValueType;
                    while (null != type)
                    {
                        if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Activity<>)))
                        {
                            return false;
                        }
                        type = type.BaseType;
                    }

                    if (selectedValueType != typeof(ConditionalValue<DynamicValue>))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _updateSearchTimer.IsEnabled = !String.IsNullOrWhiteSpace(mSearchBox.Text);
            UpdateSearchResults();
            e.Handled = true;
        }

        private void UpdateSearchResults()
        {
            string text = mSearchBox.Text;
            if (String.IsNullOrWhiteSpace(text))
            {
                ClearPrevSearchResult();
                _prevSearchKeyword = null;
            }
            else
            {
                string keyword = text.Trim();
                List<TextBlock> matchedTextBlock = new List<TextBlock>();
                FindTextInVisualTree(matchedTextBlock, _designerPresenter, keyword);
                if (matchedTextBlock.Count == 0)
                {
                    matchedTextBlock = null;
                    ClearPrevSearchResult();
                }
                else if (_prevMatchedTextBlock == null)
                {
                    _currentSearchIndex = -1;
                    Debug.WriteLine("SearchIndex: -1"); // NOXLATE
                    for (int i = 0, iEnd = matchedTextBlock.Count; i < iEnd; ++i)
                        HighlightKeyword(matchedTextBlock[i], keyword, false);
                }
                else
                {
                    // Find the difference between the old match list and the new one,
                    // and only make the necessary updates to the TextBlocks in the list.
                    int i0 = 0, count0 = _prevMatchedTextBlock.Count;
                    int i1 = 0, count1 = matchedTextBlock.Count;
                    bool keywordChanged = keyword != _prevSearchKeyword;
                    bool searchIndexSet = false;
                    while (i0 < count0 && i1 < count1)
                    {
                        TextBlock textBlock0 = _prevMatchedTextBlock[i0];
                        TextBlock textBlock1 = matchedTextBlock[i1];
                        if (textBlock0 == textBlock1)
                        {
                            // Update the TextBlocks remaining in the match list only when the search keyword changs.
                            if (!searchIndexSet && _currentSearchIndex == i0)
                            {
                                if (i0 != i1)
                                {
                                    _currentSearchIndex = i1;
                                    Debug.WriteLine("SearchIndex: {0} => {1}", i0, i1); // NOXLATE
                                }
                                searchIndexSet = true;
                            }
                            if (keywordChanged)
                                HighlightKeyword(textBlock1, keyword, _currentSearchIndex == i1);
                            ++i0;
                            ++i1;
                        }
                        else
                        {
                            int result = _textBlockComparer.Compare(textBlock0, textBlock1);
                            if (result <= 0)
                            {
                                // Reset old TextBlocks that are no longer in the match list.
                                if (!searchIndexSet && _currentSearchIndex == i0)
                                {
                                    _currentSearchIndex = -1;
                                    Debug.WriteLine("SearchIndex: -1"); // NOXLATE
                                    searchIndexSet = true;
                                }
                                ClearKeyword(textBlock0);
                                ++i0;
                            }
                            if (result >= 0)
                            {
                                // Highlight new TextBlocks that are new to the match list.
                                HighlightKeyword(textBlock1, keyword, false);
                                ++i1;
                            }
                        }
                    }
                    while (i0 < count0)
                    {
                        // Reset old TextBlocks that are no longer in the match list.
                        if (!searchIndexSet && _currentSearchIndex == i0)
                        {
                            _currentSearchIndex = -1;
                            Debug.WriteLine("SearchIndex: -1"); // NOXLATE
                            searchIndexSet = true;
                        }
                        ClearKeyword(_prevMatchedTextBlock[i0]);
                        ++i0;
                    }
                    while (i1 < count1)
                    {
                        // Highlight new TextBlocks that are new to the match list.
                        HighlightKeyword(matchedTextBlock[i1], keyword, false);
                        ++i1;
                    }
                }
                _prevSearchKeyword = keyword;
                _prevMatchedTextBlock = matchedTextBlock;
            }
        }

        private void FindTextInVisualTree(List<TextBlock> matchedTextBlock, DependencyObject rootObject, string keyword)
        {
            TextBlock textBlock = rootObject as TextBlock;
            if (textBlock != null && textBlock.IsVisible)
            {
                string text = textBlock.Text;
                if (!String.IsNullOrWhiteSpace(text) && text.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    int index = matchedTextBlock.BinarySearch(textBlock, _textBlockComparer);
                    matchedTextBlock.Insert(index < 0 ? ~index : index + 1, textBlock);
                }
            }
            for (int i = 0, iEnd = VisualTreeHelper.GetChildrenCount(rootObject); i < iEnd; ++i)
            {
                DependencyObject childObject = VisualTreeHelper.GetChild(rootObject, i);
                FindTextInVisualTree(matchedTextBlock, childObject, keyword);
            }
        }

        private void ClearPrevSearchResult()
        {
            if (_prevMatchedTextBlock != null)
            {
                for (int i = 0, iEnd = _prevMatchedTextBlock.Count; i < iEnd; ++i)
                    ClearKeyword(_prevMatchedTextBlock[i]);
                _prevMatchedTextBlock = null;
            }
        }

        private void ClearKeyword(TextBlock textBlock)
        {
            textBlock.Text = textBlock.Text; // To reset the text format to plain text
            Debug.WriteLine("Clr: " + textBlock.Text); // NOXLATE
        }

        private void HighlightKeyword(TextBlock textBlock, string keyword, bool focused)
        {
            string text = textBlock.Text;
            int index = text.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase);
            if (index >= 0)
            {
                textBlock.Inlines.Clear();
                if (index > 0)
                    textBlock.Inlines.Add(text.Substring(0, index));
                Run run = new Run(text.Substring(index, keyword.Length));
                if (focused)
                {
                    run.Background = Brushes.Blue;
                    run.Foreground = Brushes.White;
                }
                else
                {
                    run.Background = Brushes.Yellow;
                    run.Foreground = Brushes.Black;
                }
                textBlock.Inlines.Add(run);
                index += keyword.Length;
                if (index < text.Length)
                    textBlock.Inlines.Add(text.Substring(index));
            }
            Debug.WriteLine("Set: " + text); // NOXLATE
        }

        private bool FindCommand_CanExecute(object sender)
        {
            return _prevMatchedTextBlock != null && _prevMatchedTextBlock.Count > 0;
        }

        private void FindPreviousCommand_Executed(object sender)
        {
            ClearSearchFocus();
            int index = _currentSearchIndex;
            if (--_currentSearchIndex < 0)
                _currentSearchIndex = _prevMatchedTextBlock.Count - 1;
            Debug.WriteLine("SearchIndex: {0} => {1}", index, _currentSearchIndex); // NOXLATE
            SetSearchFocus();
        }

        private void FindNextCommand_Executed(object sender)
        {
            ClearSearchFocus();
            int index = _currentSearchIndex;
            if (++_currentSearchIndex >= _prevMatchedTextBlock.Count)
                _currentSearchIndex = 0;
            Debug.WriteLine("SearchIndex: {0} => {1}", index, _currentSearchIndex); // NOXLATE
            SetSearchFocus();
        }

        private void ClearSearchFocus()
        {
            if (_currentSearchIndex >= 0 && _currentSearchIndex < _prevMatchedTextBlock.Count)
                HighlightKeyword(_prevMatchedTextBlock[_currentSearchIndex], _prevSearchKeyword, false);
        }

        private void SetSearchFocus()
        {
            TextBlock textBlock = _prevMatchedTextBlock[_currentSearchIndex];
            HighlightKeyword(textBlock, _prevSearchKeyword, true);
            textBlock.BringIntoView();
        }


        private class TextBlockComparer : IComparer<TextBlock>
        {
            static readonly Point Origin = new Point();
            UIElement _rootElement;

            public TextBlockComparer(UIElement rootElement)
            {
                _rootElement = rootElement;
            }

            public int Compare(TextBlock x, TextBlock y)
            {
                Point posX = x.TranslatePoint(Origin, _rootElement);
                Point posY = y.TranslatePoint(Origin, _rootElement);
                if (posX.Y < posY.Y)
                    return -1;
                if (posX.Y > posY.Y)
                    return 1;
                if (posX.X < posY.X)
                    return -1;
                if (posX.X > posY.X)
                    return 1;
                return 0;
            }
        }

        private void SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (FindCommand_CanExecute(null))
                {
                    FindPreviousCommand_Executed(null);
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                mSearchBox.Text = String.Empty;
                this.mContentControl.Focus();
                e.Handled = true;
            }
        }
    }
}
