using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGH.Inflector.Tests
{
    [TestClass]
    public class UncapitalizeTests : InflectorTestBase
    {
        public UncapitalizeTests()
        {
            //Just lowers the first char and leaves the rest alone
            TestData.Add("some title", "some title");
            TestData.Add("some Title", "some Title");
            TestData.Add("SOMETITLE", "sOMETITLE");
            TestData.Add("someTitle", "someTitle");
            TestData.Add("some title goes here", "some title goes here");
            TestData.Add("some TITLE", "some TITLE");
        }

        [TestMethod]
        public void Uncapitalize()
        {
            foreach (var pair in TestData)
            {
                Assert.AreEqual(Inflector.Uncapitalize(pair.Key), pair.Value);
            }
        }
    }
}