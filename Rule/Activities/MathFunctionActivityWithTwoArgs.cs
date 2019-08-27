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
using System.Activities;


namespace Autodesk.IM.Rule.Activities
{
    /// <summary>
    /// Presents math functions with two arguments.
    /// </summary>
    public abstract class MathFunctionWithTwoArgs : CodeActivity<DynamicValue>
    {
        const double DefaultNumericValue = 1.0;


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.Activities.MathFunctionActivityWithTwoArgs.
        /// </summary>
        public MathFunctionWithTwoArgs()
        {
            this.Operand1 = DynamicLiteral.CreateArgument(DefaultNumericValue);
            this.Operand2 = DynamicLiteral.CreateArgument(DefaultNumericValue);
        }


        /// <summary>
        /// An operand of this function.
        /// </summary>
        public InArgument<DynamicValue> Operand1
        {
            get;
            set;
        }


        /// <summary>
        /// An operand of this function.
        /// </summary>
        public InArgument<DynamicValue> Operand2
        {
            get;
            set;
        }


        /// <summary>
        /// Execute to evaluate the math function.
        /// </summary>
        /// <param name="context">The context of execution of WF4 workflow.</param>
        /// <returns>The result of this function.</returns>
        protected override DynamicValue Execute(CodeActivityContext context)
        {
            DynamicValue operand1 = Operand1.Get(context);
            DynamicValue operand2 = Operand2.Get(context);
            return ExecuteInternal(operand1, operand2);
        }

        protected abstract DynamicValue ExecuteInternal(DynamicValue operand1, DynamicValue operand2);
    }
}
