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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

using RuleConfiguration;
using Autodesk.IM.UI.Rule;

namespace RuleConfiguration
{
    /// <summary>
    /// Context class of Design Validation Panel
    /// </summary>
    public class DesignValidationContext : ContextBase
    {
        /// <summary>
        /// Constructor only called to create the Empty instance
        /// </summary>
        public DesignValidationContext()
        {
            this.ValidationGroups = new List<ValidationGroupContext>();
            this.BindingList = new ObservableCollection<object>();

            ValidationManager.SubscribeValidationCollectionChangedEvent(validationItems_CollectionChanged);
            //this.DisplayConfig = new DisplayConfigContext();

            this.IsDesignValidationEnabled = false;
            this.Initialize();
        }


        private ValidationManager ValidationManager
        {
            get
            {
                return RuleAppExtension.ValidationManager;
            }
        }


        /// <summary>
        /// To group validation items
        /// </summary>
        private void Initialize()
        {
            this.clickedItem = null;
            this.oldSelectedFids = null;
            this.ValidationGroups.Clear();
            this.groupNameToContext = new Dictionary<string, ValidationGroupContext>();
            this.validationItem2ItemViewModel = new Dictionary<IValidationItem, ValidationItemContextBase>();
            this.validationItem2ItemGroupViewModel = new Dictionary<IValidationItem, ValidationGroupContext>();

            this.BindingList = null;

            ValidationManager validationManager = this.ValidationManager;
            for (int i = 0; i < validationManager.ValidationItemCount; i++)
            {
                IValidationItem item = validationManager[i];
                if (item == null)
                    continue;

                this.CreateValidationItemContext(item);
            }

            ObservableCollection<object> newBindingList = new ObservableCollection<object>();
            foreach (ValidationGroupContext group in this.ValidationGroups)
            {
                group.UpdateHeader();
                newBindingList.Add(group);

                foreach (var item in group.ValidationResultItems)
                {
                    if (item.IsVisible)
                        newBindingList.Add(item);
                }
            }

            this.BindingList = newBindingList;
        }


        //private void UpdateDisplayConfigSettings(int itemCount)
        //{
        //    this.DisplayConfig.IsSortingPossible = (this.ValidationGroups.Count > 1);
        //    this.DisplayConfig.IsGroupingPossible = (itemCount > 0);
        //}


        private ValidationGroupContext CreateValidationItemContext(IValidationItem item)
        {
            ValidationItemContextBase itemContext = new ValidationItemContext(this, item);
            if (this.validationItem2ItemViewModel.ContainsKey(item) == false)
                this.validationItem2ItemViewModel.Add(item, itemContext);

            //itemContext.UpdateVisibility(this.DisplayConfig.FilterContext);

            return this.AddToGroup(itemContext);
        }


        /// <summary>
        /// Add a validation item context to a proper group
        /// </summary>
        /// <param name="itemContext"></param>
        private ValidationGroupContext AddToGroup(ValidationItemContextBase itemContext)
        {
            IValidationItem item = itemContext.ValidationItem;

            string groupName = null;
            //if (this.DisplayConfig.IsByDeviceType)
                 groupName = item.DeviceType;
            //else
            //    groupName = item.Category;

            ValidationGroupContext group;
            if (!groupNameToContext.TryGetValue(groupName, out group))
            {
                group = new ValidationGroupContext(groupName);
                groupNameToContext.Add(groupName, group);

                group.IsExpandedChanged += new EventHandler(group_IsExpandedChanged);

                this.ValidationGroups.Add(group);
            }

            if (group.ValidationResultItems.Contains(itemContext) == false)
                group.ValidationResultItems.Add(itemContext);

            if (this.validationItem2ItemGroupViewModel.ContainsKey(item) == false)
                this.validationItem2ItemGroupViewModel.Add(item, group);

            return group;
        }


        private void RemoveGroup(ValidationGroupContext groupCxt)
        {
            this.ValidationGroups.Remove(groupCxt);
            this.groupNameToContext.Remove(groupCxt.SortingName);

            groupCxt.IsExpandedChanged -= new EventHandler(group_IsExpandedChanged);

            this.BindingList.Remove(groupCxt);
        }


