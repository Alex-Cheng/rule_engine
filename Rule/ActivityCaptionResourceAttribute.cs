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


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Specifies the resource key as caption of an activity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    internal class ActivityCaptionResourceAttribute : Attribute
    {
        private string _resourceKey;


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.ActivityCaptionResourceAttribute class
        /// with the specified resource key.
        /// </summary>
        /// <param name="resourceKey">The specified resource key.</param>
        public ActivityCaptionResourceAttribute(string resourceKey)
        {
            this._resourceKey = resourceKey;
        }


        /// <summary>
        /// Gets resource key.
        /// </summary>
        public string ResourceKey
        {
            get
            {
                return this._resourceKey;
            }
        }
    }
}
