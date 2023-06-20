CREATE TABLE PlaylistPictures (

	ListPicID       int IDENTITY (1, 1),
	PlaylistPicture varbinary (MAX),

	CONSTRAINT PK_PlaylistPicture PRIMARY KEY (ListPicID)

)