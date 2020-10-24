using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGH.Inflector.Tests
{
    [TestClass]
    public class DasherizeTests : InflectorTestBase
    {
        public DasherizeTests()
        {
            //Just replaces underscore with a dash
            TestData.Add("some_title", "some-title");
            TestData.Add("some-title", "some-title");
            TestData.Add("some_title_goes_here", "some-title-goes-here");
            TestData.Add("some_title and_another", "some-title and-another");
        }

        [TestMethod]
        public void Dasherize()
        {
            foreach (var pair in TestData)
            {
                Assert.AreEqual(Inflector.Dasherize(pair.Key), pair.Value);
            }
        }
    }
}