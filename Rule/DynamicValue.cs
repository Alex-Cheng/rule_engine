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
    /// <summary>
    /// Represents a loosely-typed value used in Rule System. It wraps an internal value and support
    /// conversion to various types in need.
    /// </summary>
    public struct DynamicValue : IConvertible, IComparable, IComparable<DynamicValue>, IEquatable<DynamicValue>
    {
        const double FloatingPointEqualTolerance = 1e-10;


        /// <summary>
        /// Determine whether Autodesk.IM.Rule.DynamicValue supports the specified type.
        /// Only types which implement IComparable interface are supported.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>true if supports the specified type; otherwise, false. </returns>
        public static bool SupportType(Type type)
        {
            // Only support type which implements IComparable interface.
            return typeof(IComparable).IsAssignableFrom(type);
        }


        /// <summary>
        /// Initializes a new instance of the Autodesk.IM.Rule.DynamicValue class with a specified value.
        /// </summary>
        /// <param name="value">The initial value</param>
        public DynamicValue(object value)
            : this()
        {
            if (value.GetType() == typeof(DynamicValue))
            {
                Value = ((DynamicValue)value).Value;
            }
            else
            {
                if (!SupportType(value.GetType()))
                {
                    throw new NotSupportedException();
                }

                Value = value;
            }
        }


        /// <summary>
        /// Initializes a new instance of the Autodesk.IM.Rule.DynamicValue class with another
        /// DynamicValue object.
        /// </summary>
        /// <param name="dv">The DynamicValue object used to initialize this object.</param>
        public DynamicValue(DynamicValue dv)
            : this()
        {
            Value = dv.Value;
        }


        /// <summary>
        ///  Set or get current value
        /// </summary>
        public object Value
        {
            get;
            set;
        }


        /// <summary>
        /// Return whether current status is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return Value != null;
            }
        }


        /// <summary>
        /// Converts a string value to the type of DynamicValue.
        /// </summary>
        /// <param name="v">The string value to convert.</param>
        /// <returns>A instance of DynamicValue wrapping the specified string value.</returns>
        public static implicit operator DynamicValue(string v)
        {
            return new DynamicValue()
            {
                Value = v
            };
        }


        /// <summary>
        /// Converts an integer to the type of DynamicValue.
        /// </summary>
        /// <param name="v">The integer to convert.</param>
        /// <returns>A instance of DynamicValue wrapping the specified integer.</returns>
        public static implicit operator DynamicValue(int v)
        {
            return new DynamicValue()
            {
                Value = v
            };
        }


        /// <summary>
        /// Converts a double value to the type of DynamicValue.
        /// </summary>
        /// <param name="v">The double value to convert.</param>
        /// <returns>A instance of DynamicValue wrapping the specified double value.</returns>
        public static implicit operator DynamicValue(double v)
        {
            return new DynamicValue()
            {
                Value = v
            };
        }


        /// <summary>
        /// Converts a boolean value to the type of DynamicValue.
        /// </summary>
        /// <param name="v">The boolean value to convert.</param>
        /// <returns>A instance of DynamicValue wrapping the specified boolean value.</returns>
        public static implicit operator DynamicValue(bool v)
        {
            return new DynamicValue()
            {
                Value = v
            };
        }


        /// <summary>
        /// Determines whether the specified left and right values equal.
        /// </summary>
        /// <param name="left">The specified left operand.</param>
        /// <param name="right">The specified right operand.</param>
        /// <returns>true if they are equal; otherwise, false.</returns>
        public static bool operator ==(DynamicValue left, DynamicValue right)
        {
            return left.Equals(right);
        }


        /// <summary>
        /// Determines whether the specified left and right values are not equal.
        /// </summary>
        /// <param name="left">The specified left operand.</param>
        /// <param name="right">The specified right operand.</param>
        /// <returns>true if they are not equal; otherwise, false.</returns>
        public static bool operator !=(DynamicValue left, DynamicValue right)
        {
            return !left.Equals(right);
        }


        /// <summary>
        /// Determines whether the specified left operand is greater than right operand.
        /// </summary>
        /// <param name="left">The specified left operand.</param>
        /// <param name="right">The specified right operand.</param>
        /// <returns>true if the left is greater than the right; otherwise, false.</returns>
        public static bool operator >(DynamicValue left, DynamicValue right)
        {
            try
            {
                return left.CompareTo(right) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Determines whether the specified left operand is less than right operand.
        /// </summary>
        /// <param name="left">The specified left operand.</param>
        /// <param name="right">The specified right operand.</param>
        /// <returns>true if the left is less than the right; otherwise, false.</returns>
        public static bool operator <(DynamicValue left, DynamicValue right)
        {
            try
            {
                return left.CompareTo(right) < 0;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Determines whether the specified left operand is greater than or equal to right operand.
        /// </summary>
        /// <param name="left">The specified left operand.</param>
        /// <param name="right">The specified right operand.</param>
        /// <returns>true if the left is greater than or equal to the right; otherwise, false.</returns>
        public static bool operator >=(DynamicValue left, DynamicValue right)
        {
            return left > right || left.Equals(right);
        }


        /// <summary>
        /// Determines whether the specified left operand is less than or equal to right operand.
        /// </summary>
        /// <param name="left">The specified left operand.</param>
        /// <param name="right">The specified right operand.</param>
        /// <returns>true if the left is less than or equal to the right; otherwise, false.</returns>
        public static bool operator <=(DynamicValue left, DynamicValue right)
        {
            return left < right || left.Equals(right);
        }


        /// <summary>
        /// Adds the left operand to the right operand.
        /// </summary>
        /// <param name="left">The specified left operand.</param>
        /// <param name="right">The specified right operand.</param>
        /// <returns>The adding result.</returns>
        public static DynamicValue operator +(DynamicValue left, DynamicValue right)
        {
            if (!left.IsValid || !right.IsValid)
                return new DynamicValue();

            try
            {
                double leftOp = Convert.ToDouble(left);
                double rightOp = Convert.ToDouble(right);
                return new DynamicValue(leftOp + rightOp);
            }
            catch (Exception)
            {
                // Try behave as same as String.Concat()
                return new DynamicValue(left.ToString() + right.ToString());
            }
        }


        /// <summary>
        /// Subtracts right operand from the left operand.
        /// </summary>
        /// <param name="left">The specified left operand.</param>
        /// <param name="right">The specified right operand.</param>
        /// <returns>The result of subtraction operation.</returns>
        public static DynamicValue operator -(DynamicValue left, DynamicValue right)
        {
            if (!left.IsValid || !right.IsValid)
                return new DynamicValue();

            try
            {
                double leftOp = Convert.ToDouble(left);
                double rightOp = Convert.ToDouble(right);
                return new DynamicValue(leftOp - rightOp);
            }
            catch (Exception)
            {
                return new DynamicValue();
            }
        }


        /// <summary>
        /// Multiplies the left operand with the right operand.
        /// </summary>
        /// <param name="left">The specified left operand.</param>
        /// <param name="right">The specified right operand.</param>
        /// <returns>The result of multiplication operation.</returns>
        public static DynamicValue operator *(DynamicValue left, DynamicValue right)
        {
            if (!left.IsValid || !right.IsValid)
                return new DynamicValue();

            try
            {
                double leftOp = Convert.ToDouble(left);
                double rightOp = Convert.ToDouble(right);
                return new DynamicValue(leftOp * rightOp);
            }
            catch (Exception)
            {
                return new DynamicValue();
            }
        }


        /// <summary>
        /// Divides the left operand by the right operand.
        /// </summary>
        /// <param name="left">The specified left operand.</param>
        /// <param name="right">The specified right operand.</param>
        /// <returns>The result of division operation.</returns>
        public static DynamicValue operator /(DynamicValue left, DynamicValue right)
        {
            if (!left.IsValid || !right.IsValid)
                return new DynamicValue();

            try
            {
                double leftOp = Convert.ToDouble(left);
                double rightOp = Convert.ToDouble(right);
                return new DynamicValue(leftOp / rightOp);
            }
            catch (Exception)
            {
                return new DynamicValue();
            }
        }


        public override bool Equals(object obj)
        {
            // Not support comparison with non-DynamicValue type.
            if (!(obj is DynamicValue))
                return false;

            DynamicValue right = (DynamicValue)obj;
            return Equals(right);
        }


        public override string ToString()
        {
            if (IsValid)
                return Value.ToString();
            else
                return String.Empty;
        }


        public override int GetHashCode()
        {
            if (IsValid)
                return Value.GetHashCode();
            else
                return -1;
        }


        #region IEquatable<DynamicValue> Members

        public bool Equals(DynamicValue other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            else if(Object.ReferenceEquals(this, other))
            {
                return true;
            }
            else if (this.Value == null)
            {
                return other.Value == null;
            }

            try
            {
                return ((IComparable)this).CompareTo(other) == 0;
            }
            catch (Exception)
            {
                // try to compare as strings
                return this.ToString().Equals(other.ToString());
            }
        }

        #endregion


        #region IConvertible Members

        public TypeCode GetTypeCode()
        {
            if (this.Value is IConvertible)
                return (this.Value as IConvertible).GetTypeCode();
            return TypeCode.Object;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            if (!IsValid)
                return default(bool);
            return Convert.ToBoolean(Value);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(Value);
        }

        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(Value);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(Value);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(Value);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(Value);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(Value);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(Value);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(Value);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(Value);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(Value);
        }

        public string ToString(IFormatProvider provider)
        {
            return Convert.ToString(Value);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(Value, conversionType);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(Value);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(Value);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(Value);
        }

        #endregion


        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
            {
                // Always be greater than null value, except this.Value is also null.
                return this.Value == null ? 0 : 1;
            }
            else if( this.Value == null)
            {
                return -1;
            }

            if (obj is DynamicValue)
            {
                DynamicValue other = (DynamicValue)obj;
                return CompareTo(other);
            }
            else
            {
                if (this.Value is bool)
                {
                    if (obj is bool)
                    {
                        return ((bool)this.Value).CompareTo(obj);
                    }
                    else
                    {
                        bool boolValue = Convert.ToBoolean(obj);
                        return ((bool)this.Value).CompareTo(boolValue);
                    }
                }

                if (this.Value is DateTime)
                {
                    if (obj is DateTime)
                    {
                        return ((DateTime)this.Value).CompareTo(obj);
                    }
                    else
                    {
                        DateTime dateTimeValue = Convert.ToDateTime(obj);
                        return ((DateTime)this.Value).CompareTo(dateTimeValue);
                    }
                }

                int result;
                if (TryCompareToAsNumeric(obj, out result))
                {
                    return result;
                }
                else
                {
                    // General comparison.
                    return ((IComparable)this.Value).CompareTo(obj);
                }
            }
        }

        #endregion


        #region IComparable<T> Members

        public int CompareTo(DynamicValue other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                // always be greater than null value, except this.Value is also null.
                return this.Value == null ? 0 : 1;
            }

            if (this.Value != null)
            {
                return ((IComparable)this).CompareTo(other.Value);
            }
            else
            {
                // If the counterpart also has null value, they are equal, otherwise
                // return -1(less than).
                return other.Value == null ? 0 : -1;
            }
        }

        #endregion


        #region Comparison helper

        /// <summary>
        /// This method implements an comparison with tolerance.
        /// </summary>
        private bool TryCompareToAsNumeric(object obj, out int result)
        {
            // Comparison with tolerance.
            try
            {
                double left = Convert.ToDouble(this.Value);
                double right = Convert.ToDouble(obj);
                if (Math.Abs(left - right) < FloatingPointEqualTolerance)
                {
                    result = 0;
                }
                else
                {
                    result = left.CompareTo(right);
                }
                return true;
            }
            catch
            {
                // Ignore any exception.
                result = 0;
                return false;
            }
        }

        #endregion
    }
}
