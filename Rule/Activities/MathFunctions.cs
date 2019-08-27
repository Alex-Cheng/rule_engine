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


namespace Autodesk.IM.Rule.Activities
{
    /// <summary>
    /// Provides math functions used in rules.
    /// </summary>
    public static class MathFunctions
    {
        static readonly ActivitySignature FunctionSignature = new ActivitySignature();


        /// <summary>
        /// Registers math functions with specified rule activity manager.
        /// </summary>
        /// <param name="activityManager">A object of RuleActivityManager used for registration of math functions.</param>
        public static void RegisterFunctions(RuleActivityManager activityManager)
        {
            RegisterFunction<Sinh>(activityManager);
            RegisterFunction<Cosh>(activityManager);
            RegisterFunction<Abs>(activityManager);
            RegisterFunction<Acos>(activityManager);
            RegisterFunction<Asin>(activityManager);
            RegisterFunction<Atan>(activityManager);
            RegisterFunction<Ceil>(activityManager);
            RegisterFunction<Cos>(activityManager);
            RegisterFunction<Exp>(activityManager);
            RegisterFunction<Floor>(activityManager);
            RegisterFunction<Ln>(activityManager);
            RegisterFunction<Sign>(activityManager);
            RegisterFunction<Sin>(activityManager);
            RegisterFunction<Sqrt>(activityManager);
            RegisterFunction<Tan>(activityManager);
            RegisterFunction<Atan2>(activityManager);
            RegisterFunction<Log>(activityManager);
            RegisterFunction<Mod>(activityManager);
            RegisterFunction<Power>(activityManager);
            RegisterFunction<Remainder>(activityManager);
            RegisterFunction<Round>(activityManager);
            RegisterFunction<Trunc>(activityManager);
        }


        private static void RegisterFunction<T>(RuleActivityManager activityManager) where T : CodeActivity<DynamicValue>, new()
        {
            Type functionType = typeof(T);
            string functionName = functionType.Name;

            object[] attrs = functionType.GetCustomAttributes(typeof(ActivityCaptionResourceAttribute), false);
            string functionDisplayName = null;
            if (attrs.Length > 0)
            {
                ActivityCaptionResourceAttribute attr = (ActivityCaptionResourceAttribute)(attrs[0]);
                functionDisplayName = Properties.Resources.ResourceManager.GetString(attr.ResourceKey);
            }

            if (functionDisplayName == null)
            {
                functionDisplayName = functionName;
            }

            activityManager.RegisterFunction(
                    functionName,
                    functionDisplayName,
                    () => new T(),
                    FunctionSignature,
                    typeof(T),
                    typeof(DynamicValue));
        }
    }


