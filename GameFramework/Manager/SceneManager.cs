// Copyright (c) Rodrigo Bento

using GameFramework.Base;
using GameFramework.Effect;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using static GameFramework.Diagnostics.Logger;

namespace GameFramework.Manager
{
	/// <summary>
	/// Defines scene types.
	/// </summary>
	public enum SceneType
	{
		None, Quit, Menu, Help, Credits, Play, QuitPlay, GameOver
	}

	/// <summary>
	/// Thrown when a <c>GameScene</c> is required but not present.
	/// </summary>
	public class SceneNotFoundException : SystemException
	{
	}

	/// <summary>
	/// Centralizes the management of game scenes applying the fade effect while transitioning from
	/// one scene to the other.
	/// </summary>
	public sealed class SceneManager : GameCore
	{
		/// <summary>
		/// The Content Pipeline asset for the fade effect.
		/// </summary>
		private const string kTextureBlank = "Images/txBlank";

		/// <summary>
		/// Container for <c>GameScene</c>s.
		/// </summary>
		private readonly Dictionary<SceneType, GameScene> mScenes;

		/// <summary>
		/// The <c>FadeEffect</c> instance.
		/// </summary>
		private readonly FadeEffect mFadeEffect;

		/// <summary>
		/// The rectangle used for the fade effect.
		/// </summary>
		private readonly Rectangle mFadeEffectRect;

		/// <summary>
		/// The texture used for the fade effect.
		/// </summary>
		private Texture2D mTextureBlank;

		/// <summary>
		/// Creates an instance of <c>SceneManager</c>.
		/// </summary>
		private SceneManager()
			: base()
		{
			mScenes = new Dictionary<SceneType, GameScene>();

			mFadeEffect = new FadeEffect();
			mFadeEffectRect = new Rectangle(0, 0, Core.BackBufferWidth, Core.BackBufferHeight);

			CurrentType = SceneType.None;
			NextType = SceneType.None;

			HideEffectDuration = EffectDuration.OneSec;
			HideDelayDuration = EffectDuration.OneSec;

			ShowEffectDuration = EffectDuration.TwoSecs;
			ShowDelayDuration = EffectDuration.OneSec;
		}

		/// <summary>
		/// Loads content for all scenes.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IContent.LoadContent()"/>
		/// </remarks>
		/// <exception cref="SceneNotFoundException">When there are no scenes to be managed.</exception>
		public override void LoadContent()
		{
			if (mScenes.Count == 0)
			{
				throw new SceneNotFoundException();
			}

			mTextureBlank = Core.Content.Load<Texture2D>(kTextureBlank);

			foreach (var Scene in mScenes)
			{
				Scene.Value.LoadContent();
			}

			Debug("[LoadContent] Content was loaded");

			IsContentLoaded = true;

			foreach (var Scene in mScenes)
			{
				Scene.Value.Initialize();
			}

			IsInitialized = true;

			CurrentType = SceneType.Menu;

			mScenes[CurrentType].OnEnter();

			StartTransition(FadeMode.Out);
		}

		/// <summary>
		/// Unloads content for all scenes.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IContent.UnloadContent()"/>
		/// </remarks>
		public override void UnloadContent()
		{
			foreach (var Scene in mScenes)
			{
				Scene.Value.UnloadContent();
			}

			Debug("[UnloadContent] Content was unloaded");
		}

		/// <summary>
		/// Gives the current scene opportunity to check for input.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IInteractive.CheckForInput(InputManager)"/>
		/// </remarks>
		/// <param name="Input">The <c>InputManager</c> instance containing input state.</param>
		public override void CheckForInput(InputManager Input)
		{
			if (mScenes.Count > 0)
			{
				mScenes[CurrentType].CheckForInput(Input);
			}
		}

		/// <summary>
		/// Updates the current scene state and applies a fade transition effect, if active.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IUpdate.Update(GameTime)"/>
		/// </remarks>
		/// <param name="GameTime"></param>
		public override void Update(GameTime GameTime)
		{
			if (mScenes.Count > 0)
			{
				mScenes[CurrentType].Update(GameTime);

				if (InTransition)
				{
					mFadeEffect.Apply(GameTime);

					if (mFadeEffect.IsDone)
					{
						if (mFadeEffect.Mode == FadeMode.In)
						{
							SwitchScene();

							StartTransition(FadeMode.Out, ShowEffectDuration, ShowDelayDuration);
						}
						else
						{
							StopTransition();
						}
					}
				}
			}
		}

		/// <summary>
		/// Draws the current scene to the screen drawing a fade transition texture on top of it, if active.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IDraw.Draw(GameTime)"/>
		/// </remarks>
		/// <param name="GameTime">The current time delta in milliseconds.</param>
		public override void Draw(GameTime GameTime)
		{
			if (mScenes.Count > 0)
			{
				mScenes[CurrentType].Draw(GameTime);

				if (InTransition)
				{
					if (HideEffectDuration != EffectDuration.Immediate)
					{
						SpriteBatch.Draw(mTextureBlank, mFadeEffectRect, mFadeEffect.Color);
					}
				}
			}
		}

