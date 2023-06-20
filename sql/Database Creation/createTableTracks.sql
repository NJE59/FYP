CREATE TABLE Tracks (
	
	TrackID          int IDENTITY (1, 1),
	DiscID           int NOT NULL,
	TrackNum         int DEFAULT 0,
	TrackLength      time,
	TrackName        nvarchar (255),
	TrackDescription nvarchar (MAX),
	Lyrics           nvarchar (MAX),
	TrackPath        nvarchar (255) NOT NULL,

	CONSTRAINT PK_Track      PRIMARY KEY (TrackID),
	CONSTRAINT FK_Disc       FOREIGN KEY (DiscID) REFERENCES Discs (DiscID),
	CONSTRAINT UC_TrackPath  UNIQUE      (TrackPath),
	CONSTRAINT CHK_ValidPath CHECK       (TrackPath NOT LIKE '%[<>"|?*]%')
)

  