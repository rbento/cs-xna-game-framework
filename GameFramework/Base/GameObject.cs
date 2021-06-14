// Copyright (c) Rodrigo Bento

using GameFramework.Diagnostics;
using GameFramework.Event;
using GameFramework.Manager;
using GameFramework.Util;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using static GameFramework.Diagnostics.Logger;

namespace GameFramework.Base
{
	/// <summary>
	/// The basic Game Object, which is not interactive.
	/// </summary>
	/// <remarks>
	/// Useful to create objects that do not respond to user input.
	/// </remarks>
	public abstract class GameObject : IUpdate, IDraw
	{
		/// <summary>
		/// The current angle in radians.
		/// </summary>
		private float mAngle;

		/// <summary>
		/// The location(in screen coordinates) to place this <c>GameObject</c>.
		/// </summary>
		private Vector2 mPosition;

		/// <summary>
		/// Container for <c>GameComponent</c>s.
		/// </summary>
		private readonly Dictionary<Type, GameComponent> mComponents;

		/// <summary>
		/// Creates an instance of <c>GameObject</c>.
		/// </summary>
		/// <param name="Width">The desired width.</param>
		/// <param name="Height">The desired height.</param>
		/// <param name="X">The X location(in screen coordinates) to place this <c>GameObject</c>.</param>
		/// <param name="Y">The Y location(in screen coordinates) to place this <c>GameObject</c>.</param>
		public GameObject(int Width, int Height, float X = 0f, float Y = 0f)
			: this(Width, Height, new Vector2(X, Y))
		{
		}

		/// <summary>
		/// Creates an instance of <c>GameObject</c>.
		/// </summary>
		/// <param name="Width">The desired width.</param>
		/// <param name="Height">The desired height.</param>
		/// <param name="Position">The location(in screen coordinates) to place this <c>GameObject</c>.</param>
		/// <exception cref="ArgumentException">When required arguments are invalid.</exception>
		public GameObject(int Width, int Height, Vector2 Position)
		{
			if (Width <= 0 || Height <= 0 || Position == null)
			{
				throw new ArgumentException();
			}

			this.Width = Width;
			this.Height = Height;
			this.Position = Position;

			mComponents = new Dictionary<Type, GameComponent>();

			Origin = new Vector2(this.Width / 2, this.Height / 2);

			Scale = 1f;
			Angle = 0f;

			Registry.Add(this);

			Debug("[GameObject] Created");
		}

		/// <summary>
		/// Removes this <c>GameObject</c> from the <c>Registry</c>.
		/// </summary>
		~GameObject()
		{
			Registry.Remove(this);

			Debug("[~GameObject] Destroyed");
		}

		/// <summary>
		/// Handles received messages.
		/// </summary>
		/// <param name="Telegram">The message to handle.</param>
		/// <returns><c>true</c> if the message has been received and handled, otherwise <c>false</c>.</returns>
		public virtual bool HandleMessage(Telegram Telegram) { return false; }

		/// <summary>
		/// See <see cref="IUpdate.Update(GameTime)"/>
		/// </summary>
		public virtual void Update(GameTime GameTime)
		{
		}