		/// <summary>
		/// Starts a scene transition.
		/// </summary>
		/// <param name="Mode">The <c>FadeEffect</c> mode to be utilized.</param>
		/// <param name="EffectDuration">The transition effect duration.</param>
		/// <param name="DelayDuration">The transition delay duration.</param>
		private void StartTransition(FadeMode Mode,
									 EffectDuration EffectDuration = EffectDuration.Immediate,
									 EffectDuration DelayDuration = EffectDuration.Immediate)
		{
			Debug("[StartTransition]");

			if (FadeMode.In == Mode)
			{
				mFadeEffect.FadeIn(EffectDuration, DelayDuration);
			}

			else if (FadeMode.Out == Mode)
			{
				mFadeEffect.FadeOut(EffectDuration, DelayDuration);
			}

			InTransition = true;
		}

		/// <summary>
		/// Immediatelly stop an active scene transition.
		/// </summary>
		private void StopTransition()
		{
			Debug("[StopTransition]");

			InTransition = false;
		}

		/// <summary>
		/// Changes a scene by specifying the next scene type.
		/// </summary>
		/// <param name="NextType">The type of the scene to be displayed.</param>
		/// <param name="NextEffectDuration">The transition effect duration.</param>
		/// <param name="NextDelayDuration">The transition delay duration.</param>
		public void ChangeScene(SceneType NextType,
								EffectDuration NextEffectDuration = EffectDuration.Immediate,
								EffectDuration NextDelayDuration = EffectDuration.Immediate)
		{
			if (InTransition) return;

			this.NextType = NextType;

			HideEffectDuration = NextEffectDuration;
			HideDelayDuration = NextDelayDuration;

			if (NextEffectDuration == EffectDuration.Immediate && NextDelayDuration == EffectDuration.Immediate)
			{
				SwitchScene();
			}
			else
			{
				StartTransition(FadeMode.In, HideEffectDuration, HideDelayDuration);
			}
		}

		/// <summary>
		/// Causes the current scene to exit and the next scene to enter.
		/// </summary>
		/// <remarks>
		/// Current scene executes <c>OnExit</c> and the next scene executes <c>OnEnter</c>.
		/// </remarks>
		private void SwitchScene()
		{
			Debug("[SwitchScene]");

			mScenes[CurrentType].OnExit();

			PreviousType = CurrentType;

			CurrentType = NextType;

			mScenes[CurrentType].OnEnter();

			NextType = SceneType.None;
		}

		/// <summary>
		/// Tells whether <c>SceneManager</c> has any scenes.
		/// </summary>
		public bool HasScenes
		{
			get
			{
				return mScenes.Count > 0;
			}
		}

		/// <summary>
		/// Add all scenes to the scene container.
		/// </summary>
		/// <param name="Scenes">The new scene container to add.</param>
		public void AddScenes(Dictionary<SceneType, GameScene> Scenes)
		{
			foreach (KeyValuePair<SceneType, GameScene> Scene in Scenes)
			{
				AddScene(Scene.Key, Scene.Value);
			}
		}

		/// <summary>
		/// Adds a scene to the scene container.
		/// </summary>
		/// <param name="Type">The scene type to add.</param>
		/// <param name="Scene">the scene instance to add.</param>
		public void AddScene(SceneType Type, GameScene Scene)
		{
			mScenes.Add(Type, Scene);
		}

		/// <summary>
		/// The previous <c>SceneType</c>.
		/// </summary>
		public SceneType PreviousType { get; private set; }

		/// <summary>
		/// The current <c>SceneType</c>.
		/// </summary>
		public SceneType CurrentType { get; private set; }

		/// <summary>
		/// The next <c>SceneType</c>, if any.
		/// </summary>
		public SceneType NextType { get; private set; }

		/// <summary>
		/// The transition effect duration to hide a scene.
		/// </summary>
		public EffectDuration HideEffectDuration { get; set; }

		/// <summary>
		/// The transition delay duration to hide a scene.
		/// </summary>
		public EffectDuration HideDelayDuration { get; set; }

		/// <summary>
		/// The transition effect duration to show a scene.
		/// </summary>
		public EffectDuration ShowEffectDuration { get; set; }

		/// <summary>
		/// The transition delay duration to show a scene.
		/// </summary>
		public EffectDuration ShowDelayDuration { get; set; }

		/// <summary>
		/// Whether a transition is active.
		/// </summary>
		public bool InTransition { get; private set; }

		/// <summary>
		/// The unique instance of <c>SceneManager</c>.
		/// </summary>
		public static SceneManager Instance { get; } = new SceneManager();
	}
}
