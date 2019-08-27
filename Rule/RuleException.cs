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
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Represents exception relating to rules.
    /// </summary>
    public class RuleException : Exception
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.RuleException class with
        /// rule's full name and inner exception.
        /// </summary>
        /// <param name="fullName">A string representing rule's full name.</param>
        /// <param name="innerException">An Exception object representing inner exception.</param>
        public RuleException(string fullName, Exception innerException)
            : base(String.Empty, innerException)
        {
            RuleFullName = fullName;
        }


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.RuleException class with
        /// rule's full name and message.
        /// </summary>
        /// <param name="fullName">A rule's full name indicating the rule throwing this exception.</param>
        /// <param name="message">A string representing the message of exception.</param>
        public RuleException(string fullName, string message)
            : base(message)
        {
            RuleFullName = fullName;
        }


        /// <summary>
        /// Gets or sets rule's full name.
        /// </summary>
        public string RuleFullName
        {
            get;
            set;
        }


        /// <summary>
        /// Gets message of this exception.
        /// </summary>
        public override string Message
        {
            get
            {
                string innerMessage = (InnerException != null) ? InnerException.Message : base.Message;
                return String.Format(Properties.Resources.RuleException, this.RuleFullName, innerMessage);
            }
        }
    }
}
