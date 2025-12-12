using Assecor.Api.Application.DTOs;
using Assecor.Api.Domain.Common;
using Assecor.Api.Domain.Enums;
using CSharpFunctionalExtensions;
using MediatR;

namespace Assecor.Api.Application.Queries;

public record GetPersonsByColorQuery(ColorName ColorName) : IRequest<Result<IEnumerable<PersonDto>, Error>>;
