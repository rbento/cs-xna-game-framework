// Copyright (c) Rodrigo Bento

using GameFramework.Base;
using GameFramework.Diagnostics;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace GameFramework.Manager
{
	/// <summary>
	/// Centralizes the management of time.
	/// </summary>
	public class TimeManager : IUpdate
	{
		/// <summary>
		/// The max number of delta time elements to be enqueued.
		/// </summary>
		private const int kDeltaTimeQueueMaxElements = 60;

		/// <summary>
		/// Container for delta time elements.
		/// </summary>
		private readonly Queue<float> mDeltaTimeQueue;

		/// <summary>
		/// Creates an instance of <c>TimeManager</c>.
		/// </summary>
		private TimeManager()
		{
			mDeltaTimeQueue = new Queue<float>(kDeltaTimeQueueMaxElements);
		}

		/// <summary>
		/// Adds the current delta time to the delta time queue and feeds the <c>DebugInfo</c> with
		/// time information.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="IUpdate.Update(GameTime)"/>
		/// </remarks>
		/// <param name="GameTime">The current time delta in milliseconds.</param>
		public void Update(GameTime GameTime)
		{
			TotalMilliseconds = (float)GameTime.TotalGameTime.TotalMilliseconds;
			ElapsedMilliseconds = (float)GameTime.ElapsedGameTime.TotalMilliseconds;

			AddToDeltaTimeQueue(ElapsedMilliseconds / 1000f);

			DebugInfo.AddToLeftViewport("TotalMilliseconds", TotalMilliseconds);
			DebugInfo.AddToLeftViewport("ElapsedMilliseconds", ElapsedMilliseconds);
			DebugInfo.AddToLeftViewport("DeltaTime (AVG)", DeltaTime);
		}

		/// <summary>
		/// Adds a delta time to the delta time queue.
		/// </summary>
		/// <param name="DeltaTime">The delta time to add.</param>
		private void AddToDeltaTimeQueue(float DeltaTime)
		{
			if (mDeltaTimeQueue.Count >= kDeltaTimeQueueMaxElements)
			{
				mDeltaTimeQueue.Dequeue();
			}

			mDeltaTimeQueue.Enqueue(DeltaTime);
		}

		/// <summary>
		/// The average delta time based on the last <c>kDeltaTimeQueueMaxElements</c> = 60 frames.
		/// </summary>
		public float DeltaTime
		{
			get
			{
				if (mDeltaTimeQueue.Count == 0)
				{
					return 0f;
				}

				if (mDeltaTimeQueue.Count == 1)
				{
					return mDeltaTimeQueue.Peek();
				}

				float Aggregated = 0f;

				foreach (float DeltaTimeElement in mDeltaTimeQueue)
				{
					Aggregated += DeltaTimeElement;
				}

				if (Aggregated == 0f)
				{
					return Aggregated;
				}

				float Average = Aggregated / mDeltaTimeQueue.Count;

				return Average > 0f ? Average : 0f;
			}
		}

		/// <summary>
		/// The elapsed milliseconds since the last frame.
		/// </summary>
		public float ElapsedMilliseconds { get; private set; }

		/// <summary>
		/// The total milliseconds since the game started.
		/// </summary>
		public float TotalMilliseconds { get; private set; }

		/// <summary>
		/// The <see cref="Diagnostics.DebugInfo"/> instance.
		/// </summary>
		public static DebugInfo DebugInfo => DebugInfo.Instance;

		/// <summary>
		/// The unique instance of <c>TimeManager</c>.
		/// </summary>
		public static TimeManager Instance { get; } = new TimeManager();
	}
}
