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
        [Test]
        public void EnsureTribbleApiHonoursPactWithConsumer()
        {
            //Arrange
            IPactVerifier pactVerifier = new PactVerifier(() => { }, () => { });
            pactVerifier.ProviderState("There is a tribble with id '1'");
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "./pacts/consumer-tribble_api.json");
            using (var testServer = TestServer.Create<Startup>())
            {
                pactVerifier
                    .ServiceProvider("Tribble API", testServer.HttpClient)
                    .HonoursPactWith("Consumer")
                    .PactUri(path)
                    .Verify(); 
            }
        }
    }
}
