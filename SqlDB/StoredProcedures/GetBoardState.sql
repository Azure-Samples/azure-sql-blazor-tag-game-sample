CREATE PROCEDURE [Tag].[GetBoardState]
    @XLocation int,
    @YLocation int
AS
SELECT XLocation
    , YLocation
    , ISNULL(Purple,0) AS Purple
    , ISNULL(Blue,0) AS Blue
    , ISNULL(Green,0) AS Green
    , ISNULL(Yellow,0) AS Yellow
    , ISNULL(Orange,0) AS Orange
    , ISNULL(Red,0) AS Red
FROM (
    SELECT TokenColor
        , COUNT(UserId) AS UserCount
        , XLocation
        , YLocation
    FROM Tag.Users
    WHERE ABS(XLOCATION-@XLocation) <= 1 AND ABS(YLOCATION-@YLocation) <= 1
        AND (XLOCATION != @XLocation OR YLOCATION != @YLocation)
        AND (ABS(XLOCATION-@XLocation)+ABS(YLOCATION-@YLocation) <= 1)
    GROUP BY TokenColor, XLocation, YLocation
    UNION SELECT 'Purple', 0, @XLocation, @YLocation-1
    UNION SELECT 'Purple', 0, @XLocation, @YLocation+1
    UNION SELECT 'Purple', 0, @XLocation-1, @YLocation
    UNION SELECT 'Purple', 0, @XLocation+1, @YLocation
) AS SourceTable
PIVOT (
    SUM(UserCount)
    FOR TokenColor IN (Purple, Blue, Green, Yellow, Orange, Red)
) AS PivotTable



