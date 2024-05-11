using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlace.Migrations
{
    /// <inheritdoc />
    public partial class IntialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassifiedAds",
                columns: table => new
                {
                    ClassifiedAdId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId_Value = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title_Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text_Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price_Currency_CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price_Currency_InUse = table.Column<bool>(type: "bit", nullable: false),
                    Price_Currency_DecimalPlaces = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy_Value = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_Value = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassifiedAds", x => x.ClassifiedAdId);
                });

            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    PictureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId_Value = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Size_Width = table.Column<int>(type: "int", nullable: false),
                    Size_Height = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ClassifiedAdId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.PictureId);
                    table.ForeignKey(
                        name: "FK_Picture_ClassifiedAds_ClassifiedAdId",
                        column: x => x.ClassifiedAdId,
                        principalTable: "ClassifiedAds",
                        principalColumn: "ClassifiedAdId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Picture_ClassifiedAdId",
                table: "Picture",
                column: "ClassifiedAdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Picture");

            migrationBuilder.DropTable(
                name: "ClassifiedAds");
        }
    }
}
