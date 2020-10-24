using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGH.Inflector.Tests
{
    [TestClass]
    public class HumanizeTests : InflectorTestBase
    {
        public HumanizeTests()
        {
            //Capitalizes the first word, lowercases the rest and turns underscores into spaces
            TestData.Add("some_title", "Some title");
            TestData.Add("some-title", "Some-title");
            TestData.Add("Some_title", "Some title");
            TestData.Add("someTitle", "Sometitle");
            TestData.Add("someTitle_Another", "Sometitle another");
        }

        [TestMethod]
        public void Humanize()
        {
            foreach (var pair in TestData)
            {
                Assert.AreEqual(Inflector.Humanize(pair.Key), pair.Value);
            }
        }
    }
}