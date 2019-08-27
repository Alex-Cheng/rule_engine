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
using System.Activities.Presentation.Model;
using System.Globalization;
using System.Windows.Data;

using Autodesk.IM.Rule.Activities;
using Autodesk.IM.UI.Rule;


namespace RuleConfiguration
{
    internal class ArgumentToIsActivityBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ModelItem item = value as ModelItem;
            if (null == item)
                return false;
            return typeof(Activity).IsAssignableFrom(item.ItemType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    internal class ArgumentToIsLiteralBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ModelItem item = value as ModelItem;
            if (null != item)
                return IsLiteral(item);
            InArgument inArg = value as InArgument;
            if (null != inArg)
                return IsLiteral(inArg);
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        bool IsLiteral(ModelItem item)
        {
            ModelItem activityItem = item.GetActivityItem();
            if (null == activityItem)
                return false;
            return IsLiteral(activityItem.ItemType);
        }

        bool IsLiteral(InArgument inArg)
        {
            Activity activity = inArg.Expression;
            if (null == activity)
                return false;
            Type type = activity.GetType();
            return IsLiteral(type);
        }

        bool IsLiteral(Type type)
        {
            if (type.Equals(typeof(StringLiteral)) ||
                type.Equals(typeof(DynamicStringLiteral)))
                return true;
            if (!type.IsGenericType)
                return false;
            Type genericType = type.GetGenericTypeDefinition();
            return genericType.Equals(typeof(Literal<>)) ||
                genericType.Equals(typeof(DynamicLiteral<>));
        }
    }
}
