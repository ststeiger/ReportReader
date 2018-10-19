
-- DROP TYPE IF EXISTS dbo.GeschossIdentifierTableType
-- GO

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'GeschossIdentifierTableType' AND ss.name = N'dbo')
CREATE TYPE dbo.GeschossIdentifierTableType AS TABLE
(
	GeschossIdentifierValue varchar(76) NULL
)
GO


-- DROP TYPE IF EXISTS dbo.GuidTableType
-- GO

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'GuidTableType' AND ss.name = N'dbo')
CREATE TYPE dbo.GuidTableType AS TABLE(
	GuidValue uniqueidentifier NULL
)
GO



-- DROP TYPE IF EXISTS dbo.IdTableType
-- GO

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'IdTableType' AND ss.name = N'dbo')
CREATE TYPE dbo.IdTableType AS TABLE
(
	IdValue int NULL
)
GO
