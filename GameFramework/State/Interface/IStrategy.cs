// Copyright (c) Rodrigo Bento

namespace GameFramework.State
{
	/// <summary>
	/// Defines a generic strategy to be implemented for a given type.
	/// </summary>
	/// <typeparam name="T">Any class.</typeparam>
	public interface IStrategy<T> where T : class
	{
		/// <summary>
		/// The <c>T</c> owning this instance.
		/// </summary>
		T Owner { get; }
	}
}
