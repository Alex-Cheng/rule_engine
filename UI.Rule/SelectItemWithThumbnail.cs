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
using System.Windows.Media;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Represents a selectable item with thumbnail.
    /// </summary>
    public class SelectItemWithThumbnail : SelectItem
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.SelectItemWithImage class.
        /// </summary>
        /// <param name="name">The specified name for the selectable item.</param>
        /// <param name="displayName">The specified display name showing in UI.</param>
        /// <param name="thumbnail">The thumbnail image of the selectable item.</param>
        /// <param name="value">The value contained by the selectable item.</param>        
        public SelectItemWithThumbnail(string name, string displayName, ImageSource thumbnail, object value)
            : base(name, displayName, value)
        {
            Thumbnail = thumbnail;
        }


        public ImageSource Thumbnail
        {
            get;
            private set;
        }
    }
}
