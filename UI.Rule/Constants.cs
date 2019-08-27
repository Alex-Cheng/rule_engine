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


namespace Autodesk.IM.UI.Rule
{
    public static class Constants
    {
        #region Property Names

        public const string LeftOperandPropertyName = "Left";  // NOXLATE

        public const string RightOperandPropertyName = "Right";  // NOXLATE

        public const string OperandPropertyName = "Operand"; // NOXLATE; used for unary operations

        public const string ExpressionPropertyName = "Expression"; // NOXLATE

        public const string OverridedActivitySignature = "OverridedActivitySignature";    // NOXLATE

        public const string DisplayNamePropertyName = "DisplayName";    // NOXLATE

        public const string ValuePropertyName = "Value";  // NOXLATE

        #endregion

        public const string PathSeparator = "/"; // NOXLATE

        public const string EmptyTextDisplay = "?"; // NOXLATE

        public const int DefaultNumberValue = 1;

        public const string RuleFormatOnClipboard = "Rules://";  // NOXLATE
    }
}
