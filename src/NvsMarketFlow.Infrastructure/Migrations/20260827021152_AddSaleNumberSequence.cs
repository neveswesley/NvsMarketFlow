using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NvsMarketFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSaleNumberSequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE SEQUENCE SaleNumberSequence AS INT START WITH 1 INCREMENT BY 1;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP SEQUENCE SaleNumberSequence;");
        }
    }
}
