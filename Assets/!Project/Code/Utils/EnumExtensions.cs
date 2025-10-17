namespace UnityTemplate
{
	public static class EnumExtensions
	{
		public static T Next<T>(this T src) where T : struct, System.Enum
		{
			var values = (T[])System.Enum.GetValues(typeof(T));
			int index = System.Array.IndexOf(values, src) + 1;
			return (index == values.Length) ? values[0] : values[index];
		}

		public static T Previous<T>(this T src) where T : struct, System.Enum
		{
			var values = (T[])System.Enum.GetValues(typeof(T));
			int index = System.Array.IndexOf(values, src) - 1;
			return (index < 0) ? values[^1] : values[index];
		}
	}
}