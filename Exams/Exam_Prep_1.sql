CREATE DATABASE Airport
GO
--1 DDL
CREATE TABLE Planes(
Id INT PRIMARY KEY IDENTITY(1,1)
,[Name] VARCHAR(30) NOT NULL
,Seats INT NOT NULL
,[Range] INT NOT NULL
)

CREATE TABLE LuggageTypes(
Id INT PRIMARY KEY IDENTITY(1,1)
,[Type] VARCHAR(30) NOT NULL
)

CREATE TABLE Flights(
Id INT PRIMARY KEY IDENTITY(1,1)
,DepartureTime DATETIME
,ArrivalTime DATETIME
,Origin VARCHAR(50) NOT NULL
,Destination VARCHAR(50) NOT NULL
,PlaneId INT FOREIGN KEY REFERENCES Planes(Id) NOT NULL
)

CREATE TABLE Passengers(
Id INT PRIMARY KEY IDENTITY(1,1)
,[FirstName] VARCHAR(30) NOT NULL
,[LastName] VARCHAR(30) NOT NULL
,[Age] INT NOT NULL
,[Address] VARCHAR(30) NOT NULL
,[PassportId] CHAR(11) NOT NULL
)

CREATE TABLE Luggages(
Id INT PRIMARY KEY IDENTITY(1,1)
,LuggageTypeId INT FOREIGN KEY REFERENCES LuggageTypes(Id) NOT NULL
,PassengerId INT FOREIGN KEY REFERENCES Passengers(Id) NOT NULL
)

CREATE TABLE Tickets(
Id INT PRIMARY KEY IDENTITY(1,1)
,PassengerId INT FOREIGN KEY REFERENCES Passengers(Id) NOT NULL
,FlightId INT FOREIGN KEY REFERENCES Flights(Id) NOT NULL
,LuggageId INT FOREIGN KEY REFERENCES Luggages(Id) NOT NULL
,Price DECIMAL (18,2) NOT NULL
)
--2 Insert into
GO
INSERT INTO Planes ([Name], [Seats],[Range])
VALUES 
('Airbus 336',112,5132)
,('Airbus 330',432,5325)
,('Boeing 369',231,2355)
,('Stelt 297',254,2143)
,('Boeing 338',165,5111)
,('Airbus 558',387,1342)
,('Boeing 128',345,5541)

INSERT INTO LuggageTypes(
[Type]
)
VALUES 
('Crossbody Bag')
,('School Backpack')
,('Shoulder Bag')
GO

--3 Update
UPDATE Tickets
SET Price*=1.13
WHERE FlightId IN (
	SELECT Id FROM Flights
	WHERE Destination = 'Carlsbad'
)
GO
--4 Delete
ALTER TABLE Tickets
ALTER COLUMN FlightId INT 

UPDATE Tickets
SET FlightId = NULL
WHERE FlightId in (
	SELECT Id FROM Flights
	WHERE Destination='Ayn Halagim'
)
DELETE FROM Flights
WHERE Destination= 'Ayn Halagim'

--SELECT * FROM Tickets
--WHERE FlightId IN (
--	SELECT Id FROM Flights
--	WHERE Destination='Ayn Halagim'
--)
--5  The 'Tr' Planes
GO
USE Airport
SELECT * FROM Planes AS p
	WHERE p.[Name] LIKE '%tr%'
	ORDER BY p.Id,
			P.[Name],
			P.Seats DESC,
			p.[Range] 

GO
--6 Flight Profits
SELECT 
		f.Id AS [FlightId],
		SUM(t.Price) AS [Price]
		   
		 FROM Flights AS f
	JOIN Tickets AS t ON t.FlightId=f.Id
	GROUP BY f.Id
	ORDER BY SUM(t.Price) DESC,
			 f.Id	
GO
--7 Passenger Trips
	SELECT * FROM		(SELECT 
				p.FirstName+' '+p.LastName AS [Full Name]
				,f.Origin
				,f.Destination
				FROM Passengers AS p
			JOIN Tickets AS t ON t.PassengerId=p.Id
			JOIN Flights AS f ON f.Id=t.FlightId
			) AS t
		ORDER BY t.[Full Name],t.Origin,t.Destination
--8 Non Adventures People
SELECT p.FirstName
	   ,p.LastName
	   ,p.Age FROM Passengers AS p
	LEFT JOIN Tickets AS t ON t.PassengerId=p.Id
	WHERE t.FlightId is null
	ORDER BY p.Age DESC
			 ,p.FirstName
			 ,p.LastName
--9 Full Info
		
	SELECT 
			t.[Full Name]
			,t.[Plane Name]
			,t.Trip
			,t.[Luggage Type]
			FROM	(SELECT 
			
				p.FirstName+' '+p.LastName AS [Full Name]
				,pl.[Name] AS [Plane Name]
				,f.Origin+' - '+f.Destination AS [Trip]
				,lt.[Type] AS [Luggage Type]
				,f.Origin
				,f.Destination
				FROM Passengers AS p
			LEFT JOIN Tickets AS t ON p.Id=t.PassengerId
			JOIN Flights AS f ON f.Id=t.FlightId
			JOIN Planes AS pl ON pl.Id=f.PlaneId
			 JOIN Luggages AS l ON  t.LuggageId=l.Id
			JOIN LuggageTypes AS lt ON lt.Id=l.LuggageTypeId
			) AS t
		--WHERE
		--t.[Full Name]='Adina Uvedale'
		--AND t.[Plane Name]='Babbleopia'
		ORDER BY t.[Full Name]
				 ,t.[Plane Name]
				 ,t.Origin
				 ,t.Destination
				 ,t.[Luggage Type]


--10 PSP
SELECT pa.[Name]
		   ,pa.Seats
		   ,COUNT(p.Id) AS [Passengers Count]
		   FROM Passengers AS p
	LEFT JOIN Tickets AS t ON t.PassengerId=p.Id
	JOIN Flights AS f ON t.FlightId=f.Id
	RIGHT JOIN Planes AS pa ON pa.Id=f.PlaneId
	GROUP BY pa.[Name], pa.Seats
	ORDER BY COUNT(p.Id) DESC, pa.[Name],pa.Seats

	USE Airport

--11 Vacation
GO
CREATE FUNCTION udf_CalculateTickets (@origin VARCHAR(MAX),@destination VARCHAR(MAX), @peopleCount int)
RETURNS VARCHAR(MAX)
AS
	BEGIN
	DECLARE @flightPrice DECIMAL(15,2)= (SELECT t.Price 
											
											FROM Tickets AS t
											JOIN Flights AS f ON f.Id=t.FlightId
											WHERE f.Origin=@origin AND f.Destination=@destination
										)
	IF (@peopleCount <=0)
	RETURN 'Invalid people count!'
	IF (@flightPrice IS NULL)
	RETURN 'Invalid flight!'

	DECLARE @totalPrice DECIMAL(15,2)= @flightPrice *@peopleCount;

	RETURN 'Total price '+CONVERT(VARCHAR(MAX),@totalPrice)

	END
GO

--12 Wrong Data
GO
CREATE PROCEDURE usp_CancelFlights
AS
	BEGIN
		UPDATE Flights
		SET ArrivalTime = null, DepartureTime=null
		WHERE Id IN	(
			SELECT Id FROM Flights AS f
			WHERE  ArrivalTime>=DepartureTime
			)
	END