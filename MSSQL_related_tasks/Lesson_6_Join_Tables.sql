use SoftUni

--1 Employee Address
SELECT TOP(5) e.EmployeeID,
  e.JobTitle,
  a.AddressID,
  a.AddressText
 FROM Employees AS e
	 JOIN Addresses AS a
	ON a.AddressID=e.AddressID
 ORDER BY a.AddressID 
 GO
 
 --2 Addresses with Towns
 SELECT TOP(50) e.FirstName,
   e.LastName,
   t.[Name],
   a.AddressText FROM Employees AS e
 JOIN Addresses AS a
 ON a.AddressID=e.AddressID 
 JOIN Towns AS t
 ON t.TownID= a.TownId
 ORDER BY e.FirstName, e.LastName
 GO
 
 --3 Sales Employee
 SELECT e.EmployeeID,
   e.FirstName,
   e.LastName,
   d.[Name] FROM Employees AS e
 JOIN Departments AS d
 ON d.DepartmentID=e.DepartmentID
 WHERE d.[Name]='Sales'
 ORDER BY e.EmployeeID

 --4 Employee Departments
 GO
 SELECT TOP(5) e.EmployeeID,
   e.FirstName,
   e.Salary,
   d.[Name] FROM Employees AS e
  JOIN Departments AS d
 ON d.DepartmentID=e.DepartmentID
 WHERE e.Salary >= 15000
 ORDER BY d.DepartmentID
 GO
 
 --5 Employee Without Project
 SELECT  TOP(3) e.EmployeeID,
   e.FirstName FROM Employees AS e
	LEFT JOIN EmployeesProjects AS ep
	ON ep.EmployeeID=e.EmployeeID
	LEFT JOIN Projects AS p
	ON ep.ProjectID=p.ProjectID
	WHERE ep.ProjectID IS   NULL
	ORDER BY e.EmployeeID 
GO

--6 Employees Hired After
SELECT e.FirstName,
  e.LastName,
  e.HireDate,
  d.[Name] FROM Employees AS e
JOIN Departments AS d
ON d.DepartmentId=e.DepartmentID
WHERE e.HireDate>'1.1.1999'
AND d.[Name] IN ('Sales','Finance')
ORDER BY e.HireDate 
GO

--7 Employees with Project
SELECT TOP(5) 
	   e.EmployeeID,
	   e.FirstName,
	   p.[Name]
	   FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep
ON ep.EmployeeID=e.EmployeeID
LEFT JOIN Projects AS p
ON p.ProjectID=ep.ProjectID
WHERE p.ProjectID IS NOT NULL 
	  AND p.StartDate >'08-13-2002'
	  AND p.EndDate IS NULL
ORDER BY e.EmployeeID
SELECT * FROM Projects
GO

--8 Employee 24
SELECT e.EmployeeID,
	   e.FirstName,
	   IIF (YEAR(p.StartDate)>=2005,null,p.[Name]) AS ProjectName
	   FROM Employees AS e
JOIN EmployeesProjects AS ep
ON ep.EmployeeID = e.EmployeeID
JOIN Projects AS p 
ON p.ProjectID= ep.ProjectID
WHERE e.EmployeeID IN (24)
GO

--9 Employee Manager
SELECT	e.EmployeeID,
		e.FirstName,
	    e.ManagerID,
		em.FirstName AS [ManagerName]
		 FROM Employees AS e
	LEFT JOIN Employees AS em
	ON e.ManagerID=em.EmployeeID
	WHERE e.ManagerID in (3,7)
	ORDER BY  e.EmployeeID

--10 Employee Summary
SELECT TOP(50)
	   e.EmployeeID,
	   e.FirstName+' ' +e.LastName AS [EmployeeName],
	   m.FirstName+' '+m.LastName AS [ManagerName],
	   d.[Name] AS DepartmentName
	   FROM Employees AS e
LEFT JOIN Employees AS m
ON m.EmployeeID=e.ManagerID	
LEFT JOIN Departments AS d
ON d.DepartmentID=E.DepartmentID
ORDER BY e.EmployeeID

--11 Min Average Salary
SELECT TOP(1) AVG(e.Salary) AS [MinAverageSalary] FROM Employees AS e
GROUP BY e.DepartmentID
ORDER BY AVG(e.Salary) 
GO