        void DisplayConfig_FilterChanged()
        {
            this.Initialize();
            //this.IsSelectingOnChanged = true; //though it doesn't change, let's use this switch to update selection
        }


        /// <summary>
        /// User clicked a Sorting menuitem;
        /// here we do the sorting.
        /// </summary>
        /// <param name="isAsc"></param>
        void DisplayConfig_SortingRequested(bool isAsc)
        {
            IOrderedEnumerable<ValidationGroupContext> orderedGroups = null;
            if (isAsc)
                orderedGroups = this.ValidationGroups.OrderBy(p => p.SortingName);
            else
                orderedGroups = this.ValidationGroups.OrderByDescending(p => p.SortingName);

            this.BindingList.Clear();

            List<ValidationGroupContext> newGroups = new List<ValidationGroupContext>();
            foreach (ValidationGroupContext cxt in orderedGroups)
            {
                newGroups.Add(cxt);
                this.BindingList.Add(cxt);
                foreach (ValidationItemContextBase item in cxt.ValidationResultItems)
                {
                    if (cxt.IsExpanded && item.IsVisible)
                        this.BindingList.Add(item);
                }
            }

            this.ValidationGroups = newGroups;

            //We don't sort items inside each group.
            //At first, it's not required in spec;
            //Secondly, QA doesn't think it useful to sort inside each group
        }


        /// <summary>
        /// user changed the way to group validation items - by device type of by validation type
        /// </summary>
        void DisplayConfig_GroupingChanged()
        {
            this.Initialize();
            //this.IsSelectingOnChanged = true; //though it doesn't change, let's use this switch to update selection
        }


        /// <summary>
        /// Validation item set changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void validationItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Dictionary<string, ValidationGroupContext> affectedGroups = new Dictionary<string, ValidationGroupContext>();
            if (e.NewItems != null)
            {
                foreach (var obj in e.NewItems)
                {
                    IValidationItem item = obj as IValidationItem;
                    if (item == null)
                        continue;

                    ValidationGroupContext group = this.CreateValidationItemContext(item);
                    ValidationItemContextBase itemCxt = this.validationItem2ItemViewModel[item];
                    Debug.Assert(itemCxt != null);

                    int indexToInsert = -1;
                    if (!this.BindingList.Contains(group))
                    {
                        this.BindingList.Add(group);
                        if (group.IsExpanded)
                            indexToInsert = this.BindingList.IndexOf(group) + 1;
                    }
                    else
                    {
                        if (group.IsExpanded && itemCxt.IsVisible)
                        {
                            //note that when calling group.GetVisibleChildrenCount, itemCxt is already counted
                            indexToInsert = this.BindingList.IndexOf(group) + group.GetVisibleChildrenCount();
                        }
                    }

                    if ((indexToInsert >= 0) && itemCxt.IsVisible)
                    {
                        this.BindingList.Insert(indexToInsert, itemCxt);
                    }

                    if (affectedGroups.ContainsKey(group.SortingName) == false)
                    {
                        affectedGroups.Add(group.SortingName, group);
                    }
                }
            }

            bool toClearClickedItem = false;
            if (e.OldItems != null)
            {
                foreach (var obj in e.OldItems)
                {
                    IValidationItem item = obj as IValidationItem;
                    if (item == null)
                        continue;

                    //if ((this.clickedItem != null) &&
                    //    Object.ReferenceEquals(this.clickedItem.ValidationItem, item))
                    //{
                    //    toClearClickedItem = true;
                    //}

                    ValidationGroupContext group = this.validationItem2ItemGroupViewModel[item];
                    if (group == null)
                        continue;

                    ValidationItemContextBase itemCxt = this.validationItem2ItemViewModel[item];
                    if (itemCxt == null)
                        continue;

                    group.ValidationResultItems.Remove(itemCxt);
                    this.validationItem2ItemViewModel.Remove(item);
                    this.validationItem2ItemGroupViewModel.Remove(item);
                    if (this.BindingList.Contains(itemCxt))
                    {
                        this.BindingList.Remove(itemCxt);
                    }

                    if (affectedGroups.ContainsKey(group.SortingName) == false)
                    {
                        affectedGroups.Add(group.SortingName, group);
                    }
                }
            }

