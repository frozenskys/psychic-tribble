using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace TribbleClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    public class Client
    {
        public string BaseUri { get; set; }

        public Client(string baseUri = null)
        {
            BaseUri = baseUri ?? "http://my-api";
        }

        public Tribble GetTribbles(int id)
        {
            string reasonPhrase;

            using (var client = new HttpClient { BaseAddress = new Uri(BaseUri) })
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/tribble/" + id);
                request.Headers.Add("Accept", "application/json");
                var response = client.SendAsync(request);
                var content = response.Result.Content.ReadAsStringAsync().Result;
                var status = response.Result.StatusCode;

                reasonPhrase = response.Result.ReasonPhrase; 
                request.Dispose();
                response.Dispose();
                if (status == HttpStatusCode.OK)
                {
                    return !string.IsNullOrEmpty(content) ? JsonConvert.DeserializeObject<Tribble>(content) : null;
                }
            }
            throw new Exception(reasonPhrase); 
        }
    }
}
