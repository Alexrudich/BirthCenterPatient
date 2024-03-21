using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirthCenterPatientAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    StatusChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientGenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });
            migrationBuilder.Sql($"SET IDENTITY_INSERT [dbo].[PatientGenders] ON");
            migrationBuilder.Sql($"INSERT INTO [dbo].[PatientGenders] ([Id], [Value]) VALUES (0, 'Unknown')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[PatientGenders] ([Id], [Value]) VALUES (1, 'Male')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[PatientGenders] ([Id], [Value]) VALUES (2, 'Female')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[PatientGenders] ([Id], [Value]) VALUES (3, 'Other')");
            migrationBuilder.Sql($"SET IDENTITY_INSERT [dbo].[PatientGenders] OFF");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientActivities");

            migrationBuilder.DropTable(
                name: "PatientGenders");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
