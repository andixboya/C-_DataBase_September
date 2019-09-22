
--1
CREATE DATABASE Minions
GO
use Minions
--2 
CREATE TABLE Minions (
Id NVARCHAR,
[Name] NVARCHAR,
Age INT
)

CREATE TABLE Towns (
Id int PRIMARY KEY,
[Name] NVARCHAR
)
--OR ... alter table Towns
-- ADD CONSTRAINT Pk_Id_Towns PRIMARY KEY (Id)
go
USE Minions

ALTER TABLE Minions
--ALTER COLUMN  Id nvarchar NOT NULL
ADD CONSTRAINT Pk_Id_Minions PRIMARY KEY (Id)

go
--3 how to insert FOREIGN KEY
ALTER TABLE Minions
ADD TownId INT FOREIGN KEY REFERENCES Towns(Id)
--4
--How to create primary key! 
CREATE TABLE Minions(
Id INT PRIMARY KEY,
[Name] NVARCHAR(30),
[Age] INT,
[TownId] INT FOREIGN KEY REFERENCES Towns(Id)
)
	INSERT INTO Towns (Id, [Name]) VALUES 
	(1,'Sofia'),
	(2,'Plovdiv'),
	(3,'Varna')
	INSERT INTO Minions (Id,[Name],Age,TownID) VALUES
	(1,'Kevin',22,1),
	(2,'Bob',15,3),
	(3,'Steward',NULL,2)
--5
TRUNCATE TABLE	Minions
--6
DROP TABLE Minions
DROP TABLE Towns

--7
CREATE TABLE People(
Id BIGINT PRIMARY KEY IDENTITY(1,1),
[Name] NVARCHAR(200) NOT NULL,
[Picture] VARBINARY(MAX),
[Height] DECIMAL (5,2),
[Weight] DECIMAL(5,2),
[Gender] BIT NOT NULL,
[Birthdate] DATETIME NOT NULL,
Biography TEXT 
)
INSERT INTO People VALUES
('zhoro',null,15,20,0,'2013-10-10','haha'),
('zhoro',null,15,20,0,'2013-10-10','haha'),
('zhoro',null,15,20,0,'2013-10-10','haha'),
('zhoro',null,15,20,0,'2013-10-10','haha'),
('zhoro',null,15,20,0,'2013-10-10','haha')
GO
--8 how to insert datetime (unformatted) + picuture files
CREATE TABLE Users(
Id BIGINT PRIMARY KEY IDENTITY(1,1),
Username NVARCHAR(30) UNIQUE NOT NULL,
[Password] NVARCHAR(26) NOT NULL,
[ProfilePicture] VARBINARY(MAX),
LastLoginTime DATETIME,
IsDeleted BIT NOT NULL
)
INSERT INTO Users(Username, [Password], [ProfilePicture],[LastLoginTime],[IsDeleted]) VALUES
('zhoro','hahaha',null,'2013-10-09',0),
('dude','hahaha',null,'2013-10-09',0),
('random','hahaha',null,'2013-10-09',0),
('guy','hahaha',null,'2013-10-09',0),
('yolo','hahaha',null,'2013-10-09',0)
GO
--9 MAKE COMPOSITE KEY
ALTER TABLE Users
DROP CONSTRAINT PK__Users__3214EC072D24A467

ALTER TABLE Users
ADD CONSTRAINT pk_CompositeKey PRIMARY KEY (Id, Username)

--10 how to add check constraint

ALTER TABLE Users
ADD CONSTRAINT chk_Password_Length CHECK  (LEN([Password])>5)

GO
--11 HOW to add default value
ALTER TABLE Users
ADD CONSTRAINT default_Value_Of_LastLogin 
DEFAULT GETDATE() FOR LastLoginTime

GO
--12
--1st -> remove COMPOSITE KEY
ALTER TABLE Users
DROP CONSTRAINT pk_CompositeKey

--2ND -> restore single primary
ALTER TABLE Users
ADD CONSTRAINT PK_Id_Users PRIMARY KEY (Id)

