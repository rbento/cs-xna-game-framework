// Copyright (c) Rodrigo Bento

using GameFramework.Util;

using Microsoft.Xna.Framework;

namespace GameFramework.Base
{
	/// <summary>
	/// Defines a physics component with common physics related properties.
	/// </summary>
	public class Physics : GameComponent
	{
		/// <summary>
		/// Creates an instance of <c>Physics</c>.
		/// </summary>
		/// <param name="Owner">The <c>GameObject</c> to own this instance.</param>
		public Physics(GameObject Owner) : base(Owner)
		{
			MaxForce = 0f;
			MaxSpeed = 0f;
			MaxTurnRate = 0f;
			Velocity = Vector2.Zero;
		}

		/// <summary>
		/// A unit vector pointing to a direction defined by the owner's angle.
		/// </summary>
		public Vector2 Facing
		{
			get => Maths.CreateBySizeAndAngle(1, Owner.Angle);
		}

		/// <summary>
		/// A normalized vector pointing to the direction the owner is heading.
		/// </summary>
		public Vector2 Heading
		{
			get => Vector2.Normalize(Velocity);
		}

		/// <summary>
		/// A vector perpendicular to the heading vector.
		/// </summary>
		public Vector2 Side
		{
			get => Maths.Perpendicular(Facing);
		}

		/// <summary>
		/// The owner's speed.
		/// </summary>
		public float Speed
		{
			get => Velocity.Length();
		}

		/// <summary>
		/// The owner's mass.
		/// </summary>
		public float Mass { get; set; }

		/// <summary>
		/// The maximum force that this entity can produce to power itself.
		/// </summary>
		public float MaxForce { get; set; }

		/// <summary>
		/// The maximum speed at which the owner can travel.
		/// </summary>
		public float MaxSpeed { get; set; }

		/// <summary>
		/// The maximum rate in radians per second at which the owner can rotate.
		/// </summary>
		public float MaxTurnRate { get; set; }

		/// <summary>
		/// The owner's velocity.
		/// </summary>
		public Vector2 Velocity { get; set; }
	}
}