USE Geography
--12 Highest Peaks in Bulgaria
USE Geography
SELECT c.CountryCode,
	   m.MountainRange,
	   p.PeakName,
	   p.Elevation
	   FROM Countries AS c
JOIN MountainsCountries AS mc ON mc.CountryCode=c.CountryCode
JOIN Mountains AS m ON m.Id=mc.MountainId
JOIN Peaks AS p ON p.MountainId=m.Id
WHERE p.Elevation>2835 
AND c.CountryName='Bulgaria'
ORDER BY p.Elevation DESC
GO

--13 Count Mountain Ranges
SELECT c.CountryCode,
	   COUNT(m.MountainRange) AS MountainRanges
	   FROM Countries AS c
JOIN MountainsCountries AS mc ON mc.CountryCode=c.CountryCode
JOIN Mountains AS m ON m.Id=mc.MountainId
WHERE c.CountryName IN ('United States','Russia','Bulgaria')
GROUP BY c.CountryCode

GO
--14 Countries with Rivers
SELECT TOP(5) 
	   c.CountryName
	   ,r.RiverName
	   FROM Countries AS c
LEFT JOIN CountriesRivers AS cr ON cr.CountryCode=c.CountryCode
LEFT JOIN Rivers AS r ON r.Id= cr.RiverId
JOIN Continents AS co ON co.ContinentCode=c.ContinentCode
WHERE co.ContinentName='Africa'
ORDER BY c.CountryName 
GO

--15 Continents and Currencies
SELECT st.ContinentCode
	   ,st.CurrencyCode
	   ,st.CountOfCountries FROM

(SELECT t.ContinentCode
	   ,t.CountOfCountries
	   ,t.CurrencyCode
	   ,DENSE_RANK() OVER(PARTITION BY t.ContinentCode ORDER BY t.[CountOfCountries] DESC) AS [rank]
	    FROM 
	(SELECT COUNT(c.CountryCode) [CountOfCountries]
		   ,c.CurrencyCode
		   ,c.ContinentCode
		   --,
		   FROM Countries AS c
		   JOIN Currencies as cu ON c.CurrencyCode=cu.CurrencyCode
		   JOIN Continents AS co ON co.ContinentCode=c.ContinentCode
		   GROUP BY c.ContinentCode, c.CurrencyCode
		   HAVING COUNT(c.CountryCode)>1
		   ) AS t
	) AS ST
	WHERE st.[rank]=1  
ORDER BY st.ContinentCode
	   
SELECT COUNT(c.CountryCode) AS countryCount
	   ,c.CurrencyCode
	   FROM Countries AS c
	   LEFT JOIN Continents AS co ON co.ContinentCode=c.ContinentCode
	   GROUP BY c.CurrencyCode
	   ORDER BY COUNT(c.CountryCode) DESC

SELECT 
	co.ContinentCode
	,COUNT(c.CountryName) AS [Count Of Countries]
	--,c.CountryName
	 FROM Continents as CO
	JOIN Countries AS c ON c.ContinentCode=co.ContinentCode
	--WHERE co.ContinentCode = 'EU'
	GROUP BY co.ContinentCode

SELECT c.CountryName
	   ,COUNT(c.CurrencyCode) AS [Currency Within A Country]
	   FROM Countries AS c
	JOIN Currencies AS cu ON cu.CurrencyCode=c.CurrencyCode
	GROUP BY c.CountryName
	

GO
--16 Countries without any Mountains

SELECT COUNT(c.CountryCode) AS [Count] FROM Countries AS c
LEFT JOIN MountainsCountries as mc ON mc.CountryCode=c.CountryCode
LEFT JOIN Mountains AS m ON m.Id =mc.MountainId
WHERE m.Id IS NULL

