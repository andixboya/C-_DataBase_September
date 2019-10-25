CREATE database School


use School
--1 DDL
CREATE TABLE Students(
Id INT PRIMARY KEY IDENTITY(1,1)
,
[FirstName] NVARCHAR(30) NOT NULL
,[MiddleName] NVARCHAR(25) 
,[LastName] NVARCHAR(30) NOT NULL
,[Age] INT  CONSTRAINT ck_age_restriction  CHECK ([Age] BETWEEN 5 AND 100)
,[Address] NVARCHAR(50)
,Phone NCHAR(10)
)

CREATE TABLE Subjects(
Id INT PRIMARY KEY IDENTITY(1,1)
,[Name] NVARCHAR(20) NOT NULL
,Lessons INT NOT NULL CONSTRAINT ck_lesson_restriction CHECK ([Lessons] >0)
)

CREATE TABLE StudentsSubjects(
Id INT PRIMARY KEY IDENTITY(1,1)
,StudentId INT  NOT NULL FOREIGN KEY REFERENCES Students(Id) 
,SubjectId INT NOT NULL FOREIGN KEY REFERENCES Subjects(Id)
,Grade DECIMAL(18,2) NOT NULL CONSTRAINT ck_studentsSubjects CHECK ([Grade] BETWEEN 2 AND 6)
)

CREATE TABLE Exams (
Id INT PRIMARY KEY IDENTITY(1,1)
,[Date] DATETIME
,SubjectId INT NOT NULL FOREIGN KEY REFERENCES Subjects(Id)
)

CREATE TABLE StudentsExams(
StudentId INT NOT NULL 
,ExamId INT NOT NULL
,[Grade] DECIMAL (15,2) NOT NULL CONSTRAINT ck_studentExams_grade CHECK([Grade] BETWEEN 2 AND 6)


CONSTRAINT PK_Students_Exams
PRIMARY KEY (StudentId,ExamId),

CONSTRAINT FK_StudentsExams_Students
FOREIGN KEY (StudentId)
REFERENCES Students(Id),

CONSTRAINT FK_StudentsExams_Exams
FOREIGN KEY (ExamId)
REFERENCES Exams(Id)

)

CREATE TABLE Teachers(
Id INT PRIMARY KEY IDENTITY(1,1)
,[FirstName] NVARCHAR(20) NOT NULL
,[LastName] NVARCHAR(20) NOT NULL
,[Address] NVARCHAR(20) NOT NULL
,[Phone] CHAR(10)
,SubjectId INT NOT NULL FOREIGN KEY REFERENCES Subjects(Id)
)
CREATE TABLE StudentsTeachers(
StudentId INT NOT NULL
,TeacherId INT NOT NULL

CONSTRAINT PK_Students_Teachers
PRIMARY KEY (StudentId, TeacherId)

,CONSTRAINT FK_StudentTeachers_Students
FOREIGN KEY (StudentId)
REFERENCES Students(Id)

,CONSTRAINT FK_StudentTeachers_Teachers
FOREIGN KEY (TeacherID)
REFERENCES Teachers(Id)
)
GO
--DML
--2 INSERT


INSERT INTO Teachers (FirstName,LastName,[Address],[Phone],SubjectId)
VALUES 
('Ruthanne','Bamb','84948 Mesta Junction',3105500146,6)
,('Gerrard','Lowin','370 Talisman Plaza',3324874824,2)
,('Merrile','Lambdin','81 Dahle Plaza',4373065154,5)
,('Bert','Ivie','2 Gateway Circle',4409584510,4)

INSERT INTO Subjects([Name],[Lessons])
VALUES 
('Geometry',12)
,('Health',10)
,('Drama',7)
,('Sports',9)

--3 Update

	UPDATE StudentsSubjects
	SET Grade=6.00
	WHERE Id IN
		(
		SELECT ss.Id FROM StudentsSubjects AS ss
			WHERE  ss.SubjectId IN (1, 2) AND Grade>=5.50
		)
--4  Delete
	GO
	--unnecessary :D 
	--ALTER TABLE StudentsTeachers 
	--ALTER COLUMN  TeacherId INT 
	
	--ALTER TABLE StudentsTeachers
	--ALTER COLUMN StudentId INT

	ALTER TABLE Teachers
	ALTER COLUMN SubjectId INT

	DELETE FROM StudentsTeachers
	WHERE TeacherId IN (
	SELECT t.Id FROM Teachers AS t
			WHERE t.Phone LIKE '%72%'
			)

	DELETE FROM Teachers
	WHERE Phone LIKE '%72%'

	GO
