// Copyright (c) Rodrigo Bento

using GameFramework.Base;
using GameFramework.Diagnostics;
using GameFramework.Event;
using GameFramework.Manager;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace GameFramework.State
{
	/// <summary>
	/// Represents a state to be managed by a state machine.
	/// </summary>
	/// <remarks>
	/// Common usages are to implement the different states of a given <c>GameActor</c> or
	/// different <c>GameScene</c>s.
	/// </remarks>
	/// <typeparam name="T">Any class.</typeparam>
	public abstract class State<T> : IInteractive, IUpdate, IDraw, IScene where T : class
	{
		/// <summary>
		/// Creates new instance of this class.
		/// </summary>
		/// <param name="StateMachine">The <c>StateMachine</c> owning this instance.</param>
		/// <exception cref="ArgumentNullException">When the given <c>StateMachine</c> is <c>null</c>.</exception>
		public State(StateMachine<T> StateMachine)
		{
			this.StateMachine = StateMachine ?? throw new ArgumentNullException("Trying to instantiate a State with a null StateMachine");
		}

		/// <summary>
		/// Handles the received <c>Telegram</c>s.
		/// </summary>
		/// <param name="Sender">The message sender.</param>
		/// <param name="Message">The message sent.</param>
		/// <returns><c>true</c> when the message is received and handled, otherwise <c>false</c>.</returns>
		public virtual bool OnMessage(T Sender, Telegram Message) { return false; }

		/// <summary>
		/// See <see cref="IScene.OnEnter()"/>
		/// </summary>
		public virtual void OnEnter() { }

		/// <summary>
		/// See <see cref="IScene.OnExit()"/>
		/// </summary>
		public virtual void OnExit() { }

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
		/// The <c>StateMachine</c> owning this instance.
		/// </summary>
		public StateMachine<T> StateMachine { get; private set; }

		/// <summary>
		/// The <c>T</c> owning the <c>StateMachine</c> instance.
		/// </summary>
		public T Owner => StateMachine.Owner;

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
		protected DebugInfo Debugger => DebugInfo.Instance;

		/// <summary>
		/// The <see cref="Core.SpriteBatch"/> instance.
		/// </summary>
		protected SpriteBatch SpriteBatch => Core.SpriteBatch;
	}
}
