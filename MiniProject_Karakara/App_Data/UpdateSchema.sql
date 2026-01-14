-- Run this script in your Server Explorer against existing KarakaraDB.mdf

-- 1. Add Stock Management to Products
ALTER TABLE Products ADD StockQuantity INT NOT NULL DEFAULT 10;
GO

-- 2. Create Orders Table
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
GO

-- 3. Create OrderItems Table
CREATE TABLE OrderItems (
    OrderItemID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO
