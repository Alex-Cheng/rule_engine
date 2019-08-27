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
    /// Factory of ConditionalValue instances.
    /// </summary>
    public static class ConditionalValueFactory
    {
        static readonly InArgument<DynamicValue> DefaultCondition = DynamicLiteral<bool>.CreateArgument(false);
        const int DefaultValue = 0;

        /// <summary>
        /// Creates an instance of ConditionalValue of type DynamicValue.
        /// </summary>
        /// <returns>Conditional value</returns>
        public static ConditionalValue<DynamicValue> CreateConditionalNumber()
        {
            return new ConditionalValue<DynamicValue>()
            {
                Condition = DefaultCondition,
                ValueWhenTrue = DynamicLiteral<int>.Create(0),
                ValueWhenFalse = DynamicLiteral<int>.Create(0)
            };
        }

        /// <summary>
        /// Creates an instance of ConditionalValue of type string.
        /// </summary>
        /// <returns>Conditional text</returns>
        public static ConditionalValue<string> CreateConditionalText()
        {
            return new ConditionalValue<string>()
            {
                Condition = DefaultCondition,
                ValueWhenTrue = new StringExpression()
                {
                    Elements =
                        {
                            StringLiteral.CreateArgument(Properties.Resources.PromptEditingString)
                        }
                },
                ValueWhenFalse = new StringExpression()
                {
                    Elements =
                        {
                            StringLiteral.CreateArgument(Properties.Resources.PromptEditingString)
                        }
                }
            };
        }
    }
}
