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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.IM.UI.Rule;


namespace RuleConfiguration
{
    /// <summary>
    /// UI component for select rule activity (rule activities are shown as statements in rule editor)
    /// </summary>
    public class ActivitySelector : ItemSelector
    {
        public ActivitySelector()
        {
            Contexts.Add(new ActivitySelectContext(this));
            Contexts.Add(new NamedRuleSelectContext(this));

            this.EmptyText = Properties.Resources.AddRule;

            Removable = false;
            Resetable = false;
        }
    }
}
