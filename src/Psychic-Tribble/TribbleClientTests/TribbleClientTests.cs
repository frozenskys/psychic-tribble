namespace TribbleClientTests
{
    using NUnit.Framework;
    using PactNet;
    using PactNet.Mocks.MockHttpService;
    using PactNet.Mocks.MockHttpService.Models;
    using System.Collections.Generic;
    using System.IO;

    [TestFixture]
    public class TribbleClientTests
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;
        private TribblePact _data;

        [OneTimeSetUp]
        public void SetFixture()
        {
            var path = TestContext.CurrentContext.TestDirectory;
            _data = new TribblePact(new PactConfig { LogDir = path, PactDir = Path.Combine(path, @"./pacts") });
            _mockProviderService = _data.MockProviderService;
            _mockProviderServiceBaseUri = _data.MockProviderServiceBaseUri;
            _data.MockProviderService.ClearInteractions();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _data.Dispose();
        }

        [Test]
		public void TestPact() 
		{
            _mockProviderService
            .Given("There is a tribble with id '1'")
            .UponReceiving("A GET request to retrieve the tribble")
            .With(new ProviderServiceRequest
            {
                Method = HttpVerb.Get,
                Path = "/tribble/1",
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" }
                }
            })
            .WillRespondWith(new ProviderServiceResponse
            {
                Status = 200,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json; charset=utf-8" }
                },
                Body = new 
                {
                    Id = 1,
                    Colour = "blue",
                    Furryness = "High",
                    Hungry = true
                }
            }); 
            var consumer = new TribbleClient.Client(_mockProviderServiceBaseUri);
            var result = consumer.GetTribbles(1);
            Assert.AreEqual("blue", result.Colour);
            _mockProviderService.VerifyInteractions();
        }
    }
}
