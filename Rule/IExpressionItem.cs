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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// This interface represent an item in expression. Its concrete implementation would be
    /// literal value, functions, feature attributes and etc. Developers are able to extend it.
    /// </summary>
    public interface IExpressionItem
    {
        /// <summary>
        /// Gets the name of the expression item.
        /// </summary>
        string Name
        {
            get;
        }


        /// <summary>
        /// Gets the display name of the expression item.
        /// </summary>
        string DisplayName
        {
            get;
        }


        /// <summary>
        /// Creates an instance of Activity class representing the expression item.
        /// </summary>
        /// <returns>The created instance of Activity class.</returns>
        Activity CreateActivity();


        /// <summary>
        /// Gets the expression item's type.
        /// </summary>
        Type ValueType
        {
            get;
        }
    }
}
