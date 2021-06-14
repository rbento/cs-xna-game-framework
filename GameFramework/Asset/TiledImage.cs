// Copyright (c) Rodrigo Bento

using Microsoft.Xna.Framework;

using static GameFramework.Diagnostics.Logger;

namespace GameFramework.Asset
{
	/// <summary>
	/// Defines an image divided in tiles that can be accessed by index.
	/// </summary>
	public class TiledImage : GameImage
	{
		/// <summary>
		/// The width of a frame within the image.
		/// </summary>
		protected int mFrameWidth;

		/// <summary>
		/// The height of a frame within the image.
		/// </summary>
		protected int mFrameHeight;

		/// <summary>
		/// The number of frames to be indexed.
		/// </summary>
		protected int mFrameCount;

		/// <summary>
		/// The number of frames to be iterated on a per-row basis.
		/// </summary>
		protected int mFramesPerRow;

		/// <summary>
		/// The current frame index, from zero to <c>mFrameCount</c>.
		/// </summary>
		protected int mCurrentFrameIndex;

		/// <summary>
		/// The source rectangle of each individual frame.
		/// </summary>
		protected Rectangle[] mSourceRects;

		/// <summary>
		/// Creates an instance of <c>TiledImage</c> based on a Content Pipeline asset.
		/// </summary>
		/// <param name="Asset">The content pipeline asset.</param>
		/// <param name="FrameWidth">The desired frame width.</param>
		/// <param name="FrameHeight">The desired frame height.</param>
		/// <param name="FrameCount">The number of frames to be indexed.</param>
		public TiledImage(string Asset, int FrameWidth, int FrameHeight, int FrameCount)
			: base(Asset)
		{
			this.mFrameWidth = FrameWidth;
			this.mFrameHeight = FrameHeight;
			this.mFrameCount = FrameCount;

			mFramesPerRow = Texture.Width / FrameWidth;
			mTextureData = new Color[FrameWidth * FrameHeight];
			mCurrentFrameIndex = 0;

			Origin = new Vector2(FrameWidth / 2, FrameHeight / 2);

			SetSourceRects();
		}

		/// <summary>
		/// Calculates and populates the source rectangles according to this <c>TiledImage</c> attributes.
		/// </summary>
		protected void SetSourceRects()
		{
			if (mFrameCount == 0) return;

			mSourceRects = new Rectangle[mFrameCount];

			for (int Index = 0; Index < mFrameCount; Index++)
			{
				mSourceRects[Index] = new Rectangle((Index % mFramesPerRow) * mFrameWidth, (Index / mFramesPerRow) * mFrameHeight, mFrameWidth, mFrameHeight);

				Debug($"[SetSourceRects] SourceRects[{Index}]: {mSourceRects[Index]}");
			}
		}

		/// <summary>
		/// Returns an array of <c>Color</c> for each pixel according to the current frame index, frame width and height.
		/// </summary>
		public override Color[] TextureData
		{
			get
			{
				Texture.GetData(0, mSourceRects[mCurrentFrameIndex], mTextureData, 0, mFrameWidth * mFrameHeight);
				return mTextureData;
			}
		}

		/// <summary>
		/// The source rectangle according to the current frame index.
		/// </summary>
		public override Rectangle? SourceRect
		{
			get { return mSourceRects[mCurrentFrameIndex]; }
		}

		/// <summary>
		/// The current frame index.
		/// </summary>
		public int CurrentFrameIndex
		{
			get { return mCurrentFrameIndex; }
			set { mCurrentFrameIndex = value >= 0 && value < mFrameCount ? value : 0; }
		}

		/// <summary>
		/// The frame width.
		/// </summary>
		public override int Width
		{
			get { return mFrameWidth; }
		}

		/// <summary>
		/// The frame height.
		/// </summary>
		public override int Height
		{
			get { return mFrameHeight; }
		}

		/// <summary>
		/// The top position of this image relative to its current position.
		/// </summary>
		public override float Top
		{
			get { return Y - (Height / 2); }
		}

		/// <summary>
		/// The right position of this image relative to its current position.
		/// </summary>
		public override float Right
		{
			get { return X + (Width / 2); }
		}

		/// <summary>
		/// The bottom position of this image relative to its current position.
		/// </summary>
		public override float Bottom
		{
			get { return Y + (Height / 2); }
		}

		/// <summary>
		/// The left position of this image relative to its current position.
		/// </summary>
		public override float Left
		{
			get { return X - (Width / 2); }
		}
	}
}
