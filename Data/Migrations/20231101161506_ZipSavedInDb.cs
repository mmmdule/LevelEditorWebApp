using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelEditorWebApp.Data.Migrations
{
    public partial class ZipSavedInDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadedPath",
                table: "Post");

            migrationBuilder.AddColumn<byte[]>(
                name: "ZipFile",
                table: "Post",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZipFile",
                table: "Post");

            migrationBuilder.AddColumn<string>(
                name: "UploadedPath",
                table: "Post",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
