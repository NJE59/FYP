CREATE TABLE Discs (

	DiscID    int IDENTITY (1, 1),
	AlbumID   int NOT NULL,
	DiscNum   int DEFAULT 0,
	DiscTitle nvarchar(255),

	CONSTRAINT PK_Disc     PRIMARY KEY (DiscID),
	CONSTRAINT FK_Album    FOREIGN KEY (AlbumID) REFERENCES Albums (AlbumID),
	CONSTRAINT UC_Disc     UNIQUE      (AlbumID, DiscNum),
	CONSTRAINT CHK_DiscNum CHECK       (DiscNum >= 0)

)