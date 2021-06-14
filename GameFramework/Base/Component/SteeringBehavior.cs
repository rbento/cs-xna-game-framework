// Copyright (c) Rodrigo Bento

using Microsoft.Xna.Framework;

using System;

namespace GameFramework.Base
{
	/// <summary>
	/// Defines a Steering Behaviors component.
	/// </summary>
	/// <remarks>
	/// Implementation is based on the book 'Programming Game AI by Example', by Mat Buckland.
	/// </remarks>
	public class SteeringBehavior : GameComponent
	{
		/// <summary>
		/// The <c>Physics</c> component as required by the Steering Behaviors implementation.
		/// </summary>
		private readonly Physics mPhysics;

		/// <summary>
		/// Creates an instance of <c>SteeringBehavior</c>.
		/// </summary>
		/// <param name="Owner">The <c>GameObject</c> to own this instance.</param>
		public SteeringBehavior(GameObject Owner) : base(Owner)
		{
			/// Handle mandatory dependency on Physics component.

			if (!Owner.HasComponent<Physics>())
			{
				Owner.AddComponent<Physics>(new Physics(Owner));
			}

			mPhysics = Owner.GetComponent<Physics>();
		}

		/// <summary>
		/// Adjusts the angle according to the heading vector.
		/// </summary>
		public void AutoAlignHeading()
		{
			Owner.Angle = (float)Math.Atan2(mPhysics.Heading.Y, mPhysics.Heading.X);
		}

		/// <summary>
		/// Returns a steering force that directs an agent towards a target position.
		/// </summary>
		/// <param name="TargetPosition">The target position to seek.</param>
		/// <returns>A vector representing the steering force required to reach the target position.</returns>
		public Vector2 Seek(Vector2 TargetPosition)
		{
			Vector2 Displacement = TargetPosition - Owner.Position;
			float Distance = Displacement.Length();

			if (Distance < 1f)
			{
				return Vector2.Zero;
			}

			AutoAlignHeading();

			/// The velocity that the agent would need in order to reach the target position, scaled to its maximum speed.

			Vector2 DesiredVelocity = Vector2.Normalize(Displacement) * mPhysics.MaxSpeed;

			return DesiredVelocity - mPhysics.Velocity;
		}

		/// <summary>
		/// Returns a steering force that directs an agent away from a target position when their distance
		/// is closer than a given panic distance.
		/// </summary>
		/// <param name="TargetPosition">The target position to flee from.</param>
		/// <param name="PanicDistance">The distance that will determine when the calculation should happen.</param>
		/// <returns>A vector representing a steering force that leads away from the target position.</returns>
		public Vector2 Flee(Vector2 TargetPosition, float PanicDistance = 0f)
		{
			Vector2 Displacement = Owner.Position - TargetPosition;
			float Distance = Displacement.Length();

			if (PanicDistance > 0f && Distance >= PanicDistance)
			{
				return Vector2.Zero;
			}

			AutoAlignHeading();

			Vector2 DesiredVelocity = Vector2.Normalize(Displacement) * mPhysics.MaxSpeed;

			return DesiredVelocity - mPhysics.Velocity;
		}

		/// <summary>
		/// Returns a steering force that directs an agent towards a target position, decelerating as it is approached.
		/// </summary>
		/// <param name="TargetPosition">The target position to arrive to.</param>
		/// <param name="DecelerationFactor">The deceleration factor, the lower the faster.</param>
		/// <param name="MinDistanceFromTarget">The minimum distance to the target used to consider whether the agent has arrived.</param>
		/// <returns>A vector representing the steering force required to arrive at the target position.</returns>
		public Vector2 Arrive(Vector2 TargetPosition, float DecelerationFactor = 1f, float MinDistanceFromTarget = 5f)
		{
			Vector2 Displacement = TargetPosition - Owner.Position;
			float Distance = Displacement.Length();

			if (Distance <= MinDistanceFromTarget)
			{
				return Vector2.Zero;
			}

			AutoAlignHeading();

			const float kDecelerationTweaker = 0.3f;
			float Velocity = Distance / (DecelerationFactor * kDecelerationTweaker);

			Velocity = Math.Min(Velocity, mPhysics.MaxSpeed);
			Vector2 DesiredVelocity = Displacement * Velocity / Distance;

			return DesiredVelocity - mPhysics.Velocity;
		}

