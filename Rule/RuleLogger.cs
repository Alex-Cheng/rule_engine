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
using System.Activities.Tracking;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace Autodesk.IM.Rule
{
    public class LoggingCompletedEventArgs : EventArgs
    {
        internal LoggingCompletedEventArgs(string log)
        {
            this.Log = log;
        }

        public string Log { get; private set; }
    }


    /// <summary>
    /// This class is responsible for logging during execution of rules.
    /// </summary>
    public class RuleLogger : TrackingParticipant
    {
        protected StringBuilder _log = new StringBuilder();
        protected string _indent;


        /// <summary>
        /// Gets log.
        /// </summary>
        public string Log
        {
            get
            {
                return _log.ToString();
            }
        }


        /// <summary>
        /// Begin logging for a specified rule.
        /// </summary>
        /// <param name="rule">An instance of RuleBase associated with the logger.</param>
        public virtual void BeginLogging(IRule rule)
        {
            _log.Clear();
            _log.AppendLine("\n-------------------------------------------------------"); //NOXLATE
            _log.AppendLine(String.Format("{0}: {1}", rule.DisplayName, rule.FullName)); //NOXLATE
            _indent = String.Empty;
            IncrementIndent();
        }


        /// <summary>
        /// Called when a named rule is called.
        /// </summary>
        /// <param name="rule">The named rule being called.</param>
        public virtual void OnInvokingNamedRule(NamedRule rule)
        {
            _log.AppendLine(String.Format("{0}    Invoking subrule: {1}: {2}", _indent, rule.DisplayName, rule.Path)); //NOXLATE
        }


        /// <summary>
        /// Write log when track to a rule argument.
        /// </summary>
        /// <param name="argument">The rule argument in current context.</param>
        /// <param name="logger">The logger used to write log.</param>
        protected virtual void Track(KeyValuePair<string, object> argument, StringBuilder logger)
        {
            if (argument.Key.Equals("Result"))
            {
                return;
            }

            string value = (null == argument.Value) ? "NULL" : argument.Value.ToString(); //NOXLATE
            logger.Append(String.Format(" {0}=\"{1}\"", argument.Key, value)); //NOXLATE
        }


        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            if (record is ActivityStateRecord)
            {
                ActivityStateRecord state = record as ActivityStateRecord;
                if (null == state.Activity)
                    return;
                bool closed = state.State.Equals("Closed"); //NOXLATE

                StringBuilder logLine = new StringBuilder();
                logLine.Append(String.Format(
                    "{0}{1}{2} {3} {4}", //NOXLATE
                    state.Level.ToString(), _indent,
                    closed ? "<-" : "--", //NOXLATE
                    state.Activity.Id, GetName(state.Activity)));
                if (record.Level == TraceLevel.Info)
                {
                    if (closed)
                    {
                        if (state.Arguments.ContainsKey("Result")) // NOXLATE
                        {
                            logLine.Append(String.Format(
                                " == \"{0}\"", state.Arguments["Result"])); //NOXLATE
                        }
                        DecrementIndent();
                    }
                    else
                    {
                        foreach (KeyValuePair<string, object> keyValue in state.Arguments)
                        {
                            Track(keyValue, logLine);
                        }
                    }
                }
                _log.AppendLine(logLine.ToString());
            }
            else if (record is ActivityScheduledRecord)
            {
                ActivityScheduledRecord scheduled = record as ActivityScheduledRecord;
                if (null == scheduled.Activity || null == scheduled.Child)
                    return;
                if (record.Level == TraceLevel.Info)
                    IncrementIndent();

                StringBuilder logLine = new StringBuilder();
                logLine.Append(String.Format(
                    "{0}{1}-> {2} {3}", //NOXLATE
                    scheduled.Level.ToString(), _indent, //scheduled.Activity.Id, GetName(scheduled.Activity), <- parent activity
                    scheduled.Child.Id, GetName(scheduled.Child)));
                _log.AppendLine(logLine.ToString());
            }
        }


        private void IncrementIndent()
        {
            _indent += " "; //NOXLATE
        }


        private void DecrementIndent()
        {
            _indent = (_indent.Length <= 1) ?
                String.Empty : _indent.Substring(0, _indent.Length - 1);
        }


        private string GetName(ActivityInfo info)
        {
            // strip off the generic params from the activity name
            string name = info.Name;
            int len = name.IndexOf('<'); //NOXLATE
            if (len < 1) len = name.Length;
            return name.Substring(0, len);
        }
    }
}
