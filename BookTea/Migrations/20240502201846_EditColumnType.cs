using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTea.Migrations
{
    /// <inheritdoc />
    public partial class EditColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "Books",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Rating",
                table: "Books",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
