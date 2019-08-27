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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using RuleConfiguration;


namespace RuleConfiguration
{
    /// <summary>
    /// A control showing a validation item, which is an error, or warning or message;
    /// the item may or may not have resolutions
    ///
    /// Interaction logic for PromptResultControl.xaml
    /// </summary>
    public partial class PromptResultControl : UserControl
    {
        #region Dynamic properties

        public Brush ItemSelectedBackground
        {
            get { return (Brush)GetValue(ItemSelectedBackgroundProperty); }
            set { SetValue(ItemSelectedBackgroundProperty, value); }
        }


        // Using a DependencyProperty as the backing store for ItemSelectedBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSelectedBackgroundProperty =
            DependencyProperty.Register("ItemSelectedBackground", //NOXLATE
                typeof(Brush),
                typeof(PromptResultControl),
                new UIPropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(OnSelectedBackgroundChanged)));


        static void OnSelectedBackgroundChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PromptResultControl itemCtrl = sender as PromptResultControl;

            if (itemCtrl == null)
                return;

            ValidationItemContextBase cxt = itemCtrl.DataContext as ValidationItemContextBase;
            if (cxt == null)
            {
                return;
            }

            cxt.ItemSelectedBackground = args.NewValue as Brush;
        }


        public Brush ItemSelectedBorderBrush
        {
            get { return (Brush)GetValue(ItemSelectedBorderBrushProperty); }
            set { SetValue(ItemSelectedBorderBrushProperty, value); }
        }


        // Using a DependencyProperty as the backing store for ItemSelectedBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSelectedBorderBrushProperty =
            DependencyProperty.Register("ItemSelectedBorderBrush", //NOXLATE
                typeof(Brush),
                typeof(PromptResultControl),
                new UIPropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(OnSelectedBorderChanged)));


        static void OnSelectedBorderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PromptResultControl itemCtrl = sender as PromptResultControl;

            if (itemCtrl == null)
                return;

            ValidationItemContextBase cxt = itemCtrl.DataContext as ValidationItemContextBase;
            if (cxt == null)
            {
                return;
            }

            cxt.ItemSelectedBorderBrush = args.NewValue as Brush;
        }


        public Brush ItemRollBackground
        {
            get { return (Brush)GetValue(ItemRollBackgroundProperty); }
            set { SetValue(ItemRollBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemRollBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemRollBackgroundProperty =
            DependencyProperty.Register("ItemRollBackground",  //NOXLATE
                typeof(Brush),
                typeof(PromptResultControl),
                new UIPropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(OnItemRollBackgroundChanged)));


        static void OnItemRollBackgroundChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PromptResultControl itemCtrl = sender as PromptResultControl;

            if (itemCtrl == null)
                return;

            ValidationItemContextBase cxt = itemCtrl.DataContext as ValidationItemContextBase;
            if (cxt == null)
            {
                return;
            }

            cxt.ItemRollBackground = args.NewValue as Brush;
        }


        public Brush ItemRollBorderColor
        {
            get { return (Brush)GetValue(ItemRollBorderColorProperty); }
            set { SetValue(ItemRollBorderColorProperty, value); }
        }


        // Using a DependencyProperty as the backing store for ItemRollBorderColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemRollBorderColorProperty =
            DependencyProperty.Register("ItemRollBorderColor", //NOXLATE
                typeof(Brush),
                typeof(PromptResultControl),
                new UIPropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(OnItemRollBorderColorChanged)));


        static void OnItemRollBorderColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PromptResultControl itemCtrl = sender as PromptResultControl;

            if (itemCtrl == null)
                return;

            ValidationItemContextBase cxt = itemCtrl.DataContext as ValidationItemContextBase;
            if (cxt == null)
            {
                return;
            }

            cxt.ItemRollBorderColor = args.NewValue as Brush;
        }

        #endregion of Dynamic Properties


        public PromptResultControl()
        {
            InitializeComponent();

            this.Loaded += this.PromptResultControl_Loaded;
        }


        void PromptResultControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidationItemContextBase item = this.DataContext as ValidationItemContextBase;
                if (item == null)
                    return;

                item.UpdateResolutionsButtonVisibility();
                item.UpdateGoToRuleButtonVisibility();
            }
            finally
            {
                this.Loaded -= this.PromptResultControl_Loaded;
            }
        }


        public void OnMouseHoverChanged(bool isInside)
        {
            ValidationItemContextBase item = this.DataContext as ValidationItemContextBase;
            if (item == null)
            {
                //maybe because of the item is ignored
                return;
            }

            if (item.Owner == null)
            {
                Debug.Assert(false);
                return;
            }

            //if (!item.Owner.IsSelectingOn)
            //    return;

            item.IsMouseHovered = isInside;
        }


        private void ResolveHyperlink_Click(object sender, RoutedEventArgs e)
        {
            ExecuteCommand(sender);
        }


        private void ExecuteCommand(object sender)
        {
            //this.mShowResolutionsBtn.IsChecked = false;

            ////step 1: get the executable item to execute
            //Hyperlink hyperLink = sender as Hyperlink;
            //if (DebugUtil.IsNull(hyperLink))
            //    return;

            //ExecutableItem executable = hyperLink.DataContext as ExecutableItem;

            ////step 2: get the parent validation item parameter
            //ValidationItemContextBase itemContext = this.DataContext as ValidationItemContext;
            //if (DebugUtil.IsNull(itemContext))
            //    return;

            //IValidationItem item = itemContext.ValidationItem;

            ////step 3: execute the item
            //ExecutableItemUtil.ExecuteItem(executable, item);
        }


        private void btnShowResolutions_Click(object sender, RoutedEventArgs e)
        {
            //ValidationItemContext itemContext = this.DataContext as ValidationItemContext;

            //if (itemContext == null)
            //{
            //    Debug.Assert(false, "PromptResultControl supports only ValidationItemContext"); // NOXLATE
            //    return;
            //}

            //itemContext.IsShowingDetail = this.mShowResolutionsBtn.IsChecked.Value;
        }


        //private void btnGoToRule_Click(object sender, RoutedEventArgs e)
        //{
        //    ValidationItemContext itemContext = this.DataContext as ValidationItemContext;

        //    if (itemContext == null)
        //    {
        //        Debug.Assert(false);
        //        return;
        //    }

        //    // End any active command and launch the Rule Editor.
        //    string command = string.Format("^C^C^C_AUDRULECONFIG {0}\n", itemContext.RulePointPath); //NOXLATE
        //    CommandHelper.ExecuteCommand(command, false);    
        //}


        //private void btnIgnore_Click(object sender, RoutedEventArgs e)
        //{
        //    ValidationItemContextBase item = this.DataContext as ValidationItemContextBase;
        //    if (item == null)
        //        return;

        //    item.IgnoreClicked();
        //}


        public void OnMouseUp()
        {
            ValidationItemContextBase item = this.DataContext as ValidationItemContextBase;
            if (item == null)
            {
                Debug.Assert(false);
                return;
            }

            if (item.Owner == null)
                return;

            //if (!item.Owner.IsSelectingOn)
            //    return;

            item.Owner.OnItemClicked(item);
        }

    }
}
