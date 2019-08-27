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
using System.Reflection;
using System.Activities;
using System.Activities.Expressions;
using System.Windows.Markup;


namespace Autodesk.IM.Rule.Activities
{
    /// <summary>
    /// Non generic base class of the literal dynamic activity expression.
    /// Use this to construct a DynamicLiteral when a templated DynamicLiteral cannot be used.
    /// If passing null, it will be treated as an empty string and a DynamicLiteral(string) returned.
    /// </summary>
    public abstract class DynamicLiteral : CodeActivity<DynamicValue>
    {
        protected DynamicLiteral()
        {
        }


        /// <summary>
        /// Gets DynamicValue object representing a loosely-typed value.
        /// </summary>
        public abstract DynamicValue DynamicValue
        {
            get;
        }


        /// <summary>
        /// Creates a new instance of DynamicLiteral class with specified value.
        /// </summary>
        /// <param name="value">The specified value for initialization.</param>
        /// <returns>The new DynamicLiteral object.</returns>
        public static DynamicLiteral Create(object value)
        {
            if (null == value)
                value = String.Empty;
            Type genericType = typeof(DynamicLiteral<>);
            Type specificType = genericType.MakeGenericType(value.GetType());
            return Activator.CreateInstance(specificType, value) as DynamicLiteral;
        }


        /// <summary>
        /// Create a new instance of InArgument&lt;DynamicValue> with specified value.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <returns>The new instance of InArgument&lt;DynamicValue></returns>
        public static InArgument<DynamicValue> CreateArgument(object value)
        {
            if (value is DynamicValue)
                return CreateArgument(((DynamicValue)value).Value);
            return new InArgument<DynamicValue>(Create(value));
        }


        /// <summary>
        /// Gets instance of DynamicValue class from an activity.
        /// </summary>
        /// <param name="activity">The activity containing a DynamicValue object.</param>
        /// <returns>The DynamicValue object.</returns>
        public static DynamicValue GetValueFromActivity(Activity activity)
        {
            if (activity is Literal<DynamicValue>)
                return (activity as Literal<DynamicValue>).Value;
            else if (activity is DynamicLiteral)
                return (activity as DynamicLiteral).DynamicValue;
            return default(DynamicValue);
        }
    }

    /// <summary>
    /// Represents a literal typed hard coded value in a rule, exposed as a DynamicValue.
    /// With this class it can avoid using the bulkier literal expression activity of WF4.
    /// </summary>
    public class DynamicLiteral<T> : DynamicLiteral
    {
        /// <summary>
        /// Creates a new instance of InArgument&lt;DynamicValue> with specified value.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <returns>The new instance of InArgument&lt;DynamicValue></returns>
        public static InArgument<DynamicValue> CreateArgument(T value)
        {
            return new InArgument<DynamicValue>(new DynamicLiteral<T>(value));
        }


        /// <summary>
        /// Initiailizes a new instance of Autodesk.IM.Rule.Activities.DynamicLiteral&lt;T> class.
        /// </summary>
        public DynamicLiteral()
        {
        }


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.Activities.DynamicLiteral&lt;T> class
        /// with specified value.
        /// </summary>
        /// <param name="value">The specified value for initialization.</param>
        public DynamicLiteral(T value)
        {
            this.Value = value;
        }


        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public T Value
        {
            get;
            set;
        }


        /// <summary>
        /// Gets a DynamicValue object wrapping the value of this object.
        /// </summary>
        public override DynamicValue DynamicValue
        {
            get
            {
                return new DynamicValue(this.Value);
            }
        }

        protected override DynamicValue Execute(CodeActivityContext context)
        {
            return this.DynamicValue;
        }
    }
}
