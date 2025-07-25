using ClubAccessControl.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

public class ClubeContext : DbContext
{
    public ClubeContext(DbContextOptions<ClubeContext> options) : base(options) { }

    public DbSet<Socio> Socios { get; set; }
    public DbSet<PlanoAcesso> Planos { get; set; }
    public DbSet<AreaClube> Areas { get; set; }
    public DbSet<TentativaAcesso> TentativasAcesso { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlanoAcesso>()
            .HasMany(p => p.AreasPermitidas)
            .WithMany(a => a.PlanosPermitidos)
            .UsingEntity<Dictionary<string, object>>(
                "PlanoArea",
                j => j.HasOne<AreaClube>().WithMany().HasForeignKey("AreaClubeId"),
                j => j.HasOne<PlanoAcesso>().WithMany().HasForeignKey("PlanoAcessoId"));
    }
}
