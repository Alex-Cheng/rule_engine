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
using System.Diagnostics;
using sae = System.Activities.Expressions;
using sas = System.Activities.Statements;

using Autodesk.IM.Rule;
using Autodesk.IM.Rule.Activities;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Represents a set of common activity translation items.
    /// </summary>
    internal static class CommonActivityTranslateItems
    {
        /// <summary>
        /// Adds the common activity translation items to the given translator.
        /// </summary>
        /// <param name="translator">The given ActivityTranslator object representing a translator.</param>
        public static void AddCommonTranslateItems(ActivityTranslator translator)
        {
            // Dynamic Operators (AUD dynamic value-typed operators)
            translator.AddTranslateItem(typeof(Add), Autodesk.IM.UI.Rule.Properties.Resources.Add);
            translator.AddTranslateItem(typeof(Subtract), Autodesk.IM.UI.Rule.Properties.Resources.Subtract);
            translator.AddTranslateItem(typeof(Multiply), Autodesk.IM.UI.Rule.Properties.Resources.Multiply);
            translator.AddTranslateItem(typeof(Divide), Autodesk.IM.UI.Rule.Properties.Resources.Divide);
            translator.AddTranslateItem(typeof(Equal), Autodesk.IM.UI.Rule.Properties.Resources.Equal);
            translator.AddTranslateItem(typeof(NotEqual), Autodesk.IM.UI.Rule.Properties.Resources.NotEqual);
            translator.AddTranslateItem(typeof(GreaterThan), Autodesk.IM.UI.Rule.Properties.Resources.GreaterThan);
            translator.AddTranslateItem(typeof(LessThan), Autodesk.IM.UI.Rule.Properties.Resources.LessThan);
            translator.AddTranslateItem(typeof(GreaterThanOrEqual), Autodesk.IM.UI.Rule.Properties.Resources.GreaterThanOrEqual);
            translator.AddTranslateItem(typeof(LessThanOrEqual), Autodesk.IM.UI.Rule.Properties.Resources.LessThanOrEqual);

            translator.AddTranslateItem(typeof(AndAlso), Autodesk.IM.UI.Rule.Properties.Resources.And);
            translator.AddTranslateItem(typeof(OrElse), Autodesk.IM.UI.Rule.Properties.Resources.Or);
            translator.AddTranslateItem(typeof(Xor), Autodesk.IM.UI.Rule.Properties.Resources.Xor);
            translator.AddTranslateItem(typeof(Not), Autodesk.IM.UI.Rule.Properties.Resources.Not);

            // Operators (built-in WF4 operators)
            translator.AddTranslateItem(typeof(sae.Add<,,>), Autodesk.IM.UI.Rule.Properties.Resources.Add);
            translator.AddTranslateItem(typeof(sae.Subtract<,,>), Autodesk.IM.UI.Rule.Properties.Resources.Subtract);
            translator.AddTranslateItem(typeof(sae.Multiply<,,>), Autodesk.IM.UI.Rule.Properties.Resources.Multiply);
            translator.AddTranslateItem(typeof(sae.Divide<,,>), Autodesk.IM.UI.Rule.Properties.Resources.Divide);
            translator.AddTranslateItem(typeof(sae.Equal<,,>), Autodesk.IM.UI.Rule.Properties.Resources.Equal);
            translator.AddTranslateItem(typeof(sae.GreaterThan<,,>), Autodesk.IM.UI.Rule.Properties.Resources.GreaterThan);
            translator.AddTranslateItem(typeof(sae.LessThan<,,>), Autodesk.IM.UI.Rule.Properties.Resources.LessThan);
            translator.AddTranslateItem(typeof(sae.GreaterThanOrEqual<,,>), Autodesk.IM.UI.Rule.Properties.Resources.GreaterThanOrEqual);
            translator.AddTranslateItem(typeof(sae.LessThanOrEqual<,,>), Autodesk.IM.UI.Rule.Properties.Resources.LessThanOrEqual);

            translator.AddTranslateItem(typeof(sae.And<,,>), Autodesk.IM.UI.Rule.Properties.Resources.And);
            translator.AddTranslateItem(typeof(sae.Or<,,>), Autodesk.IM.UI.Rule.Properties.Resources.Or);
            translator.AddTranslateItem(typeof(sae.Not<,>), Autodesk.IM.UI.Rule.Properties.Resources.Not);

            // Rules
            translator.AddTranslateItem(typeof(sas.If), Autodesk.IM.UI.Rule.Properties.Resources.If);
            translator.AddTranslateItem(typeof(If), "If (Dynamic)"); //NOXLATE

            // Literals
            translator.AddTranslateItem(typeof(StringLiteral), StringLiteralValueTranslateItem);

            // Types
            translator.AddTranslateItem(typeof(MatchOperator), MatchOperatorTranslateItem);
        }


        private static string StringLiteralValueTranslateItem(ActivityTranslator translator, ModelItem item)
        {
            object value = item.Properties[Constants.ValuePropertyName].Value.GetCurrentValue();
            return value.ToString();
        }


        private static string MatchOperatorTranslateItem(ActivityTranslator translator, ModelItem item)
        {
            object obj = item.GetCurrentValue();
            Debug.Assert(obj is MatchOperator);
            MatchOperator operator_ = (MatchOperator)obj;
            string result = null;
            switch (operator_)
            {
                case MatchOperator.Equals:
                    result = Autodesk.IM.UI.Rule.Properties.Resources.Equal;
                    break;
                case MatchOperator.GreaterThan:
                    result = Autodesk.IM.UI.Rule.Properties.Resources.GreaterThan;
                    break;
                case MatchOperator.GreaterThanOrEqual:
                    result = Autodesk.IM.UI.Rule.Properties.Resources.GreaterThanOrEqual;
                    break;
                case MatchOperator.LessThan:
                    result = Autodesk.IM.UI.Rule.Properties.Resources.LessThan;
                    break;
                case MatchOperator.LessThanOrEqual:
                    result = Autodesk.IM.UI.Rule.Properties.Resources.LessThanOrEqual;
                    break;
                case MatchOperator.NotEqual:
                    result = Autodesk.IM.UI.Rule.Properties.Resources.NotEqual;
                    break;
            }
            return result;
        }
    }
}
