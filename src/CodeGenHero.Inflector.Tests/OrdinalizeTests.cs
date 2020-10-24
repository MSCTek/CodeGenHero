using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGH.Inflector.Tests
{
    [TestClass]
    public class OrdinalizeTests : InflectorTestBase
    {
        public OrdinalizeTests() : base()
        {
            TestData.Add("0", "0th");
            TestData.Add("1", "1st");
            TestData.Add("2", "2nd");
            TestData.Add("3", "3rd");
            TestData.Add("4", "4th");
            TestData.Add("5", "5th");
            TestData.Add("6", "6th");
            TestData.Add("7", "7th");
            TestData.Add("8", "8th");
            TestData.Add("9", "9th");
            TestData.Add("10", "10th");
            TestData.Add("11", "11th");
            TestData.Add("12", "12th");
            TestData.Add("13", "13th");
            TestData.Add("14", "14th");
            TestData.Add("20", "20th");
            TestData.Add("21", "21st");
            TestData.Add("22", "22nd");
            TestData.Add("23", "23rd");
            TestData.Add("24", "24th");
            TestData.Add("100", "100th");
            TestData.Add("101", "101st");
            TestData.Add("102", "102nd");
            TestData.Add("103", "103rd");
            TestData.Add("104", "104th");
            TestData.Add("110", "110th");
            TestData.Add("1000", "1000th");
            TestData.Add("1001", "1001st");
        }

        [DataRow(0, "0th")]
        [DataRow(1, "1st")]
        [DataRow(2, "2nd")]
        [DataRow(3, "3rd")]
        [DataRow(4, "4th")]
        [DataRow(5, "5th")]
        [DataRow(6, "6th")]
        [DataRow(7, "7th")]
        [DataRow(8, "8th")]
        [DataRow(9, "9th")]
        [DataRow(10, "10th")]
        [DataRow(11, "11th")]
        [DataRow(12, "12th")]
        [DataRow(13, "13th")]
        [DataRow(14, "14th")]
        [DataRow(20, "20th")]
        [DataRow(21, "21st")]
        [DataRow(22, "22nd")]
        [DataRow(23, "23rd")]
        [DataRow(24, "24th")]
        [DataRow(100, "100th")]
        [DataRow(101, "101st")]
        [DataRow(102, "102nd")]
        [DataRow(103, "103rd")]
        [DataRow(104, "104th")]
        [DataRow(110, "110th")]
        [DataRow(1000, "1000th")]
        [DataRow(1001, "1001st")]
        public void OrdanizeNumbersTest(int number, string ordanized)
        {
            Assert.AreEqual(Inflector.Ordinalize(number), ordanized);
        }

        [TestMethod]
        public void Ordinalize()
        {
            foreach (var pair in TestData)
            {
                Assert.AreEqual(Inflector.Ordinalize(pair.Key), pair.Value);
            }
        }
    }
}