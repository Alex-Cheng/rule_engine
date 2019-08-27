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
using System.Diagnostics;
using System.Linq;

using Autodesk.IM.Rule;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Includes extensions methods for ModelItem.
    /// </summary>
    public static class ModelItemExtensions
    {
        /// <summary>
        /// Gets rule editing context service agaist given model item.
        /// </summary>
        public static RuleEditingContextService GetRuleEditingContextService(this ModelItem modelItem)
        {
            return modelItem.GetEditingContext().Services.GetService<RuleEditingContextService>();
        }


        /// <summary>
        /// Gets corresponding rule editing context for a given model item.
        /// </summary>
        /// <param name="modelItem">the model item</param>
        /// <returns>Corresponding rule editing context for the model item</returns>
        public static RuleEditingContext GetRuleEditingContext(this ModelItem modelItem)
        {
            RuleEditingContextService service = modelItem.GetRuleEditingContextService();
            Debug.Assert(service != null, "Something must be broken."); // NOXLATE
            return service.GetEditingContext(modelItem);
        }


        /// <summary>
        /// Gets the root rule editing context for given model item.
        /// </summary>
        /// <returns>Root rule editing context</returns>
        public static RuleEditingContext GetRootRuleEditingContext(this ModelItem modelItem)
        {
            RuleEditingContextService service = modelItem.GetRuleEditingContextService();
            return service.GetRootEditingContext();
        }


        /// <summary>
        /// Overrides rule editing context for the model item and its descendants.
        /// </summary>
        /// <param name="modelItem">the model item</param>
        /// <param name="context">the context to override</param>
        public static void OverrideRuleEditingContext(this ModelItem modelItem, RuleEditingContext context)
        {
            RuleEditingContextService service = modelItem.GetRuleEditingContextService();
            service.OverrideEditingContext(modelItem, context);
        }


        /// <summary>
        /// Gets generic type of a ModelItem object.
        /// </summary>
        /// <param name="item">The given ModelItem object.</param>
        /// <returns>The generic type of the ModelItem object if it is of generic type; otherwise,
        /// the System.Type object retrieved from its ItemType property.</returns>
        public static Type GetGenericType(this ModelItem item)
        {
            Type itemType = item.ItemType;
            Type genericType = itemType.IsGenericType ?
                itemType.GetGenericTypeDefinition() : itemType;
            return genericType;
        }


        /// <summary>
        /// Gets ModelItem object representing an Activity object, from InArgument.Expression property
        /// if given ModelItem object represents an InArgument object.
        /// </summary>
        /// <param name="item">The given ModelItem object that might represent an InArgument object.</param>
        /// <returns>The ModelItem object retrieved from InArgument.Expression if the given parameter represents an InArgument object;
        /// otherwise, the given parameter itself.</returns>
        public static ModelItem GetActivityItem(this ModelItem item)
        {
            Type genericType = item.GetGenericType();
            if (typeof(InArgument).IsAssignableFrom(genericType))
            {
                ModelProperty exprProp = item.Properties[Constants.ExpressionPropertyName];
                if (exprProp != null)
                    return exprProp.Value;
                else
                    return null;
            }
            return item;
        }


        /// <summary>
        /// Gets the expression value represented by given ModelItem object.
        /// </summary>
        /// <param name="item">The given ModelItem object.</param>
        /// <returns>The expression value.</returns>
        public static object GetExpressionValue(this ModelItem item)
        {
            ModelProperty exprProp = item.Properties[Constants.ExpressionPropertyName];
            if (null == exprProp)
            {
                ModelProperty property = item.Source;
                if (null == property)
                    return null;
                return property.ComputedValue;
            }
            return exprProp.ComputedValue;
        }


        /// <summary>
        /// Gets the value type represented by specified ModelPropety object.
        /// </summary>
        /// <param name="property">The specified ModelProperty object.</param>
        /// <returns>The value's type.</returns>
        public static Type GetValueType(this ModelProperty property)
        {
            Type argumentType;
            if (property != null && property.IsCollection)
            {
                // Get element type of collection
                argumentType = property.PropertyType.GetGenericArguments().First<Type>();
            }
            else if (property != null)
            {
                argumentType = property.PropertyType;
            }
            else
            {
                return null;
            }

            if (argumentType == typeof(InArgument<>))
            {
                return argumentType.GetGenericArguments().First<Type>();
            }
            else if (argumentType.IsGenericType)
            {
                return argumentType.GetGenericArguments().Last<Type>();
            }
            else if (argumentType == typeof(InArgument))
            {
                // If argument type is InArgument, it means all InArgument<?> are accepted.
                return typeof(object);
            }
            else if (argumentType == typeof(Activity))
            {
                return typeof(object);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }


        /// <summary>
        /// Gets the entry of registration of activity according specified ModelItem object.
        /// </summary>
        /// <param name="item">The specified ModelItem object.</param>
        /// <returns>The entry of registration of activity represented by the specified ModelItem object.</returns>
        public static ActivityEntry GetActivityEntry(this ModelItem item)
        {
            RuleEditingContext context = item.GetRuleEditingContext();
            RuleManager ruleManager = context.GetRuleSignature().Owner;
            RuleActivityManager activityManager = ruleManager.ActivityManager;
            ActivityEntry entry = activityManager.GetEntry(item.ItemType);
            return entry;
        }
    }
}