            foreach (var group in affectedGroups)
            {
                if (group.Value.ValidationResultItems.Count == 0)
                {
                    ValidationGroupContext groupCxt = group.Value;
                    this.RemoveGroup(groupCxt);
                }
                else
                {
                    group.Value.UpdateHeader();
                }
            }

            ValidationManager validationManager = this.ValidationManager;
            if (validationManager == null)
                return;

            //this.UpdateDisplayConfigSettings(validationManager.ValidationItemCount);

            //if (toClearClickedItem && (this.clickedItem != null))
            //    this.OnItemClicked(null);

            base.OnPropertyChanged("HasResolveItems");
        }


        /// <summary>
        /// User clicked to collapse or expand a group;
        /// To improve performance by XXX times we simply bind a list to Validation Result listbox,
        ///   so here we manually remove/add validation items from/to the group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void group_IsExpandedChanged(object sender, EventArgs e)
        {
            ValidationGroupContext group = sender as ValidationGroupContext;
            if (group == null)
                return;

            if (group.IsExpanded)
            {
                int index = this.BindingList.IndexOf(group) + 1;
                if (index < 1)
                {
                    Debug.Assert(false);
                    return;
                }

                foreach (var item in group.ValidationResultItems)
                {
                    if (item.IsVisible)
                        this.BindingList.Insert(index++, item);
                }
            }
            else
            {
                foreach (var item in group.ValidationResultItems)
                {
                    if (item.IsVisible)//otherwise it's not in binding list
                        this.BindingList.Remove(item);
                }
            }
        }


        /// <summary>
        /// select the facility that owns the specified item;
        /// if item == null, it means to unselect all facilities
        /// </summary>
        public void OnItemClicked(ValidationItemContextBase item)
        {
            //this.clickedItem = item;

            //if (this.ValidationGroups == null)
            //    return;

            //foreach (ValidationGroupContext group in this.ValidationGroups)
            //{
            //    if (group.ValidationResultItems == null)
            //    {
            //        Debug.Assert(false);
            //        continue;
            //    }

            //    foreach (ValidationItemContextBase cxt in group.ValidationResultItems)
            //    {
            //        if (cxt == null)
            //            continue;

            //        if (item == null)
            //        {
            //            cxt.SetStatus(ItemStatus.Normal);
            //        }
            //        else if ((item.ValidationItem != null) &&
            //            (cxt.ValidationItem != null) &&
            //            (cxt.ValidationItem.FeatureFID == item.ValidationItem.FeatureFID))
            //        {
            //            if (cxt != item)
            //                cxt.SetStatus(ItemStatus.DeviceSelected);
            //            else
            //                cxt.SetStatus(ItemStatus.PrimarySelected);
            //        }
            //        else
            //        {
            //            cxt.SetStatus(ItemStatus.Normal);
            //        }
            //    }
            //}

            ////This is a special fix, in case the clicked item has no geometry. (Bug number: 14682)
            ////oldSelectedFids is a cache, telling us which fids were already selected on drawing, so that
            ////  if selection on drawing is not changed when ACAD says selection is changed, we don't update
            ////  validation item status;
            ////We need this cache, because you can click on a same validation items many times, and each
            ////  time ACAD will throw a selection changed event, though selection doesn't change at all.
            ////Now, as we already reset validation item status above, the cache becomes expired, because
            ////  validation item status are not consistent with selected Fids anymore
            //if (this.oldSelectedFids != null)
            //    this.oldSelectedFids.Clear();

            //this.SelectFeature(item);
        }


        /// <summary>
        /// Select the feature which is responsible for the given validation item
        /// </summary>
        /// <param name="item"></param>
        //private void SelectFeature(ValidationItemContextBase item)
        //{
        //    if ((item != null) &&
        //        (item.ValidationItem != null))
        //    {
        //        long fid = item.ValidationItem.FeatureFID;
        //        SelectionHelper.SelectFeatures(new long[] { fid }, true);
        //    }
        //}


        /// <summary>
        /// un-highlight all validation items
        /// </summary>
        //public void OnFacilitySelectionCancelled()
        //{
        //    foreach (var group in this.ValidationGroups)
        //    {
        //        foreach (var item in group.ValidationResultItems)
        //        {
        //            item.OnDeviceSelectionCancelled();
        //        }
        //    }

