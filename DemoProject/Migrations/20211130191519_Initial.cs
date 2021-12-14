using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoProject.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Test_Attributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContractorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FooterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PapeSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Scale = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StageEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sheet = table.Column<int>(type: "int", nullable: true),
                    TotalSheets = table.Column<int>(type: "int", nullable: true),
                    Rev = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EngDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RusDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StageRu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Issue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClientRev = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurposeIssue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Attributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Test_Bytes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Bytes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Test_CheckResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Desription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_CheckResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Test_Checks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Checks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Test_Transmittals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransmittalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contractor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserUpload = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Transmittals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Test_Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TransmittalId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Test_Documents_Test_Transmittals",
                        column: x => x.TransmittalId,
                        principalTable: "Test_Transmittals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Test_Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageNumber = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ByteId = table.Column<int>(type: "int", nullable: false),
                    AttributesId = table.Column<int>(type: "int", nullable: true),
                    CheckResultsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Test_Files_Test_Attributes",
                        column: x => x.AttributesId,
                        principalTable: "Test_Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Test_Files_Test_Bytes",
                        column: x => x.ByteId,
                        principalTable: "Test_Bytes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Test_Files_Test_CheckResults",
                        column: x => x.CheckResultsId,
                        principalTable: "Test_CheckResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Test_Files_Test_Documents",
                        column: x => x.DocumentId,
                        principalTable: "Test_Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Test_Documents_TransmittalId",
                table: "Test_Documents",
                column: "TransmittalId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Files_AttributesId",
                table: "Test_Files",
                column: "AttributesId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Files_ByteId",
                table: "Test_Files",
                column: "ByteId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Files_CheckResultsId",
                table: "Test_Files",
                column: "CheckResultsId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Files_DocumentId",
                table: "Test_Files",
                column: "DocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Test_Checks");

            migrationBuilder.DropTable(
                name: "Test_Files");

            migrationBuilder.DropTable(
                name: "Test_Attributes");

            migrationBuilder.DropTable(
                name: "Test_Bytes");

            migrationBuilder.DropTable(
                name: "Test_CheckResults");

            migrationBuilder.DropTable(
                name: "Test_Documents");

            migrationBuilder.DropTable(
                name: "Test_Transmittals");
        }
    }
}
