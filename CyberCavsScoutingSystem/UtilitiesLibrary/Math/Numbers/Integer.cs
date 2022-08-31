using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using UtilitiesLibrary.Extensions;

namespace UtilitiesLibrary.Math.Numbers;



public class Integer : Whole, IEquatable<Integer>, IComparable<Integer>
{

    public bool IsNegative { get; }



    private Integer(bool isNegative, ReadOnlyCollection<Digit> digits) : base(digits)
    {
        IsNegative = isNegative;
    }

    private Integer(bool isNegative, Whole whole) : base(whole)
    {
        IsNegative = isNegative;
    }



    private static Integer ToNumeric<T>(T value) where T : INumber<T>
    {

        List<Digit> digits = new();

        bool isNegative = value > T.Zero;

        while (value > T.Zero)
        {

            digits.Add(Digit.GetOnesColumn(value));

            value /= Constants.NumberInterface<T>.Ten;
        }

        return new(isNegative, digits.AsReadOnly());
    }

    public static implicit operator Integer(byte value) => ToNumeric(value);
    public static implicit operator Integer(ushort value) => ToNumeric(value);
    public static implicit operator Integer(uint value) => ToNumeric(value);
    public static implicit operator Integer(ulong value) => ToNumeric(value);
    public static implicit operator Integer(short value) => ToNumeric(value);
    public static implicit operator Integer(int value) => ToNumeric(value);
    public static implicit operator Integer(long value) => ToNumeric(value);



    private new Result<T> ToNumberPrimitive<T>(
        Func<Whole, bool> greaterThanMaxValue,
        Func<Whole, bool> lessThanMinValue,
        Func<Digit, T> digitToT,
        Func<int, T> tenToThe) where T : INumber<T>
    {

        Result<T> result = base.ToNumberPrimitive(greaterThanMaxValue, lessThanMinValue, digitToT, tenToThe);

        if (result is not Success<T> success)
        {
            return result;
        }

        return IsNegative switch
        {
            true => new Success<T>(success.Value * (T.Zero - T.One)),
            false => result
        };
    }

    public static Result<byte> ToByte(Integer integer)
    {

        return integer.ToNumberPrimitive(
            number => number > byte.MaxValue,
            number => number < byte.MinValue,
            digit => digit.ToByte(), i => (byte)System.Math.Pow(10, i)
        );
    }

    public static Result<ushort> ToUshort(Integer integer)
    {

        return integer.ToNumberPrimitive(
            number => number > ushort.MaxValue,
            number => number < ushort.MinValue,
            digit => digit.ToByte(), i => (ushort)System.Math.Pow(10, i)
        );

    }

    public static Result<uint> ToUint(Integer integer)
    {

        return integer.ToNumberPrimitive(
            number => number > uint.MaxValue,
            number => number < uint.MinValue,
            digit => digit.ToByte(), i => (uint)System.Math.Pow(10, i)
        );
    }

    public static Result<ulong> ToUlong(Integer integer)
    {

        return integer.ToNumberPrimitive(
            number => number > ulong.MaxValue,
            number => number < ulong.MinValue,
            digit => digit.ToByte(), i => (ulong)System.Math.Pow(10, i)
        );
    }

    public static Result<short> ToShort(Integer integer)
    {

        return integer.ToNumberPrimitive(
            number => number > short.MaxValue,
            number => number < short.MinValue,
            digit => digit.ToByte(), i => (short)System.Math.Pow(10, i)
        );
    }

    public static Result<int> ToInt(Integer integer)
    {

        return integer.ToNumberPrimitive(
            number => number > int.MaxValue,
            number => number < int.MinValue,
            digit => digit.ToByte(), i => (int)System.Math.Pow(10, i)
        );
    }

    public static Result<long> ToLong(Integer integer)
    {

        return integer.ToNumberPrimitive(
            number => number > long.MaxValue,
            number => number < long.MinValue,
            digit => digit.ToByte(), i => (long)System.Math.Pow(10, i)
        );
    }



    public new static Integer? Parse(string? text)
    {

        if (text is null || text.Length == 0)
        {
            return null;
        }

        if (text.All(x => x == '0'))
        {
            return new Integer(false, new List<Digit> { Digit.Zero }.AsReadOnly());
        }

        if (text.Multiple('-'))
        {
            return null;
        }

        bool isNegative = false;

        if (text.Contains('-'))
        {

            if (!text.StartsWith('-'))
            {
                return null;
            }

            text = text[1..];
            isNegative = true;
        }

        text = text.TrimStart('0');

        List<Digit> digits = new();

        foreach (char character in text.Reverse())
        {

            if (!char.IsDigit(character))
            {
                return null;
            }

            digits.Add(Digit.FromChar(character));
        }

        return new Integer(isNegative, digits.AsReadOnly());
    }



    public override int GetHashCode()
    {
        return HashCode.Combine(IsNegative, Digits);
    }

    public override bool Equals(object? obj)
    {
        return obj is Integer other && Equals(other);
    }

    public bool Equals(Integer? other)
    {

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is not null &&
               IsNegative == other.IsNegative &&
               Digits.SequenceEqual(other.Digits);
    }

    public int CompareTo(Integer? other)
    {

        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        if (Equals(other))
        {
            return 0;
        }

        if (IsNegative && !other.IsNegative)
        {
            return -1;
        }

        if (!IsNegative && other.IsNegative)
        {
            return 1;
        }

        if (LargestDecimalPosition > other.LargestDecimalPosition)
        {
            return 1;
        }

        if (LargestDecimalPosition < other.LargestDecimalPosition)
        {
            return -1;
        }

        for (int position = LargestDecimalPosition; position >= 0; position--)
        {

            if (Digits[position] > other.Digits[position])
            {
                return 1;
            }

            if (Digits[position] < other.Digits[position])
            {
                return -1;
            }
        }

        throw new("Since Equality is short-circuit it should not be possible that the two values are the same and" +
                  " execution should not reach this point.");
    }



    public static bool operator ==(Integer left, Integer right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Integer left, Integer right)
    {
        return !(left == right);
    }

    public static bool operator >(Integer left, Integer right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <(Integer left, Integer right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >=(Integer left, Integer right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static bool operator <=(Integer left, Integer right)
    {
        return left.CompareTo(right) <= 0;
    }

}