--3RD ->  make User Nullable (otherwise it conflicts with min leng>3)
ALTER TABLE Users
ALTER COLUMN Username NVARCHAR(30) NULL

--4th -> FINALLY , add the constraint
ALTER TABLE Users
ADD CONSTRAINT CHK_MinLength_Username CHECK (LEN(Username)>=3)
--ALTER TABLE Users
--DROP CONSTRAINT CHK_MinLength_Username
--CHECK for the top constraint
INSERT INTO Users (Username,[Password],IsDeleted) VALUES
('ge','hahahaha',1)
GO
--13
USE Minions

CREATE DATABASE Movies

CREATE TABLE Directors(
Id INT PRIMARY KEY IDENTITY(1,1),
DirectorName NVARCHAR(30) NOT NULL,
Notes TEXT
)
CREATE TABLE Genres(
Id INT PRIMARY KEY IDENTITY(1,1),
GenreName NVARCHAR(30) UNIQUE NOT NULL,
Notes TEXT
)
CREATE TABLE Categories(
Id INT PRIMARY KEY IDENTITY(1,1),
CategoryName NVARCHAR(30) UNIQUE NOT NULL,
Notes TEXT
)
CREATE TABLE Movies (
Id INT PRIMARY KEY IDENTITY(1,1),
Title NVARCHAR(30) NOT NULL,
DirectorId INT FOREIGN KEY REFERENCES Directors(Id),
CopyrightYear INT NOT NULL,
[Length] INT NOT NULL,
GenreId INT FOREIGN KEY REFERENCES Genres(Id),
CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
Rating INT ,
Notes TEXT
)

ALTER TABLE Movies
ADD CONSTRAINT CHK_Length_Above_0 CHECK ([Length]>0)

INSERT INTO Directors(DirectorName) VALUES
('haha'),
('haha1'),
('haha2'),
('haha3'),
('haha4')

INSERT INTO Genres(GenreName) VALUES
('genre1'),
('genre2'),
('genre3'),
('genre4'),
('genre5')

INSERT INTO Categories(CategoryName) VALUES	
('Category1'),
('Category2'),
('Category3'),
('Category4'),
('Category5')

INSERT INTO Movies (Title,DirectorId,CopyrightYear,[Length],GenreId,CategoryId) VALUES
('the 1',1,2011,65,1,1),
('the 2',2,2012,64,2,2),
('the 3',3,2013,63,3,3),
('the 4',4,2014,62,4,4),
('the 5',5,2015,61,5,5)
GO
--14
CREATE DATABASE CarRental
USE Carrental

