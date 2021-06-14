// Copyright (c) Rodrigo Bento

namespace GameFramework.Base
{
	/// <summary>
	/// Represents a scene.
	/// </summary>
	public interface IScene
	{
		/// <summary>
		/// Executes when a scene is about to be displayed.
		/// </summary>
		void OnEnter();

		/// <summary>
		/// Executes when a scene is about to be hidden.
		/// </summary>
		void OnExit();
	}
}
