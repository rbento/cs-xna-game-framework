// Copyright (c) Rodrigo Bento

using GameFramework.Base;
using GameFramework.Manager;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

using static GameFramework.Diagnostics.Logger;

namespace GameFramework.Event
{
	/// <summary>
	/// Handles the dispatching of messages between <c>GameObject</c>s.
	/// </summary>
	public sealed class MessageDispatcher : GameCore, IUpdate
	{
		/// <summary>
		/// Container for enqueuing delayed <c>Telegram</c>s.
		/// </summary>
		private readonly SortedSet<Telegram> mEnqueued = new SortedSet<Telegram>();

		/// <summary>
		/// Container for dispatched <c>Telegram</c>s.
		/// </summary>
		private readonly List<Telegram> mDispatched = new List<Telegram>();

		/// <summary>
		/// Creates an instance of <c>MessageDispatcher</c>.
		/// </summary>
		private MessageDispatcher()
		{
		}

		/// <summary>
		/// Dispatches delayes messages.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IUpdate.Update(GameTime)"/>
		/// </remarks>
		/// <param name="GameTime">The current time delta in milliseconds.</param>
		public override void Update(GameTime GameTime)
		{
			DispatchDelayedMessages();
		}

		/// <summary>
		/// Dispatches delayed messages.
		/// </summary>
		/// <remarks>
		/// Messages are enqueued and sorted by dispatch time.
		/// </remarks>
		private void DispatchDelayedMessages()
		{
			if (mEnqueued.Count == 0) return;

			mDispatched.Clear();

			foreach (Telegram Telegram in mEnqueued)
			{
				float CurrentTime = TimeManager.TotalMilliseconds;

				if (CurrentTime >= Telegram.DispatchTime)
				{
					GameObject Receiver = GameObject.Registry.FindById(Telegram.ReceiverId);

					Debug($"[DispatchDelayedMessages] Delayed telegram dispatched: {Telegram}");

					Dispatch(Receiver, Telegram);

					mDispatched.Add(Telegram);
				}
			}

			foreach (Telegram telegram in mDispatched)
			{
				mEnqueued.Remove(telegram);

				Debug($"[DispatchDelayedMessages] Delayed telegram dequeued: {telegram}");
			}
		}

		/// <summary>
		/// Dispatches a <c>telegram</c> to a target <c>GameObject</c>.
		/// </summary>
		/// <remarks>
		/// Logs an error to the console if the receiver does not handle the message.
		/// </remarks>
		/// <param name="Receiver">The <c>GameObject</c> receiving this message.</param>
		/// <param name="Telegram">The message.</param>
		private void Dispatch(GameObject Receiver, Telegram Telegram)
		{
			if (!Receiver.HandleMessage(Telegram))
			{
				Error("[Dispatch] Unhandled message");
			}
		}

		/// <summary>
		/// Dispatches a message. 
		/// </summary>
		/// <remarks>
		/// A zero or negative delay means it is an instant message.
		/// </remarks>
		/// <param name="SenderId">The Id of the <c>GameObject</c> sending this message.</param>
		/// <param name="ReceiverId">The Id of the <c>GameObject</c> receiving this message.</param>
		/// <param name="Message">The message.</param>
		/// <param name="Delay">The amount of time to wait before dispatching this message.</param>
		/// <param name="ExtraInfo">Extra information in any format.</param>
		public void DispatchMessage(int SenderId, int ReceiverId, int Message, float Delay = 0f, object ExtraInfo = null)
		{
			GameObject Receiver = GameObject.Registry.FindById(ReceiverId);

			Telegram Telegram = new Telegram(SenderId, ReceiverId, Message, Delay, ExtraInfo);

			if (Delay <= 0d)
			{
				Debug($"[DispatchMessage] Instant telegram dispatched: {Telegram}");

				Dispatch(Receiver, Telegram);
			}
			else
			{
				float CurrentTime = TimeManager.TotalMilliseconds;

				Telegram.DispatchTime = CurrentTime + Delay;

				Debug($"[DispatchMessage] Delayed telegram enqueued: {Telegram}");

				mEnqueued.Add(Telegram);
			}
		}

		/// <summary>
		/// The unique instance of <c>MessageDispatcher</c>.
		/// </summary>
		public static MessageDispatcher Instance { get; } = new MessageDispatcher();
	}
}
