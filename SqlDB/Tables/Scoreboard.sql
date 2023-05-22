-- userid
-- public int Red { get; set; }
-- public int Orange { get; set; }
-- public int Yellow { get; set; }
-- public int Green { get; set; }
-- public int Blue { get; set; }
-- public int Purple { get; set; }
-- public int Rainbow { get; set; }

CREATE TABLE Tag.Scoreboard (
    UserId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    Red INT NOT NULL,
    Orange INT NOT NULL,
    Yellow INT NOT NULL,
    Green INT NOT NULL,
    Blue INT NOT NULL,
    Purple INT NOT NULL,
    Rainbow INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Tag.Users(UserId)
)