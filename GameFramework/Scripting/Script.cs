// Copyright (c) Rodrigo Bento

using System;
using System.Reflection;

using static GameFramework.Diagnostics.Logger;

namespace GameFramework.Scripting
{
	/// <summary>
	/// Provides the details of an external script and the ability to execute its methods.
	/// </summary>
	/// <remarks>
	/// The current external script language is Boo-lang.
	/// </remarks>
	public class Script
	{
		/// <summary>
		/// Creates new instance of <c>Script</c>.
		/// </summary>
		/// <param name="ScriptModule">The script module.</param>
		/// <param name="ClassName">The class name.</param>
		public Script(Type ScriptModule, string ClassName)
		{
			this.ClassName = ClassName;
			this.ScriptModule = ScriptModule;
		}

		/// <summary>
		/// Invokes a method on this script module.
		/// </summary>
		/// <param name="Name">The method name.</param>
		/// <param name="Args">The method argument list.</param>
		/// <returns>An <c>object</c> as returned by the script method.</returns>
		/// <exception cref="Exception">When there is an error with the script method invocation.</exception>
		public object InvokeMethod(string Name, object[] Args)
		{
			try
			{
				MethodInfo MethodInfo = ScriptModule.GetMethod(Name);
				return MethodInfo.Invoke(null, Args);
			}
			catch (Exception Ex)
			{
				Error($"{Ex.Message}");
				throw Ex;
			}
		}

		/// <summary>
		/// Gets the <c>string</c> representation of this <c>Script</c>.
		/// </summary>
		/// <returns>The <c>string</c> representation of this <c>Script</c>.</returns>
		public override string ToString()
		{
			return $"Script [ ScriptModule: {ScriptModule}, ClassName: {ClassName} ]";
		}

		/// <summary>
		/// The script module.
		/// </summary>
		public Type ScriptModule { get; private set; }

		/// <summary>
		/// The class name.
		/// </summary>
		public string ClassName { get; private set; }
	}
}
