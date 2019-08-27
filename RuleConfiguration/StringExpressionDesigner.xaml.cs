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
using System.Activities.Presentation.Model;
using System.Collections.Specialized;
using System.Windows;

using Autodesk.IM.Rule.Activities;

namespace RuleConfiguration
{
    /// <summary>
    /// Interaction logic for StringExpressionDesigner.xaml
    /// </summary>
    public partial class StringExpressionDesigner
    {
        private const string Space = " "; // NOXLATE
        IWeakEventListener _collectionChangedListener = null;


        public StringExpressionDesigner()
        {
            InitializeComponent();

            this._collectionChangedListener = new WeakEventListener<NotifyCollectionChangedEventArgs, CollectionChangedEventManager>(ElementCollectionChanged);
        }


        protected override void OnModelItemChanged(object newItem)
        {
            base.OnModelItemChanged(newItem);

            ModelItemCollection elements = ModelItem.Properties["Elements"].Collection; //NOXLATE
            CollectionChangedEventManager.AddListener(elements, _collectionChangedListener);
        }


        private void ElementCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Only handle the event when single item is appended and it is not the first.
            if (e.NewItems != null && e.OldItems == null && e.NewItems.Count == 1 && e.NewStartingIndex > 0)
            {
                ModelItemCollection elements = ModelItem.Properties["Elements"].Collection; //NOXLATE

                // The insert position is always at the end when users add new item by clicking on hyperlike "<add text...>"
                // If the insert position is not the end, it must be 'Replace' operation. It is because ModelItemCollection
                // does not support 'Replace' operation, the operation is achieved by combination of 'remove' and 'add'.
                // Therefore you will receive an 'add' event when users are replacing an item. That is not the case we need to handle.
                if (e.NewStartingIndex == elements.Count - 1)
                {
                    bool shouldAddSpacer = true; // Add space by default.
                    int previousItemIndex = e.NewStartingIndex - 1;
                    object previousValue = elements[previousItemIndex].GetCurrentValue();
                    string previousString = GetLiteralString(previousValue);
                    if (previousString != null)
                    {
                        // If the previous item does not end with space, we should add a space.
                        shouldAddSpacer = shouldAddSpacer && !previousString.EndsWith(Space); //NOXLATE
                    }

                    ModelItem modelItem = e.NewItems[0] as ModelItem;
                    if (modelItem != null)
                    {
                        object theValue = modelItem.GetCurrentValue();
                        string theString = GetLiteralString(theValue);
                        if (theString != null)
                        {
                            // If the newly-added string does start with space, we should add a space.
                            shouldAddSpacer = shouldAddSpacer && !theString.StartsWith(Space);
                        }
                    }

                    if (shouldAddSpacer) //NOXLATE
                    {
                        elements.Insert(e.NewStartingIndex, StringLiteral.CreateArgument(Space)); //NOXLATE
                    }
                }
            }
        }


        private string GetLiteralString(object value)
        {
            InArgument<string> argument = value as InArgument<string>;
            if (argument == null)
            {
                return null;
            }

            StringLiteral literal = argument.Expression as StringLiteral;
            if (literal == null)
            {
                return null;
            }

            return literal.Value;
        }
    }
}
