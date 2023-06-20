CREATE TABLE Listings (
	
	ListingID  int IDENTITY (1, 1),
	PlaylistID int NOT NULL,
	TrackID    int NOT NULL,
	ListingNum int NOT NULL,

	CONSTRAINT PK_Listing     PRIMARY KEY (ListingID),
	CONSTRAINT FK_Playlist    FOREIGN KEY (PlaylistID) REFERENCES Playlists (PlaylistID),
	CONSTRAINT FK_ListedTrack FOREIGN KEY (TrackID)    REFERENCES Tracks    (TrackID),
	CONSTRAINT UC_Listing     UNIQUE      (PlaylistID, ListingNum),
	CONSTRAINT CHK_ListingNum CHECK       (ListingNum >= 1)

)