using CodeGenHero.Core.Metadata;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models.Interfaces;
using Ionic.Zip;
using Moq;
using MSC.CodeGenHero.Library;
using MSC.CodeGenHero.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using cmeta = CodeGenHero.Core.Metadata;
using cmetai = CodeGenHero.Core.Metadata.Interfaces;

namespace CodeGenHero.Template.Tests
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
    public class EntityNavigationTests
    {
        private Mock<Inflector.ICodeGenHeroInflector> _mockInflector;
        private Mock<IMetadataSourceProperties> _mockMetadataSourceProperties;
        private Mock<ITemplateIdentity> _mockTemplateIdentity;

        public EntityNavigationTests() => TestContext.Progress.WriteLine($"{nameof(EntityNavigationTests)}:Constructor");

        private cmetai.IModel Model { get; set; }

        [Test, Order(2)]
        public void GetAllExcludedNavigationProperties_Test1()
        {
            TestContext.Progress.WriteLine($"{nameof(EntityNavigationTests)}:{nameof(GetAllExcludedNavigationProperties_Test1)}");
            string excludeRegExPattern = "^AspNet*|^RefreshTokens*";

            IList<IEntityNavigation> excludedEntityNavigations = new List<IEntityNavigation>();
            var dummyEntityNavigation = new EntityNavigation()
            {
                EntityType = new EntityType()
                {
                    DefiningNavigationName = "Dummy Defining Navigation Name"
                },
                Navigation = new Navigation()
                {
                    Name = "Dummy Name"
                }
            };
            excludedEntityNavigations.Add(dummyEntityNavigation);

            IProcessModel processModel = new ProcessModel(templateIdentity: _mockTemplateIdentity.Object,
                templateVariables: new Dictionary<string, string>(),
                runName: string.Empty,
                metadataSourceProperties: _mockMetadataSourceProperties.Object,
                excludedNavigationProperties: excludedEntityNavigations,
                baseWritePath: string.Empty,
                metadataSourceModel: Model);

            var allExcludedEntityNavigations = processModel.GetAllExcludedEntityNavigations(excludeRegExPattern: excludeRegExPattern, includeRegExPattern: null);

            Assert.That(allExcludedEntityNavigations.Count, Is.EqualTo(13));
        }

        [Test, Order(1)]
        public void GetForeignKeyName_Test1()
        {
            TestContext.Progress.WriteLine($"{nameof(EntityNavigationTests)}:{nameof(GetForeignKeyName_Test1)}");

            var userProfileEntityType = Model.EntityTypes.FirstOrDefault(x => x.ClrType.Name == "UserProfile");

            var dummyGenerator = new DummyGenerator(_mockInflector.Object);
            var foreignKeyNames = dummyGenerator.GetForeignKeyNames(userProfileEntityType.Navigations);

            Assert.That(foreignKeyNames.Count, Is.EqualTo(6));
            Assert.IsTrue(foreignKeyNames.Contains("FK_UserProfile_AspNetUsers"));
        }

        [Test, Order(3)]
        public void IsNavigationNameInEntityNavigations_Test1()
        {
            TestContext.Progress.WriteLine($"{nameof(EntityNavigationTests)}:{nameof(IsNavigationNameInEntityNavigations_Test1)}");
            string excludeRegExPattern = "^AspNet*|^RefreshTokens*";

            var entityNavigations = Model.GetExcludedEntityNavigationsByRegEx(excludeRegExPattern: excludeRegExPattern, includeRegExPattern: null);

            var dummyGenerator = new DummyGenerator(_mockInflector.Object);
            IEntityType entityType = entityNavigations.First(x => x.EntityType.ClrType.Name == "AspNetUser")
                .EntityType;
            var containsFoo = dummyGenerator.EntityNavigationsContainsNavigationName(entityNavigations, entityType, "foo");
            var containsUserProfile = dummyGenerator.EntityNavigationsContainsNavigationName(entityNavigations, entityType, "UserProfile");

            Assert.IsFalse(containsFoo);
            Assert.IsTrue(containsUserProfile);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            TestContext.Progress.WriteLine($"{nameof(EntityNavigationTests)}:OneTimeSetUp");

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
        public void OneTimeTearDown() => TestContext.Progress.WriteLine($"{nameof(EntityNavigationTests)}:OneTimeTearDown");

        [SetUp]
        public void Setup()
        {
            TestContext.Progress.WriteLine($"{nameof(EntityNavigationTests)}:SetUp");

            _mockInflector = new Mock<Inflector.ICodeGenHeroInflector>(MockBehavior.Strict);
            _mockTemplateIdentity = new Mock<ITemplateIdentity>(MockBehavior.Strict);
            _mockMetadataSourceProperties = new Mock<IMetadataSourceProperties>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown() => TestContext.Progress.WriteLine($"{nameof(EntityNavigationTests)}:TearDown");
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