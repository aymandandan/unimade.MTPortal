using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace unimade.MTPortal.Migrations
{
    /// <inheritdoc />
    public partial class Added_Entity_Enxtensions_And_Configurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "AbpUsers",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "AbpTenants",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AbpTenants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpTenants",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "AbpTenants",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_TenantId_UserType",
                table: "AbpUsers",
                columns: new[] { "TenantId", "UserType" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_UserType",
                table: "AbpUsers",
                column: "UserType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_TenantId_UserType",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_UserType",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "AbpTenants");
        }
    }
}