		/// <summary>
		/// Feeds the <c>DebugDraw</c> with physics data for drawing vectors if a <c>Physics</c> component is present.
		/// See <see cref="IDraw.Draw(GameTime)"/>
		/// </summary>
		public virtual void Draw(GameTime GameTime)
		{
			if (DebugDraw.IsVisible && HasComponent<Physics>())
			{
				Physics Physics = GetComponent<Physics>();

				Vector2 Head = Position + Maths.CreateBySizeAndAngle(50, Maths.AngleInRadians(Physics.Heading));
				Vector2 Side = Position + Maths.CreateBySizeAndAngle(70, Maths.AngleInRadians(Physics.Side));
				Vector2 Face = Position + Maths.CreateBySizeAndAngle(50, Maths.AngleInRadians(Physics.Facing));
				Vector2 Velocity = Position + Maths.CreateBySizeAndAngle(70, Maths.AngleInRadians(Physics.Velocity));

				/// Draw individual vectors.

				DebugDraw.DrawLine(Position, Face, Color.Orange);
				DebugDraw.DrawLine(Position, Head, Color.Blue);
				DebugDraw.DrawLine(Position, Side, Color.Yellow);
				DebugDraw.DrawLine(Position, Velocity, Color.Red);

				/// Draw bounding box.

				DebugDraw.DrawLine(UpperLeftCorner, UpperRightCorner, Color.Fuchsia);
				DebugDraw.DrawLine(UpperRightCorner, LowerRightCorner, Color.Fuchsia);
				DebugDraw.DrawLine(LowerRightCorner, LowerLeftCorner, Color.Fuchsia);
				DebugDraw.DrawLine(LowerLeftCorner, UpperLeftCorner, Color.Fuchsia);
			}
		}

		/// <summary>
		/// Adds a <c>GameComponent</c>.
		/// </summary>
		/// <typeparam name="T">The component type to add.</typeparam>
		/// <param name="Component">The <c>GameComponent</c> instance to add.</param>
		public void AddComponent<T>(GameComponent Component) where T : GameComponent
		{
			mComponents.Add(typeof(T), Component);
		}

		/// <summary>
		/// Gets an attached <c>GameComponent</c>.
		/// </summary>
		/// <typeparam name="T">The component type to get.</typeparam>
		/// <returns>The attached <c>GameComponent</c>, if any, otherwise <c>null</c>.</returns>
		public T GetComponent<T>() where T : GameComponent
		{
			if (HasComponent<T>())
			{
				return (T)mComponents[typeof(T)];
			}
			return null;
		}

		/// <summary>
		/// Tells whether a given <c>GameComponent</c> is attached to this <c>GameObject</c>.
		/// </summary>
		/// <typeparam name="T">The component type to check.</typeparam>
		/// <returns><c>true</c> if a component of the given type is attached, otherwise <c>false</c>.</returns>
		public bool HasComponent<T>() where T : GameComponent
		{
			return mComponents.ContainsKey(typeof(T));
		}

		/// <summary>
		/// Resets this <c>GameObject</c>'s state.
		/// </summary>
		public virtual void Reset()
		{
		}

		/// <summary>
		/// Removes this <c>GameObject</c> from the registry.
		/// </summary>
		public virtual void Cleanup()
		{
			Registry.Remove(this);
		}

		/// <summary>
		/// The rectangle according to current position and dimension.
		/// </summary>
		public Rectangle Rectangle
		{
			get { return new Rectangle((int)(X - Origin.X), (int)(Y - Origin.Y), Width, Height); }
		}

		/// <summary>
		/// Gets a vector representing the upper left corner of this <c>GameObject</c>.
		/// </summary>
		public Vector2 UpperLeftCorner
		{
			get
			{
				Vector2 UpperLeft = new Vector2(Left, Top);
				UpperLeft = RotatePoint(UpperLeft, UpperLeft + Origin, Angle);
				return UpperLeft;
			}
		}

		/// <summary>
		/// Gets a vector representing the upper right corner of this <c>GameObject</c>.
		/// </summary>
		public Vector2 UpperRightCorner
		{
			get
			{
				Vector2 UpperRight = new Vector2(Right, Top);
				UpperRight = RotatePoint(UpperRight, UpperRight + new Vector2(-Origin.X, Origin.Y), Angle);
				return UpperRight;
			}
		}

		/// <summary>
		/// Gets a vector representing the lower left corner of this <c>GameObject</c>.
		/// </summary>
		public Vector2 LowerLeftCorner
		{
			get
			{
				Vector2 LowerLeft = new Vector2(Left, Bottom);
				LowerLeft = RotatePoint(LowerLeft, LowerLeft + new Vector2(Origin.X, -Origin.Y), Angle);
				return LowerLeft;
			}
		}

