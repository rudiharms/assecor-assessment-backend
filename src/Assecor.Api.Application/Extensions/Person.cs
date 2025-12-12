using Assecor.Api.Application.DTOs;
using Assecor.Api.Domain.Models;

namespace Assecor.Api.Application.Extensions;

public static class PersonExtensions
{
    public static PersonDto ToPersonDto(this Person person)
    {
        return new PersonDto(person.Id, person.FirstName, person.LastName, person.Address.ToAddressDto(), person.Color.ToColorDto());
    }

    private static AddressDto ToAddressDto(this Address address)
    {
        return new AddressDto(address.City, address.ZipCode);
    }

    private static ColorDto ToColorDto(this Color color)
    {
        return new ColorDto(color.ColorName.ToString());
    }
}
