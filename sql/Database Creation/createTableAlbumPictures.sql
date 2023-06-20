CREATE TABLE AlbumPictures (

	AlbPicID     int IDENTITY (1, 1),
	AlbumPicture varbinary (MAX),

	CONSTRAINT PK_AlbumPicture PRIMARY KEY (AlbPicID)

)