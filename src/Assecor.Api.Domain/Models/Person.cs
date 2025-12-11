namespace Assecor.Api.Domain.Models;

public class Person(string firstName, string lastName, Address address, Color color)
{
    public string FirstName { get; init; } = firstName;
    public string LastName { get; init; } = lastName;
    public Address? Address { get; init; } = address;
    public Color? Color { get; init; } = color;
}
