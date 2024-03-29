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


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Represents arguments of value changing event.
    /// </summary>
    public class ValueChangingEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Gets or sets old value.
        /// </summary>
        public object OldValue
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets new value.
        /// </summary>
        public object NewValue
        {
            get;
            set;
        }
    }
}
