using System;

public class UserScores
{
    public Guid UserId { get; set; }
    public int Red { get; set; }
    public int Orange { get; set; }
    public int Yellow { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
    public int Purple { get; set; }
    public int Rainbow { get; set; }

    public void AddScores(UserScores amountsToAdd)
    {
        this.Red += amountsToAdd.Red;
        this.Orange += amountsToAdd.Orange;
        this.Yellow += amountsToAdd.Yellow;
        this.Green += amountsToAdd.Green;
        this.Blue += amountsToAdd.Blue;
        this.Purple += amountsToAdd.Purple;
        this.Rainbow += amountsToAdd.Rainbow;
    }

    //  constructor with userid parameter and default scores of 0
    public UserScores(Guid userId)
    {
        this.UserId = userId;
        this.Red = 0;
        this.Orange = 0;
        this.Yellow = 0;
        this.Green = 0;
        this.Blue = 0;
        this.Purple = 0;
        this.Rainbow = 0;
    }
}