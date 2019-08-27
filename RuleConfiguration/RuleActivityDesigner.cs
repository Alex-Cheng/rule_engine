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
using System.Activities.Presentation;
using System.Windows;


namespace RuleConfiguration
{
    public class RuleActivityDesigner : ActivityDesigner
    {
        const string DATA_FORMAT_NAME = "ModelItemFormat"; // NOXLATE

        Point _prevMousePos;
        int _prevMouseMoveTimeStamp;

        protected override void OnPreviewDragOver(DragEventArgs e)
        {
            base.OnPreviewDragOver(e);
            if (Collapsible && !ShowExpanded)
            {
                string[] formats = e.Data.GetFormats();
                int index = formats.Length - 1;
                while (index >= 0 && !formats[index].StartsWith(DATA_FORMAT_NAME))
                    --index;
                if (index >= 0)
                {
                    Point mousePos = e.GetPosition(this);
                    if (_prevMousePos != mousePos)
                    {
                        _prevMousePos = mousePos;
                        _prevMouseMoveTimeStamp = Environment.TickCount;
                    }
                    else if (Environment.TickCount - _prevMouseMoveTimeStamp > SystemParameters.MouseHoverTime.TotalMilliseconds)
                    {
                        ExpandState = true;
                        PinState = true;
                    }
                }
            }
        }
    }
}
