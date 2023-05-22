-- params userid and passkey
-- returns user info

CREATE PROCEDURE [Tag].[GetUserInfo]
    @UserIdInput varchar(40),
    @PasskeyInput varchar(40)
AS
BEGIN
    DECLARE @UserId UNIQUEIDENTIFIER = CONVERT(UNIQUEIDENTIFIER, @UserIdInput)
    DECLARE @Passkey UNIQUEIDENTIFIER = CONVERT(UNIQUEIDENTIFIER, @PasskeyInput)

    SELECT U.[UserId], U.Passkey, U.UserName, U.TokenColor,
        U.XLocation, U.YLocation
    FROM [Tag].[Users] U
    WHERE U.[UserId] = @UserId AND U.[Passkey] = @Passkey
END