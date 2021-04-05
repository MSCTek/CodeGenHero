using NUnit.Framework;

namespace TestLifeCycle
{
    [SetUpFixture]
    public class FixtureSetup
    {
        [OneTimeSetUp]
        public void OneTimeSetUp() => TestContext.Progress.WriteLine("FixtureSetup:OneTimeSetUp");

        [OneTimeTearDown]
        public void OneTimeTearDown() => TestContext.Progress.WriteLine("FixtureSetup:OneTimeTearDown");
    }

    [TestFixture]
    public class Tests
    {
        public Tests() => TestContext.Progress.WriteLine("Tests:Constructor");

        [OneTimeSetUp]
        public void OneTimeSetUp() => TestContext.Progress.WriteLine("Tests:OneTimeSetUp");

        [OneTimeTearDown]
        public void OneTimeTearDown() => TestContext.Progress.WriteLine("Tests:OneTimeTearDown");

        [SetUp]
        public void Setup() => TestContext.Progress.WriteLine("Tests:SetUp");

        [TearDown]
        public void TearDown() => TestContext.Progress.WriteLine("Tests:TearDown");

        [Test]
        public void Test1() => TestContext.Progress.WriteLine("Tests:Test1");

        [Test]
        public void Test2() => TestContext.Progress.WriteLine("Tests:Test2");
    }
}

[SetUpFixture]
public class RootFixtureSetup
{
    [OneTimeSetUp]
    public void OneTimeSetUp() => TestContext.Progress.WriteLine("RootFixtureSetup:OneTimeSetUp");

    [OneTimeTearDown]
    public void OneTimeTearDown() => TestContext.Progress.WriteLine("RootFixtureSetup:OneTimeTearDown");
}