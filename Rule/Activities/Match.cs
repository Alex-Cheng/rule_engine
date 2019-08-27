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

using System.Activities;
using System.Collections.ObjectModel;
using System.Windows.Markup;


namespace Autodesk.IM.Rule.Activities
{
    /// <summary>
    /// Represents a flow control statement 'match'. 'match' is similiar to switch...case in C language.
    /// </summary>
    [ContentProperty("Cases")]   // NOXLATE
    public class Match : MatchBase
    {
        /// <summary>
        /// Creates a new instance of Autodesk.IM.Rule.Activities.Match class.
        /// </summary>
        /// <returns></returns>
        public static Match Create()
        {
            Match instance = new Match();
            instance.InitDefault();
            return instance;
        }


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.Activities.Match class.
        /// </summary>
        public Match()
            : base()
        {
        }


        protected override void OnCaseMatched(NativeActivityContext context, ActivityInstance completedInstance, DynamicValue result, Activity matchedCase)
        {
            context.ScheduleActivity(matchedCase);
        }
    }
}
