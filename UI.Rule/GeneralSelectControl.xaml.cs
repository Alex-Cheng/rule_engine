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
using System.Windows.Documents;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Represents a general UI control where users can select an item.
    /// </summary>
    public partial class GeneralSelectControl : TreeView
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.GeneralSelectControl class.
        /// </summary>
        public GeneralSelectControl()
        {
            InitializeComponent();
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Hyperlink link = sender as Hyperlink;
            Debug.Assert(link != null, "the sender type should be Hyperlink"); //NOXLATE

            SelectItem item = link.DataContext as SelectItem;
            Debug.Assert(item != null, "the DataContext of link type should be SelectItem"); //NOXLATE

            SelectContext context = DataContext as SelectContext;
            Debug.Assert(context != null, "the DataContext of the control should derive from SelectContext."); //NOXLATE

            object arg = context.CreateNewInstance(item);
            if (arg != null)
            {
                RaiseEvent(new ItemSelectedEventArgs(ItemSelector.ItemSelectedEvent, this, arg));
            }
        }
    }
}
