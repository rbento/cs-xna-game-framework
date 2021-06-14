// Copyright (c) Rodrigo Bento

namespace GameFramework.Base
{
	/// <summary>
	/// Represents the Initialization operation which should happen right after a constructor.
	/// </summary>
	public interface IInitialize
	{
		/// <summary>
		/// Context should be considered initialized when this method returns.
		/// </summary>
		void Initialize();
	}
}
