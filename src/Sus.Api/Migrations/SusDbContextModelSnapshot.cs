using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sus.Api.Data;

#nullable disable

namespace Sus.Api.Migrations
{
    [DbContext(typeof(SusDbContext))]
    partial class SusDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Sus.Api.Models.Paciente", b =>
            {
                b.Property<long>("Id").ValueGeneratedOnAdd().HasColumnType("bigint").HasColumnName("ID");
                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));
                b.Property<string>("Cpf").IsRequired().HasMaxLength(11).HasColumnType("nvarchar(11)").HasColumnName("CPF");
                b.Property<string>("Email").HasMaxLength(100).HasColumnType("nvarchar(100)").HasColumnName("EMAIL");
                b.Property<string>("Nome").IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)").HasColumnName("NOME");
                b.Property<string>("Telefone").HasMaxLength(20).HasColumnType("nvarchar(20)").HasColumnName("TELEFONE");
                b.HasKey("Id");
                b.HasIndex("Cpf").IsUnique();
                b.ToTable("T_PACIENTE");
            });

            modelBuilder.Entity("Sus.Api.Models.UnidadeSaude", b =>
            {
                b.Property<long>("Id").ValueGeneratedOnAdd().HasColumnType("bigint").HasColumnName("ID");
                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));
                b.Property<int>("CapacidadeDiaria").HasColumnType("int").HasColumnName("CAPACIDADE_DIARIA");
                b.Property<string>("Endereco").IsRequired().HasMaxLength(200).HasColumnType("nvarchar(200)").HasColumnName("ENDERECO");
                b.Property<string>("Nome").IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)").HasColumnName("NOME");
                b.HasKey("Id");
                b.ToTable("T_UNIDADE_SAUDE");
            });

            modelBuilder.Entity("Sus.Api.Models.Atendimento", b =>
            {
                b.Property<long>("Id").ValueGeneratedOnAdd().HasColumnType("bigint").HasColumnName("ID");
                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));
                b.Property<DateTime?>("DataAtendimento").HasColumnType("datetime2").HasColumnName("DATA_ATENDIMENTO");
                b.Property<DateTime>("DataEntrada").HasColumnType("datetime2").HasColumnName("DATA_ENTRADA");
                b.Property<long>("PacienteId").HasColumnType("bigint").HasColumnName("PACIENTE_ID");
                b.Property<string>("Status").IsRequired().HasMaxLength(30).HasColumnType("nvarchar(30)").HasColumnName("STATUS");
                b.Property<long>("UnidadeId").HasColumnType("bigint").HasColumnName("UNIDADE_ID");
                b.HasKey("Id");
                b.HasIndex("PacienteId");
                b.HasIndex("UnidadeId");
                b.ToTable("T_ATENDIMENTO");
            });

            modelBuilder.Entity("Sus.Api.Models.Alerta", b =>
            {
                b.Property<long>("Id").ValueGeneratedOnAdd().HasColumnType("bigint").HasColumnName("ID");
                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));
                b.Property<long>("AtendimentoId").HasColumnType("bigint").HasColumnName("ATENDIMENTO_ID");
                b.Property<DateTime>("DataAlerta").HasColumnType("datetime2").HasColumnName("DATA_ALERTA");
                b.Property<string>("Mensagem").IsRequired().HasMaxLength(255).HasColumnType("nvarchar(255)").HasColumnName("MENSAGEM");
                b.HasKey("Id");
                b.HasIndex("AtendimentoId");
                b.ToTable("T_ALERTA");
            });

            modelBuilder.Entity("Sus.Api.Models.Auditoria", b =>
            {
                b.Property<long>("Id").ValueGeneratedOnAdd().HasColumnType("bigint").HasColumnName("ID");
                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));
                b.Property<string>("Acao").IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)").HasColumnName("ACAO");
                b.Property<long>("AtendimentoId").HasColumnType("bigint").HasColumnName("ATENDIMENTO_ID");
                b.Property<DateTime>("DataEvento").HasColumnType("datetime2").HasColumnName("DATA_EVENTO");
                b.HasKey("Id");
                b.HasIndex("AtendimentoId");
                b.ToTable("T_AUDITORIA");
            });

            modelBuilder.Entity("Sus.Api.Models.Atendimento", b =>
            {
                b.HasOne("Sus.Api.Models.Paciente", "Paciente").WithMany().HasForeignKey("PacienteId").OnDelete(DeleteBehavior.Restrict).IsRequired();
                b.HasOne("Sus.Api.Models.UnidadeSaude", "Unidade").WithMany().HasForeignKey("UnidadeId").OnDelete(DeleteBehavior.Restrict).IsRequired();
                b.Navigation("Paciente");
                b.Navigation("Unidade");
            });

            modelBuilder.Entity("Sus.Api.Models.Alerta", b =>
            {
                b.HasOne("Sus.Api.Models.Atendimento", "Atendimento").WithMany().HasForeignKey("AtendimentoId").OnDelete(DeleteBehavior.Cascade).IsRequired();
                b.Navigation("Atendimento");
            });

            modelBuilder.Entity("Sus.Api.Models.Auditoria", b =>
            {
                b.HasOne("Sus.Api.Models.Atendimento", "Atendimento").WithMany().HasForeignKey("AtendimentoId").OnDelete(DeleteBehavior.Cascade).IsRequired();
                b.Navigation("Atendimento");
            });
        }
    }
}
