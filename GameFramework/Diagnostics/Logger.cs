// Copyright (c) Rodrigo Bento

using System;
using System.Diagnostics;
using System.Reflection;

namespace GameFramework.Diagnostics
{
	/// <summary>
	/// Simple console logger utility.
	/// </summary>
	public static class Logger
	{
		/// <summary>
		/// Writes to the console with the [DEBUG] tag.
		/// </summary>
		/// <param name="Value">The value to be written.</param>
		public static void Debug(string Value)
		{
			if (!IsEnabled) return;

			Write("[DEBUG]", Value, new StackFrame(2).GetMethod());
		}

		/// <summary>
		/// Writes to the console with the [ INFO] tag.
		/// </summary>
		/// <param name="Value">The value to be written.</param>
		public static void Info(string Value)
		{
			if (!IsEnabled) return;

			Write("[ INFO]", Value, new StackFrame(2).GetMethod());
		}

		/// <summary>
		/// Writes to the console with the [ WARN] tag.
		/// </summary>
		/// <param name="Value">The value to be written.</param>
		public static void Warn(string Value)
		{
			if (!IsEnabled) return;

			Write("[ WARN]", Value, new StackFrame(2).GetMethod());
		}

		/// <summary>
		/// Writes to the console with the [ERROR] tag.
		/// </summary>
		/// <param name="Value">The value to be written.</param>
		public static void Error(string Value)
		{
			if (!IsEnabled) return;

			Write("[ERROR]", Value, new StackFrame(2).GetMethod());
		}

		/// <summary>
		/// Writes a given string to the console with the appropriate level.
		/// </summary>
		/// <remarks>
		/// The optional caller name will precede the log message if present.
		/// </remarks>
		/// <param name="Level">The log message level.</param>
		/// <param name="Text">The log message text.</param>
		/// <param name="Caller">The calling method, if any.</param>
		private static void Write(string Level, string Text, MethodBase Caller = null)
		{
			if (Caller == null)
			{
				Console.WriteLine($"{Level} : {Text}");
			}
			else
			{
				Console.WriteLine($"{Level} : {Caller.DeclaringType.Name}.{Caller.Name} : {Text}");
			}
		}

		/// <summary>
		/// Enables the logget output.
		/// </summary>
		public static void Enable()
		{
			IsEnabled = true;
		}

		/// <summary>
		/// Disables the logger output.
		/// </summary>
		public static void Disable()
		{
			IsEnabled = false;
		}

		/// <summary>
		/// Whether this logger is enabled.
		/// </summary>
		public static bool IsEnabled { get; private set; } = true;
	}
}
