using System;
using System.Linq;
using System.Reflection;

namespace NUnitInCodeRunner
{
	public class ConsoleTestLogger : ITestLogger
	{
		private int _passed;
		private int _failed;
		
		public ConsoleTestLogger()
		{
			_passed = 0;
			_failed = 0;
		}
		
		public void LogStart(MethodInfo methodInfo, object[] methodParams)
		{
			string paramDesc = methodParams
				.Select(p => p.ToString())
				.Aggregate("", (s, s1) => s + ", " + s1);
			Console.WriteLine("");
			Console.WriteLine($"Running {methodInfo.Name}{paramDesc}");
			Console.WriteLine("");
		}

		public void LogResult(bool passed)
		{
			Console.WriteLine("");
			Console.ForegroundColor = passed ? ConsoleColor.Green : ConsoleColor.Red;
			Console.WriteLine(passed ? "Passed" : "Failed");
			Console.ResetColor();
			Console.WriteLine("");
			if (passed) _passed += 1;
			else _failed += 1;
		}

		public TestSessionSuccessCode GetSuccessCode()
		{
			Console.WriteLine(_passed + " tests passed");
			Console.WriteLine(_failed + " tests failed");
			return _failed < 1 ? TestSessionSuccessCode.Success : TestSessionSuccessCode.Fail;
		}
	}
}