using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalaryCalculation.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValueSql: "datetime('now')"),
                    UpdateDate = table.Column<DateTime>(nullable: false, defaultValueSql: "datetime('now')"),
                    Code = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    Decription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValueSql: "datetime('now')"),
                    UpdateDate = table.Column<DateTime>(nullable: false, defaultValueSql: "datetime('now')"),
                    Login = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    MiddleName = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    BaseSalaryPart = table.Column<decimal>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationStructure",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValueSql: "datetime('now')"),
                    UpdateDate = table.Column<DateTime>(nullable: false, defaultValueSql: "datetime('now')"),
                    PersonId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    MaterializedPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationStructure", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrganizationStructure_OrganizationStructure_ParentId",
                        column: x => x.ParentId,
                        principalTable: "OrganizationStructure",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationStructure_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person2Groups",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValueSql: "datetime('now')"),
                    UpdateDate = table.Column<DateTime>(nullable: false, defaultValueSql: "datetime('now')"),
                    PersonID = table.Column<int>(nullable: false),
                    GroupType = table.Column<string>(nullable: false),
                    PeriodStart = table.Column<DateTime>(nullable: false),
                    PeriodEnd = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person2Groups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Person2Groups_Persons_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Persons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configs_Code",
                table: "Configs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationStructure_ParentId",
                table: "OrganizationStructure",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationStructure_PersonId",
                table: "OrganizationStructure",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person2Groups_PersonID",
                table: "Person2Groups",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Login",
                table: "Persons",
                column: "Login",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "OrganizationStructure");

            migrationBuilder.DropTable(
                name: "Person2Groups");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
