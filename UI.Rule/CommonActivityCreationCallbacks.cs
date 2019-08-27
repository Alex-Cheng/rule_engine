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
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using sas = System.Activities.Statements;

using Autodesk.IM.Rule;
using Autodesk.IM.Rule.Activities;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Common callbacks for creation of activities.
    /// </summary>
    internal static class CommonActivityCreationCallbacks
    {
        internal static void RegisterCommonActivityCreationCallbacks(ActivityFactory factory)
        {
            factory.RegisterCreationCallback(typeof(If), DynamicIfCreationCallback);

            // Register binary operators callbacks.
            Type[] binaryOperationTypes = new Type[] {
                typeof(Add),
                typeof(Subtract),
                typeof(Multiply),
                typeof(Divide),
                typeof(Equal),
                typeof(NotEqual),
                typeof(GreaterThan),
                typeof(LessThan),
                typeof(GreaterThanOrEqual),
                typeof(LessThanOrEqual),
                typeof(AndAlso),
                typeof(OrElse),
                typeof(Xor)
            };
            RegisterBinaryOperationCallbacks(factory, binaryOperationTypes);

            factory.RegisterCreationCallback(typeof(Not), DynamicNotOperationCreationCallback);

        }


        private static void RegisterBinaryOperationCallbacks(ActivityFactory factory, IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                factory.RegisterCreationCallback(type, BinaryOperationCreationCallback<DynamicValue>);
            }
        }


        private static void DynamicIfCreationCallback(Activity activity, ModelProperty itemProperty)
        {
            If ifActivity = activity as If;
            ifActivity.Condition = new InArgument<DynamicValue>(
                new Equal()
                {
                    Left = DynamicLiteral.CreateArgument(Constants.DefaultNumberValue),
                    Right = DynamicLiteral.CreateArgument(Constants.DefaultNumberValue)
                });
            ifActivity.Then = new sas.Sequence();
            ifActivity.Else = new sas.Sequence();
        }        


        private static void DynamicNotOperationCreationCallback(Activity activity, ModelProperty itemProperty)
        {
            InArgument<DynamicValue> inArg = GetOldItemArgument<DynamicValue>(itemProperty);
            dynamic unaryOperation = activity;
            if (inArg != null)
                unaryOperation.Operand = inArg;
            else
                unaryOperation.Operand = DynamicLiteral<bool>.CreateArgument(true);//so the default is 'if ... is not true'
        }


        private static void BinaryOperationCreationCallback<T>(Activity activity, ModelProperty itemProperty)
        {
            InArgument<T> left = null, right = null;
            ModelItem modelItem = itemProperty != null ? itemProperty.Value : null;
            if (modelItem != null && modelItem.Properties[Constants.ExpressionPropertyName] != null)
            {
                ModelItem oldActivity = modelItem.Properties[Constants.ExpressionPropertyName].Value;
                if (oldActivity.Properties[Constants.LeftOperandPropertyName] != null && oldActivity.Properties[Constants.RightOperandPropertyName] != null)
                {
                    left = oldActivity.Properties[Constants.LeftOperandPropertyName].ComputedValue as InArgument<T>;
                    right = oldActivity.Properties[Constants.RightOperandPropertyName].ComputedValue as InArgument<T>;
                    oldActivity.Properties[Constants.LeftOperandPropertyName].SetValue(null);
                    oldActivity.Properties[Constants.RightOperandPropertyName].SetValue(null);
                }
            }

            dynamic binaryOperation = activity;

            if (left != null && right != null)
            {
                binaryOperation.Left = left;
                binaryOperation.Right = right;
            }
            else
            {
                if (modelItem != null)
                {
                    var originalItem = modelItem.GetCurrentValue() as InArgument<T>;
                    if (originalItem != null)
                    {
                        // Clear the Expression result as the operator will have the output result.
                        // If the result is not clear, exception will throw in this case.
                        if (originalItem.Expression != null)
                            originalItem.Expression.Result = null;
                        binaryOperation.Left = originalItem;
                    }
                }

                // Assign default value
                if (typeof(T) == typeof(DynamicValue))
                {
                    // Specify default values of DynamicValue as it does not have proper default value.
                    binaryOperation.Left = binaryOperation.Left ?? DynamicLiteral.CreateArgument(Constants.DefaultNumberValue);
                    binaryOperation.Right = binaryOperation.Right ?? DynamicLiteral.CreateArgument(Constants.DefaultNumberValue);
                }
                else
                {
                    binaryOperation.Left = binaryOperation.Left ?? new InArgument<T>(default(T));
                    binaryOperation.Right = binaryOperation.Right ?? new InArgument<T>(default(T));
                }
            }
        }


        private static InArgument<T> GetOldItemArgument<T>(ModelProperty itemProperty)
        {
            ModelItem modelItem = itemProperty != null ? itemProperty.Value : null;
            if (modelItem != null && modelItem.Properties[Constants.ExpressionPropertyName] != null)
            {
                //currently 'Not' is the only one unary operator, so following code won't help users;
                //however let's keep it here in case we add more unary operators in the future
                ModelItem oldActivity = modelItem.Properties[Constants.ExpressionPropertyName].Value;
                if (oldActivity.Properties[Constants.OperandPropertyName] != null)
                {
                    if (oldActivity.Properties[Constants.OperandPropertyName].ComputedValue is InArgument<T>)
                        return oldActivity.Properties[Constants.OperandPropertyName].ComputedValue as InArgument<T>;
                }
            }
            return null;
        }
    }
}