--17 Highest Peak and Longest River by Country
--	  ,DENSE_RANK() OVER (PARTITION BY p.CountryName ORDER BY p.Elevation) AS [RankElevation)	
SELECT TOP(5) st.CountryName
	   ,st.Elevation AS [HighestPeakEleveation]
	   ,st.[Length] AS LongestRiverLength FROM	(
	SELECT 
		   t.CountryName,
		   t.Elevation,
		   t.Length,
		   DENSE_RANK() OVER (PARTITION BY t.CountryName ORDER BY t.Elevation DESC) AS [RankElevation],
		   DENSE_RANK () OVER (PARTITION BY t.CountryName ORDER BY t.[Length] DESC) AS [RankLength]
		   FROM
		(
		SELECT  c.CountryName,
				p.Elevation,
			
				r.[Length]
			FROM Countries AS c
		LEFT JOIN MountainsCountries AS mc ON mc.CountryCode=c.CountryCode
		LEFT JOIN Mountains AS m ON m.Id=mc.MountainId
		LEFT JOIN Peaks AS p ON p.MountainId=m.Id
		LEFT JOIN CountriesRivers AS cr ON cr.CountryCode=c.CountryCode
		LEFT JOIN Rivers AS r ON r.Id=cr.RiverId
		) AS T
	)AS ST
	WHERE st.RankElevation=1 AND st.RankLength=1
	ORDER BY st.Elevation DESC,st.[Length] DESC, st.CountryName




--SELECT * FROM (SELECT  c.CountryName,
--		r.[length],
--		DENSE_RANK () OVER (PARTITION BY c.CountryName ORDER BY r.[Length] DESC) AS [Rank]
--	FROM Countries AS c
--JOIN CountriesRivers AS cr ON cr.CountryCode=c.CountryCode
--JOIN Rivers AS r ON r.Id=cr.RiverId
--) AS t
--WHERE t.[Rank]=1
--GO
--	SELECT * FROM (
--	SELECT 
--	c.CountryName
--	,p.Elevation
--	,DENSE_RANK() OVER (PARTITION BY c.CountryName ORDER BY p.Elevation) AS [RankElevation]
--	 FROM Countries AS c
--		LEFT JOIN MountainsCountries AS mc ON mc.CountryCode=c.CountryCode
--		LEFT JOIN Mountains AS m ON m.Id=mc.MountainId
--		LEFT JOIN Peaks AS p ON p.MountainId=m.Id
--		) AS T 
--		where 

GO 
--18 Highest Peak Name and Elevation by Country

	SELECT TOP(5)
		 t.CountryName
		 ,ISNULL(t.PeakName,'(no highest peak)') AS [Highest Peak Name]
		 ,ISNULL(t.Elevation,0) AS [Highest Peak Elevation]
		 ,ISNULL(t.MountainRange,'(no mountain)') AS [Mountain]
		 FROM 		
		(SELECT  c.CountryName
			   ,p.PeakName
			   ,p.Elevation
			   ,m.MountainRange
			   ,DENSE_RANK() OVER(PARTITION BY c.CountryName ORDER BY p.Elevation DESC) AS [elevationRank]
			   FROM Countries AS c
			LEFT JOIN MountainsCountries AS mc ON mc.CountryCode=c.CountryCode
			LEFT JOIN Mountains AS m ON m.Id=mc.MountainId
			LEFT JOIN Peaks AS p ON p.MountainId=m.Id
			) AS t
	WHERE t.elevationRank=1
	ORDER BY t.CountryName,t.PeakName
   
   
   
   SELECT TOP(5)
  	      tt.CountryName AS Country, 
  	      IIF(tt.PeakName IS NULL, '(no highest peak)', tt.PeakName) AS [Highest Peak Name],
  	      IIF(tt.Elevation IS NULL, 0, tt.Elevation) AS [Highest Peak Elevation],
  	      IIF(tt.MountainRange IS NULL, '(no mountain)', tt.MountainRange) AS [Mountain]
    FROM
  	     (SELECT c.CountryName, p.PeakName, p.Elevation, m.MountainRange,
  	             DENSE_RANK() OVER (PARTITION BY c.CountryNAme ORDER BY p.Elevation DESC) AS [Rank]
  	       FROM Mountains AS m
  	       JOIN Peaks AS p ON p.MountainId = m.Id
  	       JOIN MountainsCountries AS mc ON mc.MountainId = m.Id
  	 RIGHT JOIN Countries AS c ON c.CountryCode = mc.CountryCode) AS tt
   WHERE tt.Rank = 1
ORDER BY tt.CountryName, tt.PeakName