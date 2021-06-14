// Copyright (c) Rodrigo Bento

using GameFramework.Diagnostics;
using GameFramework.Manager;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameFramework.Base
{
	/// <summary>
	/// Provides resources and event hooks necessary for the game framework lifecycle.
	/// </summary>
	public abstract class GameCore : IContent, IDraw, IInteractive, IUpdate
	{
		/// <summary>
		/// See <see cref="IInitialize.Initialize()"/>
		/// </summary>
		public virtual void Initialize() { }

		/// <summary>
		/// See <see cref="IContent.LoadContent(()"/>
		/// </summary>
		public virtual void LoadContent() { }

		/// <summary>
		/// See <see cref="IContent.UnloadContent()"/>
		/// </summary>
		public virtual void UnloadContent() { }

		/// <summary>
		/// See <see cref="IInteractive.CheckForInput(InputManager)"/>
		/// </summary>
		public virtual void CheckForInput(InputManager Input) { }

		/// <summary>
		/// See <see cref="IUpdate.Update(GameTime)"/>
		/// </summary>
		public virtual void Update(GameTime GameTime) { }

		/// <summary>
		/// See <see cref="IDraw.Draw(GameTime)"/>
		/// </summary>
		public virtual void Draw(GameTime GameTime) { }

		/// <summary>
		/// Whether <c>Initialize</c> was called.
		/// </summary>
		public bool IsInitialized { get; set; }

		/// <summary>
		/// Whether <c>LoadContent</c> was called.
		/// </summary>
		public bool IsContentLoaded { get; set; }

		/// <summary>
		/// The <see cref="GameFramework.Core"/> instance.
		/// </summary>
		protected Core Core => Core.Instance;

		/// <summary>
		/// The <see cref="Manager.InputManager"/> instance.
		/// </summary>
		protected InputManager InputManager => InputManager.Instance;

		/// <summary>
		/// The <see cref="Manager.TimeManager"/> instance.
		/// </summary>
		protected TimeManager TimeManager => TimeManager.Instance;

		/// <summary>
		/// The <see cref="Diagnostics.DebugDraw"/> instance.
		/// </summary>
		protected DebugDraw DebugDraw => DebugDraw.Instance;

		/// <summary>
		/// The <see cref="Diagnostics.DebugInfo"/> instance.
		/// </summary>
		protected DebugInfo DebugInfo => DebugInfo.Instance;

		/// <summary>
		/// The <see cref="Microsoft.Xna.Framework.Game.Content"/> instance.
		/// </summary>
		protected ContentManager ContentManager => Core.Content;

		/// <summary>
		/// The <see cref="Core.SpriteBatch"/> instance.
		/// </summary>
		protected SpriteBatch SpriteBatch => Core.SpriteBatch;
	}
}