CREATE TABLE Categories(
Id INT PRIMARY KEY IDENTITY(1,1),
CategoryName NVARCHAR(30) NOT NULL,
DailyRate DECIMAL(15,2) NOT NULL,
WeeklyRate DECIMAL(15,2) NOT NULL,
MonthlyRate DECIMAL(15,2)NOT NULL,
WeekendRate DECIMAL(15,2) not null
)
 
 CREATE TABLE  Cars(
 Id INT PRIMARY KEY IDENTITY(1,1),
 PlateNumber NVARCHAR(15) NOT NULL,
 Manufacturer NVARCHAR(50) ,
 Model NVARCHAR(50) NOT NULL,
 CarYear INT ,
 CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
 Doors INT ,
 Picture VARBINARY(MAX),
 Condition NVARCHAR(50),
 Available BIT NOT NULL
 )

 CREATE TABLE Employees(
 Id INT PRIMARY KEY IDENTITY(1,1),
 FirstName NVARCHAR(40) NOT NULL,
 LastName NVARCHAR(40) NOT NULL,
 Title NVARCHAR(40),
 Notes TEXT)

 CREATE TABLE Customers(
 Id INT PRIMARY KEY IDENTITY(1,1),
 DriverLicenceNumber NVARCHAR(15) NOT NULL,
 FullName NVARCHAR(30),
 [Address] NVARCHAR(30),
 ZIPCode NVARCHAR(15),
 Notes TEXT
 )

 CREATE TABLE RentalOrders(
 Id INT PRIMARY KEY IDENTITY(1,1),
 EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
 CustomerId INT FOREIGN KEY REFERENCES Customers(Id),
 CarId INT FOREIGN KEY REFERENCES Cars(id),
 TankLevel INT,
 KilometrageStart INT NOT  NULL,
 KilometrageEnd INT NOT Null,
 TotalKilometrage INT ,
 StartDate DATETIME,
 EndDate DATETIME,
 RateApplied DECIMAL(15,2),
 TaxRate DECIMAL(15,2),
 OrderStatus NVARCHAR(30),
 Notes TEXT
 )
 INSERT INTO Categories(CategoryName,DailyRate,WeeklyRate,MonthlyRate,WeekendRate) VALUES
 ('category1',1,2,3,4),
 ('category2',1,2,3,4),
 ('category3',1,2,3,4)

 INSERT INTO Cars(PlateNumber,Model,Available) VALUES
 ('plateNUm1','model1',1),
 ('plateNUm2','model2',9),
 ('plateNUm3','model3',1)

 INSERT INTO Employees(FirstName,LastName) VALUES
 ('gosho','peshev'),
 ('gosho2','peshev2'),
 ('gosho3','peshev3')

 INSERT INTO Customers(DriverLicenceNumber) VALUES
 ('o4ijriarr'),
 ('o4ijriarr1'),
 ('o4ijriar23')

 INSERT INTO RentalOrders(EmployeeId,CustomerId,CarId,KilometrageStart,KilometrageEnd) VALUES
 (1,1,1,0,150),
 (2,2,2,50,1530),
 (3,3,3,150,1535)
 GO
 --15 skip, for  now.
 GO
 --16
 CREATE DATABASE Softuni
 USE SOftuni


 CREATE TABLE Towns(
 Id INT PRIMARY KEY  IDENTITY(1,1),
 [Name] NVARCHAR(30) NOT NULL,
 )

 CREATE TABLE Addresses(
 Id int PRIMARY KEY  IDENTITY(1,1),
 [AddressText] TEXT,
 TownId INT FOREIGN KEY REFERENCES Towns(id)
 )

 CREATE TABLE Departments(
 Id INT PRIMARY KEY IDENTITY(1,1),
 [Name] NVARCHAR(30) NOT NULL
 )
 
 CREATE TABLE Employees(
 Id INT PRIMARY KEY IDENTITY(1,1),
 FirstName NVARCHAR(40) NOT NULL,
 MiddleName NVARCHAR(40),
 LastName NVARCHAR(40) NOT NULL,
 JobTitle NVARCHAR(40),
 DepartmentId INT FOREIGN KEY REFERENCES Departments(Id),
 HireDate DATETIME,
 Salary DECIMAL(15,2),
 AddressId INT FOREIGN KEY REFERENCES Addresses(ID)
 )

 --17 skip for now

 --18 skip for now

 --19 
SELECT * FROM Towns
SELECT * FROM Departments
SELECT * FROM Employees

--20
SELECT * FROM Towns AS t
ORDER BY t.[Name]

SELECT * FROM Departments AS d
ORDER BY d.[Name]

SELECT * FROM Employees AS e
ORDER BY e.Salary DESC
GO
--21
SELECT [Name] FROM Towns as t
ORDER BY t.[Name]
SELECT [Name] FROM Departments as d
ORDER BY d.[Name]
SELECT [FirstName],[LastName],JobTitle,Salary FROM Employees as e
ORDER BY e.Salary DESC 

GO
--22 Increase Salary
UPDATE Employees
SET Salary=Salary+0.1*Salary
SELECT Salary FROM Employees
SELECT * FROM Employees
--23 Decrease Tax Rate
UPDATE Payments
SET Taxrate = Taxrate-0.03*Taxrate

SELECT Taxrate FROM Payments

--24
TRUNCATE TABLE Occupancies