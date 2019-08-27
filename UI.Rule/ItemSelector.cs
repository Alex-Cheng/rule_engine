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
using System.Activities.Presentation.Model;
using System.Activities.Presentation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using Autodesk.IM.Rule;
using Autodesk.IM.Rule.Activities;
using System.Windows.Controls.Primitives;


namespace Autodesk.IM.UI.Rule
{
    /// <summary>
    /// Represents an UI component for selection an item.
    /// </summary>
    public class ItemSelector : UserControl
    {
        private ObservableCollection<SelectContext> _contexts;

        private ISelectorLink _selectorLink;

        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.ItemSelector with default selector UI.
        /// </summary>
        public ItemSelector()
        {
            ISelectorLink defaultSelectorUI = new DefaultSelectorLink(this);
            Initialize(defaultSelectorUI);
        }


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.UI.Rule.ItemSelector with given
        /// selector UI.
        /// </summary>
        /// <param name="selectorLink">The object implementing ISelectorLink interface.</param>
        public ItemSelector(ISelectorLink selectorLink)
        {
            if (selectorLink == null)
            {
                throw new ArgumentNullException("selectorLink"); //NOXLATE
            }

            Initialize(selectorLink);
        }


        /// <summary>
        /// Initializes the instance of ItemSelector with given selector UI.
        /// </summary>
        /// <param name="selectorLink">The object implementing ISelectorLink interface.</param>
        protected virtual void Initialize(ISelectorLink selectorLink)
        {
            _selectorLink = selectorLink;

            // Hook event handlers.
            _selectorLink.OnRemove += OnRemove;
            _selectorLink.OnReset += OnReset;
            _selectorLink.OnPopupOpened += OnPopupOpened;

            ItemSelected += new ItemSelectedEventHandler(ItemSelectedHandler);

            this.Content = _selectorLink.UIContent;            
        }


        public ISelectorLink SelectorLink
        {
            get
            {
                return _selectorLink;
            }
        }


        /// <summary>
        /// Gets a collection of objects of SelectContext class.
        /// </summary>
        public ObservableCollection<SelectContext> Contexts
        {
            get
            {
                if (_contexts == null)
                {
                    _contexts = new ObservableCollection<SelectContext>();
                }
                return _contexts;
            }
        }


        /// <summary>
        /// Gets default select context.
        /// </summary>
        public SelectContext DefaultSelectContext
        {
            get
            {
                return Contexts.Count > 0 ? Contexts[0] : null;
            }
        }


        #region OwnerItem dependency property

