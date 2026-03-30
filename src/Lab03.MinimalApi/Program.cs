using Lab03.MinimalApi.Data;
using Lab03.MinimalApi.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddValidators();
builder.Services.AddSwaggerDocs();
builder.Services.AddHealthChecksConfig(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db")
});

app.MapGroup("/api/produtos")
    .WithTags("Produtos")
    .MapProdutoEndpoints();

app.Run();

public partial class Program { }
