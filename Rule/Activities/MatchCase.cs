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

using System.Activities;
using System.Windows.Markup;

namespace Autodesk.IM.Rule.Activities
{
    /// <summary>
    /// Represents a case in match.
    /// </summary>
    public class MatchCase
    {
        /// <summary>
        /// Gets or sets expression of the case.
        /// </summary>
        public Activity<DynamicValue> Expression
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the activity of the case.
        /// </summary>
        [DependsOn("Expression")] //NOXLATE
        public Activity Case
        {
            get;
            set;
        }
    }
}
