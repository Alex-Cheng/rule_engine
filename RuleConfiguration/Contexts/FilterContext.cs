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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;


namespace RuleConfiguration
{
    /// <summary>
    /// Context of the Treeview of DisplayConfigControl
    /// </summary>
    public class FilterContext : DependencyObject, ICloneable
    {
        public bool? IsSelectingAll
        {
            get { return (bool?)GetValue(IsSelectingAllProperty); }
            set { SetValue(IsSelectingAllProperty, value); }
        }


        // Using a DependencyProperty as the backing store for IsSelectingAll.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectingAllProperty =
            DependencyProperty.Register("IsSelectingAll", //NOXLATE
                typeof(bool?),
                typeof(FilterContext),
                new UIPropertyMetadata(null, new PropertyChangedCallback(IsSelectingAllChanged)));


        public bool IsShowingError
        {
            get { return (bool)GetValue(IsShowingErrorProperty); }
            set { SetValue(IsShowingErrorProperty, value); }
        }


        // Using a DependencyProperty as the backing store for IsShowingError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowingErrorProperty =
            DependencyProperty.Register("IsShowingError", //NOXLATE
                typeof(bool),
                typeof(FilterContext),
                new UIPropertyMetadata(true, new PropertyChangedCallback(IsSpecificTypeChanged)));


        public bool IsShowingWarning
        {
            get { return (bool)GetValue(IsShowingWarningProperty); }
            set { SetValue(IsShowingWarningProperty, value); }
        }


        // Using a DependencyProperty as the backing store for IsShowingWarning.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowingWarningProperty =
            DependencyProperty.Register("IsShowingWarning", //NOXLATE
                typeof(bool),
                typeof(FilterContext),
                new UIPropertyMetadata(true, new PropertyChangedCallback(IsSpecificTypeChanged)));


