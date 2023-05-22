
public class UserScores
{
    public int Red { get; set; }
    public int Orange { get; set; }
    public int Yellow { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
    public int Purple { get; set; }
    public int Rainbow { get; set; }

    public int TotalPoints()
    {
        int totalPoints = 0;
        totalPoints += Red;
        totalPoints += Orange;
        totalPoints += Yellow;
        totalPoints += Green;
        totalPoints += Blue;
        totalPoints += Purple;
        totalPoints += Rainbow;

        return totalPoints;
    }

    // default constructor
    public UserScores()
    {
        Red = 0;
        Orange = 0;
        Yellow = 0;
        Green = 0;
        Blue = 0;
        Purple = 0;
        Rainbow = 0;
    }
}