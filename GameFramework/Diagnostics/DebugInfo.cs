// Copyright (c) Rodrigo Bento

using GameFramework.Base;
using GameFramework.Manager;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

namespace GameFramework.Diagnostics
{
	/// <summary>
	/// Enables on-screen text debugging.
	/// </summary>
	/// <remarks>
	/// Font properties must be configured via the font asset at GameContent/sfRoboto.spritefont file.
	/// </remarks>
	public sealed class DebugInfo : GameCore
	{
		/// <summary>
		/// The string pattern for blank lines.
		/// </summary>
		private const string kLineBreak = "***BLANK***";

		/// <summary>
		/// The font asset name to be found in the Content Pipeline.
		/// </summary>
		private const string kFont = "Fonts/sfRoboto";

		/// <summary>
		/// The default line spacing.
		/// </summary>
		private const int kLineSpacing = 24;

		/// <summary>
		/// The default maximum viewport lines to be drawn.
		/// </summary>
		private const int kMaxViewportLines = 36;

		/// <summary>
		/// The left viewport screen coordinates.
		/// </summary>
		private Vector2 mLeftViewportPosition;

		/// <summary>
		/// The right viewport screen coordinates.
		/// </summary>
		private Vector2 mRightViewportPosition;

		/// <summary>
		/// The debug font instance.
		/// </summary>
		private SpriteFont mDebugFont;

		/// <summary>
		/// The actual line spacing to be applied.
		/// </summary>
		private int mLineSpacing;

		/// <summary>
		/// Creates an instance of <c>DebugInfo</c>.
		/// </summary>
		private DebugInfo() : base()
		{
			LeftViewport = new Dictionary<string, object>();
			RightViewport = new Dictionary<string, object>();
		}

		/// <summary>
		/// Initializes the <c>DebugInfo</c> viewports.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IInitialize.Initialize()"/>
		/// </remarks>
		public override void Initialize()
		{
			mLineSpacing = kLineSpacing;

			TextColor = Color.White;

			MaxViewportLines = kMaxViewportLines;

			mLeftViewportPosition = new Vector2(LineSpacing, LineSpacing);
			mRightViewportPosition = new Vector2((Viewport.Width / 2) + LineSpacing, LineSpacing);

			IsVisible = false;
			IsInitialized = true;
		}

		/// <summary>
		/// Loads the debug font.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IContent.LoadContent()"/>
		/// </remarks>
		public override void LoadContent()
		{
			mDebugFont = ContentManager.Load<SpriteFont>(kFont);
			IsContentLoaded = true;
		}

		/// <summary>
		/// Unloads the debug font.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IContent.UnloadContent()"/>
		/// </remarks>
		public override void UnloadContent()
		{
			mDebugFont = null;
		}

		/// <summary>
		/// Toggles the <c>DebugInfo</c> by pressing F11.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IInteractive.CheckForInput(InputManager)"/>
		/// </remarks>
		/// <param name="Input">The <c>InputManager</c> instance containing input state.</param>
		public override void CheckForInput(InputManager Input)
		{
			if (Input.KeyPressed(Keys.F11))
			{
				ToggleVisibility();
			}
		}

		/// <summary>
		/// Draws the current debug info to the screen.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IDraw.Draw(GameTime)"/>
		/// </remarks>
		/// <param name="GameTime">The current time delta in milliseconds.</param>
		public override void Draw(GameTime GameTime)
		{
			if (IsVisible)
			{
				DrawViewport(LeftViewport, mLeftViewportPosition);
				DrawViewport(RightViewport, mRightViewportPosition);
			}
		}

		/// <summary>
		/// Adds debug information to be displayed on the left viewport.
		/// </summary>
		/// <param name="Key">The key to add.</param>
		/// <param name="Value">The value to be displayed.</param>
		public void AddToLeftViewport(string Key, object Value)
		{
			AddToViewport(LeftViewport, Key, Value);
		}

		/// <summary>
		/// Adds debug information to be displayed on the right viewport.
		/// </summary>
		/// <param name="Key">The key to add.</param>
		/// <param name="Value">The value to be displayed.</param>
		public void AddToRightViewport(string Key, object Value)
		{
			AddToViewport(RightViewport, Key, Value);
		}

		/// <summary>
		/// Adds debug information to the viewport container.
		/// </summary>
		/// <param name="Viewport">The viewport container.</param>
		/// <param name="Key">The key to add.</param>
		/// <param name="Value">The value to be displayed.</param>
		private void AddToViewport(Dictionary<string, object> Viewport, string Key, object Value)
		{
			if (Viewport.Count > MaxViewportLines)
			{
				return;
			}

			Viewport[Key] = Value;
		}

		/// <summary>
		/// Draws the current viewport content to the screen.
		/// </summary>
		/// <param name="Viewport">The viewport container.</param>
		/// <param name="Position">The top screen coordinate to start drawing from.</param>
		private void DrawViewport(Dictionary<string, object> Viewport, Vector2 Position)
		{
			foreach (KeyValuePair<string, object> Kvp in Viewport)
			{
				if (!Kvp.Key.Contains(kLineBreak))
				{
					DrawString($"{ Kvp.Key }: {Kvp.Value}", Position, TextColor);
				}

				Position.Y += mLineSpacing;
			}
		}

		/// <summary>
		/// Draw a text string to the screen.
		/// </summary>
		/// <param name="Text">The text to draw.</param>
		/// <param name="Position">The desired screen coordinate.</param>
		/// <param name="TextColor">The desired text color.</param>
		private void DrawString(string Text, Vector2 Position, Color TextColor)
		{
			if (Text == null) return;

			SpriteBatch.DrawString(mDebugFont, Text, Position, TextColor);
		}

		/// <summary>
		/// Toggles the <c>DebugInfo</c> visibility.
		/// </summary>
		public void ToggleVisibility()
		{
			IsVisible = !IsVisible;
		}

		/// <summary>
		/// The vertical space between lines.
		/// </summary>
		public int LineSpacing
		{
			get { return mLineSpacing; }

			set
			{
				mLineSpacing = value;
				mDebugFont.LineSpacing = mLineSpacing;
			}
		}

		/// <summary>
		/// Container for text drawn on the left side of the screen.
		/// </summary>
		private Dictionary<string, object> LeftViewport { get; }

		/// <summary>
		/// Container for text drawn on the right side of the screen.
		/// </summary>
		private Dictionary<string, object> RightViewport { get; }

		/// <summary>
		/// The maximum viewport lines to be drawn.
		/// </summary>
		public int MaxViewportLines { get; set; }

		/// <summary>
		/// The text color to be applied.
		/// </summary>
		public Color TextColor { get; set; }

		/// <summary>
		/// Whether this <c>DebugInfo</c> is visible.
		/// </summary>
		public bool IsVisible { get; private set; }

		/// <summary>
		/// The graphics device viewport instance.
		/// </summary>
		private static Viewport Viewport => Core.Instance.GraphicsDevice.Viewport;

		/// <summary>
		/// The unique instance of <c>DebugInfo</c>.
		/// </summary>
		public static DebugInfo Instance { get; private set; } = new DebugInfo();
	}
}
