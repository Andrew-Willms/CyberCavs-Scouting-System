using System;

namespace WPFUtilities.Math;



// I created this class while experimenting with ASP.NET Web APIs but thought it might be useful so I added it to this solution.
// I really need a more robust way of sharing code I frequently use between solutions.
public class Range<T> where T : IComparable {

	public T LowerBound { get; }
	public T UpperBound { get; }

	public bool IncludesLowerBound { get; }
	public bool IncludesUpperBound { get; }

	public Range(T lowerBound, T upperBound, bool includesLowerBound, bool includesUpperBound) {

		if (lowerBound.CompareTo(upperBound) > 0) {
			throw new ArgumentException($"The parameter \"{nameof(lowerBound)}\" cannot be greater than the parameter \"{nameof(upperBound)}\"");
		}

		LowerBound = lowerBound;
		UpperBound = upperBound;
		IncludesLowerBound = includesLowerBound;
		IncludesUpperBound = includesUpperBound;
	}



	public bool Contains(T value) {

		bool withinLowerBound;
		bool withinUpperBound;

		if (IncludesLowerBound) {
			withinLowerBound = LowerBound.CompareTo(value) <= 0;
		} else {
			withinLowerBound = LowerBound.CompareTo(value) < 0;
		}

		if (IncludesUpperBound) {
			withinUpperBound = UpperBound.CompareTo(value) >= 0;
		} else {
			withinUpperBound = UpperBound.CompareTo(value) < 0;
		}

		return withinLowerBound && withinUpperBound;
	}



	public override int GetHashCode() {
		return HashCode.Combine(LowerBound, UpperBound, IncludesLowerBound, IncludesUpperBound);
	}

	public virtual bool Equals(Range<T>? other) {

		if (ReferenceEquals(this, other)) {
			return true;
		}

		return other is not null && Equals(other);
	}

	public override bool Equals(object? obj) {
		return (obj is Range<T> other) && Equals(other);
	}

	public static bool operator ==(Range<T>? left, Range<T>? right) {

		if (left is null) {
			return right is null;
		}

		return left.Equals(right);
	}

	public static bool operator !=(Range<T>? left, Range<T>? right) {
		return !(left == right);
	}
	
}