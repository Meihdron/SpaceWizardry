CREATE TABLE [Auth].[Token]
(
	 TokenID INT identity (1,1) NOT NULL PRIMARY KEY clustered
	,[AccessToken] varchar(100) 
    ,[TokenType] varchar(100)
    ,[RefreshToken] varchar(100), 
    [CharacterID] INT NULL
)
