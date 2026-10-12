using Microsoft.EntityFrameworkCore;
using Sus.Api.Models;

namespace Sus.Api.Data;

public class SusDbContext(DbContextOptions<SusDbContext> options) : DbContext(options)
{
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<UnidadeSaude> UnidadesSaude => Set<UnidadeSaude>();
    public DbSet<Atendimento> Atendimentos => Set<Atendimento>();
    public DbSet<Alerta> Alertas => Set<Alerta>();
    public DbSet<Auditoria> Auditorias => Set<Auditoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Paciente>()
            .HasIndex(paciente => paciente.Cpf)
            .IsUnique();

        modelBuilder.Entity<Atendimento>()
            .HasOne(atendimento => atendimento.Paciente)
            .WithMany()
            .HasForeignKey(atendimento => atendimento.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Atendimento>()
            .HasOne(atendimento => atendimento.Unidade)
            .WithMany()
            .HasForeignKey(atendimento => atendimento.UnidadeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Alerta>()
            .HasOne(alerta => alerta.Atendimento)
            .WithMany()
            .HasForeignKey(alerta => alerta.AtendimentoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Auditoria>()
            .HasOne(auditoria => auditoria.Atendimento)
            .WithMany()
            .HasForeignKey(auditoria => auditoria.AtendimentoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