        //    this.clickedItem = null;
        //    this.oldSelectedFids = null;
        //}


        //public void ClearSelection()
        //{
        //    this.UpdateSelectionState(null);
        //}


        /// <summary>
        /// highlight validation items belong to facilities in the list
        /// </summary>
        /// <param name="list"></param>
        //public void UpdateSelectionState(List<IFacility> list)
        //{
        //    List<long> fids = new List<long>();
        //    if (list != null)
        //    {
        //        foreach (var fac in list)
        //        {
        //            if (fids.Contains(fac.FID) == false)
        //                fids.Add(fac.FID);
        //        }
        //    }

        //    //fids.Sort(); don't do sort. the real thing we want to avoid,
        //    //  is that user doesn't do anything and this event is called again;
        //    //  currently I doesn't find any ACAD event which will be always called
        //    //  when selection set is changed; so instead, I'm using ACAD.Application_Idle
        //    //  event, and when it's triggered, I get current selection set and call this
        //    //  method.

        //    if (!this.IsSelectionChanged(fids))
        //        return;

        //    oldSelectedFids = fids;

        //    foreach (var group in this.ValidationGroups)
        //    {
        //        if (group == null)
        //            continue;

        //        foreach (var item in group.ValidationResultItems)
        //        {
        //            if (item == null)
        //                continue;

        //            //This is a special fix, in case the clicked item has no geometry. (Bug number: 14682)
        //            //please note: clickedItem may not be contained in oldSelectedFids,
        //            //  as when you click on an item with no geometry, its parent would be selected
        //            //  on drawing, and the selection change will trigger this method; here as
        //            //  clickedItem has no geometry, it's certainly not in oldSelectedFids;
        //            //  however in this case, user clicked an item intentionally, so we should
        //            //  continue marking that item as primary selected item
        //            if (item == this.clickedItem)
        //                item.OnDeviceSelected(true);
        //            else if (oldSelectedFids.Contains(item.ValidationItem.FeatureFID))
        //                item.OnDeviceSelected(false);
        //            else if ((this.clickedItem != null) &&
        //                    (this.clickedItem.ValidationItem != null) &&
        //                    (this.clickedItem.ValidationItem.FeatureFID == item.ValidationItem.FeatureFID))
        //                item.OnDeviceSelected(false);
        //            else
        //                item.OnDeviceSelectionCancelled();
        //        }
        //    }
        //}


        //private bool IsSelectionChanged(List<long> selectedFids)
        //{
        //    bool isChanged = false;

        //    if (oldSelectedFids == null)
        //        isChanged = true;
        //    else if (oldSelectedFids.Count != selectedFids.Count)
        //        isChanged = true;
        //    else
        //    {
        //        int num = oldSelectedFids.Count;
        //        for (int i = 0; i < num; i++)
        //        {
        //            if (selectedFids[i] != oldSelectedFids[i])
        //            {
        //                isChanged = true;
        //                break;
        //            }
        //        }
        //    }
        //    return isChanged;
        //}


        //internal void IgnoreRequested(ValidationItemContextBase item)
        //{
        //    if ((item == null) || (item.ValidationItem == null))
        //    {
        //        Debug.Assert(false);
        //        return;
        //    }

        //    switch (item.ValidationItem.ResultType)
        //    {
        //        case ValidationType.IgnoredWarning:
        //        case ValidationType.IgnoredMessage:
        //        case ValidationType.Warning:
        //        case ValidationType.Message:
        //            break;
        //        default:
        //            Debug.Assert(false);
        //            return;
        //    }

        //    bool isToIgnore = !item.IsIgnored;

        //    bool willClickedItemVisible = true;

        //    bool isStillVisible = this.IgnoreItem(item, isToIgnore);
        //    if ((item == this.clickedItem) && (!item.IsVisible))
        //        willClickedItemVisible = false;

        //    if (this.validationItem2ItemGroupViewModel.ContainsKey(item.ValidationItem))
        //    {
        //        var group = this.validationItem2ItemGroupViewModel[item.ValidationItem];
        //        group.UpdateHeader();//show 'Filter Applied' as needed
        //    }

