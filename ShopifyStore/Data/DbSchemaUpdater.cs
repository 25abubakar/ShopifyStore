using Microsoft.EntityFrameworkCore;

namespace ShopifyStore.Data;

public static class DbSchemaUpdater
{
    public static void EnsureCompatibilityColumns(AppDbContext db)
    {
        db.Database.ExecuteSqlRaw(
            """
            IF COL_LENGTH('Orders', 'BankTransactionId') IS NULL
            BEGIN
                ALTER TABLE Orders ADD BankTransactionId nvarchar(60) NOT NULL CONSTRAINT DF_Orders_BankTransactionId DEFAULT('');
            END
            """);

        db.Database.ExecuteSqlRaw(
            """
            IF COL_LENGTH('Orders', 'PaymentScreenshotUrl') IS NULL
            BEGIN
                ALTER TABLE Orders ADD PaymentScreenshotUrl nvarchar(200) NOT NULL CONSTRAINT DF_Orders_PaymentScreenshotUrl DEFAULT('');
            END
            """);

        db.Database.ExecuteSqlRaw(
            """
            IF COL_LENGTH('Orders', 'OrderStatus') IS NULL
            BEGIN
                ALTER TABLE Orders ADD OrderStatus int NOT NULL CONSTRAINT DF_Orders_OrderStatus DEFAULT(1);
            END
            """);

        db.Database.ExecuteSqlRaw(
            """
            IF COL_LENGTH('Orders', 'PaymentStatus') IS NULL
            BEGIN
                ALTER TABLE Orders ADD PaymentStatus int NOT NULL CONSTRAINT DF_Orders_PaymentStatus DEFAULT(1);
            END
            """);
    }
}
