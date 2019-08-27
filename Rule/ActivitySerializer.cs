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
using System.Activities.XamlIntegration;
using System.IO;
using System.Text;
using System.Xaml;
using System.Xml;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// This class is responsible for serialization and deserialization of activities in rules.
    /// Activities in a rule are contained in a DynamicActivity object.
    /// </summary>
    public class ActivitySerializer
    {
        /// <summary>
        /// Gets or sets XAML schema context.
        /// </summary>
        private XamlSchemaContext _schemaContext = new XamlSchemaContext();
        public XamlSchemaContext SchemaContext
        {
            get
            {
                return _schemaContext;
            }
            set
            {
                _schemaContext = value;
            }
        }


        /// <summary>
        /// Serializes a specified DynamicActivity object into a string.
        /// </summary>
        /// <param name="da">The specified DynamicActivity object.</param>
        /// <returns>A string as result of serialization.</returns>
        public string Serialize(DynamicActivity da)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (StringWriter writer = new StringWriter(stringBuilder))
            {
                ActivitySerializer serializer = new ActivitySerializer();
                serializer.Serialize(writer, da);
            }
            string workflowXaml = stringBuilder.ToString();
            return workflowXaml;
        }


        /// <summary>
        /// Serializes a specified DynamicActivity object and write the result via specified TextWriter object.
        /// </summary>
        /// <param name="writer">The writer to write result.</param>
        /// <param name="da">The specified DynamicActivity object.</param>
        public void Serialize(TextWriter writer, DynamicActivity da)
        {
            ActivityBuilder builder = new ActivityBuilder
            {
                Name = da.Name,
                Implementation = da.Implementation.Invoke()
            };
            foreach (var prop in da.Properties)
                builder.Properties.Add(prop);

            Serialize(writer, builder);
        }


        /// <summary>
        /// Serializes a specified ActivityBuilder object and write the result via specified TextWriter object.
        /// </summary>
        /// <param name="writer">The writer to write result.</param>
        /// <param name="builder">A specified ActivityBuilder object.</param>
        public void Serialize(TextWriter writer, ActivityBuilder builder)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(writer, GetXmlSettings()))
            {
                using (NoUIXamlXmlWriter xamlWriter = new NoUIXamlXmlWriter(xmlWriter, new XamlSchemaContext()))
                {
                    using (XamlWriter activityWriter = ActivityXamlServices.CreateBuilderWriter(xamlWriter))
                    {
                        XamlServices.Save(activityWriter, builder);
                    }
                }
            }
        }


        /// <summary>
        /// Deserializes a specified XAML text into a DynamicActivity object.
        /// </summary>
        /// <param name="xaml">The specified XAML text.</param>
        /// <returns>A deserialized DynamicActivity object.</returns>
        public DynamicActivity Deserialize(string xaml)
        {
            using (TextReader tr = new StringReader(xaml))
            {
                return Deserialize(tr);
            }
        }


        /// <summary>
        /// Deserializes from a specified TextReader object to a DynamicActivity object.
        /// </summary>
        /// <param name="reader">The specified TextReader object.</param>
        /// <returns>A deserialized DynamicActivity object.</returns>
        public DynamicActivity Deserialize(TextReader reader)
        {
            try
            {
                XamlSchemaContext context = SchemaContext;
                XamlXmlReaderSettings settings = new XamlXmlReaderSettings()
                {
                    LocalAssembly = typeof(ActivitySerializer).Assembly
                };
                using (XamlXmlReader xamlReader = new XamlXmlReader(reader, context, settings))
                {
                    using (XamlReader activityReader = ActivityXamlServices.CreateReader(xamlReader))
                    {
                        Activity activity = ActivityXamlServices.Load(activityReader);
                        return activity as DynamicActivity;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return null;
        }


        private XmlWriterSettings GetXmlSettings()
        {
            return new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = "\t", //NOXLATE
                NewLineChars = Environment.NewLine,
                NewLineOnAttributes = false,
                NewLineHandling = NewLineHandling.Replace
            };
        }


        private class NoUIXamlXmlWriter : XamlXmlWriter
        {
            int _ignore = 0;

            public NoUIXamlXmlWriter(XmlWriter xmlWriter, XamlSchemaContext context)
                : base(
                    XmlWriter.Create(xmlWriter),
                    context,
                    new XamlXmlWriterSettings
                    {
                        AssumeValidInput = true
                    })
            {
            }

            public override void WriteNamespace(NamespaceDeclaration namespaceDeclaration)
            {
                if (namespaceDeclaration.Prefix.Equals("sap")) //NOXLATe
                    return;
                base.WriteNamespace(namespaceDeclaration);
            }

            public override void WriteStartObject(XamlType type)
            {
                if (IgnoreStart(type))
                    return;
                base.WriteStartObject(type);
            }

            public override void WriteEndObject()
            {
                if (IgnoreEnd())
                    return;
                base.WriteEndObject();
            }

            public override void WriteStartMember(XamlMember property)
            {
                if (IgnoreStart(property.DeclaringType))
                    return;
                base.WriteStartMember(property);
            }

            public override void WriteEndMember()
            {
                if (IgnoreEnd())
                    return;
                base.WriteEndMember();
            }

            public override void WriteValue(object value)
            {
                if (Ignore())
                    return;
                base.WriteValue(value);
            }

            bool IgnoreStart(XamlType type)
            {
                if (_ignore > 0)
                {
                    _ignore++;
                    return true;
                }
                if (null != type && null != type.UnderlyingType)
                {
                    if (type.UnderlyingType.Namespace.Equals(
                            "System.Activities.Presentation.View")) //NOXLATE
                    {
                        _ignore = 1;
                        return true;
                    }
                }
                return false;
            }

            bool Ignore()
            {
                return _ignore > 0;
            }

            bool IgnoreEnd()
            {
                if (_ignore > 0)
                {
                    _ignore--;
                    return true;
                }
                return false;
            }
        }
    }
}
