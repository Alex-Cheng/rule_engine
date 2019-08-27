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

using System.Activities;
using System.Activities.Expressions;
using System.Activities.Presentation.Model;
using System.Collections.Generic;

using Autodesk.IM.Rule;
using Autodesk.IM.Rule.Activities;
using Autodesk.IM.UI.Rule;
using IMConstants = Autodesk.IM.UI.Rule.Constants;


namespace RuleConfiguration
{
    /// <summary>
    /// Includes extensions methods for ModelItem.
    /// </summary>
    public static class AudModelItemExtensions
    {
        /// <summary>
        /// Returns valid values for given model item representing InArgument.
        /// </summary>
        /// <param name="item">The model item</param>
        /// <returns>A list of tuple(ID, Value) made of ID and string representation of valid values </returns>
        public static IEnumerable<DynamicValue> GetValidValues(this ModelItem item)
        {
            // Check if the value is in expression like "Status equals xxx".
            if (item.Source != null)
            {
                if (item.Source.Name == IMConstants.RightOperandPropertyName)
                {
                    foreach (DynamicValue value in GetValidValuesForBinaryOperator(item))
                    {
                        yield return value;
                    }
                }
            }
        }


        private static IEnumerable<DynamicValue> GetValidValuesForBinaryOperator(ModelItem item)
        {
            // Check if the value is in expression like "Status equals xxx".
            ModelProperty modelProperty = item.Parent.Properties[IMConstants.LeftOperandPropertyName];
            if (modelProperty == null)
                yield break;

            ModelProperty expr = modelProperty.Value.Properties["Expression"];
            if (expr != null)
            {
                ModelProperty property = expr.Value.Properties["PropertyName"];
                if (property != null && property.ComputedValue.ToString() == "Material")
                {
                    yield return "A";
                    yield return "B";
                    yield return "C";
                }
            }
        }
    }
}
