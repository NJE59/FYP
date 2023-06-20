CREATE TABLE TrackStyles (

	StyleID int IDENTITY (1, 1),
	GenreID int NOT NULL,
	TrackID int NOT NULL,

	CONSTRAINT PK_TrackStyle PRIMARY KEY (StyleID),
	CONSTRAINT FK_StyleGenre FOREIGN KEY (GenreID) REFERENCES Genres (GenreID),
	CONSTRAINT FK_StyleTrack FOREIGN KEY (TrackID) REFERENCES Tracks (TrackID),
	CONSTRAINT UC_TrackStyle UNIQUE (GenreID, TrackID)

)