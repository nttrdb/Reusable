

DROP TABLE IF EXISTS Setting1;
DROP TABLE IF EXISTS Setting3;

CREATE TABLE IF NOT EXISTS Setting1( 
	[Name] TEXT NOT NULL COLLATE NOCASE,
	[Value] TEXT NULL COLLATE NOCASE,
	PRIMARY KEY ([Name])
);

CREATE TABLE IF NOT EXISTS Setting3( 
    [Name] TEXT NOT NULL COLLATE NOCASE,
    [Value] TEXT NULL COLLATE NOCASE,
    [Environment] TEXT NOT NULL COLLATE NOCASE,
    [Version] TEXT NOT NULL COLLATE NOCASE,
	PRIMARY KEY ([Name], [Environment], [Version])
);