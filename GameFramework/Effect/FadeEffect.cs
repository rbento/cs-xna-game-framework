// Copyright (c) Rodrigo Bento

using Microsoft.Xna.Framework;

using static GameFramework.Diagnostics.Logger;

namespace GameFramework.Effect
{
	/// <summary>
	/// The accepted fade modes.
	/// </summary>
	public enum FadeMode
	{
		None,
		In = 1,
		Out = -1
	}

	/// <summary>
	/// A few default contants to representing an effect duration.
	/// </summary>
	public enum EffectDuration
	{
		Immediate = 0,
		VeryFast = 200,
		Fast = 500,
		Default = 800,
		Slow = 1600,
		VerySlow = 2400,
		HalfASec = 500,
		OneSec = 1000,
		OneAndAHalfSec = 1500,
		TwoSecs = 2000,
		ThreeSecs = 3000,
		FourSecs = 4000,
		FiveSecs = 5000,
		SixSecs = 6000,
		SevenSecs = 7000,
		EightSecs = 8000,
		NineSecs = 5000,
		TenSecs = 10000
	}

	/// <summary>
	/// Defines a fade effect. 
	/// </summary>
	/// <remarks>
	/// Enables the interpotation over a value of alpha in a timely manner.
	/// </remarks>
	public class FadeEffect : IEffect
	{
		/// <summary>
		/// The minimum value for any color.
		/// </summary>
		public const float kColorTransparent = 0.0f;

		/// <summary>
		/// The maximum value for any color.
		/// </summary>
		public const float kColorOpaque = 1.0f;

		/// <summary>
		/// The desired delay before starting the interpolation.
		/// </summary>
		private double mDelayDuration;

		/// <summary>
		/// The delay elapsed time counter.
		/// </summary>
		private double mDelayElapsedTime;

		/// <summary>
		/// The delay start time marker.
		/// </summary>
		private double mDelayStartTime;

		/// <summary>
		/// The amount of time taken to interpolate the alpha value according to the mode.
		/// </summary>
		private double mEffectDuration;

		/// <summary>
		/// The effect elapsed time counter.
		/// </summary>
		private double mEffectElapsedTime;

		/// <summary>
		/// The effect start time marker.
		/// </summary>
		private double mEffectStartTime;

		/// <summary>
		/// Creates an instance of <c>FadeEffect</c>.
		/// </summary>
		public FadeEffect()
		{
			Mode = FadeMode.In;
			Alpha = kColorOpaque;

			mDelayDuration = 0.0d;
			mDelayElapsedTime = 0.0d;
			mDelayStartTime = 0.0d;

			mEffectDuration = 0.0d;
			mEffectElapsedTime = 0.0d;
			mEffectStartTime = 0.0d;

			IsPrepared = false;
			IsApplying = false;
			IsApplied = true;
		}

		/// <summary>
		/// Applies the fade in effect. 
		/// </summary>
		/// <remarks>
		/// Alpha changes from 0 to 255.
		/// </remarks>
		/// <param name="EffectDuration">The amount of time taken to interpolate the alpha value from 0 to 255.</param>
		/// <param name="DelayDuration">The desired delay before starting the interpolation.</param>
		public void FadeIn(EffectDuration EffectDuration, EffectDuration DelayDuration = 0)
		{
			Fade(FadeMode.In, EffectDuration, DelayDuration);
		}

		/// <summary>
		/// Applies the fade out effect. 
		/// </summary>
		/// <remarks>
		/// Alpha changes from 255 to 0.
		/// </remarks>
		/// <param name="EffectDuration">The amount of time taken to interpolate the alpha value from 255 to 0.</param>
		/// <param name="DelayDuration">The desired delay before starting the interpolation.</param>
		public void FadeOut(EffectDuration EffectDuration, EffectDuration DelayDuration = 0)
		{
			Fade(FadeMode.Out, EffectDuration, DelayDuration);
		}

		/// <summary>
		/// Runs the fade effect.
		/// </summary>
		/// <param name="Mode">The desired mode to run.</param>
		/// <param name="EffectDuration">The amount of time taken to interpolate the alpha value according to the mode.</param>
		/// <param name="DelayDuration">The desired delay before starting the interpolation.</param>
		private void Fade(FadeMode Mode, EffectDuration EffectDuration, EffectDuration DelayDuration)
		{
			if (IsApplying) return;

			if (this.Mode == Mode) return;

			this.Mode = Mode;

			mDelayDuration = (double)DelayDuration;
			mEffectDuration = (double)EffectDuration;

			Prepare();

			Debug($"[Fade] Fading with Mode {this.Mode}, Duration {mEffectDuration}, Delay {mDelayDuration}");
		}

