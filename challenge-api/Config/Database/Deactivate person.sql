USE [CHALLENGE]
GO
/****** Object:  StoredProcedure [dbo].[DeleteBusinessEntity]    Script Date: 3/1/2019 4:09:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DeleteBusinessEntity]
(
	@PersonID int
)
AS
BEGIN

SET @PersonID = 
(SELECT TOP(1) BusinessEntityID FROM Person.Person 
WHERE BusinessEntityID = @PersonID)
if(@PersonID IS NOT NULL)
	begin

		UPDATE  Person.Person SET isValid = 0
		WHERE BusinessEntityID = @PersonID
		END
	END
--IF NOT EXISTS(SELECT 1 FROM sys.columns
--	WHERE Name = N'isValid'
--	AND object_id = object_id(N'Person.Person'))
--	BEGIN
--ALTER TABLE Person.Person
--	ADD  isValid bit default 1
--END