        public static DependencyProperty OwnerItemProperty = DependencyProperty.RegisterAttached(
            "OwnerItem", typeof(ModelItem), typeof(ItemSelector),   // NOXLATE
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnOwnerItemChanged)));


        public ModelItem OwnerItem
        {
            get
            {
                return GetValue(OwnerItemProperty) as ModelItem;
            }
            set
            {
                SetValue(OwnerItemProperty, value);
            }
        }


        public static void SetOwnerItem(DependencyObject element, ModelItem value)
        {
            element.SetValue(OwnerItemProperty, value);
        }


        public static ModelItem GetOwnerItem(DependencyObject element)
        {
            return (ModelItem)element.GetValue(OwnerItemProperty);
        }


        private static void OnOwnerItemChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ItemSelector thisSelector = (o as ItemSelector);
            if (null == thisSelector)
                return;
            thisSelector.UpdateTextDisplay();
        }

        #endregion


        #region Item dependency property

        public static DependencyProperty ItemProperty = DependencyProperty.RegisterAttached(
            "Item", typeof(ModelItem), typeof(ItemSelector),    // NOXLATE
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnItemChanged)));


        public ModelItem Item
        {
            get
            {
                return GetValue(ItemProperty) as ModelItem;
            }
            set
            {
                SetValue(ItemProperty, value);
            }
        }


        private static void OnItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ItemSelector thisSelector = (sender as ItemSelector);
            if (null == thisSelector)
                return;
            thisSelector.UpdateTextDisplay();
        }

        #endregion


        #region ItemName dependency property

        public static DependencyProperty ItemNameProperty = DependencyProperty.RegisterAttached(
            "ItemName", typeof(string), typeof(ItemSelector),   // NOXLATE
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnItemNameChanged)));


        public string ItemName
        {
            get
            {
                return GetValue(ItemNameProperty) as string;
            }
            set
            {
                SetValue(ItemNameProperty, value);
            }
        }


        public static void SetItemName(DependencyObject element, string value)
        {
            element.SetValue(ItemNameProperty, value);
        }


        public static string GetItemName(DependencyObject element)
        {
            return (string)element.GetValue(ItemNameProperty);
        }


        private static void OnItemNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ItemSelector thisSelector = (sender as ItemSelector);
            if (null == thisSelector)
                return;
            thisSelector.UpdateTextDisplay();
        }

        #endregion


        #region Resetable dependency property

        public static DependencyProperty ResetableProperty = DependencyProperty.RegisterAttached(
            "Resetable", typeof(bool), typeof(ItemSelector),    // NOXLATE
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));


        public bool Resetable
        {
            get
            {
                return (bool)GetValue(ResetableProperty);
            }
            set
            {
                SetValue(ResetableProperty, value);
            }
        }


        public static void SetResetable(DependencyObject element, string value)
        {
            element.SetValue(ResetableProperty, value);
        }


        public static bool GetResetable(DependencyObject element)
        {
            return (bool)element.GetValue(ResetableProperty);
        }

        #endregion


        #region Removable dependency property

        public static DependencyProperty RemovableProperty = DependencyProperty.RegisterAttached(
            "Removable", typeof(bool), typeof(ItemSelector),    // NOXLATE
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));


        public bool Removable
        {
            get
            {
                return (bool)GetValue(RemovableProperty);
            }
            set
            {
                SetValue(RemovableProperty, value);
            }
        }


        public static void SetRemovable(DependencyObject element, string value)
        {
            element.SetValue(RemovableProperty, value);
        }


        public static bool GetRemovable(DependencyObject element)
        {
            return (bool)element.GetValue(RemovableProperty);
        }

        #endregion


        #region EmptyText dependency property

        public static DependencyProperty EmptyTextProperty = DependencyProperty.Register(
            "EmptyText", typeof(string), typeof(ItemSelector),  // NOXLATE
            new FrameworkPropertyMetadata(Properties.Resources.Empty, new PropertyChangedCallback(OnEmptyTextChanged)));


        public string EmptyText
        {
            get
            {
                return GetValue(EmptyTextProperty) as string;
            }
            set
            {
                SetValue(EmptyTextProperty, value);
            }
        }


        private static void OnEmptyTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ItemSelector thisSelector = (sender as ItemSelector);
            if (null == thisSelector)
                return;
            thisSelector.UpdateTextDisplay();
        }

        #endregion


        #region Text dependency property

        public static DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(ItemSelector),   // NOXLATE
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnTextChanged)));


        public string Text
        {
            get
            {
                return GetValue(TextProperty) as string;
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }


        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ItemSelector thisSelector = (sender as ItemSelector);
            if (null == thisSelector)
                return;
            thisSelector.UpdateTextDisplay();
        }

        #endregion


        #region TextDisplay dependency property

        public static DependencyProperty TextDisplayProperty = DependencyProperty.Register(
            "TextDisplay", typeof(string), typeof(ItemSelector),    // NOXLATE
            new FrameworkPropertyMetadata(Properties.Resources.Empty));


        public string TextDisplay
        {
            get
            {
                return GetValue(TextDisplayProperty) as string;
            }
            set
            {
                SetValue(TextDisplayProperty, value);
            }
        }

        #endregion


        #region ValueChanging routed event.

        // Provide CLR accessors for the event
        public event RoutedEventHandler ValueChanging
        {
            add
            {
                AddHandler(ValueChangingEvent, value);
            }
            remove
            {
                RemoveHandler(ValueChangingEvent, value);
            }
        }


        public static readonly RoutedEvent ValueChangingEvent = EventManager.RegisterRoutedEvent(
            "ValueChanging", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ItemSelector)); //NOXLATE


        public static void AddValueChangingHandler(DependencyObject d, RoutedEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
                uie.AddHandler(ValueChangingEvent, handler);
        }


        public static void RemoveValueChangingHandler(DependencyObject d, RoutedEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
                uie.RemoveHandler(ValueChangingEvent, handler);
        }

        #endregion


        #region ValueChanged routed event.

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ItemSelector)); //NOXLATE


        // Provide CLR accessors for the event
        public event RoutedEventHandler ValueChanged
        {
            add
            {
                AddHandler(ValueChangedEvent, value);
            }
            remove
            {
                RemoveHandler(ValueChangedEvent, value);
            }
        }

        #endregion


        #region PopupOpening routed event.

        public static readonly RoutedEvent PopupOpeningEvent = EventManager.RegisterRoutedEvent(
            "PopupOpening", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ItemSelector)); //NOXLATE


        // Provide CLR accessors for the event
        public event RoutedEventHandler PopupOpening
        {
            add
            {
                AddHandler(PopupOpeningEvent, value);
            }
            remove
            {
                RemoveHandler(PopupOpeningEvent, value);
            }
        }


        public static void AddPopupOpeningHandler(DependencyObject d, RoutedEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
                uie.AddHandler(PopupOpeningEvent, handler);
        }


        public static void RemovePopupOpeningHandler(DependencyObject d, RoutedEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
                uie.RemoveHandler(PopupOpeningEvent, handler);
        }

        #endregion


        #region ItemSelected routed event

        public delegate void ItemSelectedEventHandler(object sender, ItemSelectedEventArgs e);


        public static readonly RoutedEvent ItemSelectedEvent = EventManager.RegisterRoutedEvent("ItemSelected", // NOXLATE
            RoutingStrategy.Bubble, typeof(ItemSelectedEventHandler), typeof(ItemSelector));


        public event ItemSelectedEventHandler ItemSelected
        {
            add
            {
                AddHandler(ItemSelector.ItemSelectedEvent, value);
            }
            remove
            {
                RemoveHandler(ItemSelector.ItemSelectedEvent, value);
            }
        }

        #endregion


        /// <summary>
        /// Gets a ModelProperty object representing the corresponding property.
        /// </summary>
        /// <returns></returns>
        public virtual ModelProperty GetModelProperty()
        {
            ModelItem ownerItem = this.OwnerItem;
            if (null == ownerItem)
                return null;
            string propertyName = this.ItemName;
            if (null == propertyName)
                return null;
            return ownerItem.Properties[propertyName];
        }


        /// <summary>
        /// Gets rule editing context.
        /// </summary>
        public RuleEditingContext RuleEditingContext
        {
            get
            {
                ModelItem item = Item;
                if (null == item)
                {
                    item = OwnerItem;
                }
                if (null == item)
                {
                    return null;
                }

                return item.GetRuleEditingContext();
            }
        }


        /// <summary>
        /// Get editing context provided by WF4 presentation.
        /// </summary>
        public EditingContext EditingContext
        {
            get
            {
                ModelItem item = Item;
                if (null == item)
                {
                    item = OwnerItem;
                }
                if (null == item)
                {
                    return null;
                }

                return item.GetEditingContext();
            }
        }


        /// <summary>
        /// Gets display text of the ItemSelector object.
        /// </summary>
        /// <returns>The display text.</returns>
        protected virtual string GetDisplayText()
        {
            ActivityTranslator translator = EditingContext.Services.GetService<ActivityTranslator>();
            return translator.Translate(this.Item);
        }


        /// <summary>
        /// Updates select contexts according current status.
        /// </summary>
        /// <param name="validValueItemHint">Hint of valid values.</param>
        protected virtual void UpdateContexts(IEnumerable<DynamicValue> validValueItemHint)
        {
        }


        /// <summary>
        /// Updates select items in all select contexts.
        /// </summary>
        protected virtual void UpdateSelectItems()
        {
            foreach (SelectContext context in Contexts)
            {
                context.UpdateSelectItems();
            }
        }


        /// <summary>
        /// Updates display text.
        /// </summary>
        protected void UpdateTextDisplay()
        {
            bool isEmpty = false;
            if (this.Item != null)
            {
                if (String.IsNullOrEmpty(this.Text))
                {
                    this.TextDisplay = GetDisplayText();
                }
                else
                {
                    this.TextDisplay = this.Text;
                }
            }
            else
            {
                this.TextDisplay = String.Empty;
            }

            if (String.IsNullOrEmpty(this.TextDisplay) && !String.IsNullOrEmpty(this.EmptyText))
            {
                this.TextDisplay = this.EmptyText;
                isEmpty = true;
            }

            _selectorLink.LinkText.FontStyle = isEmpty ? FontStyles.Italic : FontStyles.Normal;
        }


        /// <summary>
        /// Called when the value is changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="modelProperty">A ModelProperty object representing the corresponding property.</param>
        protected virtual void OnChangeValue(object newValue, ModelProperty modelProperty)
        {
            if (this.GetModelProperty() != null)
            {
                // TODO: comment out below code temporarily and it will be discovered when porting input validation stuff.
                //foreach (ValidatorAttribute attribute in this.GetModelProperty().Attributes.OfType<ValidatorAttribute>())
                //{
                //    string message = String.Empty;
                //    if (!attribute.GetValidator().Validate(newValue, this.Item, out message))
                //    {
                //        MessageBox.Show(message);
                //        return;
                //    }
                //}
            }

            if (modelProperty.IsCollection)
            {
                ReplaceInCollection(modelProperty.Collection, Item, newValue);
            }
            else
            {
                Type propType = modelProperty.PropertyType;
                if (typeof(InArgument).IsAssignableFrom(propType))
                {
                    modelProperty.SetValue(newValue);
                }
                else if (newValue is InArgument)
                {
                    InArgument inArg = newValue as InArgument;
                    if (typeof(Activity).IsAssignableFrom(propType))
                    {
                        modelProperty.SetValue(inArg.Expression);
                    }
                }
                else
                {
                    modelProperty.SetValue(newValue);
                }
            }
        }


        /// <summary>
        /// Resets the value of item selector.
        /// </summary>
        protected virtual void Reset()
        {
            if (IsCurrentActivityResetable())
            {
                ResetCurrentActivity();
            }
            else
            {
                object defaultValue = null;
                if (TryGetDefaultValue(out defaultValue))
                {
                    RaiseEvent(new ItemSelectedEventArgs(ItemSelector.ItemSelectedEvent, this, defaultValue));
                }
                else
                {
                    Debug.Assert(false, "Cannot get default value. Check if code correct."); //NOXLATE
                }
            }
        }


        /// <summary>
        /// Creates a new value.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The new instance.</returns>
        protected object CreateNewValue<T>()
        {
            if (typeof(T) == typeof(DynamicValue))
            {
                return DynamicLiteral<int>.CreateArgument(0);
            }
            else
            {
                T tvalue = default(T);
                return new InArgument<T>(tvalue);
            }
        }


        /// <summary>
        /// Suppresses opening context menu by default, as context menu for item selectors are meaningless.
        /// </summary>
        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            e.Handled = true;
        }


        private void ItemSelectedHandler(object sender, ItemSelectedEventArgs e)
        {
            ChangeValue(e.Item);
            _selectorLink.ClosePopup();
        }


        private void ChangeValue(object newValue)
        {
            ValueChangingEventArgs changingArgs = new ValueChangingEventArgs()
            {
                RoutedEvent = ValueChangingEvent,
                Source = this,
                Handled = false,
                OldValue = this.Item,
                NewValue = newValue
            };
            RaiseEvent(changingArgs);

            if (changingArgs.Handled)
                return;

            ModelProperty modelProperty = GetModelProperty();
            if (null == modelProperty)
                return;

            OnChangeValue(newValue, modelProperty);

            RoutedEventArgs changedArgs = new ValueChangedEventArgs()
            {
                RoutedEvent = ValueChangedEvent,
                Source = this,
                Handled = false,
                OldValue = this.Item,
                NewValue = newValue
            };
            RaiseEvent(changedArgs);
        }


        private void ReplaceInCollection(ModelItemCollection collection, ModelItem oldItem, object newObject)
        {
            if (null != oldItem)
            {
                int oldIndex = collection.IndexOf(oldItem);
                using (ModelEditingScope editingScope = collection.BeginEdit())
                {
                    collection.RemoveAt(oldIndex);
                    collection.Insert(oldIndex, newObject);
                    editingScope.Complete();
                }
            }
            else if (OwnerItem.ItemType == typeof(System.Activities.Statements.Sequence))
            {
                DependencyObject iterator = this;

                // Get the ItemsControl
                ItemsControl itemsControl = null;
                while (iterator != null)
                {
                    iterator = VisualTreeHelper.GetParent(iterator);

                    itemsControl = iterator as ItemsControl;
                    if (itemsControl != null)   // Found the ItemsControl!
                        break;
                }

                Debug.Assert(itemsControl != null);

                // Get the Spacer control
                iterator = this;
                DependencyObject currentSpacer = null;
                while (iterator != null)
                {
                    if (itemsControl.Items.IndexOf(iterator) != -1)
                    {
                        currentSpacer = iterator;
                        break;
                    }

                    iterator = VisualTreeHelper.GetParent(iterator);
                }

                int spacerItemIndex = 0;
                // Skip the header and footer control.
                for (int i = 1; i < itemsControl.Items.Count - 1; i++)
                {
                    if (itemsControl.Items[i].Equals(currentSpacer))
                    {
                        break;
                    }
                    if (itemsControl.Items[i].GetType() == currentSpacer.GetType())
                    {
                        ++spacerItemIndex;
                    }
                }
                collection.Insert(spacerItemIndex, newObject);
            }
            else
            {
                collection.Add(newObject);
            }
        }


        private void OnPopupOpened(object sender, EventArgs e)
        {
            PopupOpeningEventArgs args = new PopupOpeningEventArgs()
            {
                RoutedEvent = PopupOpeningEvent,
                Source = this,
                Handled = false
            };
            RaiseEvent(args);
            UpdateContexts(args.Handled ? args.ValidValueItemHint : null);
            UpdateSelectItems();
        }


        private void OnRemove(object sender, EventArgs e)
        {
            ModelItem oldItem = Item;
            ModelProperty modelProperty = GetModelProperty();
            if (modelProperty != null && modelProperty.IsCollection)
            {
                if (null != oldItem)
                    modelProperty.Collection.Remove(oldItem);
            }
            else
            {
                modelProperty.ClearValue();
            }
        }


        private void OnReset(object sender, EventArgs e)
        {
            Reset();
        }


        private bool IsCurrentActivityResetable()
        {
            ModelProperty property = GetModelProperty();
            if (property != null && property.Value != null)
            {
                object expressionValue = property.Value.GetExpressionValue();
                IResetableActivity resetableActivity = expressionValue as IResetableActivity;
                return resetableActivity != null;
            }
            return false;
        }


        private void ResetCurrentActivity()
        {
            ModelProperty property = GetModelProperty();
            Debug.Assert(property != null && property.Value != null);

            object expressionValue = property.Value.GetExpressionValue();
            Debug.Assert(expressionValue != null);

            IResetableActivity resetableActivity = expressionValue as IResetableActivity;
            Debug.Assert(resetableActivity != null);

            object newValue = resetableActivity.GetResetedValue();
            Activity<DynamicValue> newDynamicValueActivity = newValue as Activity<DynamicValue>;
            Debug.Assert(newDynamicValueActivity != null);

            // Ask for updating View Model and View.
            RaiseEvent(new ItemSelectedEventArgs(ItemSelector.ItemSelectedEvent, this, new InArgument<DynamicValue>(newDynamicValueActivity)));
        }


        private bool TryGetDefaultValue(out object defaultValue)
        {
            defaultValue = null;
            ModelProperty property = GetModelProperty();
            if (null == property)
            {
                return false;
            }

            Type argumentType;
            if (property.IsCollection)
            {
                // Get element type of collection
                argumentType = property.PropertyType.GetGenericArguments().First<Type>();
            }
            else
            {
                argumentType = property.PropertyType;
            }

            Type outputType = null;
            if (argumentType == typeof(InArgument<>))
            {
                outputType = argumentType.GetGenericArguments().First<Type>();
            }
            else if (argumentType.IsGenericType)
            {
                if (argumentType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return true; // Nullable<T>'s default value is null.
                }
                else
                {
                    outputType = argumentType.GetGenericArguments().Last<Type>();
                }
            }
            else if (argumentType == typeof(InArgument))
            {
                // If argument type is InArgument, it means all InArgument<?> are accepted.
                defaultValue = StringLiteral.CreateArgument("null"); //NOXLATE
                return true;
            }
            else if (argumentType.IsEnum)
            {
                var validValues = argumentType.GetEnumValues();
                defaultValue = validValues.GetValue(0); // Enum type's default value is the first item.
                return true;
            }

            if (outputType != null && outputType.IsValueType)
            {
                MethodInfo method = this.GetType().GetMethod("CreateNewValue", BindingFlags.NonPublic | BindingFlags.Instance); // NOXLATE
                MethodInfo generic = method.MakeGenericMethod(outputType);
                defaultValue = generic.Invoke(this, null);
                return true;
            }

            return false;
        }

        ~ItemSelector()
        {
            _selectorLink.OnPopupOpened -= OnPopupOpened;
            _selectorLink.OnRemove -= OnRemove;
            _selectorLink.OnReset -= OnReset;
        }
    }
}
