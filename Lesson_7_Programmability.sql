--1 Employees with Salary Above 35 000
USE SoftUni
GO
CREATE PROCEDURE usp_GetEmployeesSalaryAbove35000
AS
BEGIN
	SELECT 
	e.FirstName,
	E.LastName
	FROM Employees AS e
	WHERE e.Salary >35000
END
EXEC usp_GetEmployeesSalaryAbove35000
GO
--2 Employees with Salary Above Number
CREATE PROCEDURE usp_GetEmployeesSalaryAboveNumber @Number DECIMAL(18,4)
AS
	BEGIN
		SELECT 
			e.FirstName
			,e.LastName
			
		 FROM Employees AS e
		 WHERE e.Salary>=@Number
	END
GO
--3 Town Names Starting With
GO
CREATE PROCEDURE usp_GetTownsStartingWith @TownName VARCHAR(MAX)
AS
	BEGIN
	 SELECT t.[Name] FROM Towns AS t
	 WHERE SUBSTRING(t.[Name],1,LEN(@TownName))=@TownName
	END
GO
--EXEC usp_GetTownsStartingWith @TownName='b'

GO
--4 Employees from Town
CREATE PROCEDURE usp_GetEmployeesFromTown @TownName VARCHAR(MAX)
AS
	BEGIN
	 SELECT 
		e.FirstName
		,e.LastName
		 FROM Employees AS e
		 JOIN Addresses AS a ON a.AddressID=e.AddressID
		 JOIN Towns AS t ON t.TownID=a.TownID
		 WHERE t.[Name] =@TownName
	END
--EXEC usp_GetEmployeesFromTown @TownName='Sofia'
GO
--5 

CREATE FUNCTION ufn_GetSalaryLevel (@Salary DECIMAL(18,4))
RETURNS VARCHAR(30)
AS
	BEGIN
	DECLARE @rate VARCHAR(30)
		IF (@Salary<30000)
			BEGIN
				SET @rate='Low'
			END
		ELSE IF (@Salary<=50000)
			BEGIN
				SET @rate='Average'
			END
		ELSE
			BEGIN
				SET @rate='High'
			END

		RETURN @rate;
	END
GO
SELECT 
	dbo.ufn_GetSalaryLevel(e.Salary)
	,e.Salary
	AS SalaryLevel FROM Employees AS e
GO
--6 Employees By Salary Level
USE Softuni
GO

CREATE PROCEDURE usp_EmployeesBySalaryLevel @LevelOfSalary VARCHAR(40)
AS 
	BEGIN
		
		
	SELECT 
		t.FirstName
		,t.LastName
		FROM (
		SELECT
			e.FirstName
			,e.LastName
			,dbo.ufn_GetSalaryLevel(e.Salary) AS [salLevel]
			
			 FROM Employees AS e	
		) 
			AS t
		WHERE t.salLevel=@LevelOfSalary

	END
GO
--EXEC usp_EmployeesBySalaryLevel @LevelOfSalary='high'
--7 Define Function
GO
CREATE FUNCTION ufn_IsWordComprised(@setOfLetters VARCHAR(MAX), @word VARCHAR(MAX))
RETURNS BIT
AS
	BEGIN
	DECLARE @TempWord VARCHAR(MAX)=@word
	DECLARE @CurrentChar VARCHAR
	--DECLARE @IsTrue BIT = 0
		WHILE(LEN(@TempWord)>0)
			BEGIN
				SET @CurrentChar=SUBSTRING(@TempWord,1,1)

				IF(@CurrentChar like '['+@setOfLetters+']')
					BEGIN
					SET @Tempword= SUBSTRING(@Tempword,2,LEN(@Tempword))

					END
				ELSE IF (len(@Tempword)=0)
					RETURN 1
				ELSE
					RETURN 0
					
			END
		return 1
	END
GO
--select dbo.ufn_IsWordComprised ('ppp','Giuy' )
