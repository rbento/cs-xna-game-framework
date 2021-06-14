// Copyright (c) Rodrigo Bento

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace GameFramework.Diagnostics
{
	/// <summary>
	/// Enables on-screen vector drawing for debugging purposes.
	/// </summary>
	public class DebugDraw
	{
		/// <summary>
		/// The default vertex buffer size.
		/// </summary>
		private const int kDefaultVertexBufferSize = 512;

		/// <summary>
		/// The max vertex buffer index.
		/// </summary>
		private const int kMaxVertexBufferIndex = kDefaultVertexBufferSize - 1;

		/// <summary>
		/// The basic Xna rendering effect to be applied onto the vertices being drawn.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="BasicEffect"/>
		/// </remarks>
		private readonly BasicEffect mBasicEffect;

		/// <summary>
		/// Container for the current vertices being drawn in a cycle.
		/// </summary>
		/// <remarks>
		/// See also <seealso cref="VertexPositionColor"/>
		/// </remarks>
		private readonly VertexPositionColor[] mVertices;

		/// <summary>
		/// The current primitive type being drawn in a cycle.
		/// </summary>
		/// <remarks>
		/// Value is reset every <c>Begin</c>/<c>End</c> cycle.
		/// </remarks>
		private PrimitiveType mPrimitiveType;

		/// <summary>
		/// The number of vertices per primitive being drawn in a cycle.
		/// </summary>
		/// <remarks>
		/// Value is reset every <c>Begin</c>/<c>End</c> cycle.
		/// </remarks>
		private int mNumVertsPerPrimitive;

		/// <summary>
		/// The vertex index in buffer being drawn in a cycle.
		/// </summary>
		/// <remarks>
		/// Value is reset every <c>Begin</c>/<c>End</c> cycle.
		/// </remarks>
		private int mVertexIndex;

		/// <summary>
		/// Whether <c>Begin</c> has been called.
		/// </summary>
		private bool bBeginHasBeenCalled;

		/// <summary>
		/// Creates an instance of <c>DebugDraw</c>.
		/// </summary>
		private DebugDraw()
		{
			mBasicEffect = new BasicEffect(GraphicsDevice)
			{
				Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1),
				VertexColorEnabled = true
			};

			mVertices = new VertexPositionColor[kDefaultVertexBufferSize];

			mVertexIndex = 0;

			bBeginHasBeenCalled = false;
			IsVisible = false;
		}

		/// <summary>
		/// Draws a rectangle on the screen.
		/// </summary>
		/// <param name="Rect">The rectangle to draw.</param>
		/// <param name="Color">The desired color.</param>
		public void DrawRectangle(Rectangle Rect, Color Color)
		{
			if (!IsVisible) return;

			Vector2 Top = new Vector2(Rect.X, Rect.Y);
			Vector2 Right = new Vector2(Rect.X + Rect.Width, Rect.Y);
			Vector2 Bottom = new Vector2(Rect.X + Rect.Width, Rect.Y + Rect.Height);
			Vector2 Left = new Vector2(Rect.X, Rect.Y + Rect.Height);

			DrawLine(Top, Right, Color);
			DrawLine(Right, Bottom, Color);
			DrawLine(Bottom, Left, Color);
			DrawLine(Left, Top, Color);
		}

		/// <summary>
		/// Draws a point on the screen.
		/// </summary>
		/// <param name="Point">The point to draw.</param>
		/// <param name="Color">The desired color.</param>
		public void DrawPoint(Vector2 Point, Color Color)
		{
			if (!IsVisible) return;

			Begin(PrimitiveType.TriangleList);
			AddVertex(Point, Color);
			AddVertex(Point + Vector2.UnitX, Color);
			AddVertex(Point + Vector2.UnitY, Color);
			End();
		}

		/// <summary>
		/// Draws a line on the screen.
		/// </summary>
		/// <param name="A">The A vertex to draw.</param>
		/// <param name="B">The B vertex to draw.</param>
		/// <param name="Color">The desired color.</param>
		public void DrawLine(Vector2 A, Vector2 B, Color Color)
		{
			if (!IsVisible) return;

			Begin(PrimitiveType.LineList);
			AddVertex(A, Color);
			AddVertex(B, Color);
			End();
		}

		/// <summary>
		/// Begins the drawing cycle for a given primitive.
		/// </summary>
		/// <param name="Type">The primitive type to draw.</param>
		/// <exception cref="InvalidOperationException">When mismatch between calls to <c>Begin</c>/<c>End</c> is detected.</exception>
		/// <exception cref="NotSupportedException">When the primitive type is invalid.</exception>
		private void Begin(PrimitiveType Type)
		{
			if (!IsVisible) return;

			if (bBeginHasBeenCalled)
			{
				throw new InvalidOperationException("[End] must be called before [Begin] can be called again");
			}

			if (Type == PrimitiveType.LineStrip || Type == PrimitiveType.TriangleStrip)
			{
				throw new NotSupportedException("The specified type is not supported by [PrimitiveBatch]");
			}

			mPrimitiveType = Type;
			mNumVertsPerPrimitive = NumVertsPerPrimitiveType(Type);
			mBasicEffect.CurrentTechnique.Passes[0].Apply();

			bBeginHasBeenCalled = true;
		}

		/// <summary>
		/// Add a vertex to the drawing buffer.
		/// </summary>
		/// <param name="Vertex">The vertex to add.</param>
		/// <param name="Color">The desired color.</param>
		/// <exception cref="InvalidOperationException">When this method is called before <c>Begin</c>.</exception>
		public void AddVertex(Vector2 Vertex, Color Color)
		{
			if (!IsVisible) return;

			if (!bBeginHasBeenCalled)
			{
				throw new InvalidOperationException("[Begin] must be called before [AddVertex]");
			}

			if (mVertexIndex >= kMaxVertexBufferIndex)
			{
				Flush();
			}

			mVertices[mVertexIndex].Position = new Vector3(Vertex, 0);
			mVertices[mVertexIndex].Color = Color;

			mVertexIndex++;
		}

		/// <summary>
		/// Ends the drawing cycle for a given primitive.
		/// </summary>
		private void End()
		{
			if (!IsVisible) return;

			if (!bBeginHasBeenCalled)
			{
				throw new InvalidOperationException("[Begin] must be called before [End]");
			}

			Flush();

			bBeginHasBeenCalled = false;
		}

		/// <summary>
		/// Draws the current vertices by sending them to the <c>GraphicsDevice</c>.
		/// </summary>
		public void Flush()
		{
			if (!IsVisible || mVertexIndex == 0) return;

			int PrimitiveCount = mVertexIndex / mNumVertsPerPrimitive;

			GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(mPrimitiveType, mVertices, 0, PrimitiveCount);

			mVertexIndex = 0;
		}

		/// <summary>
		/// Gets the number of vertices according to the given primitive type.
		/// </summary>
		/// <param name="Type">The primitive type to check.</param>
		/// <returns>The number of vertices.</returns>
		/// <exception cref="InvalidOperationException">When the primitive type is invalid.</exception>
		private static int NumVertsPerPrimitiveType(PrimitiveType Type)
		{
			switch (Type)
			{
				case PrimitiveType.LineList:
					return 2;
				case PrimitiveType.TriangleList:
					return 3;
				default:
					throw new InvalidOperationException("Invalid [PrimitiveType]");
			}
		}

		/// <summary>
		/// Toggles the <c>DebugDraw/c> visibility.
		/// </summary>
		public void ToggleVisibility()
		{
			IsVisible = !IsVisible;
		}

		/// <summary>
		/// Whether this <c>DebugDraw</c> is visible.
		/// </summary>
		public bool IsVisible { get; private set; }

		/// <summary>
		/// The <see cref="Microsoft.Xna.Framework.GraphicsDevice"/> instance.
		/// </summary>
		private GraphicsDevice GraphicsDevice => Core.Instance.GraphicsDevice;

		/// <summary>
		/// The unique instance of <c>DebugDraw</c>.
		/// </summary>
		public static DebugDraw Instance { get; } = new DebugDraw();
	}
}
