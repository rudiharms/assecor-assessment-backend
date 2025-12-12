using Assecor.Api.Application.DTOs;
using Assecor.Api.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace Assecor.Api.Application.Queries;

public record GetPersonByIdQuery(int Id) : IRequest<Result<PersonDto, Error>>;
