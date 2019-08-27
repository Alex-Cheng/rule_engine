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
    /// Registry entry for operators.
    /// </summary>
    public class OperatorEntry : ActivityEntry
    {
        /// <summary>
        /// Category of operators.
        /// </summary>
        public enum OperatorCategory
        {
            Unary,
            Binary
        }


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.OperatorEntry class with specified
        /// name, display name, factory function, signature, activity type, return type and operator category.
        /// </summary>
        /// <param name="name">The name of operator.</param>
        /// <param name="displayName">The diaply name of operator.</param>
        /// <param name="factoryFunction">The factory function to create new instnace of operator.</param>
        /// <param name="signature">The specified operator's signature.</param>
        /// <param name="activityType">The specified System.Type object representing the operator's type.</param>
        /// <param name="returnType">The specified System.Type object representing the operator's return type.</param>
        /// <param name="category">The specified category of the operator.</param>
        public OperatorEntry(
            string name,
            string displayName,
            Func<Activity> factoryFunction,
            ActivitySignature signature,
            Type activityType,
            Type returnType,
            OperatorCategory category)
            : base(name, displayName, factoryFunction, signature, activityType)
        {
            ReturnType = returnType;
            Category = category;
        }


        /// <summary>
        /// Gets a System.Type object representing the operator's return type.
        /// </summary>
        public Type ReturnType { get; private set; }


        /// <summary>
        /// Gets a System.Type object representing the operator's category.
        /// </summary>
        public OperatorCategory Category { get; private set; }
    }
}
