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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Interaction logic for DefaultSelectorLink.xaml
    /// </summary>
    public partial class DefaultSelectorLink : UserControl, ISelectorLink
    {
        private ItemSelector _selector = null;


        public DefaultSelectorLink(ItemSelector selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("controller"); //NOXLATE
            }

            _selector = selector;

            InitializeComponent();

            mLink.LostFocus += OnLinkLostFocus;
            mPopUp.KeyUp += OnPopupKeyUp;
        }


        public ItemSelector Selector
        {
            get
            {
                return _selector;
            }
        }


        public UIElement UIContent
        {
            get
            {
                return this;
            }
        }


        public TextBlock LinkText
        {
            get
            {
                return mLinkText;
            }
        }


        public event EventHandler OnRemove;

        public event EventHandler OnReset;

        public event EventHandler OnPopupOpened;


        public void OpenPopup()
        {
            if (!mPopUp.IsOpen)
            {
                mPopUp.IsOpen = true;

                if (OnPopupOpened != null)
                {
                    OnPopupOpened(this, new EventArgs());
                }
            }
        }


        public void ClosePopup()
        {
            mPopUp.IsOpen = false;
            //Move focus out of the popup
            Keyboard.Focus(null);
            //and focus on the hyperlink again
            Keyboard.Focus(this);
        }


        private void OnPopupKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ClosePopup();
                e.Handled = true;
            }
        }


        private void OnLinkLostFocus(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Hyperlink)
            {
                this.mPopUp.IsOpen = false;
                e.Handled = true;
            }
        }


        private void OnRemoveButtonClicked(object sender, RoutedEventArgs e)
        {
            if (OnRemove != null)
            {
                OnRemove(this, new EventArgs());
            }
        }


        private void OnResetButtonClicked(object sender, RoutedEventArgs e)
        {
            if (OnReset != null)
            {
                OnReset(this, new EventArgs());
            }
        }


        private void OnHyperlinkClicked(object sender, RoutedEventArgs e)
        {
            OpenPopup();
            e.Handled = true;
        }


        private void OnHyperlinkGotFocus(object sender, RoutedEventArgs e)
        {
            OpenPopup();
            e.Handled = true;
        }
    }
}
