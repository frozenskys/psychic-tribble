# psychic-tribble

A PactNet example [![Build status](https://ci.appveyor.com/api/projects/status/0v8x7cci3bq4dwxj/branch/master?svg=true)](https://ci.appveyor.com/project/frozenskys/psychic-tribble/branch/master)

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
