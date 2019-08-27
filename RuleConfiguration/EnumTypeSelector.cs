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
using System.Collections.ObjectModel;
using System.Windows;

using Autodesk.IM.Rule;
using Autodesk.IM.UI.Rule;

namespace RuleConfiguration
{
    // TODO: SUPPORT LOCALIZATION.
    public class EnumTypeSelector : ItemSelector
    {
        public EnumTypeSelector()
        {
            // If we define the default value in UIPropertyMetadata in the definition of
            // DependencyProperty, all objects' ExclusiveItems properties share ONE instance
            // of Collection<string>!!!
            // So I set the property in constructor instead.
            ExclusiveItems = new Collection<object>();
        }


        public Type EnumType { get; set; }


        public string SelectContextName { get; set; }


        #region ExclusiveItems

        public Collection<object> ExclusiveItems
        {
            get
            {
                return (Collection<object>)GetValue(ExclusiveItemsProperty);
            }
            set
            {
                SetValue(ExclusiveItemsProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ExclusiveItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExclusiveItemsProperty =
            DependencyProperty.Register("ExclusiveItems", typeof(Collection<object>), typeof(EnumTypeSelector), new UIPropertyMetadata(null)); // NOXLATE

        #endregion


        protected override void UpdateContexts(IEnumerable<DynamicValue> validValueItemHint)
        {
            if (this.Contexts.Count > 0)
                return;
            SelectContext context = CreateContext(this.EnumType, RuleEditingContext, this, this.SelectContextName, ExclusiveItems);
            this.Contexts.Add(context);
        }


        protected static SelectContext CreateContext(
            Type enumType, RuleEditingContext context, ItemSelector parent, string contextName, Collection<object> exclusiveItems)
        {
            Type genericType = typeof(EnumTypeSelectContext<>);
            Type specificType = genericType.MakeGenericType(enumType);
            return Activator.CreateInstance(
                specificType, context, parent, contextName, exclusiveItems) as SelectContext;
        }


        protected override string GetDisplayText()
        {
            // this override is needed so that the initialization will display the correct text
            // and we have to wait until this is hit after all the Owner and ItemName have been set
            // since this.Item is never set for this control, since it cannot be for a raw type like enum.
            ModelProperty modelProperty = GetModelProperty();
            if (null != modelProperty)
            {
                // Try get display text via NaturalLanguageHelper
                ActivityTranslator translator = EditingContext.Services.GetService<ActivityTranslator>();
                string displayText = translator.Translate(modelProperty.Value);
                if (displayText == null)
                {
                    return GetEnumTextDisplay(modelProperty.ComputedValue);
                }
                else
                {
                    return displayText;
                }
            }
            else
            {
                return String.Empty;
            }
        }


        private string GetEnumTextDisplay(object enumValue)
        {
            // TODO: convert enum value to display string! for now just ToString...
            return enumValue.ToString();
        }
    }
}
