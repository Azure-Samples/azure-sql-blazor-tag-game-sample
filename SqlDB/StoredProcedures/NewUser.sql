-- no params
-- inserts a new user into the database
-- returns the new user's id

CREATE PROCEDURE [Tag].[NewUser]
AS
BEGIN
    DECLARE @BLANKPASSKEY UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'
    DECLARE @NEWUSERID UNIQUEIDENTIFIER = NEWID()

    INSERT INTO Tag.Users (UserId, Passkey, UserName, TokenColor, XLocation, YLocation)
    VALUES (@NEWUSERID, @BLANKPASSKEY, '', 'White', 0, 0)

    INSERT INTO Tag.Scoreboard (UserId, Red, Orange, Yellow, Green, Blue, Purple, Rainbow)
    VALUES (@NEWUSERID, 0, 0, 0, 0, 0, 0, 0)

    SELECT UserId, Passkey, UserName, TokenColor, XLocation, YLocation
    FROM Tag.Users
    WHERE UserId = @NEWUSERID
END