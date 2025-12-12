using Assecor.Api.Application.Abstractions;
using Assecor.Api.Domain.Common;
using Assecor.Api.Domain.Enums;
using Assecor.Api.Domain.Models;
using Assecor.Api.Infrastructure.Abstractions;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;

namespace Assecor.Api.Infrastructure.CSV;

public class CsvPersonRepository(ICsvService csvService, ILogger<CsvPersonRepository> logger) : IPersonRepository
{
    public async Task<Result<IEnumerable<Person>, Error>> GetPersonsAsync()
    {
        var csvDataResult = await csvService.GetDataAsync();

        if (csvDataResult.IsFailure)
        {
            return csvDataResult.Error;
        }

        var persons = new List<Person>();

        var personId = 1;

        foreach (var row in csvDataResult.Value)
        {
            var personResult = row.ToPerson(personId);

            if (personResult.IsFailure)
            {
                logger.LogWarning("Failed to convert CSV row to Person: {Error}", personResult.Error.Message);

                continue;
            }

            persons.Add(personResult.Value);
            personId++;
        }

        return persons;
    }

    public async Task<Result<Person, Error>> GetPersonByIdAsync(int id)
    {
        var personsResult = await GetPersonsAsync();

        if (personsResult.IsFailure)
        {
            return personsResult.Error;
        }

        var person = personsResult.Value.FirstOrDefault(person => person.Id == id);

        if (person is null)
        {
            return Errors.PersonNotFound($"Person with ID {id} not found.");
        }

        return person;
    }

    public async Task<Result<IEnumerable<Person>, Error>> GetPersonsByColorAsync(ColorName colorName)
    {
        var personsResult = await GetPersonsAsync();

        if (personsResult.IsFailure)
        {
            return personsResult.Error;
        }

        var filteredPersons = personsResult.Value.Where(p => p.Color.ColorName == colorName).ToList();

        return filteredPersons;
    }

    public async Task<Result<IEnumerable<Person>, Error>> GetPersonsByColorAsync(int colorId)
    {
        var personsResult = await GetPersonsAsync();

        if (personsResult.IsFailure)
        {
            return personsResult.Error;
        }

        var filteredPersons = personsResult.Value.Where(p => p.Color.Id == colorId).ToList();

        return filteredPersons;
    }
}
