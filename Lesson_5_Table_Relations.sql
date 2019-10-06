USE Demo
CREATE TABLE Persons (
PersonID INT PRIMARY KEY
,FirstName VARCHAR(30) NOT NULL
,Salary DECIMAL(15,2)
,PassportID INT NOT NULL
)


CREATE TABLE Passports(
PassportID INT PRIMARY KEY 
,PassportNumber CHAR(8)NOT NULL
)
--another not null in passNum


--???? ?? ? ?????? passId (not null)
--alter a table , to have a foreign key

ALTER TABLE Persons
ADD CONSTRAINT FK_Persons_Passports
FOREIGN KEY (PassportID)
REFERENCES Passports(PassportID)

GO
--2
CREATE TABLE Manufacturers(
ManufacturerID INT PRIMARY KEY IDENTITY(1,1),
[Name] VARCHAR(30) NOT NULL,
[EstablishedOn] DATETIME
)

CREATE TABLE Models(
ModelID INT PRIMARY KEY IDENTITY(101,1),
[Name] VARCHAR(30),
ManufacturerID INT REFERENCES Manufacturers(ManufacturerID) NOT NULL
)
GO
--3
CREATE TABLE Students(
StudentID INT PRIMARY KEY IDENTITY(1,1),
[Name] VARCHAR(30) NOT NULL
)

CREATE TABLE Exams(
ExamID INT PRIMARY KEY IDENTITY(101,1),
[Name] VARCHAR(30)
)

CREATE TABLE StudentsExams(
StudentID INT  NOT NULL,
ExamID INT  NOT NULL,

CONSTRAINT PK_Students_Exams
PRIMARY KEY (StudentId,ExamId),

CONSTRAINT FK_StudentsExams_Students
FOREIGN KEY (StudentId)
REFERENCES Students(StudentID),

CONSTRAINT FK_StudentsExams_Exams
FOREIGN KEY (ExamID)
REFERENCES Exams(ExamID)
)

GO
--4
CREATE TABLE Teachers(
TeacherID INT PRIMARY KEY IDENTITY(101,1),
[Name] VARCHAR(30) ,
[ManagerID] INT FOREIGN KEY REFERENCES Teachers(TeacherID)
)
GO
--5

USE Whatever
CREATE TABLE Cities(
CityID INT PRIMARY KEY IDENTITY(1,1),
[Name] VARCHAR(50)
)

CREATE TABLE Customers(
CustomerID INT PRIMARY KEY IDENTITY(1,1),
[Name] VARCHAR(50) NOT NULL,
Birthday DATE,
CityID INT FOREIGN KEY REFERENCES Cities(CityID) NOT NULL
)

CREATE TABLE ItemTypes(
ItemTypeID INT PRIMARY KEY IDENTITY(1,1),
[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE Items(
ItemID INT PRIMARY KEY IDENTITY(1,1),
[Name] VARCHAR(50) NOT NULL,
ItemTypeID INT FOREIGN KEY REFERENCES ItemTypes(ItemTypeID) NOT NULL
)

CREATE TABLE Orders(
OrderID INT PRIMARY KEY IDENTITY(1,1),
CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID) NOT NULL
)

CREATE TABLE OrderItems(
OrderID INT NOT NULL,
ItemID INT NOT NULL,

CONSTRAINT PK_Order_Item
PRIMARY KEY (OrderID,ItemID),

CONSTRAINT FK_OrderItems_Orders
FOREIGN KEY (OrderID)
REFERENCES Orders(OrderID),

CONSTRAINT FK_oRDERiTEMS_Items
FOREIGN KEY (ItemID)
REFERENCES Items(ItemID)
)

GO
--6

CREATE TABLE Subjects(
SubjectID INT PRIMARY KEY IDENTITY(1,1),
[SubjectName] VARCHAR(30) NOT NULL
)

CREATE TABLE Majors(
MajorID INT PRIMARY KEY IDENTITY(1,1),
[Name] VARCHAR(30) NOT NULL
)

CREATE TABLE Students(
StudentID INT PRIMARY KEY IDENTITY(1,1),
StudentNumber INT NOT NULL,
StudentName VARCHAR(30),
MajorID INT FOREIGN KEY REFERENCES Majors(MajorID)
)

CREATE TABLE Payments(
PaymentID INT PRIMARY KEY IDENTITY(1,1),
PaymentDate DATETIME NOT NULL,
PaymentAmount DECIMAL(15,2) NOT NULL,
StudentID INT FOREIGN KEY REFERENCES Students(StudentID)
)

CREATE TABLE Agenda(
StudentID INT NOT NULL,
SubjectID INT NOT NULL,

CONSTRAINT PK_Student_Subject
PRIMARY KEY (StudentID,SubjectID),

CONSTRAINT FK_Agenda_Student
FOREIGN KEY (StudentID)
REFERENCES Students(StudentID),

CONSTRAINT FK_Agenda_Subject
FOREIGN KEY (SubjectID)
REFERENCES Subjects(SubjectID)
)

--7

--8

--9
GO
use [Geography]
SELECT m.MountainRange,
	   p.PeakName,
	   p.Elevation
	   FROM Peaks AS p
JOIN Mountains AS m
ON m.Id=p.MountainId
WHERE m.MountainRange IN ('Rila')
ORDER BY p.Elevation DESC