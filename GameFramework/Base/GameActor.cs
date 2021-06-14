// Copyright (c) Rodrigo Bento

using GameFramework.Manager;

using Microsoft.Xna.Framework;

namespace GameFramework.Base
{
	/// <summary>
	/// A <c>GameActor</c> is an interactive <c>GameObject</c>.
	/// </summary>
	public abstract class GameActor : GameObject, IInteractive
	{
		/// <summary>
		/// Creates an instance of <c>GameActor</c>.
		/// </summary>
		/// <param name="Width">The desired width.</param>
		/// <param name="Height">The desired height.</param>        
		public GameActor(int Width, int Height)
			: base(Width, Height)
		{
		}

		/// <summary>
		/// Creates an instance of <c>GameActor</c>.
		/// </summary>
		/// <param name="Width">The desired width.</param>
		/// <param name="Height">The desired height.</param>        
		/// <param name="Position">The location(in screen coordinates) to place this <c>GameActor</c>.</param>
		public GameActor(int Width, int Height, Vector2 Position)
			: base(Width, Height, Position)
		{
		}

		/// <summary>
		/// See <see cref="IInteractive.CheckForInput(InputManager)"/>
		/// </summary>
		public virtual void CheckForInput(InputManager Input)
		{
		}

		/// <summary>
		/// The <see cref="Manager.InputManager"/> instance.
		/// </summary>
		protected static InputManager InputManager => InputManager.Instance;
	}
}
