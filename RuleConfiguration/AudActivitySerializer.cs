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
using System.IO;

using Autodesk.IM.Rule;

using System.Activities;
using System.Xaml;


namespace RuleConfiguration
{
    /// <summary>
    /// Represents an implementation of RuleConfiguration.Settings.ISerializer
    /// </summary>
    public class AudActivitySerializer
    {
        private ActivitySerializer _activitySerializer;


        public AudActivitySerializer()
        {
            _activitySerializer = new ActivitySerializer();
            _activitySerializer.SchemaContext = new XamlSchemaContext();
        }


        #region RuleConfiguration.Settings.ISerializer

        public void Serialize(TextWriter writer, object obj)
        {
            if (obj is DynamicActivity)
            {
                DynamicActivity da = obj as DynamicActivity;
                _activitySerializer.Serialize(writer, da);
            }
            else if (obj is ActivityBuilder)
            {
                ActivityBuilder ab = obj as ActivityBuilder;
                _activitySerializer.Serialize(writer, ab);
            }
            else
            {
                throw new ArgumentException("The parameter obj must be type of DynamicActivity or ActivityBuilder.", "obj"); //NOXLATE
            }
        }


        public object Deserialize(TextReader reader, System.Type objectType)
        {
            return _activitySerializer.Deserialize(reader);
        }


        public string FileExtension
        {
            get
            {
                return ".xaml"; //NOXLATE
            }
        }

        #endregion
    }
}
