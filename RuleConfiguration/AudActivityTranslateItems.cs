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
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Linq;
using sae = System.Activities.Expressions;

using Autodesk.IM.Rule;
using Autodesk.IM.Rule.Activities;
using Autodesk.IM.UI.Rule;
using IMConstants = Autodesk.IM.UI.Rule.Constants;


namespace RuleConfiguration
{
    /// <summary>
    /// Represents AUD-specific translation items.
    /// </summary>
    public static class AudActivityTranslateItems
    {
        /// <summary>
        /// Add AUD-specific translation items to given translator.
        /// </summary>
        /// <param name="translator">The given ActivityTranslator object representing a translator.</param>
        public static void AddActivityTranslateItems(ActivityTranslator translator)
        {
            translator.AddTranslateItem(typeof(sae.Literal<>), LiteralValueTranslateItem);
            translator.AddTranslateItem(typeof(DynamicLiteral<>), DynamicLiteralValueTranslateItem);
            translator.AddTranslateItem(typeof(DynamicStringLiteral), DynamicLiteralValueTranslateItem);
        }


        private static string LiteralValueTranslateItem(ActivityTranslator translator, ModelItem item)
        {
            var temp = item.GetCurrentValue() as sae.Literal<DynamicValue>;
            if (null != temp)
            {
                DynamicValue id = temp.Value;
                DynamicValue validValue = GetValidValue(item, id);
                if (null != validValue.Value)
                    return validValue.ToString();
            }
            return translator.Translate(item.Properties[IMConstants.ValuePropertyName].Value);
        }


        private static DynamicValue GetValidValue(ModelItem item, DynamicValue id)
        {
            if (item.Parent != null)
            {
                //IEnumerable<DynamicValue> validValues = item.Parent.GetValidValues();
                //if (validValues.Any())
                //{
                //    try
                //    {
                //        return (from v in validValues
                //                where v.Equals(id)
                //                select v).First();
                //    }
                //    catch (InvalidOperationException)
                //    {
                //        return default(DynamicValue);
                //    }
                //}
            }
            return default(DynamicValue);
        }


        private static string DynamicLiteralValueTranslateItem(ActivityTranslator translator, ModelItem item)
        {
            object value = item.Properties[IMConstants.ValuePropertyName].Value.GetCurrentValue();
            DynamicValue validValue = GetValidValue(item, new DynamicValue(value));
            if (null != validValue.Value)
                return validValue.ToString();
            return value.ToString();
        }
    }
}
