using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.Data.Migrations
{
    public partial class ChatMsg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ChatMessages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverUsername",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecieverImageUrl",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SendedOn",
                table: "ChatMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ApplicationUserId",
                table: "ChatMessages",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_AspNetUsers_ApplicationUserId",
                table: "ChatMessages",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_AspNetUsers_ApplicationUserId",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_ApplicationUserId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "ReceiverUsername",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "RecieverImageUrl",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "SendedOn",
                table: "ChatMessages");
        }
    }
}
