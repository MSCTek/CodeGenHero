using Ionic.Zip;
using MSC.CodeGenHero.Serialization;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using cmeta = CodeGenHero.Core.Metadata;
using cmetai = CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Core.Tests
{
    //[SetUpFixture]
    //public class FixtureSetup
    //{
    //    [OneTimeSetUp]
    //    public void OneTimeSetUp() => TestContext.Progress.WriteLine("FixtureSetup:OneTimeSetUp");

    //    [OneTimeTearDown]
    //    public void OneTimeTearDown() => TestContext.Progress.WriteLine("FixtureSetup:OneTimeTearDown");
    //}

    [TestFixture]
    public class GetExcludedEntityNavigationsByRegExTests
    {
        public GetExcludedEntityNavigationsByRegExTests() => TestContext.Progress.WriteLine($"{nameof(GetExcludedEntityNavigationsByRegExTests)}:Constructor");

        private cmetai.IModel Model { get; set; }

        [Test, Order(1)]
        public void GetExcludedEntityNavigationsByRegEx_Test1()
        {
            TestContext.Progress.WriteLine($"{nameof(GetExcludedEntityNavigationsByRegExTests)}:{nameof(GetExcludedEntityNavigationsByRegEx_Test1)}");
            string excludeRegExPattern = "^AspNet*|^RefreshTokens*";

            var entityNavigations = Model.GetExcludedEntityNavigationsByRegEx(excludeRegExPattern: excludeRegExPattern, includeRegExPattern: null);
            Assert.That(entityNavigations.Count, Is.EqualTo(12));

            var expectedExclusion = entityNavigations.FirstOrDefault(x => x.EntityType.ClrType.Name == "UserProfile"
                && x.Navigation.ClrType.Name == "AspNetUser");
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            TestContext.Progress.WriteLine($"{nameof(GetExcludedEntityNavigationsByRegExTests)}:OneTimeSetUp");

            var modelFilePath = Path.Combine(Environment.CurrentDirectory, "TestData", "ConferenceMateMetadataModel.zip");
            FileInfo fi = new FileInfo(modelFilePath);

            using (ZipFile zipFile = ZipFile.Read(modelFilePath))
            {
                ZipEntry modelZipEntry = zipFile[0];
                MemoryStream ms = new MemoryStream();
                modelZipEntry.Extract(ms);
                this.Model = SerializerUtility.DeserializeJson<cmeta.Model>(ms);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => TestContext.Progress.WriteLine($"{nameof(GetExcludedEntityNavigationsByRegExTests)}:OneTimeTearDown");

        [SetUp]
        public void Setup()
        {
            TestContext.Progress.WriteLine($"{nameof(GetExcludedEntityNavigationsByRegExTests)}:SetUp");
        }

        [TearDown]
        public void TearDown() => TestContext.Progress.WriteLine($"{nameof(GetExcludedEntityNavigationsByRegExTests)}:TearDown");
    }
}

//[SetUpFixture]
//public class RootFixtureSetup
//{
//    [OneTimeSetUp]
//    public void OneTimeSetUp() => TestContext.Progress.WriteLine("RootFixtureSetup:OneTimeSetUp");

//    [OneTimeTearDown]
//    public void OneTimeTearDown() => TestContext.Progress.WriteLine("RootFixtureSetup:OneTimeTearDown");
//}