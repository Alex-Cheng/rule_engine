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
using System.Activities.Presentation.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Autodesk.IM.UI.Rule;


namespace RuleConfiguration
{
    /// <summary>
    /// Interaction logic for ExpressionItemPresenter.xaml
    /// </summary>
    public partial class ExpressionItemPresenter : UserControl
    {
        public ExpressionItemPresenter()
        {
            InitializeComponent();
        }

        public static DependencyProperty OwnerItemProperty = ItemSelector.OwnerItemProperty.AddOwner(
            typeof(ExpressionItemPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
        public ModelItem OwnerItem
        {
            get { return GetValue(OwnerItemProperty) as ModelItem; }
            set { SetValue(OwnerItemProperty, value); }
        }

        public static DependencyProperty ItemNameProperty = ItemSelector.ItemNameProperty.AddOwner(
            typeof(ExpressionItemPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
        public string ItemName
        {
            get { return GetValue(ItemNameProperty) as string; }
            set { SetValue(ItemNameProperty, value); }
        }

        public static DependencyProperty ItemProperty = ItemSelector.ItemProperty.AddOwner(
            typeof(ExpressionItemPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
        public ModelItem Item
        {
            get { return GetValue(ItemProperty) as ModelItem; }
            set { SetValue(ItemProperty, value); }
        }
    }
}
