-- userid guid, passkey guid, username nvarchar(100), tokencolor varchar(10)
-- x and y integers
-- primary key userid

CREATE TABLE Tag.Users (
    UserId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    Passkey UNIQUEIDENTIFIER NOT NULL,
    UserName NVARCHAR(100) NOT NULL,
    TokenColor VARCHAR(10) NOT NULL,
    XLocation INT NOT NULL,
    YLocation INT NOT NULL
)