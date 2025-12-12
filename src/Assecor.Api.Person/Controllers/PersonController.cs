using Assecor.Api.Application.Queries;
using Assecor.Api.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Assecor.Api.Person.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPersons()
    {
        var result = await sender.Send(new GetPersonsQuery());

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error.Message });
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPersonById(int id)
    {
        var result = await sender.Send(new GetPersonByIdQuery(id));

        if (result.IsFailure)
        {
            return NotFound(new { error = result.Error.Message });
        }

        return Ok(result.Value);
    }

    [HttpGet("color/{colorName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPersonsByColor(ColorName colorName)
    {
        var result = await sender.Send(new GetPersonsByColorQuery(colorName));

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error.Message });
        }

        return Ok(result.Value);
    }
}
