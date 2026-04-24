# Database Schema Notes

## Main Tables
- `Users`
- `Products`
- `Orders`
- `OrderItems`
- `Reviews`

## Product Fields
- `Name`
- `Department`
- `Category`
- `Subcategory`
- `Description`
- `PricePkr`
- `Stock`
- `ImageUrl`

## Order Fields
- Customer/contact/address fields
- `PaymentMethod`
- `PaymentReference`
- `BankTransactionId` (legacy-compatible)
- `PaymentScreenshotUrl`
- `OrderStatus`
- `PaymentStatus`
- `TotalAmountPkr`
- `CreatedAt`

## Compatibility Strategy
- Startup calls `DbSchemaUpdater.EnsureCompatibilityColumns(db)`.
- Missing columns are added automatically for older databases.
- Existing product rows are backfilled for `Department` and `Subcategory`.

