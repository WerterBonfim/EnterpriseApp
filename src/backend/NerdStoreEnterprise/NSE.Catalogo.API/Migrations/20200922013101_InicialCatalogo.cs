using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Catalogo.API.Migrations
{
    public partial class InicialCatalogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Produtos",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nome = table.Column<string>("varchar(250)", nullable: false),
                    Descricao = table.Column<string>("varchar(500)", nullable: false),
                    Ativo = table.Column<bool>(nullable: false),
                    Valor = table.Column<decimal>(nullable: false),
                    DataCadastro = table.Column<DateTime>(nullable: false),
                    Imagem = table.Column<string>("varchar(250)", nullable: false),
                    QuantidadeEstoque = table.Column<int>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Produtos", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Produtos");
        }
    }
}