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

using System.Collections.Generic;
using System.Diagnostics;

using Autodesk.IM.Rule;
using Autodesk.IM.Rule.Activities;
using Autodesk.IM.UI.Rule;


namespace RuleConfiguration
{
    /// <summary>
    /// Select context for named rules.
    /// </summary>
    internal sealed class NamedRuleSelectContext : SelectContext
    {
        private Dictionary<string, HierarchicalSelectItem> pathToSelectItemMap = new Dictionary<string,HierarchicalSelectItem>();


        public NamedRuleSelectContext(ItemSelector parent)
            : base(parent)
        {
            UpdateSelectItems();
        }


        public override string SelectContextName
        {
            get
            {
                return Properties.Resources.NamedRuleSelectContext;
            }
        }


        public override object CreateNewInstance(SelectItem item)
        {
            Debug.Assert(item.Value is NamedRule);
            NamedRule namedRule = item.Value as NamedRule;

            return InvokeNamedRule.Create(namedRule.Path);
        }


        public override void UpdateSelectItems()
        {
            SelectItems.Clear();
            pathToSelectItemMap.Clear();
            RuleEditingContext context = Parent.RuleEditingContext;
            if (context == null)
            {
                return;
            }

            IEnumerable<NamedRule> namedRules = context.GetNamedRuleActivities();
            foreach (var namedRule in namedRules)
            {
                SelectItem newItem = new SelectItem(namedRule.Name, namedRule.DisplayName, namedRule);
                RulePoint parent = namedRule.Parent;
                // Build the hierarchy.
                HierarchicalSelectItem parentSelectItem = GetSelectItem(namedRule.Parent);
                parentSelectItem.Children.Add(newItem);
            }

            var ruleManager = context.GetRuleSignature().Owner;
            string rootPath = ruleManager.RootRulePoint.Path;
            if (pathToSelectItemMap.ContainsKey(rootPath))
            {
                var rootItem = pathToSelectItemMap[rootPath];
                foreach (var selectItem in rootItem.Children)
                {
                    SelectItems.Add(selectItem);
                }
            }
        }


        private HierarchicalSelectItem GetSelectItem(RuleBase rule)
        {
            if (pathToSelectItemMap.ContainsKey(rule.Path))
            {
                return pathToSelectItemMap[rule.Path];
            }
            else
            {
                HierarchicalSelectItem selectItem = new HierarchicalSelectItem(rule.Name, rule.DisplayName, null);
                pathToSelectItemMap[rule.Path] = selectItem;
                if (rule.Parent != null)
                {
                    HierarchicalSelectItem parentItem = GetSelectItem(rule.Parent);
                    parentItem.Children.Add(selectItem);
                }
                return selectItem;
            }
        }
    }
}
