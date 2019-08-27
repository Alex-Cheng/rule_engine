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
using System.Linq;
using System.Text;
using System.Windows;


namespace RuleConfiguration
{
    /// <summary>
    /// Context class of the UI which sort, group and filter validation items
    /// </summary>
    public class DisplayConfigContext : DependencyObject
    {
        public DisplayConfigContext()
        {
            this.FilterContext = new FilterContext();
            UpdateFilterTextBoxContents();
        }


        /// <summary>
        /// The string shown in filter textbox
        /// </summary>
        public string FilterDescriptionString
        {
            get { return (string)GetValue(FilterDescriptionStringProperty); }
            set { SetValue(FilterDescriptionStringProperty, value); }
        }


        // Using a DependencyProperty as the backing store for FilterDescriptionString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterDescriptionStringProperty =
            DependencyProperty.Register("FilterDescriptionString", //NOXLATE
                typeof(string),
                typeof(DisplayConfigContext),
                new UIPropertyMetadata(String.Empty));


        /// <summary>
        /// The tooltip object of filter textbox
        /// </summary>
        public object FilterDescriptionTooltip
        {
            get { return (object)GetValue(FilterDescriptionTooltipProperty); }
            set { SetValue(FilterDescriptionTooltipProperty, value); }
        }


        // Using a DependencyProperty as the backing store for FilterDescriptionTooltip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterDescriptionTooltipProperty =
            DependencyProperty.Register("FilterDescriptionTooltip", //NOXLATE
                typeof(object),
                typeof(DisplayConfigContext),
                new UIPropertyMetadata(null));


        /// <summary>
        /// Is to group content by validation type
        /// </summary>
        public bool IsByValidationType
        {
            get { return (bool)GetValue(IsByValidationTypeProperty); }
            set { SetValue(IsByValidationTypeProperty, value); }
        }


        // Using a DependencyProperty as the backing store for IsByValidationType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsByValidationTypeProperty =
            DependencyProperty.Register("IsByValidationType", //NOXLATE
            typeof(bool),
            typeof(DisplayConfigContext),
            new UIPropertyMetadata(false, new PropertyChangedCallback(IsByValidationTypeChanged)));


        static void IsByValidationTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DisplayConfigContext cxt = sender as DisplayConfigContext;
            if (cxt == null)
                return;

            bool isByDevice = !cxt.IsByValidationType;

            if (cxt.IsByDeviceType != isByDevice)
                cxt.IsByDeviceType = isByDevice;

            if (cxt.GroupingChanged != null)
                cxt.GroupingChanged.Invoke();
        }


        /// <summary>
        /// Is to group content by device type
        /// </summary>
        public bool IsByDeviceType
        {
            get { return (bool)GetValue(IsByDeviceTypeProperty); }
            set { SetValue(IsByDeviceTypeProperty, value); }
        }


        // Using a DependencyProperty as the backing store for IsByDeviceType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsByDeviceTypeProperty =
            DependencyProperty.Register("IsByDeviceType", //NOXLATE
            typeof(bool),
            typeof(DisplayConfigContext),
            new UIPropertyMetadata(true, new PropertyChangedCallback(IsByDeviceTypeChanged)));


        static void IsByDeviceTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DisplayConfigContext cxt = sender as DisplayConfigContext;
            if (cxt == null)
                return;

            bool isByValidation = !cxt.IsByDeviceType;

            if (cxt.IsByValidationType != isByValidation)
                cxt.IsByValidationType = isByValidation;

            if (cxt.GroupingChanged != null)
                cxt.GroupingChanged.Invoke();
        }


        public bool IsGroupingPossible
        {
            get { return (bool)GetValue(IsGroupingPossibleProperty); }
            set { SetValue(IsGroupingPossibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsGroupingPossible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsGroupingPossibleProperty =
            DependencyProperty.Register("IsGroupingPossible",  //NOXLATE
                typeof(bool), typeof(DisplayConfigContext), new UIPropertyMetadata(false));


        public bool IsSortingPossible
        {
            get { return (bool)GetValue(IsSortingPossibleProperty); }
            set { SetValue(IsSortingPossibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSortingPossible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSortingPossibleProperty =
            DependencyProperty.Register("IsSortingPossible",  //NOXLATE
                typeof(bool), typeof(DisplayConfigContext), new UIPropertyMetadata(false));


        public bool IsOKBtnEnabled
        {
            get { return (bool)GetValue(IsOKBtnEnabledProperty); }
            set { SetValue(IsOKBtnEnabledProperty, value); }
        }


        // Using a DependencyProperty as the backing store for IsOKBtnEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOKBtnEnabledProperty =
            DependencyProperty.Register("IsOKBtnEnabled",  //NOXLATE
                typeof(bool),
                typeof(DisplayConfigContext),
                new UIPropertyMetadata(true));


        /// <summary>
        /// the context of filter used for filtering out validation items
        /// </summary>
        public FilterContext FilterContext
        {
            get { return (FilterContext)GetValue(FilterContextProperty); }
            set { SetValue(FilterContextProperty, value); }
        }


        // Using a DependencyProperty as the backing store for FilterContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterContextProperty =
            DependencyProperty.Register("FilterContext", //NOXLATE
                typeof(FilterContext),
                typeof(DisplayConfigContext),
                new UIPropertyMetadata(null));


        /// <summary>
        /// The copy of filter context used for editing filter
        /// </summary>
        public FilterContext FilterEditingContext
        {
            get { return (FilterContext)GetValue(FilterEditingContextProperty); }
            set { SetValue(FilterEditingContextProperty, value); }
        }


        // Using a DependencyProperty as the backing store for FilterEditingContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterEditingContextProperty =
            DependencyProperty.Register("FilterEditingContext", //NOXLATE
                typeof(FilterContext),
                typeof(DisplayConfigContext),
                new UIPropertyMetadata(null));


        public void RaiseSortingRequest(bool isAsc)
        {
            if (this.SortingRequested == null)
                return;

            this.SortingRequested(isAsc);
        }


        public void ClearFilter()
        {
            this.FilterEditingContext.Clear();
        }


        public void BeginFilterTransaction()
        {
            if (this.FilterEditingContext != null)
                this.FilterEditingContext.IsValidChanged -= new Action<bool>(FilterEditingContext_IsValidChanged);

            this.FilterEditingContext = this.FilterContext.Clone() as FilterContext;

            this.FilterEditingContext.IsValidChanged += new Action<bool>(FilterEditingContext_IsValidChanged);
        }

        void FilterEditingContext_IsValidChanged(bool isValid)
        {
            this.IsOKBtnEnabled = isValid;
        }


        public void CommitFilter()
        {
            this.FilterContext.CopyFrom(this.FilterEditingContext);

            if (this.FilterChanged != null)
                this.FilterChanged.Invoke();

            UpdateFilterTextBoxContents();
        }


        private void UpdateFilterTextBoxContents()
        {
            this.FilterDescriptionString = this.FilterContext.GetDescriptionString();
            this.FilterDescriptionTooltip = this.FilterContext.GetDescriptionTooltip();
        }


        public event Action GroupingChanged;

        public event Action<bool> SortingRequested;

        public event Action FilterChanged;
    }
}
