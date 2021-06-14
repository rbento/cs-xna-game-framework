// Copyright (c) Rodrigo Bento

using GameFramework.Manager;

namespace GameFramework.Base
{
	/// <summary>
	/// An interactive game scene with hooks for executing logic on enter/exit.
	/// </summary>
	public abstract class GameScene : GameCore, IScene
	{
		/// <summary>
		/// Creates an instance of <c>GameScene</c>.
		/// </summary>
		public GameScene() : base()
		{
		}

		/// <summary>
		/// See <see cref="IScene.OnEnter()"/>
		/// </summary>
		public virtual void OnEnter()
		{
		}

		/// <summary>
		/// See <see cref="IScene.OnExit()"/>
		/// </summary>
		public virtual void OnExit()
		{
		}

		/// <summary>
		/// The <see cref="Manager.SceneManager"/> instance.
		/// </summary>
		public SceneManager SceneManager => Core.SceneManager;
	}
}
