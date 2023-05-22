-- params userid and direction
-- returns users tagged

CREATE PROCEDURE [Tag].[MoveUser]
    @UserIdInput varchar(40),
    @XLocation int,
    @YLocation int
AS
BEGIN

    DECLARE @UserId UNIQUEIDENTIFIER = CONVERT(UNIQUEIDENTIFIER, @UserIdInput)

    INSERT INTO Tag.Moves (UserId, XLocation, YLocation, TokenColor, MoveTime)
    SELECT UserId, @XLocation, @YLocation, TokenColor, GETUTCDATE()
    FROM Tag.Users
    WHERE UserId = @UserId;

    UPDATE Tag.Users
    SET XLocation = @XLocation, YLocation = @YLocation
    WHERE UserId = @UserId;

    SELECT @UserId AS UserId
        , ISNULL(Purple,0) AS Purple
        , ISNULL(Blue,0) AS Blue
        , ISNULL(Green,0) AS Green
        , ISNULL(Yellow,0) AS Yellow
        , ISNULL(Orange,0) AS Orange
        , ISNULL(Red,0) AS Red
        , 0 AS Rainbow
    FROM (
        SELECT TokenColor, COUNT(UserId) AS UserCount
        FROM Tag.Users
        WHERE UserId != @UserId
            AND XLocation = @XLocation AND YLocation = @YLocation
        GROUP BY TokenColor
    ) AS SourceTable
    PIVOT (
        SUM(UserCount)
        FOR TokenColor IN (Purple, Blue, Green, Yellow, Orange, Red)
    ) AS PivotTable

END