        public bool IsShowingMessage
        {
            get { return (bool)GetValue(IsShowingMessageProperty); }
            set { SetValue(IsShowingMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShowingMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowingMessageProperty =
            DependencyProperty.Register("IsShowingMessage", //NOXLATE
                typeof(bool),
                typeof(FilterContext),
                new UIPropertyMetadata(true, new PropertyChangedCallback(IsSpecificTypeChanged)));


        public bool IsShowingIgnoredWarning
        {
            get { return (bool)GetValue(IsShowingIgnoredWarningProperty); }
            set { SetValue(IsShowingIgnoredWarningProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShowingIgnoredWarning.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowingIgnoredWarningProperty =
            DependencyProperty.Register("IsShowingIgnoredWarning", //NOXLATE
                typeof(bool),
                typeof(FilterContext),
                new UIPropertyMetadata(false, new PropertyChangedCallback(IsSpecificTypeChanged)));


        public bool IsShowingIgnoredMessage
        {
            get { return (bool)GetValue(IsShowingIgnoredMessageProperty); }
            set { SetValue(IsShowingIgnoredMessageProperty, value); }
        }


        // Using a DependencyProperty as the backing store for IsShowingIgnoredMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowingIgnoredMessageProperty =
            DependencyProperty.Register("IsShowingIgnoredMessage", //NOXLATE
            typeof(bool),
            typeof(FilterContext),
            new UIPropertyMetadata(false, new PropertyChangedCallback(IsSpecificTypeChanged)));


        /// <summary>
        /// Once user clicked 'Selecting All' checkbox, other checkbox items need be updated
        /// </summary>
        bool isSelectingAllClicking = false;
        static void IsSelectingAllChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            FilterContext cxt = sender as FilterContext;
            if (cxt == null)
                return;

            if (args.NewValue == args.OldValue)
                return;

            if (cxt.IsSelectingAll == null)
                return;

            //this is triggered because user clicked e.g. 'Error' box, then 'Selecting All'
            //  is automatically being updated. In this case, we don't need to update anything.
            if (cxt.isSelectingSpeificTypeClicking)
                return;

            cxt.isSelectingAllClicking = true;

            try
            {
                if (cxt.IsShowingError != cxt.IsSelectingAll.Value)
                    cxt.IsShowingError = cxt.IsSelectingAll.Value;

                if (cxt.IsShowingWarning != cxt.IsSelectingAll.Value)
                    cxt.IsShowingWarning = cxt.IsSelectingAll.Value;

                if (cxt.IsShowingMessage != cxt.IsSelectingAll.Value)
                    cxt.IsShowingMessage = cxt.IsSelectingAll.Value;

                if (cxt.IsShowingIgnoredWarning != cxt.IsSelectingAll.Value)
                    cxt.IsShowingIgnoredWarning = cxt.IsSelectingAll.Value;

                if (cxt.IsShowingIgnoredMessage != cxt.IsSelectingAll.Value)
                    cxt.IsShowingIgnoredMessage = cxt.IsSelectingAll.Value;
            }
            finally
            {
                cxt.isSelectingAllClicking = false;
            }

            cxt.CheckIsValid();
        }


        /// <summary>
        /// Once user clicked any checkbox other than 'Select All', 'Select All' checkbox needs be updated
        /// </summary>
        bool isSelectingSpeificTypeClicking = false;
        static void IsSpecificTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            FilterContext cxt = sender as FilterContext;
            if (cxt == null)
                return;

            if (args.NewValue == args.OldValue)
                return;

            //this is triggered because user clicked 'Select All', then other checkboxes are being updated automatically;
            //  so we don't need to do anything here.
            if (cxt.isSelectingAllClicking)
                return;

            cxt.isSelectingSpeificTypeClicking = true;

            try
            {
                bool bTrueFound = false;
                bool bFalseFound = false;

                if (cxt.IsShowingError)
                    bTrueFound = true;
                else
                    bFalseFound = true;

                if (cxt.IsShowingWarning)
                    bTrueFound = true;
                else
                    bFalseFound = true;

                if (cxt.IsShowingMessage)
                    bTrueFound = true;
                else
                    bFalseFound = true;

                if (cxt.IsShowingIgnoredWarning)
                    bTrueFound = true;
                else
                    bFalseFound = true;

                if (cxt.IsShowingIgnoredMessage)
                    bTrueFound = true;
                else
                    bFalseFound = true;

                if (bTrueFound && bFalseFound)
                    cxt.IsSelectingAll = null;
                else if (bTrueFound)
                    cxt.IsSelectingAll = true;
                else if (bFalseFound)
                    cxt.IsSelectingAll = false;
                else
                    Debug.Assert(false);
            }
            finally
            {
                cxt.isSelectingSpeificTypeClicking = false;
            }

            cxt.CheckIsValid();
        }


        /// <summary>
        /// If it's not valid, OK button will be disabled
        /// </summary>
        private void CheckIsValid()
        {
            if (this.IsValidChanged == null)
                return;

            if (this.IsShowingError || this.IsShowingWarning || this.IsShowingMessage
                || this.IsShowingIgnoredWarning || this.IsShowingIgnoredMessage)
                this.IsValidChanged(true);
            else
                this.IsValidChanged(false);
        }


        public object Clone()
        {
            FilterContext copy = new FilterContext();
            copy.IsSelectingAll = this.IsSelectingAll;
            copy.IsShowingError = this.IsShowingError;
            copy.IsShowingWarning = this.IsShowingWarning;
            copy.IsShowingMessage = this.IsShowingMessage;
            copy.IsShowingIgnoredWarning = this.IsShowingIgnoredWarning;
            copy.IsShowingIgnoredMessage = this.IsShowingIgnoredMessage;

            return copy;
        }


        public void CopyFrom(FilterContext src)
        {
            if (src == null)
                return;

            this.IsSelectingAll = src.IsSelectingAll;
            this.IsShowingError = src.IsShowingError;
            this.IsShowingWarning = src.IsShowingWarning;
            this.IsShowingMessage = src.IsShowingMessage;
            this.IsShowingIgnoredWarning = src.IsShowingIgnoredWarning;
            this.IsShowingIgnoredMessage = src.IsShowingIgnoredMessage;
        }


        public void Clear()
        {
            this.IsSelectingAll = true;
        }


        /// <summary>
        /// Get a single string describing what types of message (error, warning, etc.) are shown
        /// </summary>
        /// <returns></returns>
        public string GetDescriptionString()
        {
            if (this.IsSelectingAll ?? false)
                return Properties.Resources.ShowingAll;

            StringBuilder sb = new StringBuilder();
            if (this.IsShowingError)
                sb.Append(Properties.Resources.Error);

            if (this.IsShowingWarning)
            {
                if (sb.Length > 0)
                    sb.Append(Properties.Resources.andWithWhiteSpaces);
                sb.Append(Properties.Resources.Warning);
            }

            if (this.IsShowingMessage)
            {
                if (sb.Length > 0)
                    sb.Append(Properties.Resources.andWithWhiteSpaces);
                sb.Append(Properties.Resources.Message);
            }

            if (this.IsShowingIgnoredWarning)
            {
                if (sb.Length > 0)
                    sb.Append(Properties.Resources.andWithWhiteSpaces);
                sb.Append(Properties.Resources.IgnoredWarning);
            }

            if (this.IsShowingIgnoredMessage)
            {
                if (sb.Length > 0)
                    sb.Append(Properties.Resources.andWithWhiteSpaces);
                sb.Append(Properties.Resources.IgnoredMessage);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Get a tooltip object describing what types of message (error, warning, etc.) are shown
        /// </summary>
        /// <returns></returns>
        public object GetDescriptionTooltip()
        {
            TextBlock toolTip = new TextBlock();

            Bold titleShown = new Bold();
            titleShown.Inlines.Add(Properties.Resources.ShownType);
            toolTip.Inlines.Add(titleShown);
            toolTip.Inlines.Add(new LineBreak());

            if (this.IsShowingError)
            {
                toolTip.Inlines.Add("-");//NOXLATE
                toolTip.Inlines.Add(Properties.Resources.Error);
                toolTip.Inlines.Add(new LineBreak());
            }

            if (this.IsShowingWarning)
            {
                toolTip.Inlines.Add("-");//NOXLATE
                toolTip.Inlines.Add(Properties.Resources.Warning);
                toolTip.Inlines.Add(new LineBreak());
            }

            if (this.IsShowingMessage)
            {
                toolTip.Inlines.Add("-");//NOXLATE
                toolTip.Inlines.Add(Properties.Resources.Message);
                toolTip.Inlines.Add(new LineBreak());
            }

            if (this.IsShowingIgnoredWarning)
            {
                toolTip.Inlines.Add("-");//NOXLATE
                toolTip.Inlines.Add(Properties.Resources.IgnoredWarning);
                toolTip.Inlines.Add(new LineBreak());
            }

            if (this.IsShowingIgnoredMessage)
            {
                toolTip.Inlines.Add("-");//NOXLATE
                toolTip.Inlines.Add(Properties.Resources.IgnoredMessage);
                toolTip.Inlines.Add(new LineBreak());
            }

            toolTip.Inlines.Add(new LineBreak());

            Bold titleHidden = new Bold();
            titleHidden.Inlines.Add(Properties.Resources.HiddenType);
            toolTip.Inlines.Add(titleHidden);
            toolTip.Inlines.Add(new LineBreak());

            if (!this.IsShowingError)
            {
                toolTip.Inlines.Add("-");//NOXLATE
                toolTip.Inlines.Add(Properties.Resources.Error);
                toolTip.Inlines.Add(new LineBreak());
            }

            if (!this.IsShowingWarning)
            {
                toolTip.Inlines.Add("-");//NOXLATE
                toolTip.Inlines.Add(Properties.Resources.Warning);
                toolTip.Inlines.Add(new LineBreak());
            }

            if (!this.IsShowingMessage)
            {
                toolTip.Inlines.Add("-");//NOXLATE
                toolTip.Inlines.Add(Properties.Resources.Message);
                toolTip.Inlines.Add(new LineBreak());
            }

            if (!this.IsShowingIgnoredWarning)
            {
                toolTip.Inlines.Add("-");//NOXLATE
                toolTip.Inlines.Add(Properties.Resources.IgnoredWarning);
                toolTip.Inlines.Add(new LineBreak());
            }

            if (!this.IsShowingIgnoredMessage)
            {
                toolTip.Inlines.Add("-");//NOXLATE
                toolTip.Inlines.Add(Properties.Resources.IgnoredMessage);
                toolTip.Inlines.Add(new LineBreak());
            }

            return toolTip;
        }

        public event Action<bool> IsValidChanged = null;
    }
}
