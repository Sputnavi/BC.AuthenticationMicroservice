using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BC.AuthenticationMicroservice.Migrations
{
    public partial class initUsersWithRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "5E17EAB5-C0D7-45FE-AD03-774E657CEBF3", "5D5E025D-85FB-46DE-8FE4-6A7686981027" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5D5E025D-85FB-46DE-8FE4-6A7686981027");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5E17EAB5-C0D7-45FE-AD03-774E657CEBF3",
                column: "ConcurrencyStamp",
                value: "322c44bd-2077-45da-80d5-bfcfd54ef9f4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "A6BAD0EA-29F8-43A0-BB4B-B37077E16076",
                column: "ConcurrencyStamp",
                value: "d28fece5-13ee-4577-ac4f-01070aa42c9e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "DD3E7256-D5EA-49A7-A390-2E6EEF3DFEB3",
                column: "ConcurrencyStamp",
                value: "448d0121-2d55-40ea-8de5-17d03a009854");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecondName", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "ABBAD0EA-29F8-43A0-BB4B-B37077E16076", 0, "17a9f7e5-72d2-4a8d-9f6c-a287a793b3e8", "Master@Master.com", false, "Master", false, null, "MASTER@MASTER.COM", "MASTER", "AQAAAAEAACcQAAAAEKuXm9bjM26FgJRuwsQvFBX9/f1YOHKIaqFsk3yWBwjxQEOP1S5ODVg8Rwx39vlycw==", null, false, "Master", "4c516ff3-cd7f-4a78-b821-aaa252d66ad1", false, "Master" },
                    { "BB3E7256-D5EA-49A7-A390-2E6EEF3DFEB3", 0, "413ce292-52cc-4587-9777-b56ec11b32bb", "User@User.com", false, "User", false, null, "USER@USER.COM", "USER", "AQAAAAEAACcQAAAAEC55x0jtxzz8MyhGydD56VF8fgx+WF4drxSl4w4g7DOSUHHdHqom7ukrTH7ZJzuOXg==", null, false, "User", "b55c087b-ffa3-49a5-a72a-54743f31b21c", false, "User" },
                    { "BB7EAB5-C0D7-45FE-AD03-774E657CEBF3", 0, "158c3055-98c4-4c71-98ec-dd2dadde4d4d", "Admin@Admin.com", false, "Admin", false, null, "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAEAACcQAAAAEIh6YqN1IWkOmJDe99HRDig7zIEIMx/v991Z8agTvDI9KTfGV9ZKQGGGA12LGSs5xQ==", null, false, "Admin", "fa8168ea-1c1d-47d2-a9e4-51d4feb8b390", false, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "A6BAD0EA-29F8-43A0-BB4B-B37077E16076", "ABBAD0EA-29F8-43A0-BB4B-B37077E16076" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "DD3E7256-D5EA-49A7-A390-2E6EEF3DFEB3", "BB3E7256-D5EA-49A7-A390-2E6EEF3DFEB3" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "5E17EAB5-C0D7-45FE-AD03-774E657CEBF3", "BB7EAB5-C0D7-45FE-AD03-774E657CEBF3" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "A6BAD0EA-29F8-43A0-BB4B-B37077E16076", "ABBAD0EA-29F8-43A0-BB4B-B37077E16076" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "DD3E7256-D5EA-49A7-A390-2E6EEF3DFEB3", "BB3E7256-D5EA-49A7-A390-2E6EEF3DFEB3" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "5E17EAB5-C0D7-45FE-AD03-774E657CEBF3", "BB7EAB5-C0D7-45FE-AD03-774E657CEBF3" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ABBAD0EA-29F8-43A0-BB4B-B37077E16076");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BB3E7256-D5EA-49A7-A390-2E6EEF3DFEB3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BB7EAB5-C0D7-45FE-AD03-774E657CEBF3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5E17EAB5-C0D7-45FE-AD03-774E657CEBF3",
                column: "ConcurrencyStamp",
                value: "ba648c99-eddc-4b10-b8f8-5ee0c3fd3670");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "A6BAD0EA-29F8-43A0-BB4B-B37077E16076",
                column: "ConcurrencyStamp",
                value: "1bf705a5-627e-4f09-aaa2-0f9782ba8520");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "DD3E7256-D5EA-49A7-A390-2E6EEF3DFEB3",
                column: "ConcurrencyStamp",
                value: "0846d20c-9435-49dd-a523-038a182518cd");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecondName", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "5D5E025D-85FB-46DE-8FE4-6A7686981027", 0, "e773f0d1-9ab8-497f-be29-2de810349c90", "admin@admin.com", false, "admin", false, null, "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAEAACcQAAAAEBU++ebPmtoAtbwjgWtRxw3OqU3aBuyf5xQ1P17kvej80MIR8x9goIoCy/EOMV5iCw==", null, false, "admin", "70a50b39-7fc0-461c-975e-60a23e7cd2db", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "5E17EAB5-C0D7-45FE-AD03-774E657CEBF3", "5D5E025D-85FB-46DE-8FE4-6A7686981027" });
        }
    }
}
