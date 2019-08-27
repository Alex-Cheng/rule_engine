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
using System.Collections.Generic;
using System.Activities;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Interface for defining a rule signature extension.
    /// This allows extended arguments to be specified for existing rule signatures and rule points.
    /// </summary>
    public interface IRuleSignatureExtension
    {
        /// <summary>
        /// Additional Input arguments provided by system. These arguments are given when invoking the workflow.
        /// </summary>
        /// <returns>Additonal input arguments.</returns>
        IEnumerable<RuleArgument> GetSystemInArguments();


        /// <summary>
        /// Additional Output arguments provided by system. These arguments are given when invoking the workflow.
        /// </summary>
        /// <returns>Additonal output arguments.</returns>
        IEnumerable<RuleArgument> GetSystemOutArguments();


        /// <summary>
        /// Returns additional providers of expression items.
        /// </summary>
        /// <returns>A collection of providers of expression items</returns>
        IEnumerable<IExpressionItemProvider> GetExpressionItemProviders();


        /// <summary>
        /// Retrieve default argument according to given argument list and add the retrieved argument
        /// into the given argument list.
        /// </summary>
        /// <param name="argumentName">The argument being asked for</param>
        /// <param name="arguments">The argument list which is used for retrieving and storing desired argument</param>
        void AddDefaultArgument(RuleArgument argument, IDictionary<RuleArgument, object> arguments);
    }
}
