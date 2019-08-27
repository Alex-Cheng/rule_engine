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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

using Autodesk.IM.Rule;
using Autodesk.IM.UI.Rule;
using IMConstants = Autodesk.IM.UI.Rule.Constants;
using RuleConfiguration;


namespace RuleConfiguration
{
    public abstract class RuleBaseContext : ContextBase
    {
        protected readonly RulePointContext _parentContext;
        protected readonly RuleConfigContext _ruleConfigContext;

        private OperationContext _copyAsXaml;


        public RuleBaseContext(string name, RulePointContext parentContext, RuleConfigContext ruleConfigContext)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name"); // NOXLATE
            }
            if (ruleConfigContext == null)
            {
                throw new ArgumentNullException("ruleConfigContext"); // NOXLATE
            }

            _name = name;
            _parentContext = parentContext;
            _ruleConfigContext = ruleConfigContext;
        }


        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                IsDirty = true;

                OnPropertyChanged("Name"); // NOXLATE
                OnPropertyChanged("Path"); // NOXLATE
            }
        }


        private string _leftDisplayText = String.Empty;
        public string LeftDisplayText
        {
            get
            {
                return _leftDisplayText;
            }
            private set
            {
                if (_leftDisplayText == value)
                {
                    return;
                }

                _leftDisplayText = value;
                OnPropertyChanged("LeftDisplayText"); // NOXLATE
            }
        }


        private string _rightDisplayText = String.Empty;
        public string RightDisplayText
        {
            get
            {
                return _rightDisplayText;
            }
            private set
            {
                if (_rightDisplayText == value)
                {
                    return;
                }

                _rightDisplayText = value;
                OnPropertyChanged("RightDisplayText"); // NOXLATE
            }
        }


        private string _highlightedDisplayText = String.Empty;
        public string HighlightedDisplayText
        {
            get
            {
                return _highlightedDisplayText;
            }
            private set
            {
                if (_highlightedDisplayText == value)
                {
                    return;
                }

                _highlightedDisplayText = value;
                OnPropertyChanged("HighlightedDisplayText"); // NOXLATE
            }
        }


        public void UpdateDisplayTexts()
        {
            string keyword = _ruleConfigContext.SearchKeyword;
            string displayText = DisplayName;
            if (String.IsNullOrEmpty(keyword))
            {
                LeftDisplayText = displayText;
                RightDisplayText = String.Empty;
                HighlightedDisplayText = String.Empty;
            }
            else
            {
                int pos = displayText.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase);
                if (pos == -1)
                {
                    LeftDisplayText = displayText;
                    RightDisplayText = String.Empty;
                    HighlightedDisplayText = String.Empty;
                }
                else
                {
                    LeftDisplayText = displayText.Substring(0, pos); // pos is the length.
                    int length = keyword.Length;
                    RightDisplayText = displayText.Substring(pos + length);

                    // use original text rather than keyword
                    HighlightedDisplayText = displayText.Substring(pos, length);
                }
            }
        }


        public abstract bool CanRename
        {
            get;
        }


        public string Path
        {
            get
            {
                if (_parentContext != null)
                {
                    return RulePathHelper.MakeRulePath(_parentContext.Path, Name);
                }
                else
                {
                    return RulePathHelper.MakeRulePath(Name);
                }
            }
        }


        public abstract string DisplayPath
        {
            get;
        }


        public RulePointContext Parent
        {
            get
            {
                return _parentContext;
            }
        }


        private string _text = null;
        public string Text
        {
            get
            {
                // Lazy load the content for higher performance and lower memory consumption.
                if (_text == null)
                {
                    _text = GetOriginalWorkflowText();
                }
                return _text;
            }
            set
            {
                _text = value;
                IsDirty = true;

                OnPropertyChanged("Text"); // NOXLATE
                OnPropertyChanged("HasContent"); // NOXLATE
            }
        }


        public bool HasContent
        {
            get
            {
                // need to avoid calling the Text property, as it will load the activity
                // _text == null means have not loaded workflow content from storage.
                if (_text == null)
                {
                    RuleBase rule = GetRule();
                    return rule.HasActivity;
                }
                else
                {
                    // If _text equals to empty, it means it has content before but users cleaned its content.
                    // The rule will be deleted when they click on 'Apply' or 'Ok' buttons.
                    return _text != String.Empty;
                }
            }
        }


        public abstract bool IsNamedRule
        {
            get;
        }


        private bool _isDirty = false;
        public bool IsDirty
        {
            get
            {
                return _isDirty;
            }
            protected set
            {
                _isDirty = value;
                if (_isDirty)
                {
                    _ruleConfigContext.IsDirty = true;
                }

                OnPropertyChanged("IsDirty"); // NOXLATE
            }
        }


        private bool _isCutted = false;
        public bool IsCutted
        {
            get
            {
                return _isCutted;
            }
            set
            {
                _isCutted = value;

                OnPropertyChanged("IsCutted"); // NOXLATE
            }
        }


        public bool IsSelected
        {
            get
            {
                return _ruleConfigContext.SelectedRule == this;
            }
            set
            {
                if (value && _ruleConfigContext.SelectedRule != this)
                {
                    _ruleConfigContext.SelectedRule = this;
                }

                if (!value && _ruleConfigContext.SelectedRule == this)
                {
                    _ruleConfigContext.SelectedRule = null;
                }

                OnPropertyChanged("IsSelected");   // NOXLATE
            }
        }


        private bool _isExpanded = false;
        public virtual bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                OnPropertyChanged("IsExpanded");  // NOXLATE
            }
        }


        public bool IsSearching
        {
            get
            {
                return _ruleConfigContext.IsSearching;
            }
        }


        /// <summary>
        /// Return operations available for this rule.
        /// </summary>
        public virtual IEnumerable<OperationContext> Operations
        {
            get
            {
                yield return _copyAsXaml;
            }
        }


        /// <summary>
        /// Signature of this rule
        /// </summary>
        public abstract RuleSignature Signature
        {
            get;
        }


        /// <summary>
        /// Get editing context for rule.
        /// </summary>
        /// <returns></returns>
        public abstract RuleEditingContext GetEditingContext();


        /// <summary>
        /// Save the rule represented by this context.
        /// </summary>
        public abstract void Commit();


        /// <summary>
        /// Get underlying RuleBase instance corresponding to this context.
        /// </summary>
        /// <returns></returns>
        public abstract RuleBase GetRule();


        protected void CopyToClipboard()
        {
            // TODO: 
        }


        protected string GetOriginalWorkflowText()
        {
            RuleBase rule = GetRule();

            if (rule != null && rule.HasActivity)
            {
                var ruleLib = RuleAppExtension.RuleManagerInstance.Storage;
                var workflow = ruleLib.GetOriginalActivity(rule.ActivityID);
                ActivitySerializer serializer = new ActivitySerializer();
                return serializer.Serialize(workflow);
            }
            else
            {
                return String.Empty;
            }
        }


        private void CopyContentAsXaml()
        {
            try
            {
                Clipboard.SetText(Text);
            }
            catch (COMException)
            {
                // Opening Clipboard operation does not always succeed,
                // so I need to catch the exception and report an error message to users.
                MessageBox.Show(
                    String.Format(Properties.Resources.CannotOpenClipboard),
                    Properties.Resources.Error,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
