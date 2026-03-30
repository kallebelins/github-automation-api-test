using FluentValidation;
using Lab03.MinimalApi.Data;
using Lab03.MinimalApi.Repositories;
using Lab03.MinimalApi.UnitOfWork;
using Lab03.MinimalApi.Validators;
using Microsoft.EntityFrameworkCore;

namespace Lab03.MinimalApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IUnitOfWork, Lab03.MinimalApi.UnitOfWork.UnitOfWork>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ProdutoRequestValidator>();
        return services;
    }

    public static IServiceCollection AddSwaggerDocs(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Lab03 - Produto API",
                Version = "v1",
                Description = "API para CRUD de Produtos usando .NET 9 Minimal API"
            });
        });
        return services;
    }

    public static IServiceCollection AddHealthChecksConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        services.AddHealthChecks()
            .AddSqlServer(
                connectionString: connectionString,
                name: "sqlserver",
                failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
                tags: ["db", "sql", "sqlserver"]);

        return services;
    }
}
