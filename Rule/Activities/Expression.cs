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
using System.Activities;


namespace Autodesk.IM.Rule.Activities
{
    /// <summary>
    /// Represents an expression consisting of activities. It is "General Rules" in UI according to product design.
    /// In code, the name 'ExpressionActivity' is more appropriate.
    /// </summary>
    /// <typeparam name="TResult">Return type</typeparam>
    public class Expression<TResult> : CodeActivity<TResult>
        where TResult : IConvertible
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public InArgument<DynamicValue> Value
        {
            get;
            set;
        }

        protected override TResult Execute(CodeActivityContext context)
        {
            DynamicValue value = Value.Get(context);
            TResult result = DynamicValueConvert.ConvertTo<TResult>(value);
            return result;
        }
    }
}
