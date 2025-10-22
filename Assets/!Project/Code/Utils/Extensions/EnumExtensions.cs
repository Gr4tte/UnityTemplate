namespace UnityTemplate
{
	public static class EnumExtensions
	{
		/// <summary>
		/// Returns the next value in the enum sequence. Wraps to the first value if at the end.
		/// </summary>
		/// <typeparam name="T">The enum type.</typeparam>
		/// <param name="src">The current enum value.</param>
		/// <returns>The next enum value, or the first value if at the end.</returns>
		public static T Next<T>(this T src) where T : struct, System.Enum
		{
			var values = (T[])System.Enum.GetValues(typeof(T));
			int index = System.Array.IndexOf(values, src) + 1;
			return (index == values.Length) ? values[0] : values[index];
		}

		/// <summary>
		/// Returns the previous value in the enum sequence. Wraps to the last value if at the start.
		/// </summary>
		/// <typeparam name="T">The enum type.</typeparam>
		/// <param name="src">The current enum value.</param>
		/// <returns>The previous enum value, or the last value if at the start.</returns>
		public static T Previous<T>(this T src) where T : struct, System.Enum
		{
			var values = (T[])System.Enum.GetValues(typeof(T));
			int index = System.Array.IndexOf(values, src) - 1;
			return (index < 0) ? values[^1] : values[index];
		}
	}
}