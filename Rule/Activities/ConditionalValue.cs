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
    /// An activity which returns result according to result of condition expression.
    /// It is similar to operator [condition]? value1 : value2.
    /// </summary>
    /// <typeparam name="T">Value's type.</typeparam>
    public class ConditionalValue<T> : NativeActivity<T>
    {
        private CompletionCallback<T> onValueComplete;


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.Activities.ConditionalValue class.
        /// </summary>
        public ConditionalValue()
        {
            onValueComplete = new CompletionCallback<T>(ContinueAtValue);
        }


        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        public InArgument<DynamicValue> Condition
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the value returned when condition is true.
        /// </summary>
        [DependsOn("Condition")] //NOXLATE
        public Activity<T> ValueWhenTrue
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the value returned when condition is false.
        /// </summary>
        [DependsOn("ValueWhenTrue")] //NOXLATE
        public Activity<T> ValueWhenFalse
        {
            get;
            set;
        }


        protected override void Execute(NativeActivityContext context)
        {
            DynamicValue dynIsTrue = Condition.Get(context);
            bool isTrue = dynIsTrue.ToBoolean(null);
            Activity<T> valueActivity = isTrue ?
                this.ValueWhenTrue : this.ValueWhenFalse;
            if (null == valueActivity)
                return;
            context.ScheduleActivity<T>(valueActivity, onValueComplete);
        }


        private void ContinueAtValue(
            NativeActivityContext context, ActivityInstance completedInstance, T result)
        {
            context.SetValue<T>(this.Result, result);
        }
    }
}
