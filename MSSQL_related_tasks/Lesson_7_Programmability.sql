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

--8 Delete Employees and Departments 
CREATE PROCEDURE usp_DeleteEmployeesFromDepartment @DepartmentId INT
AS
	BEGIN
	--removing the project keys
	DELETE FROM EmployeesProjects
	WHERE EmployeeId IN
		 (
		SELECT EmployeeID FROM EmployeesProjects 
		WHERE EmployeeID IN 
			(
			SELECT e.EmployeeID FROM Employees AS e
			WHERE e.DepartmentID=@DepartmentId
			)
		)
	--removing the manager dependency
	UPDATE Employees
	SET ManagerID = NULL
	WHERE EmployeeID IN
		(
		SELECT e.EmployeeID FROM Employees AS e
		WHERE e.DepartmentID=@DepartmentId
		)
	
	--remove department null constr in dep.
	ALTER TABLE Departments
	ALTER COLUMN ManagerId INT
	
	--remove department null constr in employees
	ALTER TABLE Employees
	ALTER COLUMN DepartmentId INT

	
	--removing manager-dependency in department
	UPDATE Departments
	SET ManagerID= null
	WHERE DepartmentID=@DepartmentId

	--removing department-dependency in employees
	UPDATE Employees
	SET DepartmentID= NULL
	WHERE DepartmentID=@DepartmentId

	--deleting employees (finally)
	DELETE FROM Employees
	WHERE DepartmentID=@DepartmentId
	--deleting departments (finally)
	DELETE FROM Departments
	WHERE DepartmentID=@DepartmentId
	--returning the count as required from the exercise
	SELECT COUNT(EmployeeID) FROM Employees
	WHERE DepartmentID = @DepartmentId

	END
--9 Find Full Name
GO
USE Bank
GO
CREATE PROCEDURE usp_GetHoldersFullName 
AS
	BEGIN
		SELECT
		ah.FirstName+' '+ah.LastName AS [Full Name]
		FROM AccountHolders AS ah
		
	END
--10  Problem 10. People with Balance Higher Than
GO

CREATE PROCEDURE usp_GetHoldersWithBalanceHigherThan @Number DECIMAL(15,2)
AS
	BEGIN
	SELECT 
		ah.FirstName
		,ah.LastName
		 
		 FROM AccountHolders AS ah
		JOIN Accounts AS ac ON ac.AccountHolderId=ah.Id
		GROUP BY ah.FirstName,ah.LastName
		HAVING  SUM(ac.Balance)>=@Number
		ORDER BY ah.FirstName,ah.LastName
	END
GO
--11 Future Value Function
CREATE FUNCTION ufn_CalculateFutureValue (@Sum DECIMAL(18,2), @Interest FLOAT, @Years INT)
RETURNS DECIMAL(15,4)
	BEGIN
		DECLARE @FutureValue DECIMAL(15,4)=0;
		SET @FutureValue=@Sum* POWER((1+@Interest),@years)
		return @FutureValue
	END
	GO
	SELECT dbo.ufn_CalculateFutureValue (123.12,0.1,5)
GO
--12 Calculating Interest


CREATE PROCEDURE usp_CalculateFutureValueForAccount @AccountId INT, @Rate FLOAT
AS
	BEGIN
		SELECT 
			ah.Id AS [Account Id]
			,ah.FirstName AS [First Name]
			,ah.LastName AS [Last Name]
			,a.Balance AS [Current Balance]
			, dbo.ufn_CalculateFutureValue (a.Balance,@Rate,5) AS [Balance in 5 years]
			FROM AccountHolders ah
			JOIN Accounts as A ON a.AccountHolderId=ah.Id		
			WHERE a.Id= @AccountId
	END
GO
--exec dbo.usp_CalculateFutureValueForAccount @AccountId=1,@Rate=0.1
GO
--13 Scalar Function: Cash in User Games Odd Rows
USE Diablo
GO
CREATE FUNCTION ufn_CashInUsersGames  (@GameName VARCHAR(MAX))
RETURNS TABLE
RETURN SELECT SUM(t.Cash) AS [SumCash] FROM 
		(
		SELECT ug.Cash
			   ,DENSE_RANK() OVER (PARTITION BY g.[Name] ORDER BY ug.Cash DESC) AS [cashRank]
			   FROM UsersGames AS ug
			JOIN Games AS g ON g.Id=ug.GameId
			WHERE g.[Name]=@GameName
		) 
	AS T
	WHERE t.cashRank%2=1
GO
 SELECT * FROM dbo.ufn_CashInUsersGames ('Love in a mist')