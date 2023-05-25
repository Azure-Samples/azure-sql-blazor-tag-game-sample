-- moveid integer identity
-- userid guid, x and y integers, tokencolor
-- move timestamp

CREATE TABLE [Tag].[Moves]
(
  [MoveId] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [UserId] UNIQUEIDENTIFIER NOT NULL,
  [XLocation] INT NOT NULL,
  [YLocation] INT NOT NULL,
  [TokenColor] VARCHAR(10) NOT NULL,
  [MoveTime] DATETIME2 NOT NULL,
  FOREIGN KEY (UserId) REFERENCES Tag.Users(UserId)
)

-- enable change tracking
ALTER TABLE Tag.Moves ENABLE CHANGE_TRACKING;