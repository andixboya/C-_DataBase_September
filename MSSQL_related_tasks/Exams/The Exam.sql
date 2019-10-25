USE DIABLO
--1 DDL
CREATE DATABASE Service
USE Airport


CREATE TABLE Users (
Id INT PRIMARY KEY IDENTITY(1,1)
,[Username] VARCHAR(30) NOT NULL UNIQUE
,[Password] VARCHAR(50) NOT NULL
,[Name] VARCHAR(50) 
,[Birthdate] DATETIME 
,[Age] INT CONSTRAINT ck_age_restriction_ CHECK ([Age] BETWEEN 14 AND 110)
,[Email] VARCHAR(50) NOT NULL
)

CREATE TABLE Departments(
Id INT PRIMARY KEY IDENTITY(1,1)
,[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE Employees(
Id INT PRIMARY KEY IDENTITY(1,1)
,[FirstName] VARCHAR(25)
,[LastName] VARCHAR (25)
,[Birthdate] DATETIME
,[Age] INT CONSTRAINT ck_age_restriction_employees CHECK ([Age] BETWEEN 18 AND 110)
,DepartmentId INT FOREIGN KEY REFERENCES Departments(Id)
)

CREATE TABLE Categories(
Id INT PRIMARY KEY IDENTITY(1,1)
,[Name] VARCHAR(50) NOT NULL
,DepartmentId INT FOREIGN KEY REFERENCES Departments(Id)
)

CREATE TABLE [Status](
Id INT PRIMARY KEY IDENTITY(1,1)
,[Label] VARCHAR(30) not null
)

CREATE TABLE Reports(
Id INT PRIMARY KEY IDENTITY(1,1)
,CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL
,StatusId INT FOREIGN KEY REFERENCES [Status](Id) NOT NULL
,OpenDate DATETIME NOT NULL
,CloseDate DATETIME
,[Description] VARCHAR(200) NOT NULL
,UserId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL
,EmployeeId INT FOREIGN KEY REFERENCES Employees(Id)
)

--2 Insert
INSERT INTO Employees ([FirstName],[LastName],Birthdate,DepartmentId)
VALUES
('Marlo','O''Malley','1958-9-21',1)
,('Niki','Stanaghan','1969-11-26',4)
,('Ayrton','Senna','1960-03-21',9)
,('Ronnie','Peterson','1944-02-14',9)
,('Giovanna','Amati','1959-07-20',5)

INSERT INTO Reports (CategoryId,StatusId,OpenDate,CloseDate,[Description],UserId,EmployeeId)
VALUES 
(1,1,'2017-04-13',null,'Stuck Road on Str.133',6,2)
,(6,3,'2015-09-05','2015-12-06','Charity trail running',3,5)
,(14,2,'2015-09-07',NULL,'Falling bricks on Str.58',5,2)
,(4,3,'2017-07-03','2017-07-06','Cut off streetlight on Str.11',1,1)

--3 Update
	UPDATE Reports
	SET CloseDate = GETDATE()
	WHERE Id IN
			(SELECT Id FROM Reports
			WHERE CloseDate IS NULL
			)
--4 Delete
DELETE  FROM Reports
WHERE StatusId=4

--5 Unasigned Reports
SELECT r.[Description]
	   ,CONVERT(VARCHAR(30),r.OpenDate,105) AS [OpenDate]
	   
	   FROM Reports AS r
WHERE EmployeeId IS Null
ORDER BY r.OpenDate,r.[Description]

--6
SELECT r.[Description]
	  ,c.[Name] AS [CategoryName]
	    FROM Reports AS r
		LEFT JOIN Categories AS c ON c.Id=r.CategoryId
ORDER BY r.[Description],c.[Name]
GO 
--7 Most Reported Category
		SELECT TOP(5) 
				   c.[Name] AS [CategoryName]
				   ,COUNT(r.Id) AS [ReportsNumber]
				   FROM Categories AS c
				JOIN Reports AS r ON r.CategoryId=c.Id
				GROUP BY c.[Name]
				ORDER BY  COUNT(r.Id) DESC, c.[Name]



GO
--8 Birthday Report
SELECT us.Username
	   ,c.[Name]
	   FROM Reports AS r
	 RIGHT JOIN [Users] AS us ON r.[userId]=us.Id
	 JOIN Categories AS c ON c.Id=r.CategoryId
	 WHERE DATEPART(MONTH,us.Birthdate)=DATEPART(MONTH,r.OpenDate)
		  AND DATEPART(DAY,us.Birthdate)=DATEPART(DAY,r.OpenDate)
	ORDER BY us.Username,c.[Name]

--9 User Per Employee
GO	
	SELECT e.FirstName+' '+e.LastName AS [FullName]
		   ,COUNT(u.Username)  AS [UsersCount]
		   FROM Employees AS e
	LEFT JOIN Reports AS r ON r.EmployeeId=e.Id
	LEFT JOIN Users AS u ON r.UserId=u.Id
	--WHERE FirstName='Bord'
	GROUP BY e.FirstName+' '+e.LastName
	ORDER BY COUNT(u.Username) DESC,e.FirstName+' '+e.LastName 
GO
--10 Full Info
 SELECT 
	   ISNULL(e.FirstName+' '+e.LastName ,'None') AS [Employee]
	   ,ISNULL(d.[Name],'None') AS [Department]
	   ,ISNULL(c.[Name],'None') AS [Category]
	   ,ISNULL(r.[Description],'None') AS [Description]
	   ,ISNULL(CONVERT(VARCHAR(30),r.OpenDate,104),'None') AS [OpenDate] 
	   ,ISNULL(s.Label,'None') AS [Status]
	   ,ISNULL(u.[Name],'None') AS [User]

	   FROM Reports AS r
	LEFT JOIN Employees AS e ON e.Id=r.EmployeeId
	LEFT JOIN Departments AS d
	ON d.Id=e.DepartmentId
	--here a potential problem
	--LEFT JOIN Categories AS c ON c.Id=r.Id
	LEFT JOIN Categories AS c ON c.Id=r.CategoryId
	LEFT JOIN [Status] AS s ON s.Id=r.StatusId
	LEFT JOIN [Users] AS u ON u.Id=r.UserId
	WHERE e.FirstName='Corny' AND e.LastName='Pickthall'
	ORDER BY 
			e.FirstName DESC
			,e.LastName DESC
			,d.[Name]
			,c.[Name]
			,r.[Description]
			,r.OpenDate
			,s.Label
			,u.[Name]


GO
--11 Hours to Complete 
CREATE FUNCTION udf_HoursToComplete  (@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT
AS
	BEGIN
		DECLARE @Result INT

		IF (@StartDate IS NULL)
		RETURN 0
		ELSE IF(@EndDate IS NULL)
		RETURN 0
		
			
			SET @Result=DATEDIFF(hour,@StartDate,@EndDate)
			RETURN @Result
	END


GO
--12 Assign Employee
CREATE  PROCEDURE usp_AssignEmployeeToReport  @EmployeeId INT, @ReportId INT
AS
	BEGIN TRANSACTION
		DECLARE @departmentOfEmployee INT=  (SELECT  e.DepartmentId FROM Employees AS e WHERE @EmployeeId=e.Id)
		DECLARE @departmentOfReport INT = (
												SELECT d.Id FROM Reports AS r
													LEFT JOIN Categories AS c ON c.Id=r.CategoryId
													LEFT JOIN Departments AS d ON d.Id=c.DepartmentId
													WHERE r.Id=@ReportId
												)

		IF (@departmentOfEmployee!=@departmentOfReport)
			BEGIN
				ROLLBACK
					RAISERROR('Employee doesn''t belong to the appropriate department!',16,1)
					RETURN
			END

		UPDATE Reports
		SET EmployeeId=@EmployeeId
		WHERE Id=@ReportId

	COMMIT

	EXEC usp_AssignEmployeeToReport 30, 1
	EXEC usp_AssignEmployeeToReport 17, 2
