// Copyright (c) Rodrigo Bento

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameFramework.Manager
{
	/// <summary>
	/// Centralizes the management of HID devices.
	/// </summary>
	public sealed class InputManager
	{
		/// <summary>
		/// Creates an instance of <c>InputManager</c>.
		/// </summary>
		private InputManager() { }

		/// <summary>
		/// Update the keyboard and mouse state.
		/// </summary>
		public void UpdateInputState()
		{
			PreviousKeyboardState = Keyboard;
			PreviousMouseState = Mouse;
		}

		/// <summary>
		/// Tells whether a key has been pressed.
		/// </summary>
		/// <param name="Key">The key to be checked.</param>
		/// <returns><c>true</c> if a key has been pressed, otherwise <c>false</c>.</returns>
		public bool KeyPressed(Keys Key)
		{
			return Keyboard.IsKeyDown(Key) && !PreviousKeyboardState.IsKeyDown(Key);
		}

		/// <summary>
		/// Tells whether a key has been released.
		/// </summary>
		/// <param name="Key">The key to be checked.</param>
		/// <returns><c>true</c> if a key has been released, otherwise <c>false</c>.</returns>
		public bool KeyReleased(Keys Key)
		{
			return !Keyboard.IsKeyDown(Key) && PreviousKeyboardState.IsKeyDown(Key);
		}

		/// <summary>
		/// Tells whether a key is being held.
		/// </summary>
		/// <param name="Key">The key to be checked.</param>
		/// <returns><c>true</c> if a key is being held, otherwise <c>false</c>.</returns>
		public bool KeyHeld(Keys Key)
		{
			return Keyboard.IsKeyDown(Key) && PreviousKeyboardState.IsKeyDown(Key);
		}

		/// <summary>
		/// Whether the middle mouse button has been pressed.
		/// </summary>
		public bool MouseMiddlePressed
		{
			get { return Mouse.MiddleButton == ButtonState.Pressed && !(PreviousMouseState.MiddleButton == ButtonState.Pressed); }
		}

		/// <summary>
		/// Whether the middle mouse button has been released.
		/// </summary>
		public bool MouseMiddleReleased
		{
			get
			{
				return !(Mouse.MiddleButton == ButtonState.Pressed) && PreviousMouseState.MiddleButton == ButtonState.Pressed;
			}
		}

		/// <summary>
		/// Whether the middle mouse button is being held.
		/// </summary>
		public bool MouseMiddleHeld
		{
			get
			{
				return Mouse.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton == ButtonState.Pressed;
			}
		}

		/// <summary>
		/// Whether the left mouse button has been pressed.
		/// </summary>
		public bool MouseLeftPressed
		{
			get
			{
				return Mouse.LeftButton == ButtonState.Pressed && !(PreviousMouseState.LeftButton == ButtonState.Pressed);
			}
		}

		/// <summary>
		/// Whether the left mouse button has been released.
		/// </summary>
		public bool MouseLeftReleased
		{
			get
			{
				return !(Mouse.LeftButton == ButtonState.Pressed) && PreviousMouseState.LeftButton == ButtonState.Pressed;
			}
		}

		/// <summary>
		/// Whether the left mouse button is being held.
		/// </summary>
		public bool MouseLeftHeld
		{
			get
			{
				return Mouse.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Pressed;
			}
		}

		/// <summary>
		/// Whether the right mouse button has been pressed.
		/// </summary>
		public bool MouseRightPressed
		{
			get
			{
				return Mouse.RightButton == ButtonState.Pressed && !(PreviousMouseState.RightButton == ButtonState.Pressed);
			}
		}

		/// <summary>
		/// Whether the right mouse button has been released.
		/// </summary>
		public bool MouseRightReleased
		{
			get
			{
				return !(Mouse.RightButton == ButtonState.Pressed) && PreviousMouseState.RightButton == ButtonState.Pressed;
			}
		}

		/// <summary>
		/// Whether the right mouse button is being held.
		/// </summary>
		public bool MouseRightHeld
		{
			get
			{
				return Mouse.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Pressed;
			}
		}

		/// <summary>
		/// The mouse screen coordinates.
		/// </summary>
		public Vector2 MousePosition
		{
			get { return new Vector2(Mouse.X, Mouse.Y); }
		}

		/// <summary>
		/// The keyboard state.
		/// </summary>
		public KeyboardState Keyboard
		{
			get { return Microsoft.Xna.Framework.Input.Keyboard.GetState(); }
		}

		/// <summary>
		/// The mouse state.
		/// </summary>
		public MouseState Mouse
		{
			get { return Microsoft.Xna.Framework.Input.Mouse.GetState(); }
		}

		/// <summary>
		/// The previous keyboard state.
		/// </summary>
		public KeyboardState PreviousKeyboardState { get; private set; }

		/// <summary>
		/// The previous mouse state.
		/// </summary>
		public MouseState PreviousMouseState { get; private set; }

		/// <summary>
		/// Whether the mouse pointer is visible.
		/// </summary>
		public bool IsMouseVisible { get; set; }

		/// <summary>
		/// The unique instance of <c>InputManager</c>.
		/// </summary>
		public static InputManager Instance { get; } = new InputManager();
	}
}
