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
using System.Diagnostics;


namespace Autodesk.IM.Rule.Activities
{
    /// <summary>
    /// Provides operators used in Rule System.
    /// </summary>
    public static class DynamicOperators
    {
        static readonly ActivitySignature OperatorSignature = new ActivitySignature();


        /// <summary>
        /// Registers operators to the specified rule activity manager.
        /// </summary>
        /// <param name="activityManager">The RuleActivityManager object used for registration of operators.</param>
        public static void RegisterOperators(RuleActivityManager activityManager)
        {
            RegisterOperator<Add>(activityManager, OperatorEntry.OperatorCategory.Binary, Properties.Resources.AddOperator);
            RegisterOperator<Subtract>(activityManager, OperatorEntry.OperatorCategory.Binary, Properties.Resources.SubtractOperator);
            RegisterOperator<Multiply>(activityManager, OperatorEntry.OperatorCategory.Binary, Properties.Resources.MultiplyOperator);
            RegisterOperator<Divide>(activityManager, OperatorEntry.OperatorCategory.Binary, Properties.Resources.DivideOperator);

            RegisterOperator<Equal>(activityManager, OperatorEntry.OperatorCategory.Binary, Properties.Resources.Equal);
            RegisterOperator<NotEqual>(activityManager, OperatorEntry.OperatorCategory.Binary, Properties.Resources.NotEqual);
            RegisterOperator<GreaterThan>(activityManager, OperatorEntry.OperatorCategory.Binary, Properties.Resources.GreaterThan);
            RegisterOperator<LessThan>(activityManager, OperatorEntry.OperatorCategory.Binary, Properties.Resources.LessThan);
            RegisterOperator<GreaterThanOrEqual>(activityManager, OperatorEntry.OperatorCategory.Binary, Properties.Resources.GreaterThanOrEqual);
            RegisterOperator<LessThanOrEqual>(activityManager, OperatorEntry.OperatorCategory.Binary, Properties.Resources.LessThanOrEqual);

            RegisterOperator<AndAlso>(activityManager, OperatorEntry.OperatorCategory.Binary, Properties.Resources.And);
            RegisterOperator<OrElse>(activityManager, OperatorEntry.OperatorCategory.Binary, Properties.Resources.Or);
            RegisterOperator<Not>(activityManager, OperatorEntry.OperatorCategory.Unary, Properties.Resources.Not);
            RegisterOperator<Xor>(activityManager, OperatorEntry.OperatorCategory.Binary, Properties.Resources.Xor);
        }


        private static void RegisterOperator<T>(
            RuleActivityManager activityManager, OperatorEntry.OperatorCategory category, string displayName)
            where T : CodeActivity<DynamicValue>, new()
        {
            string functionName = typeof(T).Name;
            activityManager.RegisterOperator(
                    functionName,
                    displayName,
                    () => new T(),
                    OperatorSignature,
                    typeof(T),
                    typeof(DynamicValue),
                    category);
        }
    }


    /// <summary>
    /// Add operator(+).
    /// </summary>
    public sealed class Add : DynamicOperator
    {
        protected override DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right)
        {
            return new DynamicValue(left + right);
        }
    }


    /// <summary>
    /// Subtract operator(-).
    /// </summary>
    public sealed class Subtract : DynamicOperator
    {
        protected override DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right)
        {
            return new DynamicValue(left - right);
        }
    }


    /// <summary>
    /// Multiply operator(*).
    /// </summary>
    public sealed class Multiply : DynamicOperator
    {
        protected override DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right)
        {
            return new DynamicValue(left * right);
        }
    }


    /// <summary>
    /// Divide operator(/).
    /// </summary>
    public sealed class Divide : DynamicOperator
    {
        protected override DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right)
        {
            return new DynamicValue(left / right);
        }
    }


    /// <summary>
    /// Equal operator(==).
    /// </summary>
    public sealed class Equal : DynamicOperator
    {
        protected override DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right)
        {
            return new DynamicValue(left == right);
        }
    }


    /// <summary>
    /// Not equal operator(!=).
    /// </summary>
    public sealed class NotEqual : DynamicOperator
    {
        protected override DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right)
        {
            return new DynamicValue(left != right);
        }
    }


    /// <summary>
    /// Greater than operator(>).
    /// </summary>
    public sealed class GreaterThan : DynamicOperator
    {
        protected override DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right)
        {
            return new DynamicValue(left > right);
        }
    }


    /// <summary>
    /// Less than operator(&lt;).
    /// </summary>
    public sealed class LessThan : DynamicOperator
    {
        protected override DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right)
        {
            return new DynamicValue(left < right);
        }
    }


    /// <summary>
    /// Greater than or equal to operator (>=).
    /// </summary>
    public sealed class GreaterThanOrEqual : DynamicOperator
    {
        protected override DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right)
        {
            return new DynamicValue(left >= right);
        }
    }


    /// <summary>
    /// Less than or equal to operator (&lt;=);
    /// </summary>
    public sealed class LessThanOrEqual : DynamicOperator
    {
        protected override DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right)
        {
            return new DynamicValue(left <= right);
        }
    }


    /// <summary>
    /// And operator(&&).
    /// </summary>
    public sealed class AndAlso : DynamicOperator
    {
        protected override DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right)
        {
            return new DynamicValue(left.ToBoolean(null) && right.ToBoolean(null));
        }
    }


    /// <summary>
    /// Or operator(||).
    /// </summary>
    public sealed class OrElse : DynamicOperator
    {
        protected override DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right)
        {
            return new DynamicValue(left.ToBoolean(null) || right.ToBoolean(null));
        }
    }


    /// <summary>
    /// Not operator(!).
    /// </summary>
    public sealed class Not : CodeActivity<DynamicValue>
    {
        public InArgument<DynamicValue> Operand { get; set; }

        protected override DynamicValue Execute(CodeActivityContext context)
        {
            DynamicValue operand = this.Operand.Get(context);
            return new DynamicValue(!operand.ToBoolean(null));
        }
    }


    /// <summary>
    /// Exclusive or operator(^).
    /// </summary>
    public sealed class Xor : DynamicOperator
    {
        protected override DynamicValue ExecuteInternal(DynamicValue left, DynamicValue right)
        {
            return new DynamicValue(left.ToBoolean(null) ^ right.ToBoolean(null));
        }
    }
}