		/// <summary>
		/// Set the appropriate state to an effect that is about to be applied.
		/// </summary>
		public void Prepare()
		{
			if (IsApplying) return;

			mDelayElapsedTime = 0;
			mDelayStartTime = 0;

			mEffectElapsedTime = 0;
			mEffectStartTime = 0;

			Alpha = (Mode == FadeMode.In) ? kColorTransparent : kColorOpaque;

			IsPrepared = true;

			Debug($"[Prepare] Prepared with Alpha {Alpha}, Mode {Mode}");
		}

		/// <summary>
		/// Applies the fade effect according to the mode set.
		/// </summary>
		/// <param name="GameTime">The current time delta in milliseconds.</param>
		public void Apply(GameTime GameTime)
		{
			if (!IsApplying && IsPrepared)
			{
				IsPrepared = false;
				IsApplying = true;
				IsApplied = false;

				Debug("[Apply] Starting");
			}

			if (IsApplied)
			{
				return;
			}

			if (mEffectDuration == 0)
			{
				Debug("[Apply] Immediate Mode - Skipping");

				DoneApplying();

				return;
			}

			if (IsApplying)
			{
				if (mDelayDuration > 0)
				{
					if (mDelayStartTime == 0)
					{
						mDelayStartTime = GameTime.TotalGameTime.TotalMilliseconds;
					}

					mDelayElapsedTime = GameTime.TotalGameTime.TotalMilliseconds - mDelayStartTime;

					if (mDelayElapsedTime <= mDelayDuration)
					{
						return;
					}
				}

				if (mEffectStartTime == 0)
				{
					mEffectStartTime = GameTime.TotalGameTime.TotalMilliseconds;
				}

				mEffectElapsedTime = GameTime.TotalGameTime.TotalMilliseconds - mEffectStartTime;

				if (mEffectElapsedTime <= mEffectDuration)
				{
					double Progress = mEffectElapsedTime * 100 / mEffectDuration;

					Debug($"[Apply] Progress: {Progress}");

					if (Progress > 0)
					{
						float IntermediateAlphaValue = (float)(Progress * kColorOpaque / 100.0d);

						Alpha = (Mode == FadeMode.In) ? IntermediateAlphaValue : 1.0f - IntermediateAlphaValue;

						Debug($"[Apply] Alpha: {Alpha}");
					}
				}
				else
				{
					DoneApplying();
				}
			}
		}

		/// <summary>
		/// Sets the appropriate state when an effect is done applying.
		/// </summary>
		private void DoneApplying()
		{
			if (mEffectDuration == 0)
			{
				Debug("[DoneApplying] - Alpha is transparent");

				Alpha = kColorTransparent;
			}
			else
			{
				Alpha = (Mode == FadeMode.In) ? kColorOpaque : kColorTransparent;
			}

			IsPrepared = false;
			IsApplying = false;
			IsApplied = true;

			Debug("[DoneApplying] Alpha / Mode : {mAlpha} / {mCurrentMode}");
		}

		/// <summary>
		/// Causes the effect to immediately fade in.
		/// </summary>
		public void Show()
		{
			Alpha = kColorOpaque;
			Mode = FadeMode.In;
			IsApplying = false;
			IsApplied = true;
		}

		/// <summary>
		/// Causes the effect to immediately fade out.
		/// </summary>
		public void Hide()
		{
			Alpha = kColorTransparent;
			Mode = FadeMode.Out;
			IsApplying = false;
			IsApplied = true;
		}

		/// <summary>
		/// Returns a white color with a variable value for the alpha component.
		/// </summary>
		public Color Color
		{
			get { return new Color(kColorOpaque, kColorOpaque, kColorOpaque, Alpha); }
		}

		/// <summary>
		/// Whether the fade effect is done.
		/// </summary>
		public bool IsDone
		{
			get { return !IsApplying && !IsPrepared && IsApplied; }
		}

		/// <summary>
		/// Whether the effect is prepared to apply.
		/// </summary>
		public bool IsPrepared { get; private set; }

		/// <summary>
		/// Whether the effect is applying.
		/// </summary>
		public bool IsApplying { get; private set; }

		/// <summary>
		/// Whether the effect is applied.
		/// </summary>
		public bool IsApplied { get; private set; }

		/// <summary>
		/// The <c>FadeMode</c> set.
		/// </summary>
		public FadeMode Mode { get; private set; }

		/// <summary>
		/// The alpha value set.
		/// </summary>
		public float Alpha { get; private set; }
	}
}
