using Assecor.Api.Application.DTOs;
using Assecor.Api.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace Assecor.Api.Application.Queries;

public record GetPersonsQuery : IRequest<Result<IEnumerable<PersonDto>, Error>>;
