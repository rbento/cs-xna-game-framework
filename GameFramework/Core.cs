// Copyright (c) Rodrigo Bento

using GameFramework.Base;
using GameFramework.Diagnostics;
using GameFramework.Manager;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace GameFramework
{
	/// <summary>
	/// 
	/// The GameFramework Core.
	/// 
	/// <list>
	/// 
	/// <item>Initializes the main XNA services such as the <c>GraphicsDeviceManager</c> and <c>SpriteBatch</c>.</item>
	/// <item>Initializes other main management services such as the <c>SceneManager</c>, <c>DebugInfo</c> and <c>DebugDraw</c>.</item>
	/// <item>Controls the window mode: FullScreen / Windowed.</item>
	/// <item>Initializes the Content Pipeline root.</item>
	/// <item>Provides centralized access to Graphics and Content services.</item>
	/// <item>Delegates instances of <c>GameScene</c> to the <c>SceneManager</c>.</item>
	/// 
	/// </list>
	/// 
	/// </summary>
	/// 
	/// <remarks>
	/// 
	/// A typical usage would be like so:
	/// 
	/// <example>
	/// <code>
	/// using (Core Core = Core.Instance)
	/// {
	///     Core.AddScene(SceneType.Menu, new MenuScene());
	///     Core.AddScene(SceneType.Play, new PlayScene());
	///     
	///     Core.Run();
	/// }
	/// </code>
	/// </example>
	/// 
	/// </remarks>
	public class Core : Microsoft.Xna.Framework.Game
	{
		/// <summary>
		/// The default backbuffer height for windowed mode.
		/// </summary>
		private const int kDefaultBackbufferHeight = 540;

		/// <summary>
		/// The default backbuffer height for windowed mode.
		/// </summary>
		private const int kDefaultBackbufferWidth = 720;

		/// <summary>
		/// A container for <c>GameScene</c> instances.
		/// </summary>
		private readonly Dictionary<SceneType, GameScene> mGameScenes;

		/// <summary>
		/// Creates an instance of <c>Core</c>.
		/// </summary>
		private Core()
		{
			Content.RootDirectory = "Content";
			GraphicsDeviceManager = new GraphicsDeviceManager(this);
			mGameScenes = new Dictionary<SceneType, GameScene>();
		}

		/// <summary>
		/// See <see cref="IInitialize.Initialize()"/>
		/// </summary>
		protected override void Initialize()
		{
			IsFixedTimeStep = true;
			IsMouseVisible = false;

			GraphicsDeviceManager.PreferredBackBufferHeight = BackBufferHeight;
			GraphicsDeviceManager.PreferredBackBufferWidth = BackBufferWidth;
			GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;

			GraphicsDeviceManager.ApplyChanges();

			SpriteBatch = new SpriteBatch(GraphicsDevice);

			DebugInfo.Initialize();
			SceneManager.Initialize();

			base.Initialize();
		}

		/// <summary>
		/// See <see cref="IContent.LoadContent()"/>
		/// </summary>
		protected override void LoadContent()
		{
			SceneManager.AddScenes(mGameScenes);

			DebugInfo.LoadContent();
			SceneManager.LoadContent();

			base.LoadContent();
		}

		/// <summary>
		/// See <see cref="IContent.UnloadContent()"/>
		/// </summary>
		protected override void UnloadContent()
		{
			SceneManager.UnloadContent();
			DebugInfo.UnloadContent();

			base.UnloadContent();
		}

		/// <summary>
		/// See <see cref="IUpdate.Update(GameTime)"/>
		/// </summary>
		protected override void Update(GameTime GameTime)
		{
			if (!IsInitialized) return;

			DebugInfo.CheckForInput(InputManager);
			SceneManager.CheckForInput(InputManager);

			SceneManager.Update(GameTime);
			TimeManager.Update(GameTime);

			InputManager.UpdateInputState();
		}

		/// <summary>
		/// See <see cref="IDraw.Draw(GameTime)"/>
		/// </summary>
		protected override void Draw(GameTime GameTime)
		{
			if (!IsInitialized) return;

			GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

			SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

			SceneManager.Draw(GameTime);
			DebugInfo.Draw(GameTime);

			SpriteBatch.End();

			DebugDraw.Flush();

			base.Draw(GameTime);
		}

		/// <summary>
		/// Adds a <c>GameScene</c> to the scenes container.
		/// </summary>
		/// <param name="Type">The <c>SceneType</c> to be used as key.</param>
		/// <param name="Scene">The <c>GameScene</c> instance to add.</param>
		public void AddScene(SceneType Type, GameScene Scene)
		{
			mGameScenes.Add(Type, Scene);
		}

		/// <summary>
		/// Toggles between windowed and fullscreen mode.
		/// </summary>
		public void ToggleFullScreen()
		{
			GraphicsDeviceManager.IsFullScreen = !GraphicsDeviceManager.IsFullScreen;
			IsMouseVisible = !GraphicsDeviceManager.IsFullScreen;
		}

		/// <summary>
		/// Whether <c>Initialize</c> was called.
		/// </summary>
		public bool IsInitialized
		{
			get
			{
				return DebugInfo.IsInitialized && SceneManager.IsInitialized;
			}
		}

		/// <summary>
		/// The current backbuffer width.
		/// </summary>
		public int BackBufferWidth { get; set; } = kDefaultBackbufferWidth;

		/// <summary>
		/// The current backbuffer height.
		/// </summary>
		public int BackBufferHeight { get; set; } = kDefaultBackbufferHeight;

		/// <summary>
		/// The <see cref="Manager.InputManager"/> instance.
		/// </summary>
		public static InputManager InputManager => InputManager.Instance;

		/// <summary>
		/// The <see cref="Manager.SceneManager"/> instance.
		/// </summary>
		public static SceneManager SceneManager => SceneManager.Instance;

		/// <summary>
		/// The <see cref="Manager.TimeManager"/> instance.
		/// </summary>
		public static TimeManager TimeManager => TimeManager.Instance;

		/// <summary>
		/// The <see cref="Diagnostics.DebugDraw"/> instance.
		/// </summary>
		public static DebugDraw DebugDraw => DebugDraw.Instance;

		/// <summary>
		/// The <see cref="Diagnostics.DebugInfo"/> instance.
		/// </summary>
		public static DebugInfo DebugInfo => DebugInfo.Instance;

		/// <summary>
		/// The <see cref="Microsoft.Xna.Framework.GraphicsDeviceManager"/> instance.
		/// </summary>
		public static GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

		/// <summary>
		/// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> instance.
		/// </summary>
		public static SpriteBatch SpriteBatch { get; private set; }

		/// <summary>
		/// The unique instance of <c>Core</c>.
		/// </summary>
		public static Core Instance { get; } = new Core();
	}
}