		/// <summary>
		/// Gets a vector representing the lower right corner of this <c>GameObject</c>.
		/// </summary>
		public Vector2 LowerRightCorner
		{
			get
			{
				Vector2 LowerRight = new Vector2(Right, Bottom);
				LowerRight = RotatePoint(LowerRight, LowerRight + new Vector2(-Origin.X, -Origin.Y), Angle);
				return LowerRight;
			}
		}

		/// <summary>
		/// Rotates a point about the given origin.
		/// </summary>
		/// <param name="Point">The point to rotate.</param>
		/// <param name="Origin">The origin to rotate about.</param>
		/// <param name="Rotation">The rotation angle in radians to apply.</param>
		/// <returns>The rotated vector.</returns>
		private Vector2 RotatePoint(Vector2 Point, Vector2 Origin, float Rotation)
		{
			Vector2 RotatedPoint = new Vector2
			{
				X = (float)(Origin.X + (Point.X - Origin.X) * Math.Cos(Rotation) - (Point.Y - Origin.Y) * Math.Sin(Rotation)),
				Y = (float)(Origin.Y + (Point.Y - Origin.Y) * Math.Cos(Rotation) + (Point.X - Origin.X) * Math.Sin(Rotation))
			};
			return RotatedPoint;
		}

		/// <summary>
		/// The top side of this <c>GameObject</c> at its current position.
		/// </summary>
		public float Top
		{
			get { return Position.Y - (Height / 2); }
		}

		/// <summary>
		/// The right side of this <c>GameObject</c> at its current position.
		/// </summary>
		public float Right
		{
			get { return Position.X + (Width / 2); }
		}

		/// <summary>
		/// The bottom side of this <c>GameObject</c> at its current position.
		/// </summary>
		public float Bottom
		{
			get { return Position.Y + (Height / 2); }
		}

		/// <summary>
		/// The left side of this <c>GameObject</c> at its current position.
		/// </summary>
		public float Left
		{
			get { return Position.X - (Width / 2); }
		}

		/// <summary>
		/// The angle in radians.
		/// </summary>
		public float Angle
		{
			get => mAngle;
			set => mAngle = MathHelper.WrapAngle(value);
		}

		/// <summary>
		/// The location(in screen coordinates) to place this <c>GameObject</c>.
		/// </summary>
		public Vector2 Position
		{
			get => mPosition;
			set => mPosition = value;
		}

		/// <summary>
		/// The X location(in screen coordinates) to place this <c>GameObject</c>.
		/// </summary>
		public float X
		{
			get => mPosition.X;
			set => mPosition.X = value;
		}

		/// <summary>
		/// The X location(in screen coordinates) to place this <c>GameObject</c>.
		/// </summary>
		public float Y
		{
			get => mPosition.Y;
			set { mPosition.Y = value; }
		}

		/// <summary>
		/// The width in pixels.
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		/// The height in pixels.
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		/// Specifies the angle(in radians) to rotate about its center.
		/// </summary>
		public float Rotation { get; set; }

		/// <summary>
		/// The scale factor.
		/// </summary>
		public float Scale { get; set; }

		/// <summary>
		/// The origin or pivot vector.
		/// </summary>
		/// <remarks>
		/// The <c>GameObject</c> rotates about this point.
		/// </remarks>
		public Vector2 Origin { get; set; }

		/// <summary>
		/// The unique Id.
		/// </summary>
		public int Id { get; private set; }

		/// <summary>
		/// The <see cref="Manager.TimeManager"/> instance.
		/// </summary>
		protected static TimeManager TimeManager => TimeManager.Instance;

		/// <summary>
		/// The <see cref="Core.Content"/> instance.
		/// </summary>
		protected static ContentManager ContentManager => Core.Content;

		/// <summary>
		/// The <see cref="Core.SpriteBatch"/> instance.
		/// </summary>
		protected static SpriteBatch SpriteBatch => Core.SpriteBatch;

