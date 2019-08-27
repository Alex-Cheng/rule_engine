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
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Represents a UI control where users can input a literal value.
    /// </summary>
    public partial class LiteralSelectControl : ItemsControl
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.LiteralSelectControl class.
        /// </summary>
        public LiteralSelectControl()
        {
            InitializeComponent();
        }


        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnHyperlinkClick(sender, e);
            }
        }


        private void OnHyperlinkClick(object sender, RoutedEventArgs e)
        {
            DependencyObject element = sender as DependencyObject;
            SelectItem item = element.GetValue(FrameworkElement.DataContextProperty) as SelectItem;
            Debug.Assert(item != null, "the DataContext should be SelectItem"); //NOXLATE

            SelectContext context = DataContext as SelectContext;
            Debug.Assert(context != null, "the DataContext of the control should derive from SelectContext."); //NOXLATE

            object arg = context.CreateNewInstance(item);
            RaiseEvent(new ItemSelectedEventArgs(ItemSelector.ItemSelectedEvent, this, arg));
        }


        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox literalTextBox = sender as TextBox;
            if (literalTextBox != null)
            {
                literalTextBox.SelectAll();
            }
        }


        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox literalTextBox = sender as TextBox;
            if (literalTextBox != null && !literalTextBox.IsKeyboardFocused)
            {
                // If the text box is not yet focused, give it the focus and
                // stop further processing of this click event.
                literalTextBox.Focus();
                e.Handled = true;
            }
        }
    }
}
