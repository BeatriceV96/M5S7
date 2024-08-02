using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InForno.Migrations
{
    public partial class UpdateProductPhotoToByteArray : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "Products",
                newName: "PhotoTemp");


            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Products",
                type: "varbinary(max)",
                nullable: true);


            migrationBuilder.Sql(
                "UPDATE Products SET Photo = CONVERT(varbinary(max), PhotoTemp)");

            migrationBuilder.DropColumn(
                name: "PhotoTemp",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoTemp",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.Sql(
                "UPDATE Products SET PhotoTemp = CONVERT(nvarchar(max), Photo)");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "PhotoTemp",
                table: "Products",
                newName: "Photo");
        }
    }
}
