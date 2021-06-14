// Copyright (c) Rodrigo Bento

using GameFramework.Base;
using GameFramework.Event;
using GameFramework.Manager;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

namespace GameFramework.State
{
	/// <summary>
	/// Defines a State Machine.
	/// </summary>
	/// <typeparam name="T">Any class.</typeparam>
	public class StateMachine<T> : IInteractive, IUpdate, IDraw where T : class
	{
		/// <summary>
		/// Container for <c>State</c>s.
		/// </summary>
		private readonly Stack<State<T>> mStates;

		/// <summary>
		/// Creates an instance of <c>StateMachine</c>.
		/// </summary>
		/// <param name="Owner">The <c>T</c> to own this instance.</param>
		/// <exception cref="ArgumentNullException">When the <c>Owner</c> is null.</exception>
		public StateMachine(T Owner)
		{
			this.Owner = Owner ?? throw new ArgumentNullException("Trying to instantiate a StateMachine with a null Owner");

			mStates = new Stack<State<T>>();
		}

		/// <summary>
		/// Changes the current state.
		/// </summary>
		/// <param name="NewState">The new state to change to.</param>
		public void ChangeState(State<T> NewState)
		{
			if (NewState == null)
			{
				return;
			}

			if (CurrentState != null)
			{
				CurrentState.OnExit();
			}

			mStates.Clear();
			mStates.Push(NewState);

			if (CurrentState != null)
			{
				CurrentState.OnEnter();
			}
		}

		/// <summary>
		/// Pops the current state making the previous state current.
		/// </summary>
		public void PopState()
		{
			if (CurrentState == null)
			{
				return;
			}

			CurrentState.OnExit();

			mStates.Pop();

			if (CurrentState != null)
			{
				CurrentState.OnEnter();
			}
		}

		/// <summary>
		/// Pushes a new state to the top of the stack.
		/// </summary>
		/// <param name="NewState">The new state to be pushed.</param>
		public void PushState(State<T> NewState)
		{
			if (NewState == null || NewState.GetType() == CurrentState.GetType())
			{
				return;
			}

			if (CurrentState != null)
			{
				CurrentState.OnExit();
			}

			mStates.Push(NewState);

			CurrentState.OnEnter();
		}

		/// <summary>
		/// Gives the current state a chance to check for input.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IInteractive.CheckForInput(InputManager)"/>
		/// </remarks>
		public void CheckForInput(InputManager Input)
		{
			if (CurrentState != null)
			{
				mStates.Peek().CheckForInput(Input);
			}
		}

		/// <summary>
		/// Gives the current state a chance to update its state.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IUpdate.Update(GameTime)"/>
		/// </remarks>
		public void Update(GameTime GameTime)
		{
			if (CurrentState != null)
			{
				mStates.Peek().Update(GameTime);
			}
		}

		/// <summary>
		/// Gives the current state a chance to draw to the screen.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IDraw.Draw(GameTime)"/>
		/// </remarks>
		public void Draw(GameTime GameTime)
		{
			if (CurrentState != null)
			{
				mStates.Peek().Draw(GameTime);
			}
		}

		/// <summary>
		/// Handles the given <c>Telegram</c> instance.
		/// </summary>
		/// <param name="Message">The message to be handled.</param>
		/// <returns><c>true</c> if the current state is not null and the message is fowarded to it, otherwise <c>false</c>.</returns>
		public bool HandleMessage(Telegram Message)
		{
			return CurrentState != null && CurrentState.OnMessage(Owner, Message);
		}

		/// <summary>
		/// The current state.
		/// </summary>
		/// <returns>The current state if any, otherwise <c>null</c>.</returns>
		public State<T> CurrentState
		{
			get { return mStates.Count > 0 ? mStates.Peek() : null; }
		}

		/// <summary>
		/// The current state.
		/// </summary>
		/// <exception cref="InvalidOperationException">If the stack is empty.</exception>
		public State<T> State
		{
			get { return mStates.Peek(); }
		}

		/// <summary>
		/// The <c>T</c> owning this instance.
		/// </summary>
		public T Owner { get; private set; }
	}
}
