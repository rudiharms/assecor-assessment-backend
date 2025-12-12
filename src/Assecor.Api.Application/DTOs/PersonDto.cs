namespace Assecor.Api.Application.DTOs;

public record PersonDto(int id, string FirstName, string LastName, AddressDto Address, ColorDto Color);
