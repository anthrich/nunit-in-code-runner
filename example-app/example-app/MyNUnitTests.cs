using NUnit.Framework;

namespace example_app
{
	[TestFixture]
	public class MyNUnitTests
	{
		private bool _isSetUp;
		
		[OneTimeSetUp]
		public void FixtureSetUp()
		{
			_isSetUp = true;
		}

		[Test]
		public void IsSetUp()
		{
			Assert.IsTrue(_isSetUp);
		}
		
		[Test]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		public void IsSetUp(int testCase)
		{
			Assert.IsTrue(_isSetUp && testCase > 0);
		}
	}
}