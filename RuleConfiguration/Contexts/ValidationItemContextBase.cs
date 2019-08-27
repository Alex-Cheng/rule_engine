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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

using RuleConfiguration;


namespace RuleConfiguration
{
    /// <summary>
    /// Base class of validation item context;
    /// </summary>
    public abstract class ValidationItemContextBase : DependencyObject
    {
        private static Brush _deviceSelectedBrush = new LinearGradientBrush(Color.FromRgb(253, 253, 251), Color.FromRgb(253, 252, 230), new Point(0.5, 0), new Point(0.5, 1));

        #region dynamic properties

        public Brush ItemSelectedBackground
        {
            get { return (Brush)GetValue(ItemSelectedBackgroundProperty); }
            set { SetValue(ItemSelectedBackgroundProperty, value); }
        }


        // Using a DependencyProperty as the backing store for ItemSelectedBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSelectedBackgroundProperty =
            DependencyProperty.Register("ItemSelectedBackground", //NOXLATE
                typeof(Brush),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(Brushes.Transparent));


        public Brush ItemSelectedBorderBrush
        {
            get { return (Brush)GetValue(ItemSelectedBorderBrushProperty); }
            set { SetValue(ItemSelectedBorderBrushProperty, value); }
        }


        // Using a DependencyProperty as the backing store for ItemSelectedBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSelectedBorderBrushProperty =
            DependencyProperty.Register("ItemSelectedBorderBrush",  //NOXLATE
                typeof(Brush),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(Brushes.Transparent));


        public Brush ItemRollBackground
        {
            get { return (Brush)GetValue(ItemRollBackgroundProperty); }
            set { SetValue(ItemRollBackgroundProperty, value); }
        }


        // Using a DependencyProperty as the backing store for ItemRollBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemRollBackgroundProperty =
            DependencyProperty.Register("ItemRollBackground", //NOXLATE
                typeof(Brush),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(Brushes.Transparent));


        public Brush ItemRollBorderColor
        {
            get { return (Brush)GetValue(ItemRollBorderColorProperty); }
            set { SetValue(ItemRollBorderColorProperty, value); }
        }


        // Using a DependencyProperty as the backing store for ItemRollBorderColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemRollBorderColorProperty =
            DependencyProperty.Register("ItemRollBorderColor", //NOXLATE
                typeof(Brush),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(Brushes.Transparent));


        public Visibility ErrorIconVisibility
        {
            get { return (Visibility)GetValue(ErrorIconVisibilityProperty); }
            set { SetValue(ErrorIconVisibilityProperty, value); }
        }


        // Using a DependencyProperty as the backing store for ErrorIconVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorIconVisibilityProperty =
            DependencyProperty.Register("ErrorIconVisibility", //NOXLATE
                typeof(Visibility),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(Visibility.Visible));


        public Uri IgnoreButtonStatusIconUri
        {
            get { return (Uri)GetValue(IgnoreButtonStatusIconUriProperty); }
            set { SetValue(IgnoreButtonStatusIconUriProperty, value); }
        }


        // Using a DependencyProperty as the backing store for IgnoreButtonStatusIconUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IgnoreButtonStatusIconUriProperty =
            DependencyProperty.Register("IgnoreButtonStatusIconUri", //NOXLATE
                typeof(Uri),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(null));


        public string IgnoreButtonTooltip
        {
            get { return (string)GetValue(IgnoreButtonTooltipProperty); }
            set { SetValue(IgnoreButtonTooltipProperty, value); }
        }


        // Using a DependencyProperty as the backing store for IgnoreButtonTooltip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IgnoreButtonTooltipProperty =
            DependencyProperty.Register("IgnoreButtonTooltip", //NOXLATE
                typeof(string),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(string.Empty));


        public string RulePointPath
        {
            get;
            set;
        }


        public Visibility CheckBoxVisibility
        {
            get { return (Visibility)GetValue(CheckBoxVisibilityProperty); }
            set { SetValue(CheckBoxVisibilityProperty, value); }
        }


        // Using a DependencyProperty as the backing store for CheckBoxVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckBoxVisibilityProperty =
            DependencyProperty.Register("CheckBoxVisibility", //NOXLATE
                typeof(Visibility),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(Visibility.Visible));


