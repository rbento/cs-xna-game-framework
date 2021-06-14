// Copyright (c) Rodrigo Bento

using GameFramework.Base;
using GameFramework.Effect;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.IO;

using static GameFramework.Diagnostics.Logger;

namespace GameFramework.Asset
{
	/// <summary>
	/// The basic Game Image / Sprite.
	/// </summary>
	/// <remarks>
	/// Contains all basic operations necessary to load, tint, fade, rotate, place and draw an image.
	/// </remarks>
	public class GameImage : IDraw
	{
		/// <summary>
		/// The color to tint the image.
		/// </summary>
		protected Color mColor;

		/// <summary>
		/// The location (in screen coordinates) to draw the <c>GameImage</c>.
		/// </summary>
		protected Vector2 mPosition;

		/// <summary>
		/// Specifies the angle (in radians) to rotate the <c>GameImage</c> about its center. 
		/// </summary>
		protected float mRotation;

		/// <summary>
		/// Container for texture data.
		/// </summary>
		/// <remarks>
		/// Contains <c>Color</c> for each individual pixel.
		/// </remarks>
		protected Color[] mTextureData;

		/// <summary>
		/// Creates an instance of <c>GameImage</c>.
		/// </summary>
		private GameImage()
		{
			mTextureData = new Color[0];

			Position = Vector2.Zero;
			Origin = Vector2.Zero;

			Color = Color.White;
			SpriteEffect = SpriteEffects.None;

			Rotation = 0f;
			Scale = 1f;
			LayerDepth = 0f;

			Red = 255;
			Green = 255;
			Blue = 255;
			Alpha = 255;

			FadeEffect = new FadeEffect();

			Debug("[GameImage] Created");
		}

		/// <summary>
		/// Creates an instance of <c>GameImage</c>.
		/// </summary>
		/// <param name="ResourceName">The name of the Content Pipeline asset or filepath to load.</param>
		/// <param name="LoadFromStream">Whether to load the resource from a stream.</param>
		public GameImage(string ResourceName, bool LoadFromStream = false)
			: this()
		{
			if (LoadFromStream)
			{
				if (!this.LoadFromStream(ResourceName))
				{
					Error("[GameImage] Unable to load resource from stream.");
				}
			}
			else
			{
				if (!Load(ResourceName))
				{
					Error("[GameImage] Unable to load Content Pipeline asset.");
				}
			}
		}

		/// <summary>
		/// Creates an instance of <c>GameImage</c>.
		/// </summary>
		/// <param name="Asset">The name of the Content Pipeline asset to load.</param>
		/// <param name="Position">The location(in screen coordinates) to place this <c>GameImage</c>.</param>
		public GameImage(string Asset, Vector2 Position)
			: this(Asset)
		{
			this.Position = Position;
		}

		/// <summary>
		/// Creates an instance of <c>GameImage</c>.
		/// </summary>
		/// <param name="Asset">The name of the Content Pipeline asset to load.</param>
		/// <param name="X">The X location(in screen coordinates) to place this <c>GameImage</c>.</param>
		/// <param name="Y">The Y location(in screen coordinates) to place this <c>GameImage</c>.</param>
		public GameImage(string Asset, float X, float Y)
			: this(Asset, new Vector2(X, Y))
		{
		}

		/// <summary>
		/// Loads texture data from a file (stream).
		/// </summary>
		/// <remarks>
		/// The result is written to this <c>GameImage</c>'s Texture.
		/// </remarks>
		/// <param name="RelativePath">The relative path of the image file to load.</param>
		/// <returns><c>true</c> if the texture data is loaded, otherwise <c>false</c>.</returns>
		protected bool LoadFromStream(string RelativePath)
		{
			return Load(RelativePath, true);
		}

