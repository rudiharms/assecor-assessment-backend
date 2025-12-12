using Assecor.Api.Application.Abstractions;
using Assecor.Api.Domain.Common;
using Assecor.Api.Domain.Models;
using CSharpFunctionalExtensions;

namespace Assecor.Api.Infrastructure.SQL;

public class SQLPersonRepository : IPersonRepository
{
    public Task<Result<IEnumerable<Person>, Error>> GetPersonsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<Person, Error>> GetPersonByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Person>, Error>> GetPersonsByColorAsync(string colorName)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Person>, Error>> GetPersonsByColorAsync(int colorId)
    {
        throw new NotImplementedException();
    }
}