		/// <summary>
		/// Pursues an evader by seeking its predicted trajectory.
		/// </summary> 
		/// <param name="Evader">The <c>GameObject</c> to pursue.</param>
		/// <returns>A vector representing the steering force required to reach the target position.</returns>
		public Vector2 Pursuit(GameObject Evader)
		{
			if (!Evader.HasComponent<Physics>())
			{
				throw new InvalidOperationException("Evader has no Physics component");
			}

			Physics EvaderPhysics = Evader.GetComponent<Physics>();

			AutoAlignHeading();

			Vector2 ToEvader = Evader.Position - Owner.Position;

			const float kHeadingAngleMargin = -0.95f; // acos(0.95) == 18 degrees
			float RelativeHeading = Vector2.Dot(mPhysics.Heading, EvaderPhysics.Heading);

			if ((Vector2.Dot(ToEvader, mPhysics.Heading) > 0) && (RelativeHeading < kHeadingAngleMargin))
			{
				return Seek(Evader.Position);
			}

			float LookAheadTime = ToEvader.Length() / (mPhysics.MaxSpeed + EvaderPhysics.Speed);

			return Seek(Evader.Position + EvaderPhysics.Velocity * LookAheadTime);
		}

		/// <summary>
		/// Evades a pursuer by seeking a predicted trajectory away from its position.
		/// </summary>
		/// <param name="Pursuer">The <c>GameObject</c> to evade.</param>
		/// <param name="ThreatDistance">The minimum distance to the pursuer used to consider whether the evasion should take place.</param>
		/// <returns>A vector representing a steering force that leads away from the pursuer position.</returns>
		public Vector2 Evade(GameObject Pursuer, float ThreatDistance = 200f)
		{
			if (!Pursuer.HasComponent<Physics>())
			{
				throw new InvalidOperationException("Pursuer has no Physics component");
			}

			Physics PursuerPhysics = Pursuer.GetComponent<Physics>();

			Vector2 Displacement = Pursuer.Position - Owner.Position;
			float Distance = Displacement.Length();

			if (Distance >= ThreatDistance)
			{
				return Vector2.Zero;
			}

			float LookAheadTime = Distance / (mPhysics.MaxSpeed + PursuerPhysics.Speed);

			return Flee(Pursuer.Position + PursuerPhysics.Velocity * LookAheadTime, ThreatDistance);
		}

		/// <summary>
		/// Produces a force that moves an agent to a point between two other agents.
		/// </summary>
		/// <param name="Left">The <c>GameObject</c> to the left of the midpoint.</param>
		/// <param name="Right">The <c>GameObject</c> to the right of the midpoint.</param>
		/// <returns>A steering force that moves an agent to the midpoint of the imaginary line connecting two other agents.</returns>
		public Vector2 Interpose(GameObject Left, GameObject Right)
		{
			if (!Left.HasComponent<Physics>())
			{
				throw new InvalidOperationException("Left GameObject has no Physics component");
			}

			Physics LeftPhysics = Left.GetComponent<Physics>();

			if (!Right.HasComponent<Physics>())
			{
				throw new InvalidOperationException("Right GameObject has no Physics component");
			}

			Physics RightPhysics = Right.GetComponent<Physics>();

			Vector2 MidPoint = (Left.Position + Right.Position) / 2.0f;

			float TimeToReachMidPoint = Vector2.Distance(Owner.Position, MidPoint) / mPhysics.MaxSpeed;

			Vector2 LeftPos = Left.Position + LeftPhysics.Velocity * TimeToReachMidPoint;
			Vector2 RightPos = Right.Position + RightPhysics.Velocity * TimeToReachMidPoint;

			MidPoint = (LeftPos + RightPos) / 2.0f;

			return Arrive(MidPoint);
		}
	}
}
