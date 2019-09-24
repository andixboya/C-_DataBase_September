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
--13
SELECT TOP(5) e.FirstName,e.LastName FROM Employees AS e
order by e.Salary DESC
GO
--14
SELECT e.FirstName,e.LastName 
FROM Employees as e
WHERE e.DepartmentId !=4 -- or <>
GO
--15
SELECT * FROM Employees AS e
ORDER BY e.Salary DESC, 
		 e.FirstName,
		 e.LastName DESC,
		 e.MiddleName
GO
--16
CREATE VIEW V_EmployeeSalaries AS
SELECT e.FirstName,e.LastName,e.Salary FROM Employees AS e
GO
SELECT *FROM V_EmployeeSalaries
GO
--17 this... was an awkward moment
CREATE VIEW V_EmployeeNameJobTitle AS
SELECT e.FirstName+' '+ISNULL(e.Middlename,'')+' '+e.LastName AS [Full Name],
e.JobTitle
FROM Employees AS e
SELECT * FROM V_EmployeeNameJobTitle
GO
--18
SELECT DISTINCT e.JobTitle FROM Employees as e
GO
--19
SELECT TOP(10) * FROM Projects AS p
ORDER BY p.StartDate,p.[Name]
GO
--20
SELECT TOP(7) e.FirstName,e.LastName,e.HireDate from Employees AS e
ORDER BY e.HireDate DESC
GO
--21
GO
UPDATE Employees 
SET Salary*=1.12
WHERE DepartmentID in (SELECT DISTINCT e.DepartmentID FROM Employees AS E
INNER JOIN Departments AS d ON d.DepartmentID=e.DepartmentID 
WHERE  d.[Name] IN ('Engineering','Tool Design','Marketing','Information Services'))
SELECT e.Salary FROM Employees as E

GO
SELECT DISTINCT e.DepartmentID FROM Employees AS E
INNER JOIN Departments AS d ON d.DepartmentID=e.DepartmentID 
WHERE  d.[Name] IN ('Engineering','Tool Design','Marketing','Information Services')
GO
--22
use Geography
SELECT p.PeakName FROM Peaks AS p
ORDER BY p.PeakName
GO
--23
SELECT TOP(30) c.CountryName,c.Population FROM Countries as C
INNER JOIN Continents as Co ON c.ContinentCode=co.ContinentCode
WHERE co.ContinentName='Europe'
ORDER BY c.[Population] DESC,C.CountryName
GO
--24 SKIP for now...
SELECT c.CountryName,
c.CountryCode,
CASE WHEN c.CurrencyCode = 'EUR' THEN 'Euro'
	  ELSE 'Not Euro' 
	  END  AS Currency
 From Countries AS c
 ORDER BY c.CountryName

GO
--25
use Diablo
select c.Name from Characters AS c
ORDER BY c.Name
