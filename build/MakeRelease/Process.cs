﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace MakeRelease
{
	public class Process
	{
		public static System.Diagnostics.Process RunningProcess = null;

		public delegate void CleanupDelegate();

		public static CleanupDelegate Cleanup;

		public enum CtrlType
		{
			CTRL_C_EVENT = 0,
			CTRL_BREAK_EVENT = 1,
			CTRL_CLOSE_EVENT = 2,
			CTRL_LOGOFF_EVENT = 5,
			CTRL_SHUTDOWN_EVENT = 6
		}

		private static bool Handler(CtrlType signal)
		{
			switch (signal)
			{
				case CtrlType.CTRL_BREAK_EVENT:
				case CtrlType.CTRL_C_EVENT:
				case CtrlType.CTRL_LOGOFF_EVENT:
				case CtrlType.CTRL_SHUTDOWN_EVENT:
				case CtrlType.CTRL_CLOSE_EVENT:
					if (Cleanup != null)
					{
						Cleanup();
					}
					return false;

				default:
					return false;
			}
		}

		public static void AddCleanupHandler(CleanupDelegate Del)
		{
			Cleanup = Del;
			SetConsoleCtrlHandler(Handler, true);
		}

		public static bool RunCommand(string Executable, string Arguments, string Tag)
		{
			// Convert spaces to acceptable spaces
			// Append "" around the executable so that it can be run
			Executable = "\"\"" + Executable + "\"\"";

			var ProcessInfo = new ProcessStartInfo("cmd.exe", "/c " + Executable + " " + Arguments);
			ProcessInfo.CreateNoWindow = true;
			ProcessInfo.UseShellExecute = false;
			ProcessInfo.RedirectStandardError = true;
			ProcessInfo.RedirectStandardOutput = true;

			RunningProcess = System.Diagnostics.Process.Start(ProcessInfo);

			RunningProcess.OutputDataReceived += (object sender, DataReceivedEventArgs Evt) =>
				Console.WriteLine(Tag + ">" + Evt.Data);
			RunningProcess.BeginOutputReadLine();

			RunningProcess.ErrorDataReceived += (object sender, DataReceivedEventArgs Evt) =>
				Console.WriteLine(Tag + "!>" + Evt.Data);
			RunningProcess.BeginErrorReadLine();

			RunningProcess.WaitForExit();

			int ReturnValue = RunningProcess.ExitCode;
			RunningProcess.Close();

			RunningProcess = null;

			return ReturnValue == 0;
		}

		// https://msdn.microsoft.com/en-us/library/windows/desktop/ms686016.aspx
		[DllImport("Kernel32")]
		private static extern bool SetConsoleCtrlHandler(SetConsoleCtrlEventHandler handler, bool add);

		// https://msdn.microsoft.com/en-us/library/windows/desktop/ms683242.aspx
		private delegate bool SetConsoleCtrlEventHandler(CtrlType sig);
	}
}