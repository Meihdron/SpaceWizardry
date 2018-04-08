create procedure [Auth].[saveToken](
	 @TokenID int					
	,@CharacterID int 				
	,@AccessToken varchar(100)		
	,@TokenType varchar(100)			
	,@refreshToken varchar(100)	
)
as begin
	if nullif(@TokenID,0) is null
	BEGIN
		insert into auth.token(
		     AccessToken
			,TokenType
			,RefreshToken
			,CharacterID
		)
		values (
			 nullif(@CharacterID,0)
			,@AccessToken
			,@TokenType
			,@refreshToken
			,@characterID
		)
		return scope_identity()
	END
	ELSE BEGIN
		update auth.token
		set 
			 AccessToken	= @AccessToken
			,TokenType		= @TokenType
			,RefreshToken	= @refreshToken
		where TokenID = @TokenID
		return @tokenID
	END
END

