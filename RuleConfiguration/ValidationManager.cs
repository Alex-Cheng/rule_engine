/*
 * Copyright (C) 2011-2012 by Autodesk, Inc. All Rights Reserved.
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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;


namespace RuleConfiguration
{
    /// <summary>
    /// Holds validation items which will be shown on Design Validation UI
    /// </summary>
    public class ValidationManager
    {
        /// <summary>
        /// Don't expose it; we need special steps to add/remove item into/from this collection
        /// </summary>
        ObservableCollection<IValidationItem> validationItems = new ObservableCollection<IValidationItem>();

        Dictionary<long, IValidationItem> idToValidationItem = new Dictionary<long, IValidationItem>();

        private Dictionary<object, ObservableCollection<IValidationItem>> attachedCollections =
            new Dictionary<object, ObservableCollection<IValidationItem>>();


        internal ValidationManager()
        {
            this.IgnoredValidationItemIds = new List<string>();
        }


        public IValidationItem this[int index]
        {
            get
            {
                if (index < 0 || index >= this.validationItems.Count)
                    return null;

                return this.validationItems[index];
            }
        }


        public int ValidationItemCount
        {
            get
            {
                return this.validationItems.Count;
            }
        }


        public void AddValidationItems(IEnumerable<IValidationItem> items)
        {
            foreach (IValidationItem item in items)
                this.AddValidationItem(item);
        }


        public void AddValidationItem(IValidationItem item)
        {
            if (item == null)
                return;

            if (this.idToValidationItem.ContainsKey(item.FID))
                return;

            //if (this.IgnoredValidationItemIds.Contains(item.ID))
            //{
            //    // The item was ignored.  When you get here it means the rule
            //    // engine deleted existing validation items and is running again
            //    // to regenerate them.
            //    if ((item.ResultType == ValidationType.Message))
            //    {
            //        item.ResultType = ValidationType.IgnoredMessage;
            //    }
            //    else if (item.ResultType == ValidationType.Warning)
            //    {
            //        item.ResultType = ValidationType.IgnoredWarning;
            //    }
            //}

            this.validationItems.Add(item);
            this.idToValidationItem.Add(item.FID, item);
        }


        public void RemoveValidationItems(IEnumerable<IValidationItem> items)
        {
            foreach (IValidationItem item in items)
                this.RemoveValidationItem(item);
        }


        public void RemoveValidationItem(object owner, long fid)
        {
            //ObservableCollection<IValidationItem> oldColl = null;
            //List<IValidationItem> toBeDeleted = new List<IValidationItem>();
            //if (this.attachedCollections.TryGetValue(owner, out oldColl))
            //{
            //    foreach (IValidationItem item in oldColl)
            //    {
            //        if (item.FeatureFID == fid)
            //        {
            //            toBeDeleted.Add(item);
            //            this.RemoveValidationItem(item, true);
            //        }
            //    }
            //    foreach (IValidationItem item in toBeDeleted)
            //    {
            //        oldColl.Remove(item);
            //    }
            //    if (oldColl != null && oldColl.Count == 0)
            //    {
            //        oldColl.CollectionChanged -= this.OnAttachedCollectionChanged;
            //        oldColl = null;
            //        this.attachedCollections.Remove(owner);
            //    }
            //}
        }


        /// <summary>
        /// isDeletingFeature indicates if it's called by deleting a feature
        /// If isDeletingFeature = true, we remove related records from ignored validation item id list;
        /// </summary>
        public void RemoveValidationItem(IValidationItem item)
        {
            if (item == null)
                return;

            if (!this.idToValidationItem.ContainsKey(item.FID))
                return;

            this.validationItems.Remove(item);
            this.idToValidationItem.Remove(item.FID);
        }


        /// <summary>
        /// Remembers IDs of ignored validation items.  When a rule is re-run
        /// against a feature the rule engine will delete existing validation
        /// items and create new ones.  This list lets us remember which items
        /// were originally ignored.
        /// </summary>
        private List<string> IgnoredValidationItemIds
        {
            get;
            set;
        }


        ///// <summary>
        ///// Add id of a validation item to ignored validation item id list.
        ///// </summary>
        //public void AddIgnoredValidationItemId(string id)
        //{
        //    if (String.IsNullOrEmpty(id))
        //    {
        //        Debug.Assert(false);
        //        return;
        //    }

        //    if (this.IgnoredValidationItemIds.Contains(id) == false)
        //        this.IgnoredValidationItemIds.Add(id);
        //}


        //public void RemoveIgnoredValidationItemId(string id)
        //{
        //    if (this.IgnoredValidationItemIds.Contains(id))
        //        this.IgnoredValidationItemIds.Remove(id);
        //}


        //public bool IsIgnored(IValidationItem item)
        //{
        //    return this.IgnoredValidationItemIds.Contains(item.ID);
        //}


        /// <summary>
        /// Needed for CIP
        /// To get error num, warning num and message num
        /// </summary>
        /// <param name="errNum">number of errors</param>
        /// <param name="warningNum">number of warnings</param>
        /// <param name="msgNum">number of messages</param>
        public void GetStatisticInformation(out int errNum, out int warningNum, out int msgNum)
        {
            errNum = 0;
            warningNum = 0;
            msgNum = 0;

            if (this.validationItems == null)
                return;

            foreach (var item in this.validationItems)
            {
                switch (item.ResultType)
                {
                    case ValidationType.Error:
                        errNum++;
                        break;

                    case ValidationType.IgnoredMessage:
                    case ValidationType.Message:
                        msgNum++;
                        break;

                    case ValidationType.IgnoredWarning:
                    case ValidationType.Warning:
                        warningNum++;
                        break;
                }
            }
        }


        //public void Attach(object owner, ObservableCollection<IValidationItem> validationItemCollection)
        //{
        //    ObservableCollection<IValidationItem> oldColl = null;
        //    if (this.attachedCollections.TryGetValue(owner, out oldColl))
        //    {
        //        if (Object.ReferenceEquals(validationItemCollection, oldColl))
        //            return;
        //        this.Detach(owner, false);
        //    }
        //    this.attachedCollections[owner] = validationItemCollection;
        //    validationItemCollection.CollectionChanged += this.OnAttachedCollectionChanged;
        //    foreach (IValidationItem item in validationItemCollection)
        //        this.AddValidationItem(item);
        //}


        //public void Detach(object owner, bool deleteIgnored)
        //{
        //    ObservableCollection<IValidationItem> oldColl = null;
        //    if (this.attachedCollections.TryGetValue(owner, out oldColl))
        //    {
        //        foreach (IValidationItem item in oldColl)
        //            this.RemoveValidationItem(item, deleteIgnored);
        //        oldColl.CollectionChanged -= this.OnAttachedCollectionChanged;
        //        this.attachedCollections.Remove(owner);
        //    }
        //}


        void OnAttachedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (IValidationItem item in e.NewItems)
                        this.AddValidationItem(item);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (IValidationItem item in e.OldItems)
                        this.RemoveValidationItem(item);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (IValidationItem item in e.NewItems)
                        this.AddValidationItem(item);
                    foreach (IValidationItem item in e.OldItems)
                        this.RemoveValidationItem(item);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    throw new NotSupportedException("Clear() not allowed on attached validation collections. Remove all items or Detach from the validation manager."); //NOXLATE
                case NotifyCollectionChangedAction.Move:
                    break;
            }
        }


        public void SubscribeValidationCollectionChangedEvent(NotifyCollectionChangedEventHandler handler)
        {
            if ((this.validationItems == null) ||
                (handler == null))
                return;

            this.validationItems.CollectionChanged += handler;
        }


        public void UnSubscribeValidationCollectionChangedEvent(NotifyCollectionChangedEventHandler handler)
        {
            if ((this.validationItems == null) ||
                (handler == null))
                return;

            this.validationItems.CollectionChanged -= handler;
        }
    }
}
