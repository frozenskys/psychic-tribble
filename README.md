# psychic-tribble

[![Build status](https://ci.appveyor.com/api/projects/status/0v8x7cci3bq4dwxj/branch/master?svg=true)](https://ci.appveyor.com/project/frozenskys/psychic-tribble/branch/master)

A PactNet example

## What is PactNet

PactNet primarily provides a fluent .NET DSL for describing HTTP requests that will be made to a service provider and the HTTP responses the consumer expects back to function correctly.
In documenting the consumer interactions, we can replay them on the provider and ensure the provider responds as expected. This basically gives us complete test symmetry and removes the basic need for integrated tests.
PactNet also has the ability to support other mock providers should we see fit.

PactNet is aiming to be Pact Specification Version 1.1 compliant (Version 2 is a WIP).

From the [Pact Specification repo](https://github.com/bethesque/pact_specification)

> "Pact" is an implementation of "consumer driven contract" testing that allows mocking of responses in the consumer codebase, and verification of the interactions in the provider codebase. The initial implementation was written in Ruby for Rack apps, however a consumer and provider may be implemented in different programming languages, so the "mocking" and the "verifying" steps would be best supported by libraries in their respective project's native languages. Given that the pact file is written in JSON, it should be straightforward to implement a pact library in any language, however, to get the best experience and most reliability of out mixing pact libraries, the matching logic for the requests and responses needs to be identical. There is little confidence to be gained in having your pacts "pass" if the logic used to verify a "pass" is inconsistent between implementations.

Read more about Pact and the problems it solves at <https://github.com/realestate-com-au/pact>

PactNet is available from <https://github.com/SEEK-Jobs/pact-net>

## The Example Code

The exmple code in this Repo consists of a Client\Server application that both support two methods

1. Get all tribbles
1. Get a tribble with a particular id

And the following tests

1. client tests that generate a Pact file and confirm that the client calls the interactions
1. Server tests that use the generated Pact file to validate that the service honours the pact

All the projects target the .NET Framework 4.5.2 with the data access being done using EF Core 1.1.0 and Sqlite

To get running type the following at the command line:

```bash
mkdir psychic-tribble && cd psychic-tribble
git clone https://github.com/frozenskys/psychic-tribble .
cake
```

### The Client

An example method on the client would look something like this.

```csharp
    public Tribble GetTribble(int id)
    {
        using (var client = new HttpClient { BaseAddress = new Uri(BaseUri) })
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/tribbles/" + id);
            request.Headers.Add("Accept", "application/json");
            var response = client.SendAsync(request);
            var content = response.Result.Content.ReadAsStringAsync().Result;
            var status = response.Result.StatusCode;
            request.Dispose();
            response.Dispose();
            if (status == HttpStatusCode.OK)
            {
                return !string.IsNullOrEmpty(content) ? JsonConvert.DeserializeObject<Tribble>(content) : null;
            }
        }
        throw new Exception();
    }
```

The pact tests for this method look like this:

```csharp
    public void TestGetTribbleReturnsATribble ()
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
                Id = 1,
                Colour = "blue",
                Furryness = "High",
                Hungry = true
            }
        });
        var consumer = new TribbleClient.Client(_mockProviderServiceBaseUri);
        var result = consumer.GetTribble(1);
        Assert.AreEqual("blue", result.Colour);
        _mockProviderService.VerifyInteractions();
    }
```

This will generate a pact file with the following interaction:

```json
"interactions": [
    {
      "description": "A GET request to retrieve the tribble",
      "provider_state": "There is a tribble with id '1'",
      "request": {
        "method": "get",
        "path": "/tribbles/1",
        "headers": {
          "Accept": "application/json"
        }
      },
      "response": {
        "status": 200,
        "headers": {
          "Content-Type": "application/json; charset=utf-8"
        },
        "body": {
          "Id": 1,
          "Colour": "blue",
          "Furryness": "High",
          "Hungry": true
        }
      }
    }
]
```

### The Server

In the server side we setup a test to verify that the Api also honours this pact:

```csharp
    public void EnsureTribbleApiHonoursPactWithConsumer()
    {
        IPactVerifier pactVerifier = new PactVerifier(() => { }, () => { });
        pactVerifier.ProviderState("There is a tribble with id '1'");
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "consumer-tribble_api.json");
        using (var testServer = TestServer.Create<Startup>())
        {
            pactVerifier
                .ServiceProvider("Tribble API", testServer.HttpClient)
                .HonoursPactWith("Consumer")
                .PactUri(path)
                .Verify(null, "There is a tribble with id '1'");
        }
    }
```

And an a sample implementation in the server:

```csharp
    public Tribble Get(int id)
    {
        Tribble tribble;

        using (var db = new TribbleContext())
        {
            tribble = db.Tribbles.FirstOrDefault(s => s.Id == id);
        }
        return tribble;
    }
```