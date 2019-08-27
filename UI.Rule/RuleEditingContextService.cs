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


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Represents service for rule editing contexts. This service manages rule editing contexts.
    /// Originally, this service has one context, root context. But activity designer could override
    /// the context.
    ///
    /// E.g. the designer of activity "number of connected [feature class] where [condition]" should override
    /// the context to make sure the feature attributes listed in [condition] are attributes of [feature class],
    /// other than attributes of the feature class corresponding to current rule point.
    ///
    /// Another example is "report and fix" activity. As validation rules are not allowed to directly modify the
    /// data model, so activities like "Add Sub-feature" and "Update Attributes" are not available in validation rules.
    /// But validation rules can contain activity "report and fix" which report an error/warning and provide
    /// corresponding resolutions. In the resolution, activities like "Add Sub-feature" and "Update Attributes"
    /// are available. It also needs overriding rule editing context.
    /// </summary>
    public class RuleEditingContextService
    {
        private RuleEditingContext _rootEditingContext;
        private Dictionary<ModelItem, RuleEditingContext> _editingContexts = new Dictionary<ModelItem, RuleEditingContext>();


        public RuleEditingContextService(RuleEditingContext rootContext)
        {
            if (rootContext == null)
            {
                throw new ArgumentNullException("rootContext"); // NOXLATE
            }

            _rootEditingContext = rootContext;
        }


        /// <summary>
        /// Get root rule editing context.
        /// </summary>
        public RuleEditingContext GetRootEditingContext()
        {
            return _rootEditingContext;
        }


        /// <summary>
        /// Get overrided rule editing context corresponding to the given model item or
        /// root editing context if no overriding.
        /// </summary>
        /// <returns>Return its or its ancestors' overriden rule editing context,
        /// if no overriden rule editing context, return root rule editing context.</returns>
        public RuleEditingContext GetEditingContext(ModelItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item"); // NOXLATE
            }

            ModelItem itemIterator = item;
            while (itemIterator != null)
            {
                if (_editingContexts.ContainsKey(itemIterator))
                {
                    return _editingContexts[itemIterator];
                }
                itemIterator = itemIterator.Parent;
            }
            return _rootEditingContext;
        }


        /// <summary>
        /// Override rule editing context on given model item and its descendants.
        /// </summary>
        public void OverrideEditingContext(ModelItem item, RuleEditingContext context)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item"); // NOXLATE
            }

            if (context == null)
            {
                throw new ArgumentNullException("context"); // NOXLATE
            }

            _editingContexts[item] = context;
        }
    }
}
