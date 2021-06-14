// Copyright (c) Rodrigo Bento

using Boo.Lang.Compiler;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Pipelines;

using GameFramework.Scripting;

using System;
using System.Collections.Generic;
using System.IO;

using static GameFramework.Diagnostics.Logger;

namespace GameFramework.Manager
{
	/// <summary>
	/// Manages Boo-lang scripts from a given directory.
	/// </summary>
	public class ScriptManager
	{
		/// <summary>
		/// The Boo compiler instance.
		/// </summary>
		private readonly BooCompiler mScriptCompiler = new BooCompiler();

		/// <summary>
		/// The script directory.
		/// </summary>
		private readonly string mScriptDirectory;

		/// <summary>
		/// Creates an instance of <c>ScriptManager</c>.
		/// </summary>
		/// <param name="ScriptDirectory">The directory containing Boo-lang scripts.</param>
		public ScriptManager(string ScriptDirectory)
		{
			mScriptDirectory = $"{Environment.CurrentDirectory}{ScriptDirectory}";
			mScriptCompiler = new BooCompiler();
		}

		/// <summary>
		/// Loads and compiles a Boo-lang script.
		/// </summary>
		/// <remarks>
		/// Logs compilation errors to the console, if any.
		/// </remarks>
		/// <param name="FileName">The script filename to load.</param>
		/// <param name="ClassName">The script class name.</param>
		/// <returns>An instance os <c>Script</c> when successfully loaded and compiled, otherwise <c>null</c>.</returns>
		public Script LoadScript(string FileName, string ClassName)
		{
			try
			{
				string FilePath = $"{mScriptDirectory}{FileName}";

				mScriptCompiler.Parameters.Input.Add(new FileInput(FilePath));
				mScriptCompiler.Parameters.Pipeline = new CompileToMemory();
				mScriptCompiler.Parameters.Ducky = true;

				CompilerContext CompilerContext = mScriptCompiler.Run();

				if (CompilerContext.GeneratedAssembly != null)
				{
					return new Script(CompilerContext.GeneratedAssembly.GetType(ClassName), ClassName);
				}
				else
				{
					foreach (CompilerError CompilerError in CompilerContext.Errors)
					{
						Error($"{CompilerError}");
					}
				}
			}
			catch (Exception Ex)
			{
				Error($"{Ex.Message}");
			}

			return null;
		}

		/// <summary>
		/// Lists filenames for all files in a directory.
		/// </summary>
		/// <returns>A list of filenames, if any, otherwise an empty list.</returns>
		public List<string> GetFileNames()
		{
			string[] FileNames = null;

			try
			{
				FileNames = Directory.GetFiles(mScriptDirectory);
			}
			catch (Exception Ex)
			{
				Error($"{Ex.Message}");
			}

			return FileNames != null ? new List<string>(FileNames) : new List<string>(0);
		}

		/// <summary>
		/// Lists all Boo scripts in a directory.
		/// </summary>
		/// <remarks>
		/// Directory is supposed to contain only Boo scripts.
		/// </remarks>
		/// <returns>A list of Boo scripts, if any, otherwise an empty list.</returns>
		public List<Script> GetScripts()
		{
			List<string> FileNames = GetFileNames();
			List<Script> Scripts = new List<Script>(FileNames.Count);

			foreach (string File in FileNames)
			{
				string FileName = Path.GetFileName(File);
				string ClassName = Path.GetFileNameWithoutExtension(File);

				Script Script = LoadScript(FileName, ClassName);

				Scripts.Add(Script);
			}

			return Scripts;
		}
	}
}
