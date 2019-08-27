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


using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Markup;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Rule")]
[assembly: AssemblyDescription("IM Rule")]


[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Autodesk, Inc.")]
[assembly: AssemblyProduct("IM Platform")]
[assembly: AssemblyCopyright("Copyright (C) 2011 by Autodesk, Inc.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: NeutralResourcesLanguage("en-US")]
[assembly: ComVisible(false)]

//TODO: Change the namespace because it has become IM component.
[assembly: XmlnsPrefix("http://www.autodesk.com/utilitydesign/rule", "rule")]
[assembly: XmlnsDefinition("http://www.autodesk.com/utilitydesign/rule", "Autodesk.IM.Rule")]
[assembly: XmlnsDefinition("http://www.autodesk.com/utilitydesign/rule", "Autodesk.IM.Rule.Activities")]
[assembly: XmlnsPrefix("http://www.autodesk.com/utilitydesign/common", "au")]
[assembly: XmlnsDefinition("http://www.autodesk.com/utilitydesign/common", "Autodesk.IM.Rule")]
