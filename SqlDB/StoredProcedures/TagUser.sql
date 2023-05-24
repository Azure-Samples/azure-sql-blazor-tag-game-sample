-- update rainbow +1 for a specific user

CREATE PROCEDURE Tag.TagUser
    @UserIdInput varchar(40)
AS
BEGIN

    DECLARE @UserId UNIQUEIDENTIFIER = CONVERT(UNIQUEIDENTIFIER, @UserIdInput)

    UPDATE Tag.Scoreboard
    SET Rainbow = Rainbow + 1
    WHERE UserId = @UserId;

    SELECT 1 AS StepValidated
END