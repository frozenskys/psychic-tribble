namespace TribbleServerTests
{
    using Microsoft.Owin.Testing;
    using PactNet;
    using TribbleServer;
    using NUnit.Framework;
    using System.IO;

    [TestFixture]
    public class TribbleServerTests
    {
        PactVerifierConfig _verifierConfig;

        [OneTimeSetUp]
        public void Setup()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "logs");
            _verifierConfig = new PactVerifierConfig { LogDir = path };
        }

        [Test]
        public void EnsureTribbleApiHonoursPactWithConsumer()
        {
            
            //Arrange
            IPactVerifier pactVerifier = new PactVerifier(() => { }, () => { }, _verifierConfig);
            pactVerifier.ProviderState("There is a tribble with id '1'");
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "./pacts/consumer-tribble_api.json");
            using (var testServer = TestServer.Create<Startup>())
            {
                pactVerifier
                    .ServiceProvider("Tribble API", testServer.HttpClient)
                    .HonoursPactWith("Consumer")
                    .PactUri(path)
                    .Verify(null, "There is a tribble with id '1'"); 
            }
        }

        [Test]
        public void EnsureTribbleApiHonoursGetTribbles()
        {
            //Arrange
            IPactVerifier pactVerifier = new PactVerifier(() => { }, () => { }, _verifierConfig);
            pactVerifier.ProviderState("There is more than one tribble");
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "./pacts/consumer-tribble_api.json");
            using (var testServer = TestServer.Create<Startup>())
            {
                pactVerifier
                    .ServiceProvider("Tribble API", testServer.HttpClient)
                    .HonoursPactWith("Consumer")
                    .PactUri(path)
                    .Verify(null, "There is more than one tribble");
            }
        }
    }
}
