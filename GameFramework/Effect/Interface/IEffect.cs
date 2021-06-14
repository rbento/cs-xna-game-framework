// Copyright (c) Rodrigo Bento

using Microsoft.Xna.Framework;

namespace GameFramework.Effect
{
	/// <summary>
	/// Represents an effect.
	/// </summary>
	interface IEffect
	{
		/// <summary>
		/// Prepares the effect.
		/// </summary>
		void Prepare();

		/// <summary>
		/// Applies the effect.
		/// </summary>
		/// <param name="GameTime">The current time delta in milliseconds.</param>
		void Apply(GameTime GameTime);
	}
}
