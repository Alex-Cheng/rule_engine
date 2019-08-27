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
using System.Collections.ObjectModel;
using System.Diagnostics;

using Autodesk.IM.Rule.Activities;
using Autodesk.IM.UI.Rule;


namespace RuleConfiguration
{
    internal class EnumTypeSelectContext<T> : SelectContext where T : struct
    {
        public EnumTypeSelectContext(RuleEditingContext context, ItemSelector parent, string contextName, Collection<object> exclusiveItems)
            : base(parent)
        {
            this.contextName = contextName;
            if (exclusiveItems != null)
            {
                ExclusiveItems = new Collection<T>();
                foreach (object item in exclusiveItems)
                {
                    if (item is T)
                    {
                        ExclusiveItems.Add((T)item);
                    }
                    else
                    {
                        Debug.Assert(false, "Should not go here!"); // NOXLATE
                    }
                }
            }
        }


        private Collection<T> ExclusiveItems
        {
            get;
            set;
        }


        string contextName;
        public override string SelectContextName
        {
            get
            {
                return contextName;
            }
        }

        public override object CreateNewInstance(SelectItem item)
        {
            return (T)item.Value;
        }

        public override void UpdateSelectItems()
        {
            Array enumValues = Enum.GetValues(typeof(T));
            SelectItems.Clear();
            foreach (object enumValue in enumValues)
            {
                string name = enumValue.ToString();
                string displayName = name;
                if (enumValue is MatchOperator)
                {
                    displayName = GetOperatorDisplayName((MatchOperator)enumValue);
                }

                // TODO: get resource strings from tagged attributes for display name!
                if (ExclusiveItems == null)
                {
                    SelectItems.Add(new SelectItem(name, displayName, enumValue));
                }
                else if (!ExclusiveItems.Contains((T)enumValue))
                {
                    SelectItems.Add(new SelectItem(name, displayName, enumValue));
                }
            }
        }


        // TODO: It is temporary solution in CC phase. In near future, I will make a general approach
        // to address localization of enum members.
        private string GetOperatorDisplayName(MatchOperator operator_)
        {
            string result = null;
            switch (operator_)
            {
                case MatchOperator.Equals:
                    result = Properties.Resources.Equal;
                    break;
                case MatchOperator.GreaterThan:
                    result = Properties.Resources.GreaterThan;
                    break;
                case MatchOperator.GreaterThanOrEqual:
                    result = Properties.Resources.GreaterThanOrEqual;
                    break;
                case MatchOperator.LessThan:
                    result = Properties.Resources.LessThan;
                    break;
                case MatchOperator.LessThanOrEqual:
                    result = Properties.Resources.LessThanOrEqual;
                    break;
                case MatchOperator.NotEqual:
                    result = Properties.Resources.NotEqual;
                    break;
            }
            return result;
        }
    }
}
