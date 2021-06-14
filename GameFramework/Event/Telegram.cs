// Copyright (c) Rodrigo Bento

using System;

namespace GameFramework.Event
{
	/// <summary>
	/// Defines a message format to be exchange between <c>GameObject</c>s.
	/// </summary>
	/// <remarks>
	/// Decouples interaction allowing completely unrelated objects to affect each other according to
	/// the contents of the received <c>Telegram</c>.
	/// </remarks>
	public class Telegram : IComparable<Telegram>
	{
		/// <summary>
		/// Creates an instance of <c>Telegram</c>.
		/// </summary>
		/// <param name="SenderId">The Id of the <c>GameObject</c> sending this message.</param>
		/// <param name="ReceiverId">The Id of the <c>GameObject</c> receiving this message.</param>
		/// <param name="Message">The message.</param>
		/// <param name="DispatchTime">The time at which this message had been sent.</param>
		/// <param name="ExtraInfo">Extra information in any format.</param>
		public Telegram(int SenderId, int ReceiverId, int Message, float DispatchTime = 0, object ExtraInfo = null)
		{
			this.SenderId = SenderId;
			this.ReceiverId = ReceiverId;
			this.Message = Message;
			this.DispatchTime = DispatchTime;
			this.ExtraInfo = ExtraInfo;
		}

		/// <summary>
		/// Compares to another telegram by <c>DispatchTime</c>.
		/// </summary>
		/// <remarks>
		/// This is required so telegrams can be properly sorted by priority.
		/// </remarks>
		/// <param name="Other">The <c>Telegram</c> instance to compare.</param>
		/// <returns><c>1</c> if it was sent after the other <c>Telegram</c>, <c>-1</c> if sent before, otherwise <c>0</c>.</returns>
		public int CompareTo(Telegram Other)
		{
			return (DispatchTime > Other.DispatchTime) ? 1 : (DispatchTime < Other.DispatchTime) ? -1 : 0;
		}

		/// <summary>
		/// Gets the <c>string</c> representation of this <c>Telegram</c>.
		/// </summary>
		/// <returns>The <c>string</c> representation of this <c>Telegram</c>.</returns>
		public override string ToString()
		{
			return $"{GetType().Name} [ SenderId: {SenderId}, ReceiverId: {ReceiverId}, Message: {Message}, DispatchTime: {DispatchTime} ]";
		}

		/// <summary>
		/// The Id of the <c>GameObject</c> sending this message.
		/// </summary>
		public int SenderId { get; private set; }

		/// <summary>
		/// The Id of the <c>GameObject</c> receiving this message, if any.
		/// </summary>
		public int ReceiverId { get; private set; }

		/// <summary>
		/// The message.
		/// </summary>
		public int Message { get; private set; }

		/// <summary>
		/// The time at which this message had been sent.
		/// </summary>
		public float DispatchTime { get; set; }

		/// <summary>
		/// May contain extra information in any format.
		/// </summary>
		public object ExtraInfo { get; private set; }
	}
}
