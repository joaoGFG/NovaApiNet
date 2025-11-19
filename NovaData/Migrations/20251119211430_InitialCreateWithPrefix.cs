using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaData.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithPrefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NET_TRILHAS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    AREA_INTERESSE = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    SKILL_RELACIONADA = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    NIVEL_MINIMO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TITULO_RECOMENDACAO = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    DESCRICAO_RECOMENDACAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NET_TRILHAS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NET_USUARIOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    AREA_INTERESSE = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: true),
                    OBJETIVO_PROFISSIONAL = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    CRIADO_EM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSDATE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NET_USUARIOS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NET_RECOMENDACOES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USUARIO_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TITULO = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    CRIADA_EM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSDATE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NET_RECOMENDACOES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NET_RECOMENDACOES_NET_USUARIOS_USUARIO_ID",
                        column: x => x.USUARIO_ID,
                        principalTable: "NET_USUARIOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NET_SKILLS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USUARIO_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NOME = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    NIVEL = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NET_SKILLS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NET_SKILLS_NET_USUARIOS_USUARIO_ID",
                        column: x => x.USUARIO_ID,
                        principalTable: "NET_USUARIOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NET_RECOMENDACOES_USUARIO_ID",
                table: "NET_RECOMENDACOES",
                column: "USUARIO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_NET_SKILLS_USUARIO_ID_NOME",
                table: "NET_SKILLS",
                columns: new[] { "USUARIO_ID", "NOME" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NET_TRILHAS_AREA_INTERESSE_SKILL_RELACIONADA_NIVEL_MINIMO",
                table: "NET_TRILHAS",
                columns: new[] { "AREA_INTERESSE", "SKILL_RELACIONADA", "NIVEL_MINIMO" });

            migrationBuilder.CreateIndex(
                name: "IX_NET_USUARIOS_EMAIL",
                table: "NET_USUARIOS",
                column: "EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NET_RECOMENDACOES");

            migrationBuilder.DropTable(
                name: "NET_SKILLS");

            migrationBuilder.DropTable(
                name: "NET_TRILHAS");

            migrationBuilder.DropTable(
                name: "NET_USUARIOS");
        }
    }
}
