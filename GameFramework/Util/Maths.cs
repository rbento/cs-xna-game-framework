// Copyright (c) Rodrigo Bento

using Microsoft.Xna.Framework;

using System;

namespace GameFramework.Util
{
	/// <summary>
	/// Defines rotation orientation.
	/// </summary>
	public enum Orientation
	{
		Clockwise = 1,
		AntiClockwise = -1
	}

	/// <summary>
	/// A helper for common math operations.
	/// </summary>
	public sealed class Maths
	{
		/// <summary>
		/// A constant value representing one degree.
		/// </summary>
		private const double kOneDgree = Math.PI / 180d;

		/// <summary>
		/// No instances.
		/// </summary>
		private Maths()
		{
		}

		/// <summary>
		/// Creates a vector with the specified magnitude and direction.
		/// </summary>
		/// <param name="Size">The desired vector magnitude.</param>
		/// <param name="Angle">The desired vector direction.</param>
		/// <returns>A vector accrding to the specified magniture and direction.</returns>
		public static Vector2 CreateBySizeAndAngle(float Size, float Angle)
		{
			return new Vector2((float)Math.Cos(Angle) * Size, (float)Math.Sin(Angle) * Size);
		}

		/// <summary>
		/// Truncates a vector to the specified length.
		/// </summary>
		/// <param name="Vector">The vector to be truncated.</param>
		/// <param name="MaxLength">The desired vector length.</param>
		public static void Truncate(ref Vector2 Vector, float MaxLength)
		{
			if (MaxLength >= 0 && Vector.Length() > MaxLength)
			{
				Vector.Normalize();
				Vector *= MaxLength;
			}
		}

		/// <summary>
		/// Gets the orientation of a given A vector in relation to a B vector.
		/// </summary>
		/// <param name="A">The source vector.</param>
		/// <param name="B">The comparison vector.</param>
		/// <returns><c>-1</c> for AntiClockwise, <c>1</c> for Clockwise.</returns>
		public static int Sign(Vector2 A, Vector2 B)
		{
			return (A.Y * B.X > A.X * B.Y) ? -1 : 1;
		}

		/// <summary>
		/// Gets a vector perpendicular to the given vector.
		/// </summary>
		/// <param name="Vector">The source vector.</param>
		/// <returns>A vector perpendicular to the diven vector.</returns>
		public static Vector2 Perpendicular(Vector2 Vector)
		{
			return new Vector2(-Vector.Y, Vector.X);
		}

		/// <summary>
		/// Gets the angle in radians from a given vector.
		/// </summary>
		/// <param name="Vector">The source vector.</param>
		/// <returns>The angle in radians.</returns>
		public static float AngleInRadians(Vector2 Vector)
		{
			return (float)Math.Atan2(Vector.Y, Vector.X);
		}

		/// <summary>
		/// Wraps a vector representing a position, at a specific edge, when a minimum or maximum boundary is crossed.
		/// </summary>
		/// <param name="Position">The vector representing a position.</param>
		/// <param name="EdgeX">The maximum edge boundary for the X component.</param>
		/// <param name="EdgeY">The maximum edge boundary for the Y component.</param>
		public static void WrapAroundEdge(ref Vector2 Position, int EdgeX, int EdgeY)
		{
			if (Position.X > EdgeX) { Position.X = 0.0f; }
			if (Position.X < 0) { Position.X = EdgeX; }
			if (Position.Y < 0) { Position.Y = EdgeY; }
			if (Position.Y > EdgeY) { Position.Y = 0.0f; }
		}

		/// <summary>
		/// Wraps a value when it cross the minimum or maximum boundaries.
		/// </summary>
		/// <param name="Value">The value to be wrapped.</param>
		/// <param name="Min">The minimum desired value.</param>
		/// <param name="Max">The maximum desired value.</param>
		/// <returns>The wrapped value.</returns>
		public static int Wrap(int Value, int Min, int Max)
		{
			return (Value < Min) ? Max : (Value > Max) ? Min : Value;
		}

		/// <summary>
		/// Wraps a value when it cross the minimum or maximum boundaries.
		/// </summary>
		/// <param name="Value">The value to be wrapped.</param>
		/// <param name="Min">The minimum desired value.</param>
		/// <param name="Max">The maximum desired value.</param>
		/// <returns>The wrapped value.</returns>
		public static float Wrap(float Value, float Min, float Max)
		{
			return (Value < Min) ? Max : (Value > Max) ? Min : Value;
		}

		/// <summary>
		/// Clamps a value when it cross the minimum or maximum boundaries.
		/// </summary>
		/// <param name="Value">The value to be clamped.</param>
		/// <param name="Min">The minimum desired value.</param>
		/// <param name="Max">The maximum desired value.</param>
		/// <returns>The clamped value.</returns>
		public static int Clamp(int Value, int Min, int Max)
		{
			return (Value < Min) ? Min : (Value > Max) ? Max : Value;
		}

		/// <summary>
		/// Clamps a value when it cross the minimum or maximum boundaries.
		/// </summary>
		/// <param name="Value">The value to be clamped.</param>
		/// <param name="Min">The minimum desired value.</param>
		/// <param name="Max">The maximum desired value.</param>
		/// <returns>The clamped value.</returns>
		public static float Clamp(float Value, float Min, float Max)
		{
			return (Value < Min) ? Min : (Value > Max) ? Max : Value;
		}

		/// <summary>
		/// Converts an angle in radians to degrees.
		/// </summary>
		/// <param name="AngleInRadians">The angle in radians to be converted.</param>
		/// <returns>The angle in degrees.</returns>
		public static double ToDegrees(double AngleInRadians)
		{
			return AngleInRadians * kOneDgree;
		}

		/// <summary>
		/// Converts an angle in radians to degrees.
		/// </summary>
		/// <param name="AngleInRadians">The angle in radians to be converted.</param>
		/// <returns>The angle in degrees.</returns>
		public static float ToDegrees(float AngleInRadians)
		{
			return (float)(AngleInRadians * kOneDgree);
		}

		/// <summary>
		/// Converts an angle in radians to degrees.
		/// </summary>
		/// <param name="AngleInRadians">The angle in radians to be converted.</param>
		/// <returns>The angle in degrees.</returns>
		public static float ToDegrees(int AngleInRadians)
		{
			return ToDegrees((float)AngleInRadians);
		}

		/// <summary>
		/// Converts an angle in degrees to radians.
		/// </summary>
		/// <param name="AngleInDegrees">The angle in degrees to be converted.</param>
		/// <returns>The angle in radians.</returns>
		public static double ToRadians(double AngleInDegrees)
		{
			return AngleInDegrees / kOneDgree;
		}

		/// <summary>
		/// Converts an angle in degrees to radians.
		/// </summary>
		/// <param name="AngleInDegrees">The angle in degrees to be converted.</param>
		/// <returns>The angle in radians.</returns>
		public static float ToRadians(float AngleInDegrees)
		{
			return (float)(AngleInDegrees / kOneDgree);
		}

		/// <summary>
		/// Converts an angle in degrees to radians.
		/// </summary>
		/// <param name="AngleInDegrees">The angle in degrees to be converted.</param>
		/// <returns>The angle in radians.</returns>
		public static float ToRadians(int AngleInDegrees)
		{
			return ToRadians((float)AngleInDegrees);
		}
	}
}
