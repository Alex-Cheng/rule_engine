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

using System.Collections.Generic;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Represents the signature of an activity.
    /// </summary>
    public class ActivitySignature
    {
        /// <summary>
        /// Initializes a new instance of the Autodesk.IM.Rule.ActivitySignature class.
        /// </summary>
        public ActivitySignature()
        {
            CanModifyData = false;
            SystemInArguments = new List<RuleArgument>();
            SystemOutArguments = new List<RuleArgument>();
        }

        /// <summary>
        /// Indicates whether or not the activity can modify data model.
        /// </summary>
        public bool CanModifyData { get; set; }

        /// <summary>
        /// Input arguments provided by system. These arguments are given when invoking the workflow.
        /// </summary>
        public List<RuleArgument> SystemInArguments { get; set; }

        /// <summary>
        /// Output arguments provided by system. These arguments are given when invoking the workflow.
        /// </summary>
        public List<RuleArgument> SystemOutArguments { get; set; }
    }
}
