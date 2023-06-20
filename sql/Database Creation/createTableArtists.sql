CREATE TABLE Artists (
	
	ArtistID   int IDENTITY (1, 1),
	ArtPicID   int NOT NULL,
	ArtistName nvarchar (255) NOT NULL,
	ArtistBio  nvarchar (MAX),

	CONSTRAINT PK_Artist        PRIMARY KEY (ArtistID),
	CONSTRAINT FK_ArtistPicture FOREIGN KEY (ArtPicID) REFERENCES ArtistPictures(ArtPicID),
	CONSTRAINT UC_ArtPicID      UNIQUE      (ArtPicID),
	CONSTRAINT UC_ArtistName    UNIQUE      (ArtistName),
	CONSTRAINT CHK_ArtistName   CHECK       (ArtistName <> '')

)