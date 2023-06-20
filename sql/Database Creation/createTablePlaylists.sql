CREATE TABLE Playlists (
	
	PlaylistID          int IDENTITY (1, 1),
	ListPicID           int NOT NULL,
	PlaylistName        nvarchar (255) NOT NULL,
	PlaylistDescription nvarchar (MAX),

	CONSTRAINT PK_Playlist        PRIMARY KEY (PlaylistID),
	CONSTRAINT FK_PlaylistPicture FOREIGN KEY (ListPicID) REFERENCES PlaylistPictures (ListPicID),
	CONSTRAINT UC_PlaylistPicture UNIQUE      (ListPicID),
	CONSTRAINT UC_PlaylistName    UNIQUE      (PlaylistName),
	CONSTRAINT CHK_PlaylistName   CHECK       (PlaylistName <> '')

)