--5 Teen Students
SELECT s.FirstName
	   ,s.LastName
	   ,s.[Age] FROM Students AS s
WHERE s.[Age] >=12
ORDER BY s.[FirstName],s.[LastName]
GO
--6 Students Teachers
SELECT s.[FirstName]
	   ,s.[LastName]
	   ,COUNT(t.Id)  AS [TeachersCount]
	   FROM Students AS s
	JOIN StudentsTeachers AS st ON st.StudentId=s.Id
	JOIN Teachers AS t ON t.Id=st.TeacherId
	GROUP BY s.FirstName,s.LastName
GO
--7 Students To Go
SELECT 
		s.FirstName+' '+s.LastName AS [Full Name]
		FROM Students AS s
	LEFT JOIN StudentsExams AS se ON se.StudentId=s.Id
	LEFT JOIN Exams AS e ON E.Id=se.ExamId
	WHERE ExamId IS NULL
	ORDER BY s.FirstName+' '+s.LastName 
GO
--8 TOP Students

SELECT TOP(10) s.FirstName
	   ,s.LastName
	   , CONVERT(DECIMAL(15,2),AVG(se.Grade)) AS [Grade]
	   FROM Students AS s
	JOIN StudentsExams AS se ON se.StudentId=s.Id
	GROUP BY s.FirstName,s.LastName
	ORDER BY AVG(se.Grade) DESC
	,S.FirstName
	,s.LastName
GO
--9 Not So In The Studying
SELECT 
	   s.FirstName+' '+ISNULL(+s.MiddleName+' ','')+s.LastName AS [Full Name]
	   FROM Students AS s
	LEFT JOIN StudentsSubjects AS ss ON ss.StudentId=s.Id
	WHERE ss.Id IS NULL
	ORDER BY s.FirstName+ISNULL(s.MiddleName+' ','')+s.LastName

--10 Average Grade Per Subject
SELECT s.[Name]
	   , AVG(ss.Grade) AS [AverageGrade]
	   FROM Subjects AS s
	JOIN StudentsSubjects AS ss ON SS.SubjectId=S.Id
	GROUP BY s.[Name],s.Id
	ORDER BY s.Id
--11 Exam Grades
USE School
GO
CREATE FUNCTION udf_ExamGradesToUpdate (@studentId INT, @grade DECIMAL(15,2))
RETURNS VARCHAR(MAX)
AS
	BEGIN
		DECLARE @id INT = (SELECT COUNT(Id) FROM Students WHERE Id=@studentId)
		IF(@id!= 1)
			BEGIN
			RETURN 'The student with provided id does not exist in the school!'
			END
		IF(@grade>6.00)
			BEGIN
			RETURN 'Grade cannot be above 6.00!'
			END
		DECLARE @higherBound DECIMAL(15,2)= @grade+0.5
		DECLARE @studentName VARCHAR(MAX) = (SELECT s.FirstName FROM Students AS s WHERE s.Id=@studentId)
		DECLARE @count INT = ( SELECT COUNT(*) FROM Students AS s
			LEFT JOIN StudentsExams as se ON se.StudentId=s.Id
			WHERE se.Grade BETWEEN @grade AND @higherBound AND s.Id=@studentId
			)
			return 'You have to update '+CONVERT(VARCHAR(MAX),@count) +' grades for the student '+@studentName
	END
GO
SELECT dbo.udf_ExamGradesToUpdate(12,5.50)

--12 Exclude from School
GO
CREATE  PROCEDURE usp_ExcludeFromSchool
	@StudentId INT
AS
	BEGIN TRANSACTION
		
		DECLARE @id INT = (SELECT COUNT(Id) FROM Students WHERE Id=@StudentId)
		IF(@id!= 1)
			BEGIN
				ROLLBACK
					RAISERROR('This school has no student with the provided id!',16,1)
			END

		DELETE FROM StudentsTeachers
		WHERE StudentId= @StudentId

		DELETE FROM StudentsExams
		WHERE StudentId=@StudentId

		DELETE FROM StudentsSubjects
		WHERE StudentId=@StudentId

		DELETE FROM Students
		WHERE Id=@StudentId

	COMMIT