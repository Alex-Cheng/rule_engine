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
using System.Text;


namespace Autodesk.IM.Rule
{
    /// <summary>
    /// This class contains helper methods manipulating rule paths.
    /// </summary>
    public static class RulePathHelper
    {
        public const string PathSeparator = "/"; // NOXLATE

        /// <summary>
        /// Makes a path with path parts, e.g. make string Validation/Electric when pass "Validation" and "Electric".
        /// </summary>
        /// <param name="pathParts">Parts of path</param>
        /// <returns>Path like [part1]/[part2]/[part3]/...</returns>
        public static string MakeRulePath(params string[] pathParts)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var part in pathParts)
            {
                sb.Append(part);
                sb.Append(PathSeparator);
            }
            if (sb.Length > 0)
            {
                return sb.ToString(0, sb.Length - PathSeparator.Length);
            }
            return String.Empty;
        }

        /// <summary>
        /// Extracts rule path to get its components.
        /// </summary>
        /// <param name="path">Rule path</param>
        /// <returns>An array of components</returns>
        public static string[] GetRulePathComponents(string path)
        {
            if (path == null)
            {
                return new string[0];
            }
            return path.Split(PathSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Extracts named rule's path, get its name and the path of parent rule point.
        /// </summary>
        /// <param name="namedRulePath">The path of named rule.</param>
        /// <param name="rulePointPath">The path of parent rule point.</param>
        /// <param name="ruleName">The rule name</param>
        /// <returns>Return true when succeed, otherwise return false.</returns>
        public static bool ExtractNamedRulePath(string namedRulePath, out string rulePointPath, out string ruleName)
        {
            if (namedRulePath == null)
            {
                rulePointPath = null;
                ruleName = null;
                return false;
            }

            int pos = namedRulePath.LastIndexOf(PathSeparator);
            if (pos == -1 || pos == 0 || pos == namedRulePath.Length - 1)
            {
                rulePointPath = null;
                ruleName = null;
                return false;
            }

            rulePointPath = namedRulePath.Substring(0, pos);
            ruleName = namedRulePath.Substring(pos + 1);
            return true;
        }
    }
}
