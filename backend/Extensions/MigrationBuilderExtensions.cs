using Microsoft.EntityFrameworkCore.Migrations;

public static class MigrationBuilderExtensions
{
    public static void AddUpdateTimestampTrigger(this MigrationBuilder migrationBuilder, string tableName)
    {
        migrationBuilder.Sql(@"
            CREATE OR REPLACE FUNCTION update_modified_column()
            RETURNS TRIGGER AS $$
            BEGIN 
                NEW.updated_at = now();
                RETURN NEW;
            END;
            $$ language 'plpgsql';
        ");

        migrationBuilder.Sql($"DROP TRIGGER IF EXISTS update_{tableName}_modtime ON \"{tableName}\";");

        migrationBuilder.Sql($@"
            CREATE TRIGGER update_{tableName}_modtime
            BEFORE UPDATE ON ""{tableName}""
            FOR EACH ROW 
            EXECUTE PROCEDURE update_modified_column();
        ");
    }
}
