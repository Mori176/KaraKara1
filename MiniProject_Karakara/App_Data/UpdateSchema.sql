-- Run this script in your Server Explorer against existing KarakaraDB.mdf

-- 1. Add Stock Management to Products
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'StockQuantity')
BEGIN
    ALTER TABLE Products ADD StockQuantity INT NOT NULL DEFAULT 10;
END
GO

-- 2. Create Orders Table if not exists, else add Status column
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
BEGIN
    CREATE TABLE Orders (
        OrderID INT PRIMARY KEY IDENTITY(1,1),
        OrderNumber NVARCHAR(50) NOT NULL UNIQUE,
        UserID INT NOT NULL,
        OrderDate DATETIME NOT NULL,
        TotalAmount DECIMAL(18,2) NOT NULL,
        Tax DECIMAL(18,2) NOT NULL,
        AmountToPay DECIMAL(18,2) NOT NULL,
        Status NVARCHAR(20) DEFAULT 'Confirmed',
        FOREIGN KEY (UserID) REFERENCES Users(Id)
    );
END
ELSE
BEGIN
    -- If table exists, check for Status column
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Orders]') AND name = 'Status')
    BEGIN
        ALTER TABLE Orders ADD Status NVARCHAR(20) DEFAULT 'Confirmed';
    END
END
GO

-- 3. Create OrderItems Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderItems')
BEGIN
    CREATE TABLE OrderItems (
        OrderItemID INT PRIMARY KEY IDENTITY(1,1),
        OrderID INT NOT NULL,
        ProductID INT NOT NULL,
        Quantity INT NOT NULL,
        Price DECIMAL(18,2) NOT NULL,
        FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
        FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
    );
END
GO

-- 4. Update GetUserOrderHistory Stored Procedure to include Status
-- We use CREATE OR ALTER if supported, or DROP/CREATE. Assuming SQL Server 2016+
CREATE OR ALTER PROCEDURE GetUserOrderHistory
    @UserID INT
AS
BEGIN
    SELECT 
        O.OrderNumber, 
        O.OrderID, 
        O.OrderDate, 
        O.TotalAmount, 
        O.Tax, 
        O.AmountToPay, 
        O.Status,
        P.ProductName, 
        OI.Quantity, 
        OI.Price,
        (OI.Quantity * OI.Price) AS Subtotal
    FROM Orders O
    JOIN OrderItems OI ON O.OrderID = OI.OrderID
    JOIN Products P ON OI.ProductID = P.ProductID
    WHERE O.UserID = @UserID
    ORDER BY O.OrderDate DESC
END
GO
