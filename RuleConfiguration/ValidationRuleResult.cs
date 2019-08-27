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

using RuleConfiguration;


namespace RuleConfiguration
{
    /// <summary>
    /// This class represents the result of valdiation.
    /// </summary>
    public class ValidationRuleResult
    {
        private List<IValidationItem> _validationItems = new List<IValidationItem>();

        public bool IsOK
        {
            get
            {
                bool isOK = true;
                foreach (var item in _validationItems)
                {
                    if (item.ResultType == ValidationType.Warning ||
                        item.ResultType == ValidationType.Error)
                    {
                        isOK = false;
                        break;
                    }
                }
                return isOK;
            }
        }

        public IEnumerable<IValidationItem> ResultItems
        {
            get
            {
                return _validationItems.AsEnumerable();
            }
        }

        public void ClearResult()
        {
            _validationItems.Clear();
        }

        public void AddResult(object featItem, ValidationType resultType, string deviceType, string message)
        {
            _validationItems.Add(
                new PipeValidationItem
                {
                    FeatureItem = featItem,
                    Title = message,
                    ResultType = resultType,
                    DeviceType = deviceType
                });
        }

        public void AddResult(IValidationItem result)
        {
            _validationItems.Add(result);
        }
    }
}
