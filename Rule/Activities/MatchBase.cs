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
using System.Activities;
using System.Collections.ObjectModel;
using System.Windows.Markup;


namespace Autodesk.IM.Rule.Activities
{
    /// <summary>
    /// Base class for Match class and MatchWithResult class. The MatchWithResult class is a
    /// variation of Match class. It returns specific value for each branch.
    /// </summary>
    [ContentProperty("Cases")]   // NOXLATE
    public abstract class MatchBase : NativeActivity<DynamicValue>
    {
        /// <summary>
        /// Initializes a new instance of subclass of Autodesk.IM.Rule.Activities.MatchBase class.
        /// </summary>
        public MatchBase()
        {
            Cases = new Collection<MatchCase>();
            this.CaseIndex = new Variable<int>();
            onMatchValueComplete = new CompletionCallback<DynamicValue>(ContinueAtMatchValue);
        }


        /// <summary>
        /// Initializes the instance with default values.
        /// </summary>
        public void InitDefault()
        {
            this.Operator = MatchOperator.Equals;
            this.Expression = DynamicLiteral<int>.CreateArgument(1);
        }


        /// <summary>
        /// Gets or sets operator of macth.
        /// </summary>
        public MatchOperator Operator
        {
            get;
            set;
        }


        /// <summary>
        /// The expression of match, equivalent to the variable n in "switch(n)...case" in C language.
        /// </summary>
        [DependsOn("Operator")] //NOXLATE
        public InArgument<DynamicValue> Expression
        {
            get;
            set;
        }


        /// <summary>
        /// Collection of branches.
        /// </summary>
        [DependsOn("Expression")] //NOXLATE
        public Collection<MatchCase> Cases
        {
            get;
            set;
        }


        /// <summary>
        /// Default branch, equivalent to "default:" in C language.
        /// </summary>
        [DependsOn("Cases")] //NOXLATE
        public Activity Default
        {
            get;
            set;
        }


        protected Variable<int> CaseIndex
        {
            get;
            set;
        }


        protected CompletionCallback<DynamicValue> onMatchValueComplete;


        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            //call base.CacheMetadata to add the Activities and Variables to this activity's metadata
            base.CacheMetadata(metadata);

            // Declare children activities
            foreach (MatchCase matchCase in this.Cases)
            {
                metadata.AddChild(matchCase.Expression);
                metadata.AddChild(matchCase.Case);
            }
            metadata.AddImplementationVariable(this.CaseIndex);
        }


        protected override void Execute(NativeActivityContext context)
        {
            if (this.Cases.Count == 0 || null == this.Expression)
                return;

            context.SetValue<int>(this.CaseIndex, 0);

            MatchCase matchCase = this.Cases[0];
            context.ScheduleActivity(matchCase.Expression, onMatchValueComplete);
        }


        protected abstract void OnCaseMatched(NativeActivityContext context, ActivityInstance completedInstance, DynamicValue result, Activity matchedCase);


        protected virtual void ContinueAtMatchValue(
            NativeActivityContext context, ActivityInstance completedInstance, DynamicValue result)
        {
            if (completedInstance.State == ActivityInstanceState.Faulted)
                context.Abort(new Exception(Properties.Resources.FailMatchValue));

            int caseIndex = context.GetValue<int>(this.CaseIndex);

            DynamicValue expressionValue = context.GetValue<DynamicValue>(this.Expression);
            if (DoValuesMatch(expressionValue, result))
            {
                MatchCase matchCase = this.Cases[caseIndex];
                this.OnCaseMatched(context, completedInstance, result, matchCase.Case);
            }
            else
            {
                caseIndex++;
                if (caseIndex >= this.Cases.Count)
                {
                    if (null != this.Default)
                        this.OnCaseMatched(context, completedInstance, result, this.Default);
                    return;
                }
                context.SetValue<int>(this.CaseIndex, caseIndex);
                MatchCase nextMatchCase = this.Cases[caseIndex];
                context.ScheduleActivity(nextMatchCase.Expression, onMatchValueComplete);
            }
        }

        protected virtual bool DoValuesMatch(DynamicValue exprValue, DynamicValue result)
        {
            switch (this.Operator)
            {
                case MatchOperator.Equals:
                    return exprValue == result;
                case MatchOperator.LessThan:
                    return exprValue < result;
                case MatchOperator.GreaterThan:
                    return exprValue > result;
                case MatchOperator.LessThanOrEqual:
                    return exprValue <= result;
                case MatchOperator.GreaterThanOrEqual:
                    return exprValue >= result;
                case MatchOperator.NotEqual:
                    return exprValue != result;
            }
            return false;
        }
    }
}
