CREATE TABLE ArtistPictures (

	ArtPicID      int IDENTITY (1, 1),
	ArtistPicture varbinary (MAX),

	CONSTRAINT PK_ArtistPicture PRIMARY KEY (ArtPicID)

)