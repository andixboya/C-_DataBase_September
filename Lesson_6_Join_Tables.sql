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
