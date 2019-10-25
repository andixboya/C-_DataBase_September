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
--14
USE Diablo
SELECT TOP (50) [Name],
				--CONVERT(DATE,[Start],102) AS [Start] 
				FORMAT([Start],'yyyy-MM-dd','en-US') AS [Start]
	   FROM Games
	   WHERE DATEPART(year,[Start]) IN (2011,2012) 
	   ORDER BY [Start],[Name]
GO
--15
GO
SELECT * FROM (SELECT 
	u.Username,
    SUBSTRING(u.Email ,charindex('@',u.Email ,1)+1,len(u.email))AS [Email Provider] FROM Users AS u)AS f
ORDER BY f.[Email Provider],f.Username
GO
--16
SELECT u.Username,u.IpAddress FROM Users AS u
WHERE u.IpAddress LIKE '___.1%.%.___%'
ORDER BY u.Username
GO
--17
SELECT * FROM (SELECT g.[Name] AS [Game],
	CASE
	WHEN DATEPART(hour,g.Start) BETWEEN 0 AND 11 THEN 'Morning'
	WHEN DATEPART(hour,g.Start) BETWEEN 12 AND 17 THEN 'Afternoon'
	WHEN DATEPART(hour,g.Start) BETWEEN 18 AND 23 THEN 'Evening'
	--lol, at 23, not 24, silly!
	END AS [Part of the Day],
	CASE
	WHEN g.duration BETWEEN 0 AND 3 THEN 'Extra Short'
	WHEN g.duration BETWEEN 4 AND 6 THEN 'Short'
	WHEN g.duration >=6 THEN 'Long'
	WHEN g.Duration IS NULL THEN 'Extra Long'
	END AS [Duration]
	FROM Games AS g
	) AS temp
	ORDER BY temp.Game,
			 temp.Duration,
			 temp.[Part of the Day]
GO

--18
--SELECT * FROM Games
USE SoftUni
CREATE TABLE Orders (
ID INT PRIMARY KEY IDENTITY(1,1),
ProductName NVARCHAR(50),
OrderDate DATETIME
)
INSERT INTO Orders  VALUES
('Butter','2016-09-19 00:00:00.000'),
('Milk','2016-09-30 00:00:00.000'),
('Cheese','2016-09-04 00:00:00.000')

SELECT O.ProductName,o.OrderDate,
DATEADD(DAY,3,o.OrderDate) AS [Pay Due],
DATEADD(MONTH,1,o.OrderDate) AS [Deliver Due]
 FROM Orders AS o