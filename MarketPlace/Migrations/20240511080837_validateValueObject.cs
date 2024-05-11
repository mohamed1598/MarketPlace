using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlace.Migrations
{
    /// <inheritdoc />
    public partial class validateValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picture_ClassifiedAds_ClassifiedAdId",
                table: "Picture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Picture",
                table: "Picture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassifiedAds",
                table: "ClassifiedAds");

            migrationBuilder.RenameColumn(
                name: "Id_Value",
                table: "ClassifiedAds",
                newName: "ClassifiedAdId1");

            migrationBuilder.AddColumn<Guid>(
                name: "PictureId1",
                table: "Picture",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Picture",
                table: "Picture",
                column: "PictureId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassifiedAds",
                table: "ClassifiedAds",
                column: "ClassifiedAdId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_ClassifiedAds_ClassifiedAdId",
                table: "Picture",
                column: "ClassifiedAdId",
                principalTable: "ClassifiedAds",
                principalColumn: "ClassifiedAdId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picture_ClassifiedAds_ClassifiedAdId",
                table: "Picture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Picture",
                table: "Picture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassifiedAds",
                table: "ClassifiedAds");

            migrationBuilder.DropColumn(
                name: "PictureId1",
                table: "Picture");

            migrationBuilder.RenameColumn(
                name: "ClassifiedAdId1",
                table: "ClassifiedAds",
                newName: "Id_Value");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Picture",
                table: "Picture",
                column: "PictureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassifiedAds",
                table: "ClassifiedAds",
                column: "ClassifiedAdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_ClassifiedAds_ClassifiedAdId",
                table: "Picture",
                column: "ClassifiedAdId",
                principalTable: "ClassifiedAds",
                principalColumn: "ClassifiedAdId");
        }
    }
}
