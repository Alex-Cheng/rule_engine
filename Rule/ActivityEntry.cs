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


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Represents a registry of activity
    /// </summary>
    public class ActivityEntry
    {
        public ActivityEntry(string name, string displayName, Func<Activity> factoryFunction, ActivitySignature signature, Type activityType)
        {
            Name = name;
            DisplayName = displayName;
            Create = factoryFunction;
            Signature = signature;
            ActivityType = activityType;
        }

        public string Name
        {
            get;
            private set;
        }

        public string DisplayName
        {
            get;
            private set;
        }

        public Func<Activity> Create
        {
            get; private set;
        }

        public ActivitySignature Signature
        {
            get; private set;
        }

        public Type ActivityType
        {
            get;
            private set;
        }
    }
}
