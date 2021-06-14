// Copyright (c) Rodrigo Bento

using System;

namespace GameFramework.Util
{
	/// <summary>
	/// A helper for common random number operations.
	/// </summary>
	public sealed class Randoms
	{
		/// <summary>
		/// No instances.
		/// </summary>
		private Randoms()
		{
		}

		/// <summary>
		/// Unique <c>RandomGenerator</c> instance.
		/// </summary>
		private static readonly Random RandomGenerator = new Random(Guid.NewGuid().GetHashCode());

		/// <summary>
		/// Generates a random non-inclusive integer.
		/// </summary>
		/// <remarks>
		/// Useful for generating random container indices.
		/// </remarks>
		/// <param name="Min">The minimum desired value.</param>
		/// <param name="Max">The maximum desired value.</param>
		/// <param name="bInclusive">Whether the max value is inclusive.</param>
		/// <returns>A random integer according to the specified range.</returns>
		public static int NextInt(int Min = int.MinValue, int Max = int.MaxValue, bool bInclusive = false)
		{
			if (bInclusive)
			{
				Max = (Max < int.MaxValue) ? Max + 1 : Max;
			}
			return RandomGenerator.Next(Min, Max);
		}

		/// <summary>
		/// Generates a random inclusive integer.
		/// </summary>
		/// <param name="Min">The minimum desired value.</param>
		/// <param name="Max">The maximum desired value.</param>
		/// <returns>A random inclusive integer according to the specified range.</returns>
		public static int NextIntInclusive(int Min = int.MinValue, int Max = int.MaxValue)
		{
			return NextInt(Min, Max, true);
		}

		/// <summary>
		/// Generates a random float.
		/// </summary>
		/// <param name="Min">The minimum desired value.</param>
		/// <param name="Max">The maximum desired value.</param>
		/// <returns>A random float according to the specified range.</returns>
		public static float NextFloat(float Min = float.MinValue, float Max = float.MaxValue)
		{
			double Range = Min - Max;
			double Sample = RandomGenerator.NextDouble();
			double Scaled = (Sample * Range) + Min;
			return (float)(Scaled < 0.0f && Min >= 0.0f ? Scaled * -1 : Scaled);
		}
	}
}
