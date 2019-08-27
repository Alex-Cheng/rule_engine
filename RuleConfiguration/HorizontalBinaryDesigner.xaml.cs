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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

using Autodesk.IM.Rule;
using Autodesk.IM.Rule.Activities;
using IMConstants = Autodesk.IM.UI.Rule.Constants;


namespace RuleConfiguration
{
    // Interaction logic for HorizontalBinaryDesigner.xaml
    public partial class HorizontalBinaryDesigner
    {
        private bool _hasValidValues = false;
        IWeakEventListener _propChangedListener = null;

        public HorizontalBinaryDesigner()
        {
            InitializeComponent();

            _propChangedListener = new WeakEventListener<PropertyChangedEventArgs, PropertyChangedEventManager>(this.ModelItem_PropertyChanged);
        }

        protected override void OnModelItemChanged(object newItem)
        {
            base.OnModelItemChanged(newItem);

            PropertyChangedEventManager.AddListener(ModelItem, _propChangedListener, String.Empty);
        }

        void ModelItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // If left-hand operand changed, check if it become a feature property having valid values, e.g. Domain Attribute
            if (e.PropertyName == IMConstants.LeftOperandPropertyName)
            {
                IEnumerable<DynamicValue> validValues = ModelItem.Properties[IMConstants.RightOperandPropertyName].Value.GetValidValues();
                ModelProperty rightOperand = ModelItem.Properties[IMConstants.RightOperandPropertyName];

                // if expression like "1 equals 1" becomes "status equals 1", the right-hand operand should be refresh.
                // if expression like "Status equals new" becomes "1 equals new", the right-hand operand should be refresh, too.
                if (validValues.Any())
                {
                    // TODO: refactor to remove redundant code.
                    DynamicValue dv = validValues.First();
                    rightOperand.SetValue(DynamicLiteral.CreateArgument(dv.Value));
                    _hasValidValues = true;
                }
                else if (_hasValidValues)
                {
                    rightOperand.SetValue(DynamicLiteral.CreateArgument(IMConstants.DefaultNumberValue));
                    _hasValidValues = false;
                }
            }
        }
    }
}
