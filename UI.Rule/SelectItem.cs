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


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Represents a selectable item.
    /// </summary>
    public class SelectItem
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.SelectItem class
        /// with specified name, display name and value.
        /// </summary>
        /// <param name="name">The specified name for the selectable item.</param>
        /// <param name="displayName">The specified display name showing in UI.</param>
        /// <param name="value">The value contained by the selectable item.</param>
        public SelectItem(string name, string displayName, object value)
        {
            Name = name;
            DisplayName = displayName;
            Value = value;
        }


        /// <summary>
        /// Gets or sets name of the selectable item.
        /// </summary>
        public string Name
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets display name of the selectable item.
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets value of the selectable item.
        /// </summary>
        public object Value
        {
            get;
            set;
        }
    }
}
