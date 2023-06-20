CREATE  TABLE Albums (
	
	AlbumID	     int IDENTITY (1, 1),
	ArtistID     int,
	AlbPicID     int NOT NULL,
	ReleaseYear  int,
	AlbumTitle   nvarchar (255),
	Descritption nvarchar (MAX),

	CONSTRAINT PK_Album        PRIMARY KEY (AlbumID),
	CONSTRAINT FK_AlbArtist    FOREIGN KEY (ArtistID) REFERENCES Artists       (ArtistID),
	CONSTRAINT FK_AlbumPicture FOREIGN KEY (AlbPicID) REFERENCES AlbumPictures (AlbPicID),
	CONSTRAINT UC_AlbumPicture UNIQUE      (AlbPicID),
	CONSTRAINT UC_Album        UNIQUE      (ArtistID, AlbumTitle, ReleaseYear),
	CONSTRAINT CHK_ReleaseYear CHECK	   (ReleaseYear >= 0)

)