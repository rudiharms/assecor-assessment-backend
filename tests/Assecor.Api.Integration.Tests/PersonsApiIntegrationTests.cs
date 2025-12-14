using System.Net;
using System.Text;
using System.Text.Json;
using Assecor.Api.Application.DTOs;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Assecor.Api.Integration.Tests;

//Note to reviewer, only works on csv data source for now.
//Did not get around to also implement for sql data source via testcontainers in time.
[Collection(nameof(TestCollection))]
public class PersonsApiIntegrationTests : IAsyncLifetime
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private HttpClient? _client;
    private CustomWebApplicationFactory? _factory;

    public async Task InitializeAsync()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        _client?.Dispose();
        _factory?.Dispose();
        await Task.CompletedTask;
    }

    [Fact]
    public async Task GetPersons_Returns_Persons()
    {
        var response = await _client!.GetAsync("/persons");

        var persons = await DeserializeResponse<List<PersonDto>>(response);
        var firstPerson = persons![0];

        using (new AssertionScope())
        {
            persons.Should().NotBeNull();
            persons.Should().HaveCount(3);

            firstPerson.Id.Should().Be(1);
            firstPerson.Name.Should().Be("Hans");
            firstPerson.LastName.Should().Be("Müller");
            firstPerson.ZipCode.Should().Be("67742");
            firstPerson.City.Should().Be("Lauterecken");
            firstPerson.Color.Should().Be("blau");
        }
    }

    [Fact]
    public async Task GetPersonById_Returns_Correct_Person_Data()
    {
        var response = await _client!.GetAsync("/persons/2");

        var person = await DeserializeResponse<PersonDto>(response);

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            person.Should().NotBeNull();
            person.Id.Should().Be(2);
            person.Name.Should().Be("Peter");
            person.LastName.Should().Be("Petersen");
            person.ZipCode.Should().Be("18439");
            person.City.Should().Be("Stralsund");
            person.Color.Should().Be("grün");
        }
    }

    [Fact]
    public async Task GetPersonById_Returns_ProblemDetails()
    {
        var response = await _client!.GetAsync("/persons/999");

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        }
    }

    [Theory]
    [InlineData("blau")]
    [InlineData("grün")]
    [InlineData("violett")]
    public async Task GetPersonsByColor_Returns_Ok_For_Valid_Colors(string color)
    {
        var response = await _client!.GetAsync($"/persons/color/{color}");

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var persons = await DeserializeResponse<List<PersonDto>>(response);
            persons.Should().NotBeNull();
            persons.Should().HaveCount(1);
        }
    }

    [Fact]
    public async Task GetPersonsByColor_Returns_BadRequest_For_Invalid_Colors()
    {
        var response = await _client!.GetAsync("/persons/color/invalid");

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        }
    }

    [Fact]
    public async Task GetPersonsByColor_Handles_Numeric_String_As_Valid()
    {
        var response = await _client!.GetAsync("/persons/color/1");

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var persons = await DeserializeResponse<List<PersonDto>>(response);
            persons.Should().NotBeNull();
            persons.Should().HaveCount(1);
        }
    }

    [Fact]
    public async Task CreatePerson_Returns_Created_Person()
    {
        var newPerson = new CreatePersonDto("Erika", "Musterfrau", "54321", "München", "gelb");

        var response = await _client!.PostAsync("/persons", SerializeRequest(newPerson));
        var createdPerson = await DeserializeResponse<PersonDto>(response);

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
            createdPerson!.Id.Should().BeGreaterThan(0);
            createdPerson.Name.Should().Be("Erika");
            createdPerson.LastName.Should().Be("Musterfrau");
            createdPerson.ZipCode.Should().Be("54321");
            createdPerson.City.Should().Be("München");
            createdPerson.Color.Should().Be("gelb");
        }
    }

    [Fact]
    public async Task CreatePerson_Persist_Person()
    {
        var newPerson = new CreatePersonDto("Anna", "Schmidt", "22222", "Köln", "weiß");

        var createResponse = await _client!.PostAsync("/persons", SerializeRequest(newPerson));
        var createdPerson = await DeserializeResponse<PersonDto>(createResponse);

        var getResponse = await _client!.GetAsync($"/persons/{createdPerson!.Id}");

        using (new AssertionScope())
        {
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var retrievedPerson = await DeserializeResponse<PersonDto>(getResponse);
            retrievedPerson.Should().BeEquivalentTo(createdPerson);
        }
    }

    private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(content, _jsonOptions);
    }

    private StringContent SerializeRequest<T>(T data)
    {
        var json = JsonSerializer.Serialize(data, _jsonOptions);

        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}
