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


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Represents a hierarchical selectable item which contains sub-items.
    /// </summary>
    public class HierarchicalSelectItem : SelectItem
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.HierarchicalSelectItem class
        /// with specified name, display name and value.
        /// </summary>
        /// <param name="name">The specified name for the selectable item.</param>
        /// <param name="displayName">The specified display name showing in UI.</param>
        /// <param name="value">The value contained by the selectable item.</param>
        public HierarchicalSelectItem(string name, string displayName, object value)
            : base(name, displayName, value)
        {
            Children = new ObservableCollection<SelectItem>();
        }


        /// <summary>
        /// Gets sub-items contained by the selectable item.
        /// </summary>
        public ObservableCollection<SelectItem> Children
        {
            get;
            private set;
        }
    }
}
