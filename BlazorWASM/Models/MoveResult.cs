
public class MoveResult
{
    public int XLocation { get; set; }
    public int YLocation { get; set; }
    public UserScores PointsEarned { get; set; }

    public MoveResult()
    {
        XLocation = 0;
        YLocation = 0;
        PointsEarned = new UserScores();
    }
}