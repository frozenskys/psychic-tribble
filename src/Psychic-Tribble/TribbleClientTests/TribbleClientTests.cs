namespace TribbleClientTests
{
    using NUnit.Framework;
    using PactNet.Mocks.MockHttpService;
    using PactNet.Mocks.MockHttpService.Models;
    using System.Collections.Generic;

    [TestFixture]
    public class TribbleClientTests
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        [OneTimeSetUp]
        public void SetFixture()
        {
            var data = new TribblePact();
            _mockProviderService = data.MockProviderService;
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
            data.MockProviderService.ClearInteractions(); 
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
                Path = "/tribbles/1",
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
                    Id = "1",
                    Colour = "blue",
                    Furryness = "High",
                    Hungry = "False"
                }
            }); 
            var consumer = new TribbleClient.Client(_mockProviderServiceBaseUri);
            var result = consumer.GetTribbles(1);
            Assert.AreEqual("blue", result.Colour);
            _mockProviderService.VerifyInteractions();
        }
    }
}