        //    //as there might be filter, clickedItem may become filtered, so let's make nothing clicked
        //    if (!willClickedItemVisible)
        //        this.OnItemClicked(null);

        //    if (isToIgnore && this.Connection != null)
        //        FeatureHandlerManagers.Instance.Get(this.Connection).OnValidationItemsIgnored(new List<IValidationItem>() { item.ValidationItem });

        //    ValidationManager validationManager = this.ValidationManager;
        //    if (DebugUtil.IsNull(validationManager))
        //        return;

        //    if (isToIgnore)
        //        validationManager.AddIgnoredValidationItemId(item.ValidationItem.ID);
        //    else
        //        validationManager.RemoveIgnoredValidationItemId(item.ValidationItem.ID);
        //}


        //private bool IgnoreItem(ValidationItemContextBase item, bool isToIgnore)
        //{
        //    bool isStillVisible = true;
        //    bool isChanged = false;
        //    if (isToIgnore)
        //    {
        //        if (item.ValidationItem.ResultType == ValidationType.Message)
        //        {
        //            item.ValidationItem.ResultType = ValidationType.IgnoredMessage;
        //            isChanged = true;

        //            if (!this.DisplayConfig.FilterContext.IsShowingIgnoredMessage)
        //                isStillVisible = false;
        //        }
        //        else if (item.ValidationItem.ResultType == ValidationType.Warning)
        //        {
        //            item.ValidationItem.ResultType = ValidationType.IgnoredWarning;
        //            isChanged = true;

        //            if (!this.DisplayConfig.FilterContext.IsShowingIgnoredWarning)
        //                isStillVisible = false;
        //        }
        //    }
        //    else
        //    {
        //        if (item.ValidationItem.ResultType == ValidationType.IgnoredMessage)
        //        {
        //            item.ValidationItem.ResultType = ValidationType.Message;
        //            isChanged = true;

        //            if (!this.DisplayConfig.FilterContext.IsShowingMessage)
        //                isStillVisible = false;
        //        }
        //        else if (item.ValidationItem.ResultType == ValidationType.IgnoredWarning)
        //        {
        //            item.ValidationItem.ResultType = ValidationType.Warning;
        //            isChanged = true;

        //            if (!this.DisplayConfig.FilterContext.IsShowingWarning)
        //                isStillVisible = false;
        //        }
        //    }

        //    if (isChanged)
        //    {
        //        item.UpdateUiByValidationType();
        //        item.UpdateVisibility(this.DisplayConfig.FilterContext);

        //        if (!item.IsVisible)// user blocked it, so it's must visible
        //        {
        //            int index = this.BindingList.IndexOf(item);
        //            if (index >= 0)
        //                this.BindingList.RemoveAt(index);
        //        }
        //        else
        //        {
        //            //else we don't do anything. Currently blocking / unblocking can only done when the validation item
        //            //  is visible. It was visible, and it will still be visible, so nothing should we do
        //        }
        //    }

        //    return isStillVisible;
        //}


        /// <summary>
        /// used to LOGICALLY maintain validation items in two levels: group and item
        /// </summary>
        List<ValidationGroupContext> validationGroups = null;
        public List<ValidationGroupContext> ValidationGroups
        {
            get
            {
                return this.validationGroups;
            }
            set
            {
                if (this.validationGroups != value)
                {
                    this.validationGroups = value;
                    base.OnPropertyChanged("ValidationGroups");//NOXLATE
                }
            }
        }


        //bool isSelectingOn;
        //public bool IsSelectingOn
        //{
        //    get
        //    {
        //        return this.isSelectingOn;
        //    }
        //    set
        //    {
        //        if (this.isSelectingOn != value)
        //            this.IsSelectingOnChanged = true;

        //        this.isSelectingOn = value;

        //        base.OnPropertyChanged("IsSelectingOn"); //NOXLATE

        //        this.SelectButtonTitle = value ? Properties.Resources.SelectionOn : Properties.Resources.SelectionOff;
        //        base.OnPropertyChanged("SelectButtonTitle"); //NOXLATE

        //        if (!value)
        //        {
        //            this.OnItemClicked(null);

        //            this.oldSelectedFids = null;//so next time when turning on selection, we can update items status per ACAD selection
        //        }
        //    }
        //}


