// Copyright (c) Rodrigo Bento

using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Base
{
	/// <summary>
	/// Defines an Axis-Aligned Bounding Box collision component.
	/// </summary>
	public class AABBCollision : GameComponent
	{
		/// <summary>
		/// Creates an instance of <c>AABBCollision</c>.
		/// </summary>
		/// <param name="Owner">The <c>GameObject</c> to own this instance.</param>
		public AABBCollision(GameObject Owner) : base(Owner)
		{
		}

		/// <summary>
		/// Tells whether this <c>GameObject</c> collides with another.
		/// </summary>
		/// <param name="Other">The <c>GameObject</c> instance to check.</param>
		/// <returns><c>true</c> if there is a collision, otherwise <c>false</c>.</returns>
		public bool CollidesWith(GameObject Other)
		{
			if (!IsEnabled || Other == null)
			{
				return false;
			}

			/// First determines the axes of both parties for later verification. 

			List<Vector2> SourceRectAxis = new List<Vector2>
			{
				Owner.UpperRightCorner - Owner.UpperLeftCorner,
				Owner.UpperRightCorner - Owner.LowerRightCorner,

				Other.UpperLeftCorner - Other.LowerLeftCorner,
				Other.UpperLeftCorner - Other.UpperRightCorner
			};

			/// A game object collision will be detected only if all axes collide.
			/// This is true according to the Seperating Axis Theorem.

			foreach (Vector2 Axis in SourceRectAxis)
			{
				if (!IsAxisCollision(Other, Axis))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Determines whether a collision has occurred on an axis in one of the planes parallel to a game object's rectangle.
		/// </summary>
		/// <param name="Other">The <c>GameObject</c> instance to evaluate.</param>
		/// <param name="Axis">The axis vector to evaluate.</param>
		/// <returns><c>true</c> if a collision is detected, otherwise <c>false</c>.</returns>
		private bool IsAxisCollision(GameObject Other, Vector2 Axis)
		{
			/// Corners of the rectangle are projected onto the given axis.
			/// This results in a scalar that later will be used to detect the collision.

			List<int> RectScalars = new List<int>
			{
				GenerateScalar(Owner.UpperLeftCorner, Axis),
				GenerateScalar(Owner.UpperRightCorner, Axis),
				GenerateScalar(Owner.LowerLeftCorner, Axis),
				GenerateScalar(Owner.LowerRightCorner, Axis)
			};

			List<int> OtherRectScalars = new List<int>
			{
				GenerateScalar(Other.UpperLeftCorner, Axis),
				GenerateScalar(Other.UpperRightCorner, Axis),
				GenerateScalar(Other.LowerLeftCorner, Axis),
				GenerateScalar(Other.LowerRightCorner, Axis)
			};

			/// Determine the min/max of both rectangles. 

			int RectMin = RectScalars.Min();
			int RectMax = RectScalars.Max();

			int OtherRectMin = OtherRectScalars.Min();
			int OtherRectMax = OtherRectScalars.Max();

			/// Collision will be detected if rectangles overlap.
			/// Ex: RectMin is less than otherRectMax, etc...

			return (RectMin <= OtherRectMax && RectMax >= OtherRectMax) || (OtherRectMin <= RectMax && OtherRectMax >= RectMax);
		}

		/// <summary>
		/// Generates a scalar by projecting a rectangle's corner into an axis.
		/// </summary>
		/// <param name="RectangleCorner">The rectangle corner vector to project.</param>
		/// <param name="Axis">The axis vector to process.</param>
		/// <returns>An scalar of type <c>int</c>.</returns>
		private int GenerateScalar(Vector2 RectangleCorner, Vector2 Axis)
		{
			/// Applies the vector projection formula by taking the corner of a rectangle and projecting it onto the given axis.

			float Numerator = (RectangleCorner.X * Axis.X) + (RectangleCorner.Y * Axis.Y);
			float Denominator = (Axis.X * Axis.X) + (Axis.Y * Axis.Y);
			float Quotient = Numerator / Denominator;

			Vector2 ProjectedCorner = new Vector2(Quotient * Axis.X, Quotient * Axis.Y);

			/// Convert the resulting vector into a truncated scalar.

			return (int)((Axis.X * ProjectedCorner.X) + (Axis.Y * ProjectedCorner.Y));
		}

		/// <summary>
		/// Whether this component is enabled.
		/// </summary>
		/// <remarks>
		/// <c>false</c> indicates collision detection is disabled.
		/// </remarks>
		public bool IsEnabled { get; set; } = true;
	}
}
