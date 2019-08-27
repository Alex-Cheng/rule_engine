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
using System.Collections.ObjectModel;
using System.Windows;

using RuleConfiguration;


namespace RuleConfiguration
{
    /// <summary>
    /// Represents validation item showing error, warning or message;
    /// it may or may not have resolution item;
    /// </summary>
    public class ValidationItemContext : ValidationItemContextBase
    {
        public ValidationItemContext(DesignValidationContext owner, IValidationItem item)
            : base(owner, item)
        {
        }


        public bool IsShowingDetail
        {
            get { return (bool)GetValue(IsShowingDetailProperty); }
            set { SetValue(IsShowingDetailProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShowingDetail.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowingDetailProperty =
            DependencyProperty.Register("IsShowingDetail", //NOXLATE
                typeof(bool),
                typeof(ValidationItemContext),
                new UIPropertyMetadata(false, new PropertyChangedCallback(OnIsShowingDetailChanged)));


        static void OnIsShowingDetailChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ValidationItemContext cxt = sender as ValidationItemContext;
            if (cxt == null)
                return;

            //cxt.UpdateResolutionContext();
        }


        //private void UpdateResolutionContext()
        //{
        //    if (this.IsShowingDetail)
        //    {
        //        ResolutionValidationItem item = this.ValidationItem as ResolutionValidationItem;
        //        this.Resolutions = (item != null) ? item.ResolutionItems : null;
        //    }
        //    else
        //    {
        //        this.Resolutions = null;
        //    }
        //}


        //public Visibility ResolutionsVisibility
        //{
        //    get { return (Visibility)GetValue(ResolutionsVisibilityProperty); }
        //    set { SetValue(ResolutionsVisibilityProperty, value); }
        //}


        // Using a DependencyProperty as the backing store for ResolutionsVisibility.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ResolutionsVisibilityProperty =
        //    DependencyProperty.Register("ResolutionsVisibility", //NOXLATE
        //    typeof(Visibility), typeof(ValidationItemContext), new UIPropertyMetadata(Visibility.Hidden));


        //public ObservableCollection<ResolutionItem> Resolutions
        //{
        //    get { return (ObservableCollection<ResolutionItem>)GetValue(ResolutionsProperty); }
        //    set { SetValue(ResolutionsProperty, value); }
        //}


        // Using a DependencyProperty as the backing store for Resolutions.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ResolutionsProperty =
        //    DependencyProperty.Register("Resolutions", //NOXLATE
        //        typeof(ObservableCollection<ResolutionItem>), typeof(ValidationItemContext), new UIPropertyMetadata(null));
    }
}