		/// <summary>
		/// Loads texture data from an asset processed content pipeline.
		/// </summary>
		/// <remarks>
		/// The result is written to this <c>GameImage</c>'s Texture.
		/// </remarks>
		/// <param name="Asset">The name of the Content Pipeline asset to load.</param>
		/// <returns><c>true</c> if the texture data is loaded, otherwise <c>false</c>.</returns>
		protected bool Load(string Asset)
		{
			return Load(Asset, false);
		}

		/// <summary>
		/// Loads texture data either from an asset processed content pipeline or from a file (stream).
		/// </summary>
		/// <remarks>
		/// The result is written to this <c>GameImage</c>'s Texture.
		/// </remarks>
		/// <param name="ResourceName">The name of the asset or filepath to load.</param>
		/// <param name="bFromStream">Tells whether the resource being loaded is a file rather than an asset.</param>
		/// <returns><c>true</c> if the texture data is loaded, otherwise <c>false</c>.</returns>
		private bool Load(string ResourceName, bool bFromStream)
		{
			if (null == ResourceName || ResourceName.Length == 0)
			{
				Error("[Load] Trying to load with a null/empty asset value");

				return false;
			}

			try
			{
				if (bFromStream)
				{
					Stream Stream = TitleContainer.OpenStream(ResourceName);
					Texture = Texture2D.FromStream(Core.Instance.GraphicsDevice, Stream);
				}
				else
				{
					Texture = ContentManager.Load<Texture2D>(ResourceName);
				}

				SourceRect = new Rectangle(0, 0, Width, Height);
				Origin = new Vector2(Width / 2, Height / 2);
				Scale = 1f;

				return true;
			}
			catch (ContentLoadException e)
			{
				Error(e.InnerException.Message);
			}

			return false;
		}

		/// <summary>
		/// Draws this <c>GameImage</c> to the screen.
		/// </summary>
		/// <remarks>
		/// See <see cref="IDraw.Draw(GameTime)"/>
		/// </remarks>
		public virtual void Draw(GameTime GameTime)
		{
			if (null == Texture) return;
			if (null == SpriteBatch) return;

			FadeEffect.Apply(GameTime);

			SpriteBatch.Draw(Texture, Position, SourceRect, Color * FadeEffect.Alpha, Rotation, Origin, Scale, SpriteEffect, LayerDepth);
		}

		/// <summary>
		/// Tells whether the mouse pointer is over this image according to its position and dimension.
		/// </summary>
		/// <param name="MousePosition">A vector representing the mouse pointer position.</param>
		/// <returns><c>true</c> if the mouse position intersects with this image, otherwise <c>false</c>.</returns>
		public bool MouseOver(Vector2 MousePosition)
		{
			return MousePosition.X >= X - Origin.X &&
				   MousePosition.X <= X - Origin.X + Width &&
				   MousePosition.Y >= Y - Origin.Y &&
				   MousePosition.Y <= Y - Origin.Y + Height;
		}

		/// <summary>
		/// Retrieve the color of a pixel according to the given position vector.
		/// </summary>
		/// <param name="Position">The desired pixel position.</param>
		/// <returns>A <c>Color</c> if the given position is not out of bounds, otherwise <c>Color.Black</c>.</returns>
		public Color PixelAtPosition(Vector2 Position)
		{
			Color[] Pixels = TextureData;
			int Index = (int)Position.Y * SourceRect.Value.Width + (int)Position.X;
			return (Index >= 0 && Index < Pixels.Length) ? Pixels[Index] : Color.Black;
		}

