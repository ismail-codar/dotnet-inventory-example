CREATE TABLE inventory.dbo.ProductUnit (
	UnitId int IDENTITY(0,1) NOT NULL,
	UnitName varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT Units_PK PRIMARY KEY (UnitId)
);


CREATE TABLE inventory.dbo.Product (
	ProductId int IDENTITY(0,1) NOT NULL,
	ProductName varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	UnitPrice decimal(10,2) NOT NULL,
	UnitId int NOT NULL,
	UnitsInStock int NOT NULL,
	Description varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT Product_PK PRIMARY KEY (ProductId),
	CONSTRAINT Product_FK FOREIGN KEY (UnitId) REFERENCES inventory.dbo.ProductUnit(UnitId) ON DELETE CASCADE
);


CREATE TABLE inventory.dbo.StockBuilding (
	StockBuildingId int IDENTITY(0,1) NOT NULL,
	BuildingName varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT StockBuilding_PK PRIMARY KEY (StockBuildingId)
);


CREATE TABLE inventory.dbo.StockRoom (
	StockRoomId int IDENTITY(0,1) NOT NULL,
	RoomName varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	StockBuildingId int NULL,
	CONSTRAINT StockRoom_PK PRIMARY KEY (StockRoomId),
	CONSTRAINT StockRoom_FK FOREIGN KEY (StockBuildingId) REFERENCES inventory.dbo.StockBuilding(StockBuildingId) ON DELETE CASCADE
);

CREATE TABLE inventory.dbo.WorkOrder (
	WorkOrderId int IDENTITY(0,1) NOT NULL,
	SourceRoomId int NULL,
	TargetRoomId int NULL,
	[Date] date NOT NULL,
	CONSTRAINT WorkOrder_PK PRIMARY KEY (WorkOrderId),
	CONSTRAINT WorkOrder_FK_SourceRoom FOREIGN KEY (SourceRoomId) REFERENCES inventory.dbo.StockRoom(StockRoomId),
	CONSTRAINT WorkOrder_FK_TargetRoom FOREIGN KEY (TargetRoomId) REFERENCES inventory.dbo.StockRoom(StockRoomId) ON DELETE CASCADE
);

CREATE TABLE inventory.dbo.WorkOrderProduct (
	WorkOrderId int NOT NULL,
	ProductId int NOT NULL,
	Quantity int NOT NULL,
	CONSTRAINT WorkOrderProduct_PK PRIMARY KEY (WorkOrderId,ProductId),
	CONSTRAINT WorkOrderProduct_FK_Product FOREIGN KEY (ProductId) REFERENCES inventory.dbo.Product(ProductId) ON DELETE CASCADE,
	CONSTRAINT WorkOrderProduct_FK_WorkOrder FOREIGN KEY (WorkOrderId) REFERENCES inventory.dbo.WorkOrder(WorkOrderId)
);


CREATE TABLE inventory.dbo.ProductStock (
	StockRoomId int NOT NULL,
	Quantity int NOT NULL,
	ProductId int NOT NULL,
	CONSTRAINT ProductStock_PK_StockRoom_Product PRIMARY KEY (StockRoomId,ProductId),
	CONSTRAINT ProductStock_FK FOREIGN KEY (ProductId) REFERENCES inventory.dbo.Product(ProductId) ON DELETE CASCADE,
	CONSTRAINT ProductStock_FK_1 FOREIGN KEY (StockRoomId) REFERENCES inventory.dbo.StockRoom(StockRoomId) ON DELETE CASCADE
);