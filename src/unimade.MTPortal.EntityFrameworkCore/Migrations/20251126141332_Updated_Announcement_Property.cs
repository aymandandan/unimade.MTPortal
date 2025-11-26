using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace unimade.MTPortal.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Announcement_Property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublishedDate",
                table: "AppAnnouncements",
                newName: "PublishDate");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "AppAnnouncements",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateIndex(
                name: "IX_AppAnnouncements_TenantId_IsPublished_PublishDate",
                table: "AppAnnouncements",
                columns: new[] { "TenantId", "IsPublished", "PublishDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppAnnouncements_TenantId_IsPublished_PublishDate",
                table: "AppAnnouncements");

            migrationBuilder.RenameColumn(
                name: "PublishDate",
                table: "AppAnnouncements",
                newName: "PublishedDate");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "AppAnnouncements",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }
    }
}
