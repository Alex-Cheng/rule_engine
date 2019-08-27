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


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Represents an argument of a rule.
    /// </summary>
    public class RuleArgument
    {
        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.RuleArgument class.
        /// </summary>
        public RuleArgument()
        {
        }


        /// <summary>
        /// Initializes a new instance of Autodesk.IM.Rule.RuleArgument with
        /// specified id, name and argument type.
        /// </summary>
        /// <param name="id">A GUID to identify the argument.</param>
        /// <param name="name">The specified argument name.</param>
        /// <param name="argumentType">A System.Type object representing the arugment's type.</param>
        public RuleArgument(Guid id, string name, Type argumentType)
        {
            Id = id;
            Name = name;
            ArgumentType = argumentType;
        }


        /// <summary>
        /// Gets or sets argument ID.
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the argument name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the argument type.
        /// </summary>
        public Type ArgumentType
        {
            get;
            set;
        }


        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            RuleArgument right = obj as RuleArgument;
            if ((object)right == null)
            {
                return false;
            }

            return this.Id == right.Id;
        }


        public bool Equals(RuleArgument right)
        {
            if ((object)right == null)
            {
                return false;
            }

            return this.Id == right.Id;
        }


        public static bool operator ==(RuleArgument left, RuleArgument right)
        {
            if (Object.ReferenceEquals(left, right))
            {
                return true;
            }

            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return left.Equals(right);
        }


        public static bool operator !=(RuleArgument left, RuleArgument right)
        {
            return !(left == right);
        }
    }
}
