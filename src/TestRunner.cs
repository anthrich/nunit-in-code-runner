using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace NUnitInCodeRunner
{
	public class TestRunner
	{
		private readonly List<TestClassRunner> _testClassRunners;
		private readonly ITestLogger _logger;

		public TestRunner(ITestLogger testLogger = null)
		{
			_logger = testLogger;
			if(testLogger == null) _logger = new ConsoleTestLogger();
			
			_testClassRunners = Assembly.GetCallingAssembly()
				.GetTypes()
				.Where(t => t.GetCustomAttributes(typeof(TestFixtureAttribute), false).Length > 0)
				.Select(t => new TestClassRunner(t, _logger))
				.ToList();
		}

		public int Run()
		{
			_testClassRunners.ForEach(t => t.Run());
			return (int)_logger.GetSuccessCode();
		}
	}
}