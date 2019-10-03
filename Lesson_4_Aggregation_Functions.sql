--1
SELECT COUNT(*) AS [Count] FROM WizzardDeposits 
--2
select * FROM WizzardDeposits
GO
SELECT TOP(1) w.MagicWandSize FROM WizzardDeposits AS w
ORDER BY w.MagicWandSize DESC
GO
--3
SELECT w.DepositGroup, 
	   Max(w.MagicWandSize) AS LongestMagicWand
FROM WizzardDeposits AS w
GROUP BY w.DepositGroup
GO
--4
select TOP(2) x.DepositGroup FROM (SELECT w.DepositGroup, 
	   Avg(w.MagicWandSize) AS AverageMagicWandSize
FROM WizzardDeposits AS w
GROUP BY w.DepositGroup ) AS X
ORDER BY X.AverageMagicWandSize
GO
--5
SELECT w.DepositGroup,
	   SUM(w.DepositAmount) as TotalSum
FROM WizzardDeposits AS w
GROUP BY w.DepositGroup
GO
--6
SELECT w.DepositGroup,
	   SUM(w.DepositAmount) as TotalSum
FROM WizzardDeposits AS w
WHERE w.MagicWandCreator='Ollivander family'
GROUP BY w.DepositGroup
GO
--7
--WITH nested queries
SELECT * from (SELECT w.DepositGroup,
	   SUM(w.DepositAmount) as TotalSum
FROM WizzardDeposits AS w
WHERE w.MagicWandCreator='Ollivander family'
GROUP BY w.DepositGroup) AS P
WHERE P.TotalSum<=150000
ORDER BY P.DepositGroup DESC

SELECT w.DepositGroup,
	   SUM(w.DepositAmount) as TotalSum
FROM WizzardDeposits AS w
WHERE w.MagicWandCreator='Ollivander family'
GROUP BY w.DepositGroup
HAVING SUM(w.DepositAmount)<=150000
ORDER BY w.DepositGroup DESC
GO
--8
SELECT w.DepositGroup,
	   w.MagicWandCreator,
	MIN(w.DepositCharge) AS MinDepositCharge 
FROM WizzardDeposits AS w
GROUP BY w.DepositGroup,w.MagicWandCreator
ORDER BY w.MagicWandCreator,w.DepositGroup
GO
--9
SELECT r.AgeGroup,
	   COUNT(r.AgeGroup)  AS [WizardCount]
	   FROM (SELECT  
	   CASE
	   WHEN w.Age BETWEEN 0 AND 10 THEN '[0-10]'
	   WHEN w.Age BETWEEN 11 AND 20 THEN '[11-20]'
	   WHEN w.Age BETWEEN 21 AND 30 THEN '[21-30]'
	   WHEN w.Age BETWEEN 31 AND 40 THEN '[31-40]'
	   WHEN w.Age BETWEEN 41 AND 50 THEN '[41-50]'
	   WHEN w.Age BETWEEN 51 AND 60 THEN '[51-60]'
	   ELSE '[61+]' 
	   END AS AgeGroup
	 FROM WizzardDeposits AS w) AS r
GROUP BY r.AgeGroup
GO
--10
SELECT 
	SUBSTRING(w.FirstName,1,1)AS [FirstLetter] 	
FROM WizzardDeposits AS w
WHERE w.DepositGroup IN ('Troll Chest')
GROUP BY SUBSTRING(w.FirstName,1,1)
ORDER BY SUBSTRING(w.FirstName,1,1)
GO
--11
SELECT w.DepositGroup,
	   w.IsDepositExpired,
	   AVG(w.DepositInterest) AS [AverageInterest] FROM WizzardDeposits AS w
	WHERE w.DepositStartDate> '01/01/1985'
	GROUP BY w.DepositGroup, W.IsDepositExpired
	ORDER BY w.DepositGroup DESC, W.IsDepositExpired
GO
--12
SELECT SUM(x.[Deposit DIfference] ) AS [SumDifference] FROM (SELECT w.FirstName AS [Host Wizard],
	   w.DepositAmount AS [Host Wizard Deposit],
	   LEAD(w.FirstName,1) OVER(ORDER BY Id) as [Guest Wizard],
	   LEAD(w.DepositAmount,1) OVER (ORDER BY Id) AS [Guest Wizard Deposit],
	   w.DepositAmount-LEAD(w.DepositAmount,1) OVER (ORDER BY ID) AS [Deposit DIfference]
FROM WizzardDeposits	 AS w) AS x
GO
--13
GO
USE Softuni
SELECT e.DepartmentID,
	   SUM(e.Salary) AS [TotalSalary]
	   
FROM Employees AS e
GROUP BY e.DepartmentID
ORDER BY e.DepartmentID
GO
--14
SELECT e.DepartmentID,
	   MIN(e.Salary)AS [MinimumSalary]
	   FROM Employees AS e
	   where e.HireDate>'01/01/2000'
	   GROUP BY e.DepartmentID
	   HAVING e.DepartmentID IN (2,5,7) 
GO
--15
SELECT *
INTO NewOne
FROM Employees AS e
WHERE e.Salary>30000

DELETE FROM NewOne 
WHERE ManagerID=42

UPDATE NewOne
SET Salary+=5000
WHERE DepartmentID=1

SELECT n.DepartmentID,
	   AVG(n.Salary) AS AverageSalary
 FROM NewOne AS n
 GROUP BY n.DepartmentID
 GO
 --16
 SELECT e.DepartmentID,
	   MAX(e.Salary) AS [MaxSalary] FROM Employees AS e
	   GROUP BY e.DepartmentID
	   HAVING MAX(e.Salary) NOT BETWEEN 30000 AND 70000
GO
--17
SELECT COUNT(t.Salary)  AS [Count] FROM 
(SELECT e.Salary ,
	e.ManagerID
FROM Employees AS e
WHERE e.managerId IS NULL
) as t
GO
--18
SELECT t.DepartmentID,
	MAX(t.Salary) AS [ThirdHighestSalary]
	 FROM  (
	 SELECT e.DepartmentID,
	   DENSE_RANK() OVER (PARTITION BY e.DepartmentId ORDER BY e.Salary DESC) AS [Rank],
	   e.Salary
	   FROM Employees AS e
	   ) AS t
	   WHERE t.[Rank] IN 3
	   GROUP BY t.DepartmentID,t.[Rank]
GO
--19

 SELECT TOP(10)  e.FirstName,
	   e.LastName,
	   e.DepartmentID
 FROM Employees AS e
 WHERE e.Salary>
	   (SELECT 
		AVG(em.Salary) 
		FROM Employees AS em
		WHERE em.DepartmentID= e.DepartmentID
		)
	ORDER BY e.DepartmentID
