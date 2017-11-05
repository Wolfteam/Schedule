using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Schedule.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestCase(12, 3, ExpectedResult = 15)]
        [TestCase(12, 2, ExpectedResult = 14)]
        [TestCase(12, 4, ExpectedResult = 16)]
        public int TestMethod1(int x, int y)
        {
            return x + y;
        }
    }
}
