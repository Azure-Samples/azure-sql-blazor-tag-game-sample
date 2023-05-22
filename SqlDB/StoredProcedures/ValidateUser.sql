-- params userid and passkey
-- returns 1 if valid, 0 if not

CREATE PROCEDURE [Tag].[ValidateUser]
    @UserIdInput varchar(40),
    @PasskeyInput varchar(40)
AS
BEGIN
    DECLARE @UserId UNIQUEIDENTIFIER = CONVERT(UNIQUEIDENTIFIER, @UserIdInput)
    DECLARE @Passkey UNIQUEIDENTIFIER = CONVERT(UNIQUEIDENTIFIER, @PasskeyInput)

    SELECT COUNT([UserId]) AS UserValidated
    FROM [Tag].[Users]
    WHERE [UserId] = @UserId AND [Passkey] = @Passkey
END