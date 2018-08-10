using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace NUnitInCodeRunner
{
	public class TestClassRunner
	{
		private readonly Type _testClassType;
		private readonly List<MethodInfo> _testMethods;
		private readonly List<MethodInfo> _oneTimeSetUpMethods;
		private readonly List<MethodInfo> _testSetUpMethods;
		private readonly List<MethodInfo> _oneTimeTearDownMethods;
		private readonly ITestLogger _logger;
		
		public TestClassRunner(Type testClassType, ITestLogger logger)
		{
			_testClassType = testClassType;
			_testMethods = testClassType.GetMethods()
				.Where(m => m.GetCustomAttributes<TestAttribute>(false).Any())
				.ToList();
			_oneTimeSetUpMethods = testClassType.GetMethods()
				.Where(m => m.GetCustomAttributes<OneTimeSetUpAttribute>(false).Any())
				.ToList();
			_testSetUpMethods = testClassType.GetMethods()
				.Where(m => m.GetCustomAttributes<SetUpAttribute>(false).Any())
				.ToList();
			_oneTimeTearDownMethods = testClassType.GetMethods()
				.Where(m => m.GetCustomAttributes<OneTimeTearDownAttribute>(false).Any())
				.ToList();
			_logger = logger;
		}

		public void Run()
		{
			var testClassInstance = Activator.CreateInstance(_testClassType);
			_oneTimeSetUpMethods.ForEach(m => m.Invoke(testClassInstance, new object[0]));
			_testMethods.ForEach(tm => RunTestMethod(tm, testClassInstance));
			_oneTimeTearDownMethods.ForEach(m => m.Invoke(testClassInstance, new object[0]));
		}

		private void RunTestMethod(MethodInfo methodInfo, object classInstance)
		{
			_testSetUpMethods.ForEach(m => m.Invoke(classInstance, new object[0]));
			var methodTestCasesArgs = methodInfo.GetCustomAttributes<TestCaseAttribute>(false)
				.Select(testCase => testCase.Arguments)
				.ToList();
			
			if (!methodTestCasesArgs.Any())
			{
				methodTestCasesArgs.Add(new object[0]);
			}
			
			methodTestCasesArgs.ForEach(args =>
			{
				_logger.LogStart(methodInfo, args);
				try
				{
					methodInfo.Invoke(classInstance, args);
					_logger.LogResult(true);
				}
				catch(Exception e)
				{
					_logger.LogResult(false);
				}
			});
		}
	}
}