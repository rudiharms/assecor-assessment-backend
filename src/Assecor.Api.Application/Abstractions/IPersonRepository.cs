using Assecor.Api.Application.DTOs;
using Assecor.Api.Domain.Common;
using CSharpFunctionalExtensions;

namespace Assecor.Api.Application.Abstractions;

public interface IPersonRepository
{
    Task<Result<IEnumerable<PersonDto>, Error>> GetPersonsAsync();
    Task<Result<PersonDto, Error>> GetPersonByIdAsync(int id);
    Task<Result<IEnumerable<PersonDto>, Error>> GetPersonsByColorAsync(string color);
}
