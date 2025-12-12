using Assecor.Api.Application.Abstractions;
using Assecor.Api.Application.DTOs;
using Assecor.Api.Application.Extensions;
using Assecor.Api.Application.Queries;
using Assecor.Api.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace Assecor.Api.Application.Handlers;

public class GetPersonByIdQueryHandler(IPersonRepository personRepository) : IRequestHandler<GetPersonByIdQuery, Result<PersonDto, Error>>
{
    public async Task<Result<PersonDto, Error>> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        var personResult = await personRepository.GetPersonByIdAsync(request.Id);

        if (personResult.IsFailure)
        {
            return personResult.Error;
        }

        return personResult.Value.ToPersonDto();
    }
}
