using System.Reflection;

namespace NUnitInCodeRunner
{
	public interface ITestLogger
	{
		void LogStart(MethodInfo methodInfo, object[] methodParams);
		void LogResult(bool passed);
		TestSessionSuccessCode GetSuccessCode();
	}
}