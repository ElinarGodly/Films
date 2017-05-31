-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `updateAllTest2`(
IN 
fImdb char(7),
fName varchar(45),
fRate decimal(2,1),
dImdb char(7),
dName varchar(45),
aImdb char(7),
aName varchar(45),
fYear char(4)
)
BEGIN
declare ve INT;
declare fID INT;
DECLARE aId INT;
DECLARE dID INT;

SELECT count(*) into ve FROM films
WHERE FilmImdbID = fImdb;

IF (EXISTS (SELECT filmID from films where FilmImdbID = '0287978')=1) THEN
	UPDATE films SET FilmName = fName
					, ImdbRating = fRate
					, FilmYear = fYear
	WHERE filmImdbID = fImdb;
	SELECT filmID into fID FROM films
	WHERE filmImdbID = fImdb;
	
else	
	insert into `films` (`FilmImdbID`, `FilmName`, `ImdbRating`, `FilmYear`)
						values (fImdb, fName, fRate, fYear);
	SELECT filmID into fID FROM films
	WHERE filmImdbID = fImdb;
END IF;

SET ve = 0;

SELECT count(*) into ve FROM actors
WHERE ActorImdbID = aImdb;

if(ve>=1) THEN
	UPDATE actors SET ActorName = aName
	WHERE ActorImdbID = aImdb;
	SELECT actorID into aID FROM actors
	WHERE actorImdbID = aImdb;
else
	Insert Into `actors`(`ActorImdbID`, `ActorName`)
				VALUES(aImdb,aName);
	SELECT actorID into aID FROM actors
	WHERE actorImdbID = aImdb;
END IF;

SET ve = 0;

SELECT count(*) into ve FROM film_actor
WHERE FilmID = fID and ActorID = aID;

if(ve>=1) THEN
	UPDATE film_actor SET ActorID = aId, FilmID = fID
	WHERE FilmID = fID and ActorID = aID;
else
	Insert Into `film_actor`(`ActorID`, `FilmID`)
				VALUES(aID, fID);
END IF;

SET ve = 0;

SELECT count(*) into ve FROM directors
WHERE DirectorImdbID = dImdb;

if(ve>=1) THEN
	UPDATE directors SET DirectorName = dName
	WHERE DirectorImdbID = dImdb;
	SELECT directorID into dID FROM directors
	WHERE DirectorImdbID = dImdb;
else
	Insert Into `directors`(`DirectorImdbID`, `DirectorName`)
				VALUES(dImdb,dName);
	SELECT directorID into dID FROM directors
	WHERE DirectorImdbID = dImdb;
END IF;

SET ve = 0;

SELECT count(*) into ve FROM film_director
WHERE FilmID = fID and DirectorID = dID;

if(ve>=1) THEN
	UPDATE film_director SET DirectorID = dId, FilmID = fID
	WHERE FilmID = fID and DirectorID = aID;
else
	Insert Into `film_director`(`DirectorID`, `FilmID`)
				VALUES(dID, fID);
END IF;


END