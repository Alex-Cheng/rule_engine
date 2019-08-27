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
using System.Windows;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Represents arguments of 'item selected' event.
    /// </summary>
    public class ItemSelectedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.ItemSelectedEventArgs class.
        /// </summary>
        public ItemSelectedEventArgs()
        {
        }


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.ItemSelectedEventArgs class with specified
        /// routed event and source parameters.
        /// </summary>
        /// <param name="routedEvent">Specified routed event.</param>
        /// <param name="source">Specified source of the event.</param>
        public ItemSelectedEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
        }


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.ItemSelectedEventArgs class with specified
        /// routed event, source parameters and selected item.
        /// </summary>
        /// <param name="routedEvent">Specified routed event.</param>
        /// <param name="source">Specified source of the event.</param>
        /// <param name="item">The selected item.</param>
        public ItemSelectedEventArgs(RoutedEvent routedEvent, object source, object item)
            : base(routedEvent, source)
        {
            Item = item;
        }


        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public object Item
        {
            get;
            set;
        }
    }
}
