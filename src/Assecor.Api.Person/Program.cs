using Assecor.Api.Infrastructure;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Assecor.Api.Application.Queries.GetPersonsQuery).Assembly));

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "v1");
        }
    );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
