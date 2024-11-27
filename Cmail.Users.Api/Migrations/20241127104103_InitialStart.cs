using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cmail.Users.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_ip = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    prefix_phone = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    bith_day = table.Column<int>(type: "integer", nullable: false),
                    bith_month = table.Column<int>(type: "integer", nullable: false),
                    bith_year = table.Column<int>(type: "integer", nullable: false),
                    gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    verification_code = table.Column<string>(type: "text", nullable: true),
                    is_verified = table.Column<bool>(type: "boolean", nullable: false),
                    verification_code_expire_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_login_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_reported = table.Column<bool>(type: "boolean", nullable: false),
                    report_count = table.Column<int>(type: "integer", nullable: true),
                    is_blocked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
