// Copyright (c) Rodrigo Bento

using GameFramework.Util;

using Microsoft.Xna.Framework;

namespace GameFramework.Asset
{
	/// <summary>
	/// Defines animation properties with allows switching between tiles of a tiled image in a timely manner.
	/// </summary>
	public class AnimatedImage : TiledImage
	{
		/// <summary>
		/// Duration of each frame in milliseconds.
		/// </summary>
		private long mTimePerFrame;

		/// <summary>
		/// The total elapsed time when an animation is playing.
		/// </summary>
		private long mTotalElapsed;

		/// <summary>
		/// Animation will play, starting from this frame index.
		/// </summary>
		private int mFromFrameIndex;

		/// <summary>
		/// Animation will play, ending at this frame index.
		/// </summary>
		private int mToFrameIndex;

		/// <summary>
		/// Whether the animation is paused.
		/// </summary>
		private bool bIsPaused;

		/// <summary>
		/// Creates an instance of <c>AnimatedImage</c>.
		/// </summary>
		/// <param name="Asset">The tiled image asset to be loaded.</param>
		/// <param name="FrameWidth">The desired frame width.</param>
		/// <param name="FrameHeight">The desired frame height.</param>
		/// <param name="FrameCount">The number of frames to be considered.</param>
		/// <param name="FrameTime">The time per frame.</param>
		public AnimatedImage(string Asset, int FrameWidth, int FrameHeight, int FrameCount, long FrameTime)
			: base(Asset, FrameWidth, FrameHeight, FrameCount)
		{
			mFromFrameIndex = 0;
			mToFrameIndex = base.mFrameCount - 1;
			mTotalElapsed = 0;
			mTimePerFrame = FrameTime;
			SetSourceRects();
		}

		/// <summary>
		/// See <see cref="Base.IUpdate.Update(GameTime)"/>
		/// </summary>
		public void Update(GameTime GameTime)
		{
			if (!bIsPaused && mTimePerFrame > 0)
			{
				mTotalElapsed += GameTime.ElapsedGameTime.Milliseconds;

				if (mTotalElapsed > mTimePerFrame)
				{
					mCurrentFrameIndex = Maths.Wrap(mCurrentFrameIndex + 1, FromFrameIndex, ToFrameIndex);
					mTotalElapsed -= mTimePerFrame;
				}
			}
		}

		/// <summary>
		/// See <see cref="Base.IDraw.Draw(GameTime)"/>
		/// </summary>
		public override void Draw(GameTime GameTime)
		{
			base.Draw(GameTime);
		}

		/// <summary>
		/// Pauses the current animation.
		/// </summary>
		public void Pause()
		{
			bIsPaused = true;
		}

		/// <summary>
		/// Unpauses the current animation.
		/// </summary>
		public void Unpause()
		{
			bIsPaused = false;
		}

		/// <summary>
		/// Animation will play, starting from this frame index.
		/// </summary>
		public int FromFrameIndex
		{
			get { return mFromFrameIndex; }
			set { mFromFrameIndex = Maths.Clamp(value, 0, mFrameCount - 1); }
		}

		/// <summary>
		/// Animation will play, ending at this frame index.
		/// </summary>
		public int ToFrameIndex
		{
			get { return mToFrameIndex; }
			set { mToFrameIndex = Maths.Clamp(value, 0, mFrameCount - 1); }
		}
	}
}