        public IValidationItem ValidationItem
        {
            get { return (IValidationItem)GetValue(ValidationItemProperty); }
            set { SetValue(ValidationItemProperty, value); }
        }


        // Using a DependencyProperty as the backing store for ValidationItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidationItemProperty =
            DependencyProperty.Register("ValidationItem", //NOXLATE
                                        typeof(IValidationItem),
                                        typeof(ValidationItemContextBase),
                                        new UIPropertyMetadata(null, new PropertyChangedCallback(OnValidationItemChanged)));


        static void OnValidationItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ValidationItemContextBase cxt = sender as ValidationItemContextBase;
            if (cxt == null)
                return;

            cxt.FeatureIdentifier = String.Empty;
            if (args.NewValue != null)
            {
                IValidationItem validationItem = args.NewValue as IValidationItem;

                object o = validationItem.FeatureItem;
                if (o != null)
                    cxt.FeatureIdentifier = o.ToString();
            }
        }


        public string FeatureIdentifier
        {
            get { return (string)GetValue(FeatureIdentifierProperty); }
            set { SetValue(FeatureIdentifierProperty, value); }
        }


        // Using a DependencyProperty as the backing store for FeatureIdentifier.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FeatureIdentifierProperty =
            DependencyProperty.Register("FeatureIdentifier", //NOXLATE
                typeof(string),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(String.Empty));


        /// <summary>
        /// The text string in Orange shown on header of ValidationItemHeader
        /// </summary>
        public string ItemTypeStringForValidationItemHeader
        {
            get { return (string)GetValue(ItemTypeStringForValidationItemHeaderProperty); }
            set { SetValue(ItemTypeStringForValidationItemHeaderProperty, value); }
        }


        // Using a DependencyProperty as the backing store for ItemTypeStringForValidationItemHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTypeStringForValidationItemHeaderProperty =
            DependencyProperty.Register("ItemTypeStringForValidationItemHeader", //NOXLATE
                typeof(string),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(String.Empty));


        public ItemStatus Status
        {
            get { return (ItemStatus)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }


        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", //NOXLATE
                typeof(ItemStatus),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(ItemStatus.Normal));


        public bool IsMouseHovered
        {
            get { return (bool)GetValue(IsMouseHoveredProperty); }
            set { SetValue(IsMouseHoveredProperty, value); }
        }


        // Using a DependencyProperty as the backing store for IsMouseHovered.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMouseHoveredProperty =
            DependencyProperty.Register("IsMouseHovered", //NOXLATE
                typeof(bool),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(false, new PropertyChangedCallback(OnIsMouseHoveredChanged)));


        static void OnIsMouseHoveredChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ValidationItemContextBase cxt = sender as ValidationItemContextBase;
            if ((cxt == null) ||
                (cxt.ValidationItem == null))
                return;

