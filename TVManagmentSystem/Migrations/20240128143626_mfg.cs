using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TVManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class mfg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Infos_ChanellID",
                table: "Infos");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_ChanellID",
                table: "Infos",
                column: "ChanellID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Infos_ChanellID",
                table: "Infos");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_ChanellID",
                table: "Infos",
                column: "ChanellID",
                unique: true);
        }
    }
}
