using Assecor.Api.Domain.Common;
using CSharpFunctionalExtensions;

namespace Assecor.Api.Domain.Models;

public class Person
{
    private const int MaxFirstNameLength = 200;
    private const int MaxLastNameLength = 200;

    private Person(int id, string firstName, string lastName, Address address, Color color)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Color = color;
    }

    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public Address Address { get; }
    public Color Color { get; }

    public static Result<Person, Error> Create(int id, string firstName, string lastName, Address address, Color color)
    {
        firstName = firstName.Trim();
        lastName = lastName.Trim();

        if (firstName.Length > MaxFirstNameLength)
        {
            return Errors.PersonValidationFailed(nameof(FirstName), MaxFirstNameLength);
        }

        if (lastName.Length > MaxLastNameLength)
        {
            return Errors.PersonValidationFailed(nameof(LastName), MaxLastNameLength);
        }

        var person = new Person(id, firstName, lastName, address, color);

        return person;
    }
}
