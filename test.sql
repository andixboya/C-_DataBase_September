--1
CREATE DATABASE Minions
GO
use Minions
--2
CREATE TABLE Minions (
Id NVARCHAR,
[Name] NVARCHAR,
Age INT
)

CREATE TABLE Towns (
Id int PRIMARY KEY,
[Name] NVARCHAR
)
--OR ... alter table Towns
-- ADD CONSTRAINT Pk_Id_Towns PRIMARY KEY (Id)
go
USE Minions

ALTER TABLE Minions
--ALTER COLUMN  Id nvarchar NOT NULL
ADD CONSTRAINT Pk_Id_Minions PRIMARY KEY (Id)

go
--3
ALTER TABLE Minions
ADD TownId INT FOREIGN KEY REFERENCES Towns(Id)
--4
--RECREATED MINIONS, because i was going berserk! 
CREATE TABLE Minions(
Id INT PRIMARY KEY,
[Name] NVARCHAR(30),
[Age] INT,
[TownId] INT FOREIGN KEY REFERENCES Towns(Id)
)
	INSERT INTO Towns (Id, [Name]) VALUES 
	(1,'Sofia'),
	(2,'Plovdiv'),
	(3,'Varna')
	INSERT INTO Minions (Id,[Name],Age,TownID) VALUES
	(1,'Kevin',22,1),
	(2,'Bob',15,3),
	(3,'Steward',NULL,2)
--5

