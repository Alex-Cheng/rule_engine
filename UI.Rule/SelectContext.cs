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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;


namespace Autodesk.IM.UI.Rule
{
    public abstract class SelectContext : ContextBase
    {
        public SelectContext(ItemSelector parent)
        {
            Parent = parent;
            SelectItems = new ObservableCollection<SelectItem>();
        }

        public abstract string SelectContextName
        {
            get;
        }

        public override string DisplayName
        {
            get
            {
                return SelectContextName;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public ObservableCollection<SelectItem> SelectItems
        {
            get;
            set;
        }

        public abstract object CreateNewInstance(SelectItem item);

        public abstract void UpdateSelectItems();

        public bool IsDefaultContext
        {
            get
            {
                return Parent.DefaultSelectContext == this;
            }
        }

        public ItemSelector Parent { get; set; }

        protected Type GetOutputType()
        {
            ModelProperty property = Parent.GetModelProperty();
            return property.GetValueType();
        }
    }
}
