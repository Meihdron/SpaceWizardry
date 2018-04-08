create Procedure Auth.LoadToken(
@TokenID int
)as
BEGIN
	SELECT	 TokenID	
		    ,AccessToken	
			,TokenType	
			,RefreshToken
			,CharacterID
	FROM Auth.Token 
	WHERE TokenID = @TokenID
END