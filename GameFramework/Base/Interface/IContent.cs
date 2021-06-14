// Copyright (c) Rodrigo Bento

namespace GameFramework.Base
{

	/// <summary>
	/// Represents the loading/unloading of content.
	/// </summary>
	public interface IContent
	{
		/// <summary>
		/// Content should be considered loaded when this method returns.
		/// </summary>
		void LoadContent();

		/// <summary>
		/// Content should be considered unloaded when this method returns.
		/// </summary>
		void UnloadContent();
	}
}