    /// <summary>
    /// Math function sinh(x).
    /// </summary>
    [ActivityCaptionResource("Sinh")] //NOXLATE
    public sealed class Sinh : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Sinh(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function cosh(x).
    /// </summary>
    [ActivityCaptionResource("Cosh")] //NOXLATE
    public sealed class Cosh : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Cosh(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function abs(x).
    /// </summary>
    [ActivityCaptionResource("Abs")] //NOXLATE
    public sealed class Abs : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Abs(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function arccos(x).
    /// </summary>
    [ActivityCaptionResource("Acos")] //NOXLATE
    public sealed class Acos : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Acos(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function arcsin(x).
    /// </summary>
    [ActivityCaptionResource("Asin")] //NOXLATE
    public sealed class Asin : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Asin(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function arctan(x).
    /// </summary>
    [ActivityCaptionResource("Atan")] //NOXLATE
    public sealed class Atan : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Atan(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function ceil(x).
    /// </summary>
    [ActivityCaptionResource("Ceil")] //NOXLATE
    public sealed class Ceil : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Ceiling(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function cos(x).
    /// </summary>
    [ActivityCaptionResource("Cos")] //NOXLATE
    public sealed class Cos : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Cos(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function exp(x).
    /// </summary>
    [ActivityCaptionResource("Exp")] //NOXLATE
    public sealed class Exp : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Exp(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function floor(x).
    /// </summary>
    [ActivityCaptionResource("Floor")] //NOXLATE
    public sealed class Floor : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Floor(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function ln(x).
    /// </summary>
    [ActivityCaptionResource("Ln")] //NOXLATE
    public sealed class Ln : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Log(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function sign(x).
    /// </summary>
    [ActivityCaptionResource("Sign")] //NOXLATE
    public sealed class Sign : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Sign(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function sin(x).
    /// </summary>
    [ActivityCaptionResource("Sin")] //NOXLATE
    public sealed class Sin : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Sin(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function sqrt(x).
    /// </summary>
    [ActivityCaptionResource("Sqrt")] //NOXLATE
    public sealed class Sqrt : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Sqrt(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function tan(x).
    /// </summary>
    [ActivityCaptionResource("Tan")] //NOXLATE
    public sealed class Tan : MathFunctionWithOneArg
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand)
        {
            return new DynamicValue(Math.Tan(operand.ToDouble(null)));
        }
    }


    /// <summary>
    /// Returns the angle whose tangent is the quotient of two specified numbers(operand1/operand2).
    /// </summary>
    [ActivityCaptionResource("Atan2")] //NOXLATE
    public sealed class Atan2 : MathFunctionWithTwoArgs
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand1, DynamicValue operand2)
        {
            return new DynamicValue(Math.Atan2(operand1.ToDouble(null), operand2.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function log(x).
    /// </summary>
    [ActivityCaptionResource("Log")] //NOXLATE
    public sealed class Log : MathFunctionWithTwoArgs
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand1, DynamicValue operand2)
        {
            return new DynamicValue(Math.Log(operand1.ToDouble(null), operand2.ToDouble(null)));
        }
    }


    /// <summary>
    /// Math function f(x, y) = x mod y.
    /// </summary>
    [ActivityCaptionResource("Mod")] //NOXLATE
    public sealed class Mod : MathFunctionWithTwoArgs
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand1, DynamicValue operand2)
        {
            return new DynamicValue(operand1.ToDouble(null) % operand2.ToDouble(null));
        }
    }


    /// <summary>
    /// Returns a specified number raised to the specified power.
    /// </summary>
    [ActivityCaptionResource("Power")] //NOXLATE
    public sealed class Power : MathFunctionWithTwoArgs
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand1, DynamicValue operand2)
        {
            return new DynamicValue(Math.Pow(operand1.ToDouble(null), operand2.ToDouble(null)));
        }
    }


    /// <summary>
    /// Returns the remainder resulting from the division of a specified number by
    /// another specified number.
    /// </summary>
    [ActivityCaptionResource("Remainder")] //NOXLATE
    public sealed class Remainder : MathFunctionWithTwoArgs
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand1, DynamicValue operand2)
        {
            // TODO: check if Remainder in FDO is identical to Math.IEEERemainder
            return new DynamicValue(Math.IEEERemainder(operand1.ToDouble(null), operand2.ToDouble(null)));
        }
    }


    /// <summary>
    /// Rounds a double-precision floating-point value to a specified number of fractional digits.
    /// </summary>
    [ActivityCaptionResource("Round")] //NOXLATE
    public sealed class Round : MathFunctionWithTwoArgs
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand1, DynamicValue operand2)
        {
            return new DynamicValue(Math.Round(operand1.ToDouble(null), operand2.ToInt32(null)));
        }
    }


    /// <summary>
    /// Truncate a number to a specified digits. For example, Truncate(1.476, 2) returns 1.47.
    /// </summary>
    [ActivityCaptionResource("Trunc")] //NOXLATE
    public sealed class Trunc : MathFunctionWithTwoArgs
    {
        protected override DynamicValue ExecuteInternal(DynamicValue operand1, DynamicValue operand2)
        {
            int digits = operand2.ToInt32(null);
            return Math.Truncate(operand1.ToDouble(null) * Math.Pow(10, digits)) / Math.Pow(10, digits);
        }
    }
}
