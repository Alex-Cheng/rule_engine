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
using System.Activities.Presentation.View;
using System.Windows;
using System.Windows.Controls;


namespace Autodesk.IM.UI.Rule
{
    public static class RuleDesignerHelper
    {
        public static void HideExpandCollapseControl(DesignerView designerView)
        {
            //the item control named 'BreadCrumbBarLayout' holds the Name rendering control;
            //however it's not public. Instead of writting a bunch of reflection code,
            //let's go around it this way:

            //at first, get the 'expland all' or 'collapse all' button
            FrameworkElement currentExpandButton = LogicalTreeHelper.FindLogicalNode(designerView, "expandAllButton") as FrameworkElement;  // NOXLATE
            if (currentExpandButton == null)
            {
                currentExpandButton = LogicalTreeHelper.FindLogicalNode(designerView, "collapseAllButton") as FrameworkElement; // NOXLATE
            }

            //then, find the grand father of 'expand/collapse all' button
            if (currentExpandButton != null)
            {
                FrameworkElement expandButtonParent = currentExpandButton.Parent as FrameworkElement;
                if (expandButtonParent != null)
                {
                    expandButtonParent.Visibility = Visibility.Collapsed;
                }
            }
        }


        public static void ChangeRuleContentAlighment(DesignerView designerView)
        {
            Viewbox viewBox = LogicalTreeHelper.FindLogicalNode(designerView, "viewBox") as Viewbox; // NOXLATE
            if (viewBox != null)
            {
                viewBox.HorizontalAlignment = HorizontalAlignment.Left;
                viewBox.VerticalAlignment = VerticalAlignment.Top;
            }
        }
    }
}
