// Copyright (c) Rodrigo Bento

using GameFramework.Manager;

namespace GameFramework.Base
{
	/// <summary>
	/// Represents interactions via user input.
	/// </summary>
	public interface IInteractive
	{
		/// <summary>
		/// Checks for user input via Keyboard or Mouse.
		/// </summary>
		/// <param name="Input">The <c>InputManager</c> instance containing input state.</param>
		void CheckForInput(InputManager Input);
	}
}
