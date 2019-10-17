 --8 Programmability
 --14 Create Table Logs
USE Bank
CREATE TABLE Logs(
LogId INT PRIMARY KEY IDENTITY(1,1)
,AccountId INT FOREIGN KEY REFERENCES Accounts(Id)
,OldSum DECIMAL(15,4)
,NewSum DECIMAL (15,4)
)
GO

CREATE TRIGGER LogTrigger ON Accounts FOR UPDATE
 --, we can add multiple events here! (with comma)
AS
	BEGIN
		DECLARE @AccountId INT =(SELECT  Id FROM inserted);
		DECLARE @OldSum DECIMAL(15,4)= (SELECT  Balance FROM deleted )
		DECLARE @NewSum DECIMAL (15,4) =(SELECT Balance FROM inserted)
		
		

	INSERT INTO Logs (
				AccountId
				,OldSum
				,NewSum
				) 
	VALUES(
		@AccountId
		,@OldSum
		,@NewSum
		)
	END
GO
--UPDATE Accounts
--SET Balance +=1600
--WHERE Id=1

--SELECT * FROM Logs
--SELECT * FROM Accounts

GO
--15 Create Table Emails

CREATE  TABLE NotificationEmails(
	Id INT PRIMARY KEY IDENTITY(1,1)
	,Recipient VARCHAR(50) NOT NULL
	,[Subject] VARCHAR(50) NOT NULL
	,[Body] VARCHAR (MAX) NOT NULL
)

GO
CREATE TRIGGER tr_EmailAddition ON Logs FOR INSERT
AS
	BEGIN
		DECLARE @Recipient INT = (SELECT AccountId FROM inserted)
		DECLARE @Subject VARCHAR(MAX) = 'Balance change for account: '+CONVERT(VARCHAR(MAX),@Recipient)
		DECLARE @Date SMALLDATETIME = GETDATE()
		DECLARE @OldSum DECIMAL(18,2)= (SELECT OldSum FROM inserted)
		DECLARE @NewSum DECIMAL(18,2)= (SELECT NewSum FROM inserted)
		DECLARE @Body VARCHAR(MAX)= 'On '+CONVERT(VARCHAR(50),@Date,0)+' your balance was changed from '+CONVERT(VARCHAR(50),@OldSum)+' to '+CONVERT(VARCHAR(50),@NewSum)+'.'
		
	INSERT INTO NotificationEmails (
				Recipient
				,[Subject]
				,[Body]
	)
	VALUES (
			@Recipient
			,@Subject
			,@Body
	)
	END
--UPDATE Accounts
--SET Balance +=1600
--WHERE Id=1
--SELECT * FROM Logs
--SELECT * FROM NotificationEmails

GO
--16 Deposit Money
CREATE PROC usp_DepositMoney (@AccountId INT, @MoneyAmount DECIMAL(18,4))
AS
	BEGIN
		UPDATE Accounts 
		SET Balance+=@MoneyAmount
		WHERE Id=@AccountId
		
	END
 EXEC dbo.usp_DepositMoney @AccountId =1,@MoneyAmount=10
 GO
 --17 - Withdraw Money Procedure

 CREATE PROC usp_WithdrawMoney @AccountId INT , @MoneyAmount DECIMAL(18,4)
 AS
	BEGIN
		UPDATE Accounts 
		SET Balance-=@MoneyAmount
		WHERE Id=@AccountId
		
	END
GO
--18 Money Transfer /the idea is to use transaction here!/
	
CREATE OR ALTER PROC usp_TransferMoney @SenderId INT, @ReceiverId INT, @Amount DECIMAL(18,4)
AS
	BEGIN TRANSACTION
		
		IF (@Amount< 0 OR @ReceiverId=@SenderId) --works without OR @ReceiverId=@SenderId
			BEGIN
				ROLLBACK
					RAISERROR('Invalid amount!',16,1)
				--RETURN return is optional here!
			END
		EXEC dbo.usp_WithdrawMoney @SenderId,@Amount
		EXEC dbo.usp_DepositMoney @ReceiverId, @Amount
COMMIT
EXEC dbo.usp_TransferMoney @SenderId=1, @ReceiverId=1 , @Amount = 350
SELECT * from Accounts
SELECT * FROM Accounts
GO
--19 skipped
--20 skipped
--21  Employees with Three Projects
USE SoftUni
GO 
CREATE PROC usp_AssignProject @employeeID INT, @projectID INT
AS
BEGIN TRANSACTION
DECLARE @countOfProjects INT = (SELECT  COUNT(ep.ProjectID) AS result FROM Employees AS e
	JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
	JOIN Projects AS p ON p.ProjectID= ep.ProjectID
	WHERE  e.EmployeeID= @employeeID)

IF (@countOfProjects>=3)
	BEGIN
		ROLLBACK
		RAISERROR('The employee has too many projects!',16,1)
		RETURN
	END
DECLARE @test INT= (SELECT  employeeID FROM Employees WHERE EmployeeID= @employeeID)

IF (@test IS NULL)
	BEGIN
		ROLLBACK
		RAISERROR('No such employee',16,2)
		RETURN
	END
DECLARE @project INT = (SELECT TOP(1) ProjectID FROM Projects WHERE ProjectID= @projectID)

IF (@project IS NULL)
	BEGIN
		ROLLBACK
		RAISERROR('Project does not exist',16,3)
		RETURN
	END

INSERT INTO EmployeesProjects VALUES
(@employeeID , @projectID)
COMMIT
GO
--22 skipped
