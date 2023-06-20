CREATE TABLE Genres (

	GenreID          int IDENTITY (1, 1),
	GenreName        nvarchar (255) NOT NULL,
	GenreDescription nvarchar (MAX),

	CONSTRAINT PK_Genre  PRIMARY KEY (GenreID),
	CONSTRAINT UC_Genre  UNIQUE      (GenreName),
	CONSTRAINT CHK_Genre CHECK       (GenreName <> '')

)