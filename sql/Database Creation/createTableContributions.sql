CREATE TABLE Contributions (

	ContributionID int IDENTITY (1, 1),
	ArtistID       int NOT NULL,
	TrackID        int NOT NULL,

	CONSTRAINT PK_Contribution PRIMARY KEY (ContributionID),
	CONSTRAINT FK_ContArtist   FOREIGN KEY (ArtistID) REFERENCES Artists (ArtistID),
	CONSTRAINT FK_ContTrack    FOREIGN KEY (TrackID)  REFERENCES Tracks (TrackID),
	CONSTRAINT UC_Contribution UNIQUE      (ArtistID, TrackID)

)