using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Services4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    CREATE OR REPLACE FUNCTION update_modified_column()
                    RETURNS TRIGGER AS $$
                    BEGIN 
                        NEW.""updated_at"" = now();
                        RETURN NEW;
                    END;
                    $$ language 'plpgsql';
                ");

            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS update_modtime ON ""services"";");


            migrationBuilder.Sql(@"
                CREATE TRIGGER update_modtime
                BEFORE UPDATE ON ""services""
                FOR EACH ROW 
                EXECUTE PROCEDURE update_modified_column();
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS update_service_modtime ON ""services"";");
        }
    }
}
