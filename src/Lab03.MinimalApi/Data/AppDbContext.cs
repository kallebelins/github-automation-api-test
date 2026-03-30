using Lab03.MinimalApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lab03.MinimalApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Produto> Produtos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.ToTable("Produtos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Descricao).HasMaxLength(500);
            entity.Property(e => e.Preco).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Quantidade).IsRequired();
            entity.Property(e => e.Ativo).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CriadoEm).IsRequired();
            entity.Property(e => e.AtualizadoEm);
        });
    }
}
