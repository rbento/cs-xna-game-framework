// Copyright (c) Rodrigo Bento

using Microsoft.Xna.Framework;

namespace GameFramework.Base
{
	/// <summary>
	/// Represents the Draw operation.
	/// </summary>
	public interface IDraw
	{
		/// <summary>
		/// Draws game content to the screen.
		/// </summary>
		/// <param name="GameTime">The current time delta in milliseconds.</param>
		void Draw(GameTime GameTime);
	}
}