            cxt.OnIsMouseHoveredChanged();
        }


        public Visibility GoToRuleButtonVisibility
        {
            get { return (Visibility)GetValue(GoToRuleButtonVisibilityProperty); }
            set { SetValue(GoToRuleButtonVisibilityProperty, value); }
        }


        // Using a DependencyProperty as the backing store for GoToRuleButtonVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GoToRuleButtonVisibilityProperty =
            DependencyProperty.Register("GoToRuleButtonVisibility", //NOXLATE
                typeof(Visibility),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(Visibility.Collapsed));


        public Visibility ResolutionsButtonVisibility
        {
            get { return (Visibility)GetValue(ResolutionsButtonVisibilityProperty); }
            set { SetValue(ResolutionsButtonVisibilityProperty, value); }
        }


        // Using a DependencyProperty as the backing store for ResolutionsButtonVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResolutionsButtonVisibilityProperty =
            DependencyProperty.Register("ResolutionsButtonVisibility", //NOXLATE
                typeof(Visibility),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(Visibility.Collapsed));


        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }


        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", //NOXLATE
                typeof(Brush),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(Brushes.Transparent));


        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", //NOXLATE
                typeof(Brush),
                typeof(ValidationItemContextBase),
                new UIPropertyMetadata(Brushes.Transparent));


        bool bIsVisble;
        public bool IsVisible
        {
            get
            {
                return this.bIsVisble;
            }
            set
            {
                this.bIsVisble = value;
            }
        }


        #endregion dynamic properties


        protected ValidationItemContextBase(DesignValidationContext owner, IValidationItem item)
        {
            if (item == null)
                throw new NullReferenceException("ValidationItemBase.Constructor: Parameter item can't be null"); // NOXLATE

            if (owner == null)
                throw new NullReferenceException("ValidationItemBase.Constructor: Parameter owner can't be null"); // NOXLATE

            this._owner = owner;

            this.IsVisible = true;

            this.ValidationItem = item;

            SetStatus(ItemStatus.Normal);

            this.UpdateUiByValidationType();
        }


        /// <summary>
        /// If it's an error item, hide checkbox and show error icon;
        ///   otherwise show checkbox and hide error icon
        /// Also sets item type string shown on validation item header
        /// </summary>
        public void UpdateUiByValidationType()
        {
            if (this.ValidationItem == null)
                return;

            switch (this.ValidationItem.ResultType)
            {
                case ValidationType.Error:
                    this.CheckBoxVisibility = Visibility.Collapsed;
                    this.ErrorIconVisibility = Visibility.Visible;
                    break;

                default:
                    this.CheckBoxVisibility = this.IsMouseHovered ? Visibility.Visible : Visibility.Hidden;
                    this.ErrorIconVisibility = Visibility.Collapsed;
                    break;
            }

            switch (ValidationItem.ResultType)
            {
                case ValidationType.Error:
                    this.ItemTypeStringForValidationItemHeader = Properties.Resources.ValidationItemHeaderFailure;
                    this.IgnoreButtonStatusIconUri = new Uri("../Icons/Block.png", UriKind.Relative); //NOXLATE
                    this.IgnoreButtonTooltip = string.Empty;
                    break;

                case ValidationType.Message:
                    this.ItemTypeStringForValidationItemHeader = Properties.Resources.ValidationItemHeaderMessage;
                    this.IgnoreButtonStatusIconUri = new Uri("../Icons/Block.png", UriKind.Relative); //NOXLATE
                    this.IgnoreButtonTooltip = Properties.Resources.ClickToBlock;
                    break;

                case ValidationType.Warning:
                    this.ItemTypeStringForValidationItemHeader = Properties.Resources.ValidationItemHeaderWarning;
                    this.IgnoreButtonStatusIconUri = new Uri("../Icons/Block.png", UriKind.Relative); //NOXLATE
                    this.IgnoreButtonTooltip = Properties.Resources.ClickToBlock;
                    break;

                case ValidationType.IgnoredWarning:
                    this.ItemTypeStringForValidationItemHeader = Properties.Resources.ValidationItemHeaderIgnoredWarning;
                    this.IgnoreButtonStatusIconUri = new Uri("../Icons/Unblock.ico", UriKind.Relative); //NOXLATE
                    this.IgnoreButtonTooltip = Properties.Resources.ClickToUnblock;
                    break;

                case ValidationType.IgnoredMessage:
                    this.IgnoreButtonStatusIconUri = new Uri("../Icons/Unblock.ico", UriKind.Relative); //NOXLATE
                    this.ItemTypeStringForValidationItemHeader = Properties.Resources.ValidationItemHeaderIgnoredMessage;
                    this.IgnoreButtonTooltip = Properties.Resources.ClickToUnblock;
                    break;
            }

            UpdateResolutionsButtonVisibility();

            UpdateGoToRuleButtonVisibility();

            UpdateBackground();
        }


        public void SetStatus(ItemStatus itemStatus)
        {
            if (itemStatus == this.Status)
                return;

            this.Status = itemStatus;

            UpdateBackground();
        }


        private void UpdateBackground()
        {
            if (this._owner == null)
            {
                Debug.Assert(false);
                return;
            }

            if (this.IsMouseHovered && (this.Status != ItemStatus.PrimarySelected))
            {
                this.Background = this.ItemRollBackground;
                this.BorderBrush = this.ItemRollBorderColor;
            }
            else
            {
                switch (this.Status)
                {
                    case ItemStatus.Normal:
                        this.Background = Brushes.White;
                        this.BorderBrush = Brushes.Transparent;
                        break;

                    case ItemStatus.PrimarySelected:
                        this.Background = this.ItemSelectedBackground;
                        this.BorderBrush = this.ItemSelectedBorderBrush;
                        break;

                    case ItemStatus.DeviceSelected:
                        this.Background = _deviceSelectedBrush;
                        this.BorderBrush = this.ItemSelectedBorderBrush;
                        break;
                }
            }
        }


        internal void IgnoreClicked()
        {
            if (this._owner == null)
            {
                Debug.Assert(false);
                return;
            }

            if (this.ValidationItem == null)
                return;

            //this._owner.IgnoreRequested(this);
        }


        internal void OnDeviceSelectionCancelled()
        {
            if (this.Status != ItemStatus.Normal) //to clear selection
                this.SetStatus(ItemStatus.Normal);
        }


        internal void OnDeviceSelected(bool isPrimarySelection)
        {
            this.SetStatus(isPrimarySelection ? ItemStatus.PrimarySelected : ItemStatus.DeviceSelected);
        }


        //public void UpdateVisibility(FilterContext filter)
        //{
        //    if (filter == null)
        //    {
        //        Debug.Assert(false);
        //        return;
        //    }

        //    bool ret = false;
        //    switch (this.ValidationItem.ResultType)
        //    {
        //        case ValidationType.Error:
        //            if (filter.IsShowingError)
        //                ret = true;
        //            break;

        //        case ValidationType.IgnoredMessage:
        //            if (filter.IsShowingIgnoredMessage)
        //                ret = true;
        //            break;

        //        case ValidationType.IgnoredWarning:
        //            if (filter.IsShowingIgnoredWarning)
        //                ret = true;
        //            break;

        //        case ValidationType.Message:
        //            if (filter.IsShowingMessage)
        //                ret = true;
        //            break;

        //        case ValidationType.Warning:
        //            if (filter.IsShowingWarning)
        //                ret = true;
        //            break;
        //    }

        //    this.IsVisible = ret;
        //}


        private void OnIsMouseHoveredChanged()
        {
            this.UpdateUiByValidationType();
        }


        public void UpdateGoToRuleButtonVisibility()
        {
            IValidationItem item = this.ValidationItem;

            bool hasRule = false;
            if (item != null)
            {
                if (string.IsNullOrWhiteSpace(item.RulePointPath) == false)
                {
                    hasRule = true;
                    this.RulePointPath = item.RulePointPath;
                }
            }

            if (hasRule)
            {
                this.GoToRuleButtonVisibility = this.IsMouseHovered ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                this.GoToRuleButtonVisibility = Visibility.Collapsed;
            }
        }


        public void UpdateResolutionsButtonVisibility()
        {
            //ResolutionValidationItem item = this.ValidationItem as ResolutionValidationItem;

            //bool needDetail = false;
            //if (item != null)
            //{
            //    if ((item.ResolutionItems != null) && (item.ResolutionItems.Count > 0))
            //        needDetail = true;
            //}

            //if (needDetail)
            //{
            //    this.ResolutionsButtonVisibility = this.IsMouseHovered ? Visibility.Visible : Visibility.Collapsed;
            //}
            //else
            //{
            //    this.ResolutionsButtonVisibility = Visibility.Collapsed;
            //}
        }

        protected DesignValidationContext _owner = null;
        public DesignValidationContext Owner
        {
            get
            {
                return this._owner;
            }
            set
            {
                this._owner = value;
            }
        }

        internal bool IsIgnored
        {
            get
            {
                if (this.ValidationItem == null)
                {
                    Debug.Assert(false);
                    return false;
                }

                switch (this.ValidationItem.ResultType)
                {
                    case ValidationType.IgnoredMessage:
                    case ValidationType.IgnoredWarning:
                        return true;
                }

                return false;
            }
        }

    }


    /// <summary>
    /// Status of a validation item
    /// </summary>
    public enum ItemStatus
    {
        Normal,
        PrimarySelected,
        DeviceSelected
    }
}
