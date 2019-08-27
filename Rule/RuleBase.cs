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
using System.Xml.Serialization;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// Represents the abstraction of Rules.
    /// </summary>
    public abstract class RuleBase : IRule
    {
        public const string RuleClassName = "Rule"; // NOXLATE


        /// <summary>
        /// Initializes a new instance of subclasses of Autodesk.IM.Rule.RuleBase.
        /// It is used for deserialization.
        /// </summary>
        public RuleBase()
        {  }


        /// <summary>
        /// Gets its owner which is an instance of RuleManager.
        /// </summary>
        [XmlIgnore]
        public abstract RuleManager Owner
        {
            get;
        }


        /// <summary>
        /// Gets full name of the rule.
        /// </summary>
        public string FullName
        {
            get
            {
                return Path;
            }
        }


        /// <summary>
        /// Gets Rule's Name.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets Rule's display text in UI
        /// </summary>
        public abstract string DisplayName
        {
            get;
        }

        /// <summary>
        /// Gets Rule's parent which is an instance of Rule Point.
        /// </summary>
        public abstract RulePoint Parent
        {
            get;
        }

        /// <summary>
        /// Gets Rule's signature.
        /// </summary>
        public abstract RuleSignature Signature
        {
            get;
        }

        /// <summary>
        /// Gets Rule's path which indicates its position in Rule hierarchy.
        /// </summary>
        public virtual string Path
        {
            get
            {
                if (Parent != null)
                {
                    return String.Format("{0}/{1}", Parent.Path, Name); // NOXLATE
                }
                else
                {
                    return Name;
                }
            }
        }

        /// <summary>
        /// Gets Acitivity's ID used for saving and loading of Rule's activities.
        /// </summary>
        public abstract string ActivityID
        {
            get;
        }

        /// <summary>
        /// Gets an instance of DynamicActivity class which consists of activities.
        /// </summary>
        public DynamicActivity Activity
        {
            get
            {
                return Owner.Storage.GetActivity(this.ActivityID);
            }
        }

        /// <summary>
        /// Returns true if the rule has an underlying activity defined, otherwise false.
        /// </summary>
        public bool HasActivity
        {
            get
            {
                return Owner.Storage.HasActivity(this.ActivityID);
            }
        }


        /// <summary>
        /// Saves the activities of the rule.
        /// </summary>
        /// <param name="da">The instance of DynamicActivity class containing the Rule's activities.</param>
        public abstract void Save(DynamicActivity da);

        /// <summary>
        /// Deletes the rule.
        /// </summary>
        public abstract void Delete();
    }
}
