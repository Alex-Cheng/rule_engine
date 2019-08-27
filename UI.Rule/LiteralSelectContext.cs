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
using System.Activities.Expressions;
using System.Diagnostics;
using System.Reflection;

using Autodesk.IM.Rule;
using Autodesk.IM.Rule.Activities;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Context of editing literal values.
    /// </summary>
    public sealed class LiteralSelectContext : SelectContext
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.LiteralSelectContext class
        /// with specified ItemSelector instance as parent.
        /// </summary>
        /// <param name="parent">The specified ItemSelector instance as parent.</param>
        public LiteralSelectContext(ItemSelector parent)
            : base(parent)
        {
        }


        public override void UpdateSelectItems()
        {
            SelectItems.Clear();
            string literalValue = Parent.TextDisplay;
            SelectItems.Add(new SelectItem(Properties.Resources.LiteralValue, literalValue, literalValue));
        }


        public override object CreateNewInstance(SelectItem item)
        {
            InArgument argument = null;
            string value = item.Value as string;
            Type outputType = GetOutputType();
            if (outputType == typeof(object))
            {
                // For Literal<T>, the T can only be value type or string type.
                outputType = typeof(string);
            }
            Debug.Assert(outputType.IsValueType || outputType == typeof(string));

            if (outputType == typeof(string))
                return CreateNewValueString(value);
            else if (outputType == typeof(DynamicValue))
                return CreateNewValueDynamic(value);

            MethodInfo method = this.GetType().GetMethod("CreateNewValue", BindingFlags.NonPublic | BindingFlags.Instance); // NOXLATE
            MethodInfo generic = method.MakeGenericMethod(outputType);
            argument = generic.Invoke(this, new object[] { value }) as InArgument;
            return argument;
        }


        public override string SelectContextName
        {
            get
            {
                return Properties.Resources.LiteralValueSelectContext;
            }
        }


        private InArgument CreateNewValueString(string value)
        {
            return StringLiteral.CreateArgument(value);
        }


        private InArgument CreateNewValueDynamic(string value)
        {
            double dvalue = 0.0;
            if (Double.TryParse(value, out dvalue))
                return DynamicLiteral.CreateArgument(dvalue);

            int ivalue = 0;
            if (Int32.TryParse(value, out ivalue))
                return DynamicLiteral.CreateArgument(ivalue);

            bool bvalue = false;
            if (Boolean.TryParse(value, out bvalue))
                return DynamicLiteral.CreateArgument(bvalue);

            return DynamicStringLiteral.CreateArgument(value);
        }


        private InArgument CreateNewValue<T>(string value)
        {
            T tvalue;
            DynamicValueConvert.TryConvertTo<T>(value, out tvalue);
            Literal<T> literal = new Literal<T>(tvalue);
            InArgument<T> item = new InArgument<T>(literal);
            return item;
        }
    }
}
