using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HiringService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateContractTabler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contracts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    proposal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    client_id = table.Column<Guid>(type: "uuid", nullable: false),
                    hiring_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    effective_date_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    effective_date_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    policy_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contract", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_contract_client_id",
                table: "contracts",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_contract_client_id_is_active",
                table: "contracts",
                columns: new[] { "client_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_contract_client_id_policy_number_is_active",
                table: "contracts",
                columns: new[] { "client_id", "policy_number", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_contract_client_id_proposal_id_is_active",
                table: "contracts",
                columns: new[] { "client_id", "proposal_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_contract_client_id_proposal_id_policy_number_is_active",
                table: "contracts",
                columns: new[] { "client_id", "proposal_id", "policy_number", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_contract_is_active",
                table: "contracts",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_contract_proposal_id",
                table: "contracts",
                column: "proposal_id");

            migrationBuilder.CreateIndex(
                name: "ix_contract_proposal_id_is_active",
                table: "contracts",
                columns: new[] { "proposal_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_contract_proposal_id_policy_number_is_active",
                table: "contracts",
                columns: new[] { "proposal_id", "policy_number", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ux_contract_client_id_policy_number",
                table: "contracts",
                columns: new[] { "client_id", "policy_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_contract_client_id_proposal_id",
                table: "contracts",
                columns: new[] { "client_id", "proposal_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_contract_client_id_proposal_id_policy_number",
                table: "contracts",
                columns: new[] { "client_id", "proposal_id", "policy_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_contract_external_id",
                table: "contracts",
                column: "external_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_contract_policy_number",
                table: "contracts",
                column: "policy_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_contract_proposal_id_policy_number",
                table: "contracts",
                columns: new[] { "proposal_id", "policy_number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contracts");
        }
    }
}
