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


namespace Autodesk.IM.Rule
{
    public static class DynamicValueConvert
    {
        /// <summary>
        /// Convert source data to target type.
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="source">Source value</param>
        /// <returns>Result of conversion</returns>
        public static T ConvertTo<T>(object source)
        {
            T tvalue;
            try
            {
                if (typeof(T) == typeof(DynamicValue))
                {
                    tvalue = (T)(object)(new DynamicValue(source));
                }
                else
                {
                    tvalue = (T)System.Convert.ChangeType(source, typeof(T));
                }
            }
            catch (FormatException)
            {
                tvalue = default(T);
            }
            catch (Exception)
            {
                tvalue = default(T);
                //shouldn't go here
                System.Diagnostics.Debug.Assert(false);
            }
            return tvalue;
        }

        /// <summary>
        /// Convert source data to
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="source">Source value</param>
        /// <param name="outResult">Result of conversion</param>
        /// <returns>return </returns>
        public static bool TryConvertTo<T>(object source, out T outResult)
        {
            try
            {
                outResult = ConvertTo<T>(source);
                return true;
            }
            catch
            {
                outResult = default(T);
                return false;
            }
        }
    }
}
