// Copyright (c) Rodrigo Bento

using Microsoft.Xna.Framework;

namespace GameFramework.Base
{
	/// <summary>
	/// Represents the Update operation.
	/// </summary>
	public interface IUpdate
	{
		/// <summary>
		/// Updates the game state.
		/// </summary>
		/// <param name="GameTime">The current time delta in milliseconds.</param>
		void Update(GameTime GameTime);
	}
}
