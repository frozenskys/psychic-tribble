using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace TribbleClientTests
{
    public class TribblePact : IDisposable
    {
        public IPactBuilder PactBuilder { get; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort => 1234;
        public string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";

        public TribblePact()
        {
            PactBuilder = new PactBuilder(new PactConfig { LogDir=@"C:\temp\" }); 
            PactBuilder.ServiceConsumer("Consumer").HasPactWith("Tribble API");
            MockProviderService = PactBuilder.MockService(MockServerPort); 
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}
