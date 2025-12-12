using Assecor.Api.Application.Abstractions;
using Assecor.Api.Application.DTOs;
using Assecor.Api.Application.Extensions;
using Assecor.Api.Application.Queries;
using Assecor.Api.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace Assecor.Api.Application.Handlers;

public class GetPersonsByColorQueryHandler(IPersonRepository personRepository)
    : IRequestHandler<GetPersonsByColorQuery, Result<IEnumerable<PersonDto>, Error>>
{
    public async Task<Result<IEnumerable<PersonDto>, Error>> Handle(GetPersonsByColorQuery request, CancellationToken cancellationToken)
    {
        var personsResult = await personRepository.GetPersonsByColorAsync(request.ColorName);

        if (personsResult.IsFailure)
        {
            return personsResult.Error;
        }

        var personDtos = personsResult.Value.Select(static person => person.ToPersonDto());

        return Result.Success<IEnumerable<PersonDto>, Error>(personDtos);
    }
}
