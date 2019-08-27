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
using System.Activities.Presentation;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Autodesk.IM.UI.Rule;
using RuleConfiguration;


namespace RuleConfiguration
{
    /// <summary>
    /// Interaction logic for RuleConfigDialog.xaml
    /// </summary>
    public partial class RuleConfigDialog : Window
    {
        private static RuleConfigDialog _instance = null;

        private InternalDesigner _ruleDesigner = null;
        private WorkflowDesigner _workflowDesigner = null;

        public static RuleConfigDialog GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RuleConfigDialog();
            }
            return _instance;
        }


        private RuleConfigDialog()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            _ruleConfigContext = new RuleConfigContext();
            _ruleConfigContext.PropertyChanged += new PropertyChangedEventHandler(RuleConfigContext_PropertyChanged);
            this.DataContext = _ruleConfigContext;
        }


        /// <summary>
        /// Get or set the path of selected rule, otherwise return null.
        /// </summary>
        public string SelectedRulePath
        {
            get
            {
                if (_ruleConfigContext != null && _ruleConfigContext.SelectedRule != null)
                {
                    return _ruleConfigContext.SelectedRule.Path;
                }
                return null;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    _ruleConfigContext.SelectedRule = null;
                }
                else
                {
                    // If the rule does not exist, set SelectedRule property to null.
                    _ruleConfigContext.SelectedRule = _ruleConfigContext.GetRuleContext(value);
                }
            }
        }

        private RuleConfigContext _ruleConfigContext;
        internal RuleConfigContext RuleConfigContext
        {
            get
            {
                return _ruleConfigContext;
            }
        }


        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // If users accept (e.g. click on 'OK' button) and the rules have been changed, save rules.
            if (this.DialogResult == true && _ruleConfigContext.IsDirty)
            {
                _ruleConfigContext.CommitWorkingSet();
            }
            // If users cancelled the dialog and the rules have been changed, ask users if they want to save changes.
            else if (_ruleConfigContext.IsDirty)
            {
                MessageBoxResult result = MessageBox.Show(
                    Properties.Resources.PreClosingPrompt,
                    Properties.Resources.Warning,
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        _ruleConfigContext.CommitWorkingSet();
                    }
                }
            }

            DetachDesigner();
            DataContext = null;
            _ruleConfigContext = null;
            _instance = null;

            GC.Collect();
        }


        private void DetachDesigner()
        {
            if (_ruleDesigner != null)
            {
                mDesignerViewContent.Content = null;
                _ruleDesigner.SetDesigner(null);
                _workflowDesigner = null;
            }
        }


        private void RuleNameTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox nameTextBox = sender as TextBox;
            if (e.Key == Key.Enter && nameTextBox != null)
            {
                var bindExpression = nameTextBox.GetBindingExpression(TextBox.TextProperty);
                if (bindExpression != null)
                {
                    bindExpression.UpdateSource();
                }
                e.Handled = true;
            }
        }


        private void RuleNameTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            if (txtBox != null)
            {
                txtBox.SelectAll();
                // TODO: without this line, the width of text box will be set to 30 and I don't know where it is set.
                txtBox.ClearValue(TextBox.WidthProperty);
            }
        }


        private void RuleConfigContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RuleConfigContext context = sender as RuleConfigContext;
            Debug.Assert(context != null);

            if (e.PropertyName == "EditingRule") // NOXLATE
            {
                RuleBaseContext editingRule = context.EditingRule;
                if (editingRule == null)
                {
                    DetachDesigner();

                    if (context.SelectedRule != null && context.SelectedRule is RulePointContext)
                    {
                        RulePointContext rpc = context.SelectedRule as RulePointContext;
                        mDesignerViewContent.Content = rpc.CreateContentOperation;
                    }
                    else
                    {
                        mDesignerViewContent.Content = "TODO: show help doc here."; // NOXLATE
                    }
                }
                else
                {
                    RuleAppExtension.RuleDesignerManagerInst.UpdateDesignerAttributes();
                    _workflowDesigner = new WorkflowDesigner();
                    _workflowDesigner.Text = context.EditingRule.Text;
                    _workflowDesigner.Load();
                    RuleEditingContext editingContext = context.EditingRule.GetEditingContext();
                    ServiceManager editingContextServices = _workflowDesigner.Context.Services;
                    editingContextServices.Publish<RuleEditingContextService>(new RuleEditingContextService(editingContext));
                    editingContextServices.Publish<ActivityTranslator>(RuleAppExtension.ActivityTranslatorInst);
                    editingContextServices.Publish<ActivityFactory>(RuleAppExtension.ActivityFactoryInst);
                    _workflowDesigner.ModelChanged += new EventHandler(WorkflowDesigner_ModelChanged);
                    if (_ruleDesigner == null)
                        _ruleDesigner = new InternalDesigner();
                    _ruleDesigner.SetDesigner(_workflowDesigner);
                    mDesignerViewContent.Content = _ruleDesigner;
                    //mRulesTreeView.Focus();
                }
            }
        }


        private void WorkflowDesigner_ModelChanged(object sender, EventArgs e)
        {
            WorkflowDesigner designer = sender as WorkflowDesigner;
            if (designer != null)
            {
                designer.Flush();
                _ruleConfigContext.EditingRule.Text = designer.Text;
            }
        }


        private object ExceptionOnNamingHandler(object bindExpression, Exception exception)
        {
            MessageBox.Show(exception.Message, Properties.Resources.Error, MessageBoxButton.OK);
            return exception.Message;
        }


        private void TreeViewItem_MouseRightButtonDown(object sender, MouseEventArgs e)
        {
            // Enable selection by right-click, if the item is not selected before.
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                SelectRule(item.DataContext as RuleBaseContext);
                e.Handled = true;
            }
        }


        private void ListViewItem_MouseRightButtonDown(object sender, MouseEventArgs e)
        {
            // Enable selection by right-click, if the item is not selected before.
            ListViewItem item = sender as ListViewItem;
            if (item != null)
            {
                SelectRule(item.DataContext as RuleBaseContext);
                e.Handled = true;
            }
        }


        private void SelectRule(RuleBaseContext ruleContext)
        {
            if (ruleContext != null)
            {
                if (!ruleContext.IsSelected)
                {
                    ruleContext.IsSelected = true;
                }
            }
        }


        void HelpCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
        }
    }
}
