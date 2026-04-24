-- ShopifyStore complete SQL schema + reporting queries (SQL Server)

CREATE TABLE AppUsers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(120) NOT NULL,
    Username NVARCHAR(80) NOT NULL UNIQUE,
    Password NVARCHAR(200) NOT NULL,
    Role INT NOT NULL -- 1=CEO, 2=Admin, 3=Employee
);

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Description NVARCHAR(1000) NOT NULL DEFAULT '',
    PricePkr DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL,
    Category NVARCHAR(120) NOT NULL,
    ImageUrl NVARCHAR(300) NOT NULL DEFAULT ''
);

CREATE TABLE Reviews (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerName NVARCHAR(120) NOT NULL,
    Rating INT NOT NULL,
    Comment NVARCHAR(800) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ProductId INT NOT NULL,
    CONSTRAINT FK_Reviews_Products FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerName NVARCHAR(150) NOT NULL,
    Phone NVARCHAR(30) NOT NULL,
    Address NVARCHAR(500) NOT NULL,
    PaymentMethod INT NOT NULL, -- 1=COD, 2=BankTransfer
    BankTransactionId NVARCHAR(60) NOT NULL DEFAULT '',
    PaymentScreenshotUrl NVARCHAR(200) NOT NULL DEFAULT '',
    OrderStatus INT NOT NULL DEFAULT 1,  -- 1=Pending,2=PaymentUnderReview,3=Confirmed,4=Dispatched,5=Delivered,6=Cancelled
    PaymentStatus INT NOT NULL DEFAULT 1, -- 1=Unpaid,2=Paid,3=VerificationPending,4=Rejected
    TotalAmountPkr DECIMAL(18,2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPricePkr DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

GO

-- Useful reports for dashboard

-- 1) Total sales amount and total orders
SELECT
    COUNT(*) AS TotalOrders,
    ISNULL(SUM(TotalAmountPkr), 0) AS TotalSalesPkr
FROM Orders;

-- 2) Available and sold-out products
SELECT
    SUM(CASE WHEN Stock > 0 THEN 1 ELSE 0 END) AS AvailableProducts,
    SUM(CASE WHEN Stock <= 0 THEN 1 ELSE 0 END) AS SoldOutProducts
FROM Products;

-- 3) Top selling products
SELECT TOP 10
    p.Id,
    p.Name,
    SUM(oi.Quantity) AS SoldQty,
    SUM(oi.Quantity * oi.UnitPricePkr) AS RevenuePkr
FROM OrderItems oi
INNER JOIN Products p ON p.Id = oi.ProductId
GROUP BY p.Id, p.Name
ORDER BY SoldQty DESC;

-- 4) Daily sales trend
SELECT
    CAST(CreatedAt AS DATE) AS SaleDate,
    COUNT(*) AS OrdersCount,
    SUM(TotalAmountPkr) AS SalesPkr
FROM Orders
GROUP BY CAST(CreatedAt AS DATE)
ORDER BY SaleDate DESC;

-- 5) Bank transfer pending verification
SELECT
    Id, CustomerName, Phone, TotalAmountPkr, BankTransactionId, PaymentStatus, OrderStatus, CreatedAt
FROM Orders
WHERE PaymentMethod = 2
  AND PaymentStatus = 3
ORDER BY CreatedAt DESC;