        //public bool IsSelectingOnChanged
        //{
        //    get;
        //    set;
        //}


        //private bool ItemCanBeAutoResolved(ResolutionValidationItem item)
        //{
        //    ResolutionItem resolutionItem = item.DefaultResolution;
        //    if (resolutionItem != null)
        //    {
        //        ValidationItemCommand command = resolutionItem.ResolutionCommand;
        //        if (command != null)
        //        {
        //            if (command.CanExecute(null))
        //                return true;
        //        }
        //    }

        //    return false;
        //}


        //public bool HasResolveItems
        //{
        //    get
        //    {
        //        foreach (ValidationGroupContext group in this.ValidationGroups)
        //        {
        //            foreach (ValidationItemContextBase cxt in group.ValidationResultItems)
        //            {
        //                ResolutionValidationItem item = cxt.ValidationItem as ResolutionValidationItem;
        //                if (item != null && this.ItemCanBeAutoResolved(item))
        //                    return true;
        //            }
        //        }
        //        return false;
        //    }
        //}


        /// <summary>
        /// If design validation panel is enabled; when active AUD document is null, it becomes disabled
        /// </summary>
        bool isDesignValidationEnabled;
        public bool IsDesignValidationEnabled
        {
            get
            {
                return this.isDesignValidationEnabled;
            }
            set
            {
                this.isDesignValidationEnabled = value;

                base.OnPropertyChanged("IsDesignValidationEnabled"); //NOXLATE
            }
        }


        /// <summary>
        /// The real collection bound to validation result listbox
        /// Though validation result is shown with two levels - group and validation item,
        ///   to improve performance by XXX times, we bind a single list to the listbox
        /// </summary>
        ObservableCollection<object> bindingList = null;
        public ObservableCollection<object> BindingList
        {
            get
            {
                return this.bindingList;
            }
            set
            {
                this.bindingList = value;
                base.OnPropertyChanged("BindingList"); //NOXLATE
            }
        }


        /// <summary>
        /// an Empty context used to be bound when Design Validation UI is disabled
        /// </summary>
        static readonly DesignValidationContext emptyInstance = new DesignValidationContext();
        public static DesignValidationContext Empty
        {
            get
            {
                Debug.Assert(emptyInstance != null);
                return emptyInstance;
            }
        }


        public string SelectButtonTitle
        {
            get;
            private set;
        }


        ///// <summary>
        ///// Context object of the UI which sets sorting, grouping and filtering options
        ///// </summary>
        //public DisplayConfigContext DisplayConfig
        //{
        //    get;
        //    private set;
        //}


        #region dictionaries for quick lookup
        Dictionary<string, ValidationGroupContext> groupNameToContext;
        Dictionary<IValidationItem, ValidationItemContextBase> validationItem2ItemViewModel;
        Dictionary<IValidationItem, ValidationGroupContext> validationItem2ItemGroupViewModel;
        #endregion


        ValidationItemContextBase clickedItem = null;

        /// <summary>
        /// a record of fids selected on drawing
        /// </summary>
        List<long> oldSelectedFids = null;


        // Resolve all the resolvable validation items.
        //public void ResolveAll()
        //{
        //    List<Tuple<long, string>> fids = new List<Tuple<long, string>>();

        //    // If item has a default resolution, add it to the list.
        //    foreach (ValidationGroupContext group in this.ValidationGroups)
        //    {
        //        foreach (ValidationItemContextBase cxt in group.ValidationResultItems)
        //        {
        //            ResolutionValidationItem item = cxt.ValidationItem as ResolutionValidationItem;
        //            if (item == null || item.DefaultResolution == null)
        //                continue;

        //            long fid = item.FeatureFID;
        //            string handlerName = item.HandlerName;
        //            fids.Add(new Tuple<long, string>(fid, handlerName));
        //        }
        //    }

        //    // Global auto-validate and auto-resolve should always be on.
        //    FeatureHandlerManager featureHandlerManager = FeatureHandlerManagers.Instance.Get(this.Connection);
        //    Debug.Assert(featureHandlerManager.AutoValidate);
        //    Debug.Assert(featureHandlerManager.AutoResolve);

        //    // Revalidate and auto-resolve where possible.
        //    featureHandlerManager.Evaluate(fids);
        //}
    }
}
