using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IMS.Infrastructure.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ims");
                        
            migrationBuilder.CreateTable(
                name: "Truck",
                schema: "ims",
                columns: table => new
                {
                    TruckId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<int>(nullable: false),
                    ManufactureYear = table.Column<int>(nullable: false),
                    ModelYear = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Truck", x => x.TruckId);
                });
                      
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Truck",
                schema: "ims");
        }
    }
}
