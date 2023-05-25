using System;

//   [MoveId] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
//   [UserId] UNIQUEIDENTIFIER NOT NULL,
//   [XLocation] INT NOT NULL,
//   [YLocation] INT NOT NULL,
//   [TokenColor] VARCHAR(10) NOT NULL,
//   [MoveTime] DATETIME2 NOT NULL,

public class UserMove
{
    public int MoveId { get; set; }
    public Guid UserId { get; set; }
    public int XLocation { get; set; }
    public int YLocation { get; set; }
    public string TokenColor { get; set; }
    public DateTime MoveTime { get; set; }
}