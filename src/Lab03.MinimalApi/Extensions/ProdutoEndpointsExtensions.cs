using FluentValidation;
using Lab03.MinimalApi.DTOs;
using Lab03.MinimalApi.Entities;
using Lab03.MinimalApi.Repositories;
using Lab03.MinimalApi.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Lab03.MinimalApi.Extensions;

public static class ProdutoEndpointsExtensions
{
    public static RouteGroupBuilder MapProdutoEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllProdutos)
            .WithName("GetAllProdutos")
            .WithSummary("Listar todos os produtos")
            .Produces<IEnumerable<ProdutoResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", GetProdutoById)
            .WithName("GetProdutoById")
            .WithSummary("Obter produto por ID")
            .Produces<ProdutoResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateProduto)
            .WithName("CreateProduto")
            .WithSummary("Criar novo produto")
            .Produces<ProdutoResponse>(StatusCodes.Status201Created)
            .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", UpdateProduto)
            .WithName("UpdateProduto")
            .WithSummary("Atualizar produto")
            .Produces<ProdutoResponse>(StatusCodes.Status200OK)
            .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}", DeleteProduto)
            .WithName("DeleteProduto")
            .WithSummary("Excluir produto")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }

    private static async Task<IResult> GetAllProdutos(
        IProdutoRepository repository,
        CancellationToken cancellationToken)
    {
        var produtos = await repository.GetAllAsync(cancellationToken);
        var response = produtos.Select(MapToResponse);
        return Results.Ok(response);
    }

    private static async Task<IResult> GetProdutoById(
        int id,
        IProdutoRepository repository,
        CancellationToken cancellationToken)
    {
        var produto = await repository.GetByIdAsync(id, cancellationToken);
        if (produto is null)
            return Results.NotFound();

        return Results.Ok(MapToResponse(produto));
    }

    private static async Task<IResult> CreateProduto(
        ProdutoRequest request,
        IValidator<ProdutoRequest> validator,
        IProdutoRepository repository,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var produto = MapToEntity(request);
        await repository.AddAsync(produto, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Results.CreatedAtRoute("GetProdutoById", new { id = produto.Id }, MapToResponse(produto));
    }

    private static async Task<IResult> UpdateProduto(
        int id,
        ProdutoRequest request,
        IValidator<ProdutoRequest> validator,
        IProdutoRepository repository,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var produto = await repository.GetByIdAsync(id, cancellationToken);
        if (produto is null)
            return Results.NotFound();

        produto.Nome = request.Nome;
        produto.Descricao = request.Descricao;
        produto.Preco = request.Preco;
        produto.Quantidade = request.Quantidade;
        produto.Ativo = request.Ativo;
        produto.AtualizadoEm = DateTime.UtcNow;

        repository.Update(produto);
        await unitOfWork.CommitAsync(cancellationToken);

        return Results.Ok(MapToResponse(produto));
    }

    private static async Task<IResult> DeleteProduto(
        int id,
        IProdutoRepository repository,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var produto = await repository.GetByIdAsync(id, cancellationToken);
        if (produto is null)
            return Results.NotFound();

        repository.Delete(produto);
        await unitOfWork.CommitAsync(cancellationToken);

        return Results.NoContent();
    }

    private static ProdutoResponse MapToResponse(Produto produto) => new()
    {
        Id = produto.Id,
        Nome = produto.Nome,
        Descricao = produto.Descricao,
        Preco = produto.Preco,
        Quantidade = produto.Quantidade,
        Ativo = produto.Ativo,
        CriadoEm = produto.CriadoEm,
        AtualizadoEm = produto.AtualizadoEm
    };

    private static Produto MapToEntity(ProdutoRequest request) => new()
    {
        Nome = request.Nome,
        Descricao = request.Descricao,
        Preco = request.Preco,
        Quantidade = request.Quantidade,
        Ativo = request.Ativo,
        CriadoEm = DateTime.UtcNow
    };
}
