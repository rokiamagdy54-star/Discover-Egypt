using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class ArabicVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "ReadyPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleAr",
                table: "ReadyPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityAr",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Nationalities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "CustomPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleAr",
                table: "CustomPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleAr",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "ReadyPlans");

            migrationBuilder.DropColumn(
                name: "TitleAr",
                table: "ReadyPlans");

            migrationBuilder.DropColumn(
                name: "CityAr",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "CustomPlans");

            migrationBuilder.DropColumn(
                name: "TitleAr",
                table: "CustomPlans");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TitleAr",
                table: "Categories");
        }
    }
}
