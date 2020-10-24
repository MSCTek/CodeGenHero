using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGH.Inflector.Tests
{
    [TestClass]
    public class TitleizeTests : InflectorTestBase
    {
        public TitleizeTests()
        {
            //Uppercases individual words and removes some characters
            TestData.Add("some title", "Some Title");
            TestData.Add("some-title", "Some Title");
            TestData.Add("sometitle", "Sometitle");
            TestData.Add("some-title: The begining", "Some Title: The Begining");
            TestData.Add("some_title:_the_begining", "Some Title: The Begining");
            TestData.Add("some title: The_begining", "Some Title: The Begining");
        }

        [TestMethod]
        public void Titleize()
        {
            foreach (var pair in TestData)
            {
                Assert.AreEqual(Inflector.Titleize(pair.Key), pair.Value);
            }
        }
    }
}