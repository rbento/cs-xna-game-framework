// Copyright (c) Rodrigo Bento

namespace GameFramework.Base
{
	/// <summary>
	/// Base class for game components.
	/// </summary>
	public abstract class GameComponent
	{
		/// <summary>
		/// Creates an instance of <c>GameComponent</c>.
		/// </summary>
		/// <param name="Owner">The <c>GameObject</c> to own this instance.</param>
		public GameComponent(GameObject Owner)
		{
			this.Owner = Owner;
		}

		/// <summary>
		/// The <c>GameObject</c> owning this instance.
		/// </summary>
		public GameObject Owner { get; private set; }
	}
}
