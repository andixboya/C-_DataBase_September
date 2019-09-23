USE SOftuni
--2
SELECT * FROM Departments
GO
--3
SELECT [Name] FROM Departments
GO
--4
SELECT [FirstName],[Lastname],[Salary]
  FROM Employees
GO
--5
SELECT [FirstName],[MiddleName],[LastName] FROM Employees
GO
--6
USE Softuni

SELECT FirstName+'.'+LastName+'@softuni.bg' 
	AS [Full Email Address]
  FROM Employees
GO

--7
SELECT 
	DISTINCT Salary 
	FROM Employees

GO
--8
SELECT * 
	FROM Employees AS e
	WHERE e.JobTitle ='Sales representative'

GO
--9
	SELECT FirstName,LastName,JobTitle 
	FROM Employees 
	as e
	where e.Salary BETWEEN 20000 AND 30000
GO
--10
SELECT e.FirstName+' '+ISNULL(e.MiddleName+' ','')+e.LastName  AS [Full Name]
FROM Employees AS e
WHERE e.Salary IN (25000,14000,12500,23600)
GO
--11
GO
SELECT e.FirstName,e.LastName FROM Employees as e
	WHERE e.ManagerId IS NULL

GO
--12
SELECT e.FirstName,e.LastName,e.Salary 
FROM Employees AS e
WHERE e.Salary>=50000
ORDER BY e.Salary DESC
GO