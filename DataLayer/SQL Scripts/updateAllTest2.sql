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
declare fID INT;
DECLARE aId INT;
DECLARE dID INT;

-- makes sure the MySQL does not truncate input values 
SET sql_mode = STRICT_ALL_TABLES;

START TRANSACTION;
IF (EXISTS (SELECT filmID from films where FilmImdbID = fImdb)) THEN
	UPDATE films SET FilmName = fName
					, ImdbRating = fRate
					, FilmYear = fYear
	WHERE filmImdbID = fImdb;
else	
	insert into `films` (`FilmImdbID`, `FilmName`, `ImdbRating`, `FilmYear`)
						values (fImdb, fName, fRate, fYear);
END IF;

SELECT filmID into fID FROM films
WHERE filmImdbID = fImdb;
	
if(EXISTS (SELECT actorID FROM actors WHERE ActorImdbID = aImdb)) THEN
	UPDATE actors SET ActorName = aName
	WHERE ActorImdbID = aImdb;
else
	Insert Into `actors`(`ActorImdbID`, `ActorName`)
				VALUES(aImdb,aName);
END IF;

SELECT actorID into aID FROM actors
	WHERE actorImdbID = aImdb;
	
if(EXISTS (SELECT filmID, actorID FROM film_actor WHERE FilmID = fID and ActorID = aID)) THEN
	UPDATE film_actor SET ActorID = aId, FilmID = fID
	WHERE FilmID = fID and ActorID = aID;
else
	Insert Into `film_actor`(`ActorID`, `FilmID`)
				VALUES(aID, fID);
END IF;

if(EXISTS(SELECT DirectorID FROM directors WHERE DirectorImdbID = dImdb)) THEN
	UPDATE directors SET DirectorName = dName
	WHERE DirectorImdbID = dImdb;
else
	Insert Into `directors`(`DirectorImdbID`, `DirectorName`)
				VALUES(dImdb,dName);
END IF;

SELECT directorID into dID FROM directors
	WHERE DirectorImdbID = dImdb;
	
if(EXISTS(SELECT filmID, directorID from  film_director WHERE FilmID = fID and DirectorID = dID)) THEN
	UPDATE film_director SET DirectorID = dId, FilmID = fID
	WHERE FilmID = fID and DirectorID = aID;
else
	Insert Into `film_director`(`DirectorID`, `FilmID`)
				VALUES(dID, fID);
END IF;

COMMIT;

END