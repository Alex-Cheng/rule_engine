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
using System.Collections.ObjectModel;
using System.Text;


namespace Autodesk.IM.Rule.Activities
{
    /// <summary>
    /// Represents a string expression.
    /// </summary>
    public sealed class StringExpression : CodeActivity<string>
    {
        /// <summary>
        /// Collection of elements comprising a string.
        /// </summary>
        private Collection<InArgument> elements = new Collection<InArgument>();
        public Collection<InArgument> Elements
        {
            get
            {
                return elements;
            }
            set
            {
                elements = value;
            }
        }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            for (int i = 0; i < this.Elements.Count; i++)
            {
                RuntimeArgument argArgument = new RuntimeArgument(
                    "Element_" + i, this.Elements[i].ArgumentType, ArgumentDirection.In); //NOXLATE
                metadata.Bind(this.Elements[i], argArgument);
                metadata.AddArgument(argArgument);
            }
        }

        protected override string Execute(CodeActivityContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (InArgument arg in Elements)
            {
                object result = arg.Get(context);
                if (result != null)
                {
                    stringBuilder.Append(result.ToString());
                }
            }
            return stringBuilder.ToString();
        }
    }
}
