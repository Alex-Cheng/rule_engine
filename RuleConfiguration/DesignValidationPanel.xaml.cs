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
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

using RuleConfiguration;


namespace RuleConfiguration
{
    /// <summary>
    /// Interaction logic for DesignValidationPanel.xaml
    /// </summary>
    public partial class DesignValidationPanel : UserControl
    {
        public DesignValidationPanel()
        {
            InitializeComponent();
        }


        //private void ReportButton_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    DesignValidationContext cxt = this.DataContext as DesignValidationContext;
        //    if (cxt == null)
        //    {
        //        Debug.Assert(false);
        //        return;
        //    }
        //    AudDocument audDoc = AudApplication.DocumentManager.Active;
        //    ValidationItemReport report = new ValidationItemReport(audDoc);
        //    foreach (ValidationGroupContext grp in cxt.ValidationGroups)
        //        report.AddValidationItems(
        //            from ValidationItemContextBase vicb in grp.ValidationResultItems
        //            select vicb.ValidationItem, true);
        //    report.Report(Properties.Resources.DesignValidation);
        //}


        //private void ResolveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    DesignValidationContext cxt = this.DataContext as DesignValidationContext;
        //    if (cxt != null)
        //        cxt.ResolveAll();
        //}


        private void validationGroupsControl_CleanUpVirtualizedItem(object sender, CleanUpVirtualizedItemEventArgs e)
        {
            if (e == null)
                return;

            ValidationGroupContext group = e.Value as ValidationGroupContext;
            if (group == null)
                return;

            group.DisconnectByVirtualizationCleanup();
        }


        private void UserControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Focus();
        }


        private void itemControlBorder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = sender as Border;

            if (border == null)
            {
                Debug.Assert(false);
                return;
            }

            PromptResultControl itemCtrl = border.Child as PromptResultControl;
            if (itemCtrl == null)
            {
                Debug.Assert(false);
                return;
            }

            itemCtrl.OnMouseHoverChanged(true);
        }


        private void itemControlBorder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = sender as Border;

            if (border == null)
            {
                Debug.Assert(false);
                return;
            }

            PromptResultControl itemCtrl = border.Child as PromptResultControl;
            if (itemCtrl == null)
            {
                Debug.Assert(false);
                return;
            }

            itemCtrl.OnMouseHoverChanged(false);
        }


        private void itemControlBorder_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Border border = sender as Border;

            if (border == null)
            {
                Debug.Assert(false);
                return;
            }

            PromptResultControl itemCtrl = border.Child as PromptResultControl;
            if (itemCtrl == null)
            {
                Debug.Assert(false);
                return;
            }

            itemCtrl.OnMouseUp();
        }

    }
}
