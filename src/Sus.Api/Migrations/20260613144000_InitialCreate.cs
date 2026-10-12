using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sus.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_PACIENTE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TELEFONE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_PACIENTE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "T_UNIDADE_SAUDE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ENDERECO = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CAPACIDADE_DIARIA = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_UNIDADE_SAUDE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "T_ATENDIMENTO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PACIENTE_ID = table.Column<long>(type: "bigint", nullable: false),
                    UNIDADE_ID = table.Column<long>(type: "bigint", nullable: false),
                    DATA_ENTRADA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DATA_ATENDIMENTO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ATENDIMENTO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_ATENDIMENTO_T_PACIENTE_PACIENTE_ID",
                        column: x => x.PACIENTE_ID,
                        principalTable: "T_PACIENTE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_ATENDIMENTO_T_UNIDADE_SAUDE_UNIDADE_ID",
                        column: x => x.UNIDADE_ID,
                        principalTable: "T_UNIDADE_SAUDE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_ALERTA",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MENSAGEM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DATA_ALERTA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ATENDIMENTO_ID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ALERTA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_ALERTA_T_ATENDIMENTO_ATENDIMENTO_ID",
                        column: x => x.ATENDIMENTO_ID,
                        principalTable: "T_ATENDIMENTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_AUDITORIA",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACAO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DATA_EVENTO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ATENDIMENTO_ID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AUDITORIA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_AUDITORIA_T_ATENDIMENTO_ATENDIMENTO_ID",
                        column: x => x.ATENDIMENTO_ID,
                        principalTable: "T_ATENDIMENTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "IX_T_ALERTA_ATENDIMENTO_ID", table: "T_ALERTA", column: "ATENDIMENTO_ID");
            migrationBuilder.CreateIndex(name: "IX_T_ATENDIMENTO_PACIENTE_ID", table: "T_ATENDIMENTO", column: "PACIENTE_ID");
            migrationBuilder.CreateIndex(name: "IX_T_ATENDIMENTO_UNIDADE_ID", table: "T_ATENDIMENTO", column: "UNIDADE_ID");
            migrationBuilder.CreateIndex(name: "IX_T_AUDITORIA_ATENDIMENTO_ID", table: "T_AUDITORIA", column: "ATENDIMENTO_ID");
            migrationBuilder.CreateIndex(name: "IX_T_PACIENTE_CPF", table: "T_PACIENTE", column: "CPF", unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "T_ALERTA");
            migrationBuilder.DropTable(name: "T_AUDITORIA");
            migrationBuilder.DropTable(name: "T_ATENDIMENTO");
            migrationBuilder.DropTable(name: "T_PACIENTE");
            migrationBuilder.DropTable(name: "T_UNIDADE_SAUDE");
        }
    }
}
