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