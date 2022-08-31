using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UtilitiesLibrary.Math.Numbers;



public readonly struct Number : IEquatable<Number>, IComparable<Number>
{

    public bool IsNegative { get; }

    private int DecimalPosition { get; }

    private ReadOnlyCollection<Digit> Digits { get; }

    public bool IsInteger => DecimalPosition == 0;

    private int LargestDecimalPosition => Digits.Count - DecimalPosition;
    private int SmallestDecimalPosition => -DecimalPosition;



    private Number(bool isNegative, int decimalPosition, ReadOnlyCollection<Digit> digits)
    {

        IsNegative = isNegative;
        DecimalPosition = decimalPosition;
        Digits = digits;
    }


    public override string ToString()
    {
        throw new NotImplementedException();
    }


    private Digit GetDigitInPosition(int position)
    {
        return Digits[position + DecimalPosition];
    }



    public static implicit operator Number(byte value)
    {

        List<Digit> digits = new();

        while (value > 0)
        {
            digits.Add(Digit.GetOnesColumn(value));
            value /= 10;
        }

        return new(false, 0, digits.AsReadOnly());
    }

    public static implicit operator Number(ushort value)
    {

        List<Digit> digits = new();

        while (value > 0)
        {
            digits.Add(Digit.GetOnesColumn(value));
            value /= 10;
        }

        return new(false, 0, digits.AsReadOnly());
    }

    public static implicit operator Number(uint value)
    {

        List<Digit> digits = new();

        while (value > 0)
        {
            digits.Add(Digit.GetOnesColumn(value));
            value /= 10;
        }

        return new(false, 0, digits.AsReadOnly());
    }

    public static implicit operator Number(ulong value)
    {

        List<Digit> digits = new();

        while (value > 0)
        {
            digits.Add(Digit.GetOnesColumn(value));
            value /= 10;
        }

        return new(false, 0, digits.AsReadOnly());
    }

    public static implicit operator Number(short value)
    {

        bool isNegative = value < 0;

        List<Digit> digits = new();

        while (value > 0)
        {
            digits.Add(Digit.GetOnesColumn(value));
            value /= 10;
        }

        return new(isNegative, 0, digits.AsReadOnly());
    }

    public static implicit operator Number(int value)
    {

        bool isNegative = value < 0;

        List<Digit> digits = new();

        while (value > 0)
        {
            digits.Add(Digit.GetOnesColumn(value));
            value /= 10;
        }

        return new(isNegative, 0, digits.AsReadOnly());
    }

    public static implicit operator Number(long value)
    {

        bool isNegative = value < 0;

        List<Digit> digits = new();

        while (value > 0)
        {
            digits.Add(Digit.GetOnesColumn(value));
            value /= 10;
        }

        return new(isNegative, 0, digits.AsReadOnly());
    }

    public static implicit operator Number(float value)
    {

        bool isNegative = value < 0;

        int placesRightOfDecimalPoint = 0;
        while (System.Math.Abs(value % 1 - value) > 1e-10)
        {

            placesRightOfDecimalPoint++;
            value *= 10;
        }

        List<Digit> digits = new();

        while (value > 0)
        {
            digits.Add(Digit.GetOnesColumn(value));
            value /= 10;
        }

        return new(isNegative, placesRightOfDecimalPoint, digits.AsReadOnly());
    }

    public static implicit operator Number(double value)
    {

        bool isNegative = value < 0;

        int placesRightOfDecimalPoint = 0;
        while (System.Math.Abs(value % 1 - value) > 1e-10)
        {

            placesRightOfDecimalPoint++;
            value *= 10;
        }

        List<Digit> digits = new();

        while (value > 0)
        {
            digits.Add(Digit.GetOnesColumn(value));
            value /= 10;
        }

        return new(isNegative, placesRightOfDecimalPoint, digits.AsReadOnly());
    }

    public static implicit operator Number(decimal value)
    {

        bool isNegative = value < 0;

        int placesRightOfDecimalPoint = 0;
        while (System.Math.Abs(value % 1 - value) > 1e-10M)
        {

            placesRightOfDecimalPoint++;
            value *= 10;
        }

        List<Digit> digits = new();

        while (value > 0)
        {
            digits.Add(Digit.GetOnesColumn(value));
            value /= 10;
        }

        return new(isNegative, placesRightOfDecimalPoint, digits.AsReadOnly());
    }



    public static Result<byte> ToByte(Number number)
    {

        if (number > byte.MaxValue)
        {
            return new Failure<byte>(new ValueTooLargeError());
        }

        if (number < byte.MinValue)
        {
            return new Failure<byte>(new ValueTooSmallError());
        }

        if (!number.IsInteger)
        {
            return new Failure<byte>(new ValueIsNotWholeNumberError());
        }

        byte value = 0;
        for (int position = 0; position <= number.LargestDecimalPosition; position++)
        {
            value += (byte)(number.GetDigitInPosition(position).ToByte() * (byte)System.Math.Pow(10, position));
        }

        return new Success<byte>(value);
    }

    public static Result<ushort> ToUshort(Number number)
    {
        throw new NotImplementedException();
    }

    public static Result<uint> ToUint(Number number)
    {
        throw new NotImplementedException();
    }

    public static Result<ulong> ToUlong(Number number)
    {
        throw new NotImplementedException();
    }

    public static Result<short> ToShort(Number number)
    {
        throw new NotImplementedException();
    }

    public static Result<int> ToInt(Number number)
    {
        throw new NotImplementedException();
    }

    public static Result<long> ToLong(Number number)
    {
        throw new NotImplementedException();
    }

    public static Result<float> ToFloat(Number number)
    {
        throw new NotImplementedException();
    }

    public static Result<double> ToDouble(Number number)
    {
        throw new NotImplementedException();
    }



    public static Number? Parse(string? text)
    {
        throw new NotImplementedException();
    }



    public override int GetHashCode()
    {
        return HashCode.Combine(IsNegative, DecimalPosition, Digits);
    }

    public override bool Equals(object? obj)
    {
        return obj is Number other && Equals(other);
    }

    public bool Equals(Number other)
    {

        return IsNegative == other.IsNegative &&
               DecimalPosition == other.DecimalPosition &&
               Digits == other.Digits;
    }

    public int CompareTo(Number other)
    {

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

        int smallestDecimalPosition = System.Math.Min(SmallestDecimalPosition, other.SmallestDecimalPosition);
        for (int position = LargestDecimalPosition; position > smallestDecimalPosition; position--)
        {

            if (GetDigitInPosition(position) > other.GetDigitInPosition(position))
            {
                return 1;
            }

            if (GetDigitInPosition(position) < other.GetDigitInPosition(position))
            {
                return -1;
            }
        }

        throw new("Since Equality is short-circuit it should not be possible that the two values are the same and" +
                  " execution should not reach this point.");
    }



    public static bool operator ==(Number left, Number right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Number left, Number right)
    {
        return !(left == right);
    }

    public static bool operator >(Number left, Number right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <(Number left, Number right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >=(Number left, Number right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static bool operator <=(Number left, Number right)
    {
        return left.CompareTo(right) <= 0;
    }

    // Todo: implement operators between this and the number types
}