using Bogus;
using FakeAddressApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Fake Address API",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var addressService = new AddressService();
var addresses = addressService.GenerateFakeAddresses(4);

app.MapGet("/addresses", ([FromServices] ILogger<Program> logger, string? state, string? zip) =>
{
    logger.LogInformation("Getting fake addresses.  State: {State}, Zip: {Zip}", state, zip);

    var result = addresses.AsQueryable();

    if (!string.IsNullOrWhiteSpace(state))
        result = result.Where(a => a.State.Equals(state, StringComparison.OrdinalIgnoreCase));

    if (!string.IsNullOrWhiteSpace(zip))
        result = result.Where(a => a.ZipCode == zip);

    return Results.Ok(result);
});

app.MapPost("/new-addresses", ([FromServices] ILogger<Program> logger, AddressRequest request) =>
{
    logger.LogInformation("Generating {Count} new fake addresses", request.Count);

    addresses = addressService.GenerateFakeAddresses(request.Count);

    return Results.Ok(new { message = $"Generated {request.Count} new fake addresses." });
});

app.MapPost("/append-addresses", ([FromServices] ILogger<Program> logger, AddressRequest request) =>
{
    logger.LogInformation("Generating {Count} more fake addresses", request.Count);

    addresses.AddRange(addressService.GenerateFakeAddresses(request.Count));

    return Results.Ok(new { message = $"Generated {request.Count} more fake addresses.  Address count: {addresses.Count}" });
});

await app.RunAsync();

namespace FakeAddressApi
{
    public class AddressService
    {
        public List<Address> GenerateFakeAddresses(int count)
        {
            var faker = new Faker<Address>()
                .RuleFor(a => a.Street, f => f.Address.StreetAddress())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.State, f => f.Address.StateAbbr())
                .RuleFor(a => a.ZipCode, f => f.Address.ZipCode());

            var generatedAddresses = faker.Generate(count);

            generatedAddresses.Add(new Address()
            {
                Street = "1 Hardcoded Way",
                City = "The Woods",
                State = "RI",
                ZipCode = "02814"
            });

            return generatedAddresses;
        }
    }

    public record Address
    {
        public string Street { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        public string ZipCode { get; init; } = string.Empty;
    }

    public record AddressRequest(int Count);
}