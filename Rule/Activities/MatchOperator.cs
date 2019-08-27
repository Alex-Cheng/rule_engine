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

namespace Autodesk.IM.Rule.Activities
{
    /// <summary>
    /// Enumerable of match operators.
    /// </summary>
    public enum MatchOperator
    {
        Equals = 1,
        LessThan = 2,
        GreaterThan = 3,
        LessThanOrEqual = 4,
        GreaterThanOrEqual = 5,
        NotEqual = 6,
        /* not sure we need these... will need support for Boolean expressions
        True = 7,
        False = 8,
        And = 9,
        Or = 10
         * */
    }
}
