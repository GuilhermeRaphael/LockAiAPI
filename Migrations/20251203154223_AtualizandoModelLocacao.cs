using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LockAi.Migrations
{
    /// <inheritdoc />
    public partial class AtualizandoModelLocacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanoLocacaoId",
                table: "Locacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Locacoes_PlanoLocacaoId",
                table: "Locacoes",
                column: "PlanoLocacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locacoes_PlanosLocacao_PlanoLocacaoId",
                table: "Locacoes",
                column: "PlanoLocacaoId",
                principalTable: "PlanosLocacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locacoes_PlanosLocacao_PlanoLocacaoId",
                table: "Locacoes");

            migrationBuilder.DropIndex(
                name: "IX_Locacoes_PlanoLocacaoId",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "PlanoLocacaoId",
                table: "Locacoes");
        }
    }
}
