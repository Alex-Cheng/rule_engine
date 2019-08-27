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
using System.Xml.Serialization;


namespace Autodesk.IM.Rule.Activities
{
    /// <summary>
    /// Base class for literal expression activities which have a Uid property for localization.
    /// If indicated in the constructor, the class will initialize the Uid to a new Guid value.
    /// </summary>
    public abstract class UidLiteral<T> : CodeActivity<T>
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.Activities.UidLiteral with
        /// specified ID.
        /// </summary>
        /// <param name="createID"></param>
        protected UidLiteral(bool createID)
        {
            if (createID)
                this.Uid = Guid.NewGuid().ToString();
        }


        /// <summary>
        /// Gets or sets UID whill will be used for localization.
        /// </summary>
        public string Uid
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Represents a string constant/literal value in a rule, exposed as a string.
    /// The purpose of this class is to wrap a string in an activity and provide an ID for localization.
    /// </summary>
    public class StringLiteral : UidLiteral<string>
    {
        /// <summary>
        /// Create a new instance of InArgument class with specified value.
        /// </summary>
        /// <param name="value">The value to create an instance of InArgument class.</param>
        /// <returns>The created object.</returns>
        public static InArgument<string> CreateArgument(string value)
        {
            return new InArgument<string>(new StringLiteral(value, true));
        }


        /// <summary>
        /// Gets string from an activity.
        /// </summary>
        /// <param name="activity">The activity containing a string.</param>
        /// <returns>The string contained in the specified activity.</returns>
        public static string GetStringFromActivity(Activity activity)
        {
            if (activity is StringLiteral)
                return (activity as StringLiteral).Value;
            if (activity is DynamicStringLiteral)
                return (activity as DynamicStringLiteral).Value;
            if (activity is DynamicLiteral<string>)
                return (activity as DynamicLiteral<string>).Value;
            if (activity is Literal<string>)
                return (activity as Literal<string>).Value;
            return null;
        }


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.Activities.StringLiteral class.
        /// It is used by deserialization.
        /// </summary>
        public StringLiteral()
            : base(false)
        {
        }


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.Activities.StringLiteral class
        /// with value and ID.
        /// </summary>
        /// <param name="value">The value to create the instance.</param>
        /// <param name="createID">The specified ID.</param>
        public StringLiteral(string value, bool createID)
            : base(createID)
        {
            this.Value = value;
        }


        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        protected override string Execute(CodeActivityContext context)
        {
            return this.Value;
        }
    }

    /// <summary>
    /// Represents a string constant/literal value in a rule, exposed as a DynamicValue.
    /// The purpose of this class is to wrap a string in an activity and provide an ID for localization.
    /// </summary>
    public class DynamicStringLiteral : UidLiteral<DynamicValue>
    {
        /// <summary>
        /// Creates a new instance of InArgument&lt;T> with specified value.
        /// </summary>
        /// <param name="value">The specified value to create the argument.</param>
        /// <returns>The newly-created InArgument object.</returns>
        public static InArgument<DynamicValue> CreateArgument(string value)
        {
            return new InArgument<DynamicValue>(new DynamicStringLiteral(value, true));
        }


        /// <summary>
        /// Creates a new instance of DynamicStringLiteral class.
        /// </summary>
        public DynamicStringLiteral()
            : base(false)
        {
        }


        /// <summary>
        /// Creates a new instance of DynamicStringLiteral class with specified value and ID.
        /// </summary>
        /// <param name="value">The specified value for initialization.</param>
        /// <param name="createID">The specified ID.</param>
        public DynamicStringLiteral(string value, bool createID)
            : base(createID)
        {
            this.Value = value;
        }


        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        protected override DynamicValue Execute(CodeActivityContext context)
        {
            return new DynamicValue(this.Value);
        }
    }
}
