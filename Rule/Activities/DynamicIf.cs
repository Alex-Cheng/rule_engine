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
using System.Windows.Markup;


namespace Autodesk.IM.Rule.Activities
{
    /// <summary>
    /// Dynamic condition version of If activity from WF4.
    /// </summary>
    [ActivityCaptionResource(CaptionResource)]
    public class If : NativeActivity
    {
        public const string CaptionResource = "IfDisplayName"; //NOXLATE


        /// <summary>
        /// Create a new instance of Autodesk.IM.Rule.Activities.If.
        /// </summary>
        /// <returns>The new instance.</returns>
        public static If Create()
        {
            If instance = new If();
            return instance;
        }


        /// <summary>
        /// Gets or sets the condition of If activity.
        /// </summary>
        public InArgument<DynamicValue> Condition { get; set; }


        /// <summary>
        /// Gets or sets the activity which will execute when condition is true.
        /// </summary>
        [DependsOn("Condition")] //NOXLATE
        public Activity Then { get; set; }


        /// <summary>
        /// Gets or sets the activity which will execute when condition is false.
        /// </summary>
        [DependsOn("Then")] //NOXLATE
        public Activity Else { get; set; }


        protected override void Execute(NativeActivityContext context)
        {
            DynamicValue dynCondition = context.GetValue<DynamicValue>(this.Condition);
            bool condition = DynamicValueConvert.ConvertTo<bool>(dynCondition);
            if (condition)
            {
                if (null != this.Then)
                    context.ScheduleActivity(this.Then);
            }
            else
            {
                if (null != this.Else)
                    context.ScheduleActivity(this.Else);
            }
        }
    }
}