		/// <summary>
		/// Tests whether pixels of this image intersect with pixels of a given image.
		/// </summary>
		/// <param name="Other">The <c>GameImage</c> to test pixel intersection against.</param>
		/// <returns><c>true</c> if pixels intersect, otherwise <c>false</c>.</returns>
		public bool PixelCollidesWith(GameImage Other)
		{
			Color[] ColorBytes = TextureData;
			Color[] OtherColorBytes = Other.TextureData;

			Rectangle TextureRect = (Rectangle)SourceRect;
			Rectangle OtherTextureRect = (Rectangle)Other.SourceRect;

			int X1 = Math.Max(TextureRect.X, OtherTextureRect.X);
			int X2 = Math.Min(TextureRect.X + TextureRect.Width, OtherTextureRect.X + OtherTextureRect.Width);

			int Y1 = Math.Max(TextureRect.Y, OtherTextureRect.Y);
			int Y2 = Math.Min(TextureRect.Y + TextureRect.Height, OtherTextureRect.Y + OtherTextureRect.Height);

			for (int Y = Y1; Y < Y2; ++Y)
			{
				for (int X = X1; X < X2; ++X)
				{
					if (((ColorBytes[(X - TextureRect.X) + (Y - TextureRect.Y) * TextureRect.Width].PackedValue & 0xFF000000) >> 24) > 20 &&
						((OtherColorBytes[(X - OtherTextureRect.X) + (Y - OtherTextureRect.Y) * OtherTextureRect.Width].PackedValue & 0xFF000000) >> 24) > 20)
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Immediatelly hides an image.
		/// </summary>
		public void Hide()
		{
			FadeEffect.Hide();
		}

		/// <summary>
		/// Immediatelly shows an image.
		/// </summary>
		public void Show()
		{
			FadeEffect.Show();
		}

		/// <summary>
		/// Interpolates on the image alpha up to <c>1</c>.
		/// </summary>
		/// <param name="Duration">The amount of time in milliseconds that the interpolation should take to complete.</param>
		/// <param name="Delay">The delay before starting the interpolation.</param>
		public void FadeIn(EffectDuration Duration = EffectDuration.Default, EffectDuration Delay = EffectDuration.Immediate)
		{
			FadeEffect.FadeIn(Duration, Delay);
		}

		/// <summary>
		/// Interpolates on the image alpha down to <c>0</c>.
		/// </summary>
		/// <param name="Duration">The amount of time in milliseconds that the interpolation should take to complete.</param>
		/// <param name="Delay">The delay before starting the interpolation.</param>
		public void FadeOut(EffectDuration Duration = EffectDuration.Default, EffectDuration Delay = EffectDuration.Immediate)
		{
			FadeEffect.FadeOut(Duration, Delay);
		}

		/// <summary>
		/// Tells whether the fade effect is active.
		/// </summary>
		/// <returns><c>true</c> if it is active, otherwise <c>false</c>.</returns>        
		public bool IsFading()
		{
			return FadeEffect.IsApplying;
		}

		/// <summary>
		/// Tells whether the fade effect is done applying.
		/// </summary>
		/// <returns><c>true</c> if it is done fading, otherwise <c>false</c>.</returns>        
		public bool IsDoneFading()
		{
			return FadeEffect.IsDone;
		}

		/// <summary>
		/// The rectangle according to current position and dimension.
		/// </summary>
		public Rectangle Rectangle
		{
			get { return new Rectangle((int)(X - Origin.X), (int)(Y - Origin.Y), Width, Height); }
		}

		/// <summary>
		/// Container for texture data.
		/// </summary>
		/// <remarks>
		/// Contains <c>Color</c> for each individual pixel.
		/// </remarks>
		public virtual Color[] TextureData
		{
			get
			{
				if (mTextureData.Length == 0)
				{
					mTextureData = new Color[Width * Height];
					Texture.GetData(mTextureData);
				}
				return mTextureData;
			}
		}

		/// <summary>
		/// A byte representing the Red color component to tint the image.
		/// </summary>
		public byte Red
		{
			get { return mColor.R; }
			set { mColor.R = value; }
		}

		/// <summary>
		/// A byte representing the Green color component to tint the image. 
		/// </summary>
		public byte Green
		{
			get { return mColor.G; }
			set { mColor.G = value; }
		}

		/// <summary>
		/// A byte representing the Blue color component to tint the image. 
		/// </summary>
		public byte Blue
		{
			get { return mColor.B; }
			set { mColor.B = value; }
		}

		/// <summary>
		/// A byte representing the Alpha color component to tint the image.
		/// </summary>
		public byte Alpha
		{
			get { return mColor.A; }
			set { mColor.A = value; }
		}

		/// <summary>
		/// The image X position.
		/// </summary>
		public float X
		{
			get { return Position.X; }
			set { mPosition.X = value; }
		}

		/// <summary>
		/// The image Y position.
		/// </summary>
		public float Y
		{
			get { return Position.Y; }
			set { mPosition.Y = value; }
		}

		/// <summary>
		/// The image width according to its texture.
		/// </summary>
		public virtual int Width
		{
			get { return Texture.Width; }
		}

		/// <summary>
		/// The image height according to its texture.
		/// </summary>
		public virtual int Height
		{
			get { return Texture.Height; }
		}

		/// <summary>
		/// The top side of this <c>GameImage</c> at its current position.
		/// </summary>
		public virtual float Top
		{
			get { return Y; }
		}

		/// <summary>
		/// The right side of this <c>GameImage</c> at its current position.
		/// </summary>
		public virtual float Right
		{
			get { return X + Width; }
		}

		/// <summary>
		/// The bottom side of this <c>GameImage</c> at its current position.
		/// </summary>
		public virtual float Bottom
		{
			get { return Y + Height; }
		}

		/// <summary>
		/// The left side of this <c>GameImage</c> at its current position.
		/// </summary>
		public virtual float Left
		{
			get { return X; }
		}

		/// <summary>
		/// The image fade effect.
		/// </summary>
		public FadeEffect FadeEffect { get; set; }

		/// <summary>
		/// A texture.
		/// </summary>
		public Texture2D Texture { get; private set; }

		/// <summary>
		/// The location (in screen coordinates) to draw the <c>GameImage</c>.
		/// </summary>
		public Vector2 Position
		{
			get { return mPosition; }
			set { mPosition = value; }
		}

		/// <summary>
		/// Specifies the angle (in radians) to rotate the <c>GameImage</c> about its center. 
		/// </summary>
		public float Rotation
		{
			get { return mRotation; }
			set { mRotation = MathHelper.WrapAngle(value); }
		}

		/// <summary>
		/// A rectangle that specifies (in texels) the source texels from a texture.  
		/// </summary>
		/// <remarks>
		/// Use null to draw the entire texture.
		/// </remarks>
		public virtual Rectangle? SourceRect { get; set; }

		/// <summary>
		/// The sprite origin; the default is (0,0) which represents the upper-left corner.
		/// </summary>
		public Vector2 Origin { get; set; }

		/// <summary>
		/// Scale factor. 
		/// </summary>
		public float Scale { get; set; }

		/// <summary>
		/// The depth of a layer. 
		/// </summary>
		/// <remarks>
		/// By default, 0 represents the front layer and 1 represents a back layer. 
		/// Use <c>SpriteSortMode</c> sprites are to be sorted during drawing.
		/// </remarks>
		public float LayerDepth { get; set; }

		/// <summary>
		/// The color to tint a sprite. 
		/// </summary>
		/// <remarks>
		/// Use <c>Color.White</c> for full color with no tinting.
		/// </remarks>
		public Color Color { get; set; }

		/// <summary>
		/// The effects to apply while drawing.
		/// </summary>
		public SpriteEffects SpriteEffect { get; set; }

		/// <summary>
		/// The <see cref="Core.Content"/> instance.
		/// </summary>
		protected ContentManager ContentManager => Core.Content;

		/// <summary>
		/// The <see cref="Core.SpriteBatch"/> instance.
		/// </summary>
		protected SpriteBatch SpriteBatch => Core.SpriteBatch;

		/// <summary>
		/// The <see cref="GameFramework.Core"/> instance.
		/// </summary>
		protected Core Core => Core.Instance;
	}
}
