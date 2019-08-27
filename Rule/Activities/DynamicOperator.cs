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
    /// Represents an operator with two arguments.
    /// </summary>
    public abstract class DynamicOperator : CodeActivity<DynamicValue>
    {
        /// <summary>
        /// Gets or sets left operand.
        /// </summary>
        public InArgument<DynamicValue> Left
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets right operand.
        /// </summary>
        public InArgument<DynamicValue> Right
        {
            get;
            set;
        }

        protected override DynamicValue Execute(CodeActivityContext context)
        {
            DynamicValue left = this.Left.Get(context);
            DynamicValue right = this.Right.Get(context);
            return ExecuteInternal(left, right);
        }

        protected abstract DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right);
    }
}
