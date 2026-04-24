using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ShopifyStore.Data;

public static class DbSchemaUpdater
{
    public static bool HasMigrationsHistoryTable(AppDbContext db)
        => TableExists(db, "__EFMigrationsHistory");

    public static void EnsureCompatibilityColumns(AppDbContext db)
    {
        if (!TableExists(db, "Orders"))
        {
            return;
        }

        db.Database.ExecuteSqlRaw(
            """
            IF COL_LENGTH('dbo.Orders', 'BankTransactionId') IS NULL
            BEGIN
                ALTER TABLE dbo.Orders ADD BankTransactionId nvarchar(60) NOT NULL CONSTRAINT DF_Orders_BankTransactionId DEFAULT('');
            END
            """);

        db.Database.ExecuteSqlRaw(
            """
            IF COL_LENGTH('dbo.Orders', 'PaymentScreenshotUrl') IS NULL
            BEGIN
                ALTER TABLE dbo.Orders ADD PaymentScreenshotUrl nvarchar(200) NOT NULL CONSTRAINT DF_Orders_PaymentScreenshotUrl DEFAULT('');
            END
            """);

        db.Database.ExecuteSqlRaw(
            """
            IF COL_LENGTH('dbo.Orders', 'OrderStatus') IS NULL
            BEGIN
                ALTER TABLE dbo.Orders ADD OrderStatus int NOT NULL CONSTRAINT DF_Orders_OrderStatus DEFAULT(1);
            END
            """);

        db.Database.ExecuteSqlRaw(
            """
            IF COL_LENGTH('dbo.Orders', 'PaymentStatus') IS NULL
            BEGIN
                ALTER TABLE dbo.Orders ADD PaymentStatus int NOT NULL CONSTRAINT DF_Orders_PaymentStatus DEFAULT(1);
            END
            """);
    }

    private static bool TableExists(AppDbContext db, string tableName)
    {
        var connection = db.Database.GetDbConnection();
        var openedHere = connection.State != ConnectionState.Open;
        if (openedHere)
        {
            connection.Open();
        }

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText =
                "SELECT COUNT(1) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = @tableName";

            var parameter = command.CreateParameter();
            parameter.ParameterName = "@tableName";
            parameter.Value = tableName;
            command.Parameters.Add(parameter);

            var count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
        finally
        {
            if (openedHere)
            {
                connection.Close();
            }
        }
    }
}