		/// <summary>
		/// The <see cref="Diagnostics.DebugDraw"/> instance.
		/// </summary>
		protected static DebugDraw DebugDraw => DebugDraw.Instance;

		/// <summary>
		/// The <see cref="Diagnostics.DebugInfo"/> instance.
		/// </summary>
		protected static DebugInfo DebugInfo => DebugInfo.Instance;

		/// <summary>
		/// The <see cref="GameFramework.Core"/> instance.
		/// </summary>
		protected static Core Core => Core.Instance;

		/// <summary>
		/// The <c>GameObject</c> Registry.
		/// </summary>
		/// <remarks>
		/// All <c>GameObject</c>s are registered and receive an <c>Id</c> with which it can be queried later.
		/// </remarks>
		public static class Registry
		{
			/// <summary>
			/// Generates a random and unique integer id.
			/// </summary>
			/// <returns>A unique integer id.</returns>
			public static int GenerateUniqueId()
			{
				int NextInt = Randoms.NextInt();

				if (Ids.Contains(NextInt))
				{
					NextInt = GenerateUniqueId(); // Recurse in this unlikely situation 
				}
				else
				{
					Ids.Add(NextInt);
				}

				return NextInt;
			}

			/// <summary>
			/// Adds a <c>GameObject</c> to the registry.
			/// </summary>
			/// <param name="GameObject">The <c>GameObject</c> instance to add.</param>
			public static void Add(GameObject GameObject)
			{
				Debug("[Add] Adding GameOject...");

				if (GameObject == null)
				{
					throw new ArgumentNullException();
				}

				if (GameObject.Id == 0)
				{
					GameObject.Id = GenerateUniqueId();
				}

				Debug($"[Add] GameObject Count={Objects.Count}");

				Objects.Add(GameObject.Id, GameObject);

				Debug($"[Add] Added GameObject.Id={GameObject.Id}");
				Debug($"[Add] GameObject Count={Objects.Count}");

				DebugInfo.AddToLeftViewport("GameObject Count", Objects.Count);
			}

			/// <summary>
			/// Removes a <c>GameObject</c> from the registry.
			/// </summary>
			/// <param name="GameObject">The <c>GameObject</c> instance to remove.</param>
			public static void Remove(GameObject GameObject)
			{
				Debug("[Remove] Removing GameObject...");

				if (GameObject == null) return;

				Debug($"[Remove] GameObject Count={Objects.Count}");

				Ids.Remove(GameObject.Id);
				Objects.Remove(GameObject.Id);

				Debug($"[Remove] Removed GameObject.Id={GameObject.Id}");
				Debug($"[Remove] GameObject Count={Objects.Count}");

				DebugInfo.AddToLeftViewport("GameObject Count", Objects.Count);
			}

			/// <summary>
			/// Queries a <c>GameObject</c> by its id.
			/// </summary>
			/// <param name="Id">The <c>GameObject</c> id to query.</param>
			/// <returns>A <c>GameObject</c> instance if any, otherwise <c>null</c>.</returns>
			public static GameObject FindById(int Id)
			{
				if (Objects.Count == 0)
				{
					Debug("[FindById] No GameObject instances to be found.");

					return null;
				}

				if (Objects.TryGetValue(Id, out GameObject GameObject))
				{
					Debug($"[FindById] Found GameObject.Id={GameObject.Id}");
				}
				else
				{
					Debug($"[FindById] Unable to find GameObject.Id={GameObject.Id}");
				}

				return GameObject;
			}

			/// <summary>
			/// The <c>GameObject</c> registry.
			/// </summary>
			private static readonly Dictionary<int, GameObject> Objects = new Dictionary<int, GameObject>();

			/// <summary>
			/// Container for generated ids. 
			/// </summary>            
			private static readonly SortedSet<int> Ids = new SortedSet<int>();
		}
	}
}
