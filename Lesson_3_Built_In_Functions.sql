--1
USE Softuni
SELECT e.FirstName,e.LastName FROM Employees as e
WHERE e.FirstName LIKE 'SA%'
--2
GO
SELECT e.FirstName,e.LastName FROM Employees as e
WHERE e.LastName LIKE '%ei%'
GO
--3
SELECT e.FirstName FROM Employees AS e
WHERE DATEPART(year,e.HireDate) BETWEEN 1995 AND 2005
AND e.DepartmentID IN (3,10)
GO
--4
SELECT e.FirstName,e.LastName FROM Employees AS e
WHERE e.JobTitle NOT LIKE  '%engineer%'
GO
--5
SELECT t.[Name] FROM Towns AS t
WHERE LEN(t.[Name]) IN (5,6)
ORDER BY t.[Name]
GO
--6
SELECT t.TownID,t.[Name] FROM Towns AS t
WHERE t.[Name] LIKE '[MKBE]%'
ORDER BY t.[Name]
GO
--7
SELECT t.TownID,t.[Name] FROM Towns AS t
--WHERE t.[Name] NOT LIKE '[RBD]%'
WHERE t.[Name] LIKE '[^RBD]%'
ORDER BY t.[Name]
GO
--8
CREATE VIEW V_EmployeesHiredAfter2000 AS
SELECT e.FirstName,e.LastName FROM Employees AS e
WHERE DATEPART(year,e.HireDate) >2000
GO
--9
SELECT e.FirstName,e.LastName FROM Employees AS e
WHERE LEN(e.LastName) =5
GO
--10
SELECT E.EmployeeID,e.FirstName,e.LastName,e.Salary,
DENSE_RANK() OVER (PARTITION BY e.Salary ORDER BY e.employeeId) as [Rank]
FROM Employees AS e
WHERE e.Salary BETWEEN 10000 AND 50000
ORDER BY  E.Salary DESC
GO
--11
SELECT * FROM (SELECT E.EmployeeID,e.FirstName,e.LastName,e.Salary,
DENSE_RANK() OVER (PARTITION BY e.Salary ORDER BY e.employeeId) as [Rank]
FROM Employees AS e
WHERE e.Salary BETWEEN 10000 AND 50000) AS x
WHERE x.[Rank] =2
ORDER BY x.Salary DESC
--12
USE Geography
SELECT c.CountryName,c.IsoCode FROM Countries AS c
WHERE c.CountryName LIKE '%a%a%a%'
ORDER BY c.IsoCode
GO
--13
SELECT *FROM (SELECT P.PeakName,
	   r.RiverName,
	   LOWER(p.PeakName)+SUBSTRING(LOWER(R.RiverName),2,len(R.RiverName)) AS [Mix] 
FROM Peaks AS p ,
	 Rivers AS r
	 WHERE RIGHT(p.PeakName,1)=LEFT(r.RiverName,1)) AS X
	 ORDER BY X.Mix
GO