using System.Numerics;

namespace WPFUtilities.Math; 



public static class Constants {

	public static class NumberInterface<T> where T : INumber<T> {

		public static readonly T Two = T.One + T.One;
		public static readonly T Three = Two + T.One;
		public static readonly T Four = Three + T.One;
		public static readonly T Five = Four + T.One;
		public static readonly T Six = Five + T.One;
		public static readonly T Seven = Six + T.One;
		public static readonly T Eight = Seven + T.One;
		public static readonly T Nine = Eight + T.One;
		public static readonly T Ten = Nine + T.One;
	}

}