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
using System.Activities;
using System.Collections.Generic;
using System.IO;

using Autodesk.IM.Rule;
using System;
using System.Xml.Serialization;


namespace RuleConfiguration
{
    /// <summary>
    /// Represents a rule library implementing Autodesk.IM.Rule.IRuleLibrary interface for testing purpose.
    /// </summary>
    internal class RuleLibrary : IRuleLibrary
    {
        private ActivitySerializer _serializer = new ActivitySerializer();
        private Dictionary<string, DynamicActivity> _activities = new Dictionary<string, DynamicActivity>();
        private Dictionary<string, NamedRule> _namedrules = new Dictionary<string, NamedRule>();


        public ActivitySerializer Serializer
        {
            get
            {
                return _serializer;
            }
        }


        public void Save()
        {
            string folder = Environment.CurrentDirectory;
            // Serialize _activities
            foreach (var kv in _activities)
            {
                string fn = Path.Combine(folder, Uri.EscapeDataString(kv.Key)) + ".xaml";                

                // Write to file
                using (TextWriter tw = new StreamWriter(fn))
                {
                    _serializer.Serialize(tw, kv.Value);
                }
            }

            // Serialize _namedrules
            foreach (var kv in _namedrules)
            {
                string fn = Path.Combine(folder, Uri.EscapeDataString(kv.Key.Replace("/", Path.PathSeparator.ToString()))) + ".xml";

                // Write to file
                XmlSerializer serializer = new XmlSerializer(typeof(NamedRule));
                using (TextWriter tw = new StreamWriter(fn))
                {
                    serializer.Serialize(tw, kv.Value);
                }
            }
        }


        public void Load()
        {
            // Deserialize _activities
            _activities.Clear();
            string folder = Environment.CurrentDirectory;
            foreach (string filePath in Directory.GetFiles(folder, "*.xaml"))
            {
                string fn = Path.GetFileNameWithoutExtension(filePath);
                string key = Uri.UnescapeDataString(fn);
                using (TextReader tr = new StreamReader(filePath))
                {
                    DynamicActivity da = (DynamicActivity)_serializer.Deserialize(tr);
                    _activities.Add(key, da);
                }
            }

            // Deserialize _namedrules
            // TODO: ignore it right now.
            _namedrules.Clear();
        }


        public bool HasActivity(string activityId)
        {
            return _activities.ContainsKey(activityId);
        }


        public DynamicActivity GetActivity(string activityId)
        {
            string xaml = Serializer.Serialize(_activities[activityId]);
            return Serializer.Deserialize(xaml);
        }


        public DynamicActivity GetOriginalActivity(string activityId)
        {
            return _activities[activityId];
        }


        public IEnumerable<NamedRule> GetNamedRules()
        {
            return _namedrules.Values;
        }


        public IEnumerable<NamedRule> GetNamedRules(string path)
        {
            foreach (var nr in GetNamedRules())
            {
                if (nr.ParentRulePointPath == path)
                {
                    yield return nr;
                }
            }
        }


        public void SetNamedRule(NamedRule namedRule, DynamicActivity workflow)
        {
            _namedrules[namedRule.Path] = namedRule;
            _activities[namedRule.ActivityID] = workflow;
        }


        public void SetRulePointWorkflow(RulePoint rulePoint, DynamicActivity workflow)
        {
            _activities[rulePoint.ActivityID] = workflow;
        }


        public void RemoveNamedRule(NamedRule theOneToRemove)
        {
            _namedrules.Remove(theOneToRemove.Path);
            _activities.Remove(theOneToRemove.ActivityID);
        }


        public void RemoveRulePointWorkflow(RulePoint rulePoint)
        {
            _activities.Remove(rulePoint.ActivityID);
        }
    }
}
