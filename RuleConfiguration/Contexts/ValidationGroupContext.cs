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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;

using Autodesk.Gis.UI.VisualLibrary.CustomControls;

using RuleConfiguration;
using Autodesk.IM.UI.Rule;


namespace RuleConfiguration
{
    /// <summary>
    /// Context of expander holding a group of validation item on Design Validation UI
    /// </summary>
    public class ValidationGroupContext : ContextBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="groupName">name of this group</param>
        public ValidationGroupContext(string groupName)
        {
            this.ValidationResultItems = new List<ValidationItemContextBase>();
            this._groupName = groupName;
        }


        /// <summary>
        /// Work out an object shown as header of the expander this context reprents
        /// </summary>
        public void UpdateHeader()
        {
            int errNum = 0;
            int warningNum = 0;
            foreach (ValidationItemContextBase item in this.ValidationResultItems)
            {
                if (item.ValidationItem.ResultType == ValidationType.Error)
                    errNum++;
                else if (item.ValidationItem.ResultType == ValidationType.Warning)
                    warningNum++;
            }

            var devices = from device in this.ValidationResultItems
                          group device by device.ValidationItem.FeatureItem into g
                          select g;

            {
                Bold bold = new Bold();
                bold.Inlines.Add(this._groupName);
                TextBlock headerBlockExp = new TextBlock(bold);

                var hiddenItems = from item in this.ValidationResultItems
                                  where item.IsVisible == false
                                  select item;
                if (hiddenItems.Any())
                    headerBlockExp.Inlines.Add(Properties.Resources.FilterApplied);

                headerBlockExp.TextTrimming = System.Windows.TextTrimming.CharacterEllipsis;

                this.HeaderExpanded = headerBlockExp;
            }

            {
                Bold bold = new Bold();
                bold.Inlines.Add(this._groupName);
                TextBlock headerBlockColl = new TextBlock(bold);
                string headerExt = String.Format(Properties.Resources.ValidationGroupHeader, devices.Count(), errNum, warningNum);
                headerBlockColl.Inlines.Add(headerExt);

                headerBlockColl.TextTrimming = System.Windows.TextTrimming.CharacterEllipsis;

                this.HeaderCollapsed = headerBlockColl;
            }
        }


        /// <summary>
        /// WPF virtualization is disposing invisible framework element;
        /// To a validation group visual item, its context contains framework element
        ///   as its header, and we should disconnect header from its parent;
        ///   otherwise the next time it can't be correctly connect to a new validation
        ///   group visual item
        /// </summary>
        public void DisconnectByVirtualizationCleanup()
        {
            System.Windows.FrameworkElement headerCol = HeaderCollapsed as System.Windows.FrameworkElement;
            Disconnect(headerCol);

            System.Windows.FrameworkElement headerExp = HeaderExpanded as System.Windows.FrameworkElement;
            Disconnect(headerExp);
        }


        private void Disconnect(System.Windows.FrameworkElement headerObject)
        {
            if (headerObject != null)
            {
                if (headerObject.TemplatedParent != null)
                {
                    ContentPresenter p = headerObject.TemplatedParent as ContentPresenter;
                    if (p != null)
                    {
                        p.Content = null;
                    }
                }
                else if (headerObject.Parent != null)
                {
                    AdvancedExpander exp = headerObject.Parent as AdvancedExpander;
                    if (exp != null)
                    {
                        exp.Header = null;
                    }
                }
            }
        }


        /// <summary>
        /// The name in string format, used to sort groups;
        /// </summary>
        string _groupName = null;
        public string SortingName
        {
            get
            {
                return this._groupName;
            }
        }


        /// <summary>
        /// The object used as header of the expander this context represents;
        /// used when expander is expanded
        /// </summary>
        object _headerObjExp = null;
        public object HeaderExpanded
        {
            get
            {
                return this._headerObjExp;
            }
            set
            {
                this._headerObjExp = value;
                base.OnPropertyChanged("HeaderExpanded"); //NOXLATE
            }
        }


        /// <summary>
        /// The object used as header of the expander this context represents;
        /// used when expander is collapsed
        /// </summary>
        object _headerObjColl = null;
        public object HeaderCollapsed
        {
            get
            {
                return this._headerObjColl;
            }
            set
            {
                this._headerObjColl = value;
                base.OnPropertyChanged("HeaderCollapsed"); //NOXLATE
            }
        }


        /// <summary>
        /// Is the expander expanded;
        /// </summary>
        bool _isExpanded = true;
        public bool IsExpanded
        {
            get
            {
                return this._isExpanded;
            }
            set
            {
                if (this._isExpanded != value)
                {
                    this._isExpanded = value;
                    base.OnPropertyChanged("IsExpanded"); //NOXLATE

                    if (this.IsExpandedChanged != null)
                        this.IsExpandedChanged.Invoke(this, null);
                }
            }
        }


        public event EventHandler IsExpandedChanged;


        /// <summary>
        /// Validation items holded in this group
        /// </summary>
        List<ValidationItemContextBase> validationResItems = null;
        public List<ValidationItemContextBase> ValidationResultItems
        {
            get
            {
                return this.validationResItems;
            }
            set
            {
                if (this.validationResItems != value)
                {
                    this.validationResItems = value;
                    base.OnPropertyChanged("ValidationResultItems"); //NOXLATE
                }
            }
        }

        internal int GetVisibleChildrenCount()
        {
            int ret = 0;

            if (this.validationResItems == null)
                ret = 0;
            else
            {
                foreach(var item in this.validationResItems)
                {
                    if (item.IsVisible)
                        ret++;
                }
            }

            return ret;
        }
    }
}
