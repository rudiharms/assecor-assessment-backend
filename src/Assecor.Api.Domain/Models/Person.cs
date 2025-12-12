namespace Assecor.Api.Domain.Models;

public class Person(int id, string firstName, string lastName, Address address, Color color)
{
    public int Id { get; init; } = id;
    public string FirstName { get; init; } = firstName;
    public string LastName { get; init; } = lastName;
    public Address Address { get; init; } = address;
    public Color Color { get; init; } = color;
}
