-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `deleteSelectedRecord`(IN ImdbID char(7))
BEGIN
DECLARE deleteID INT;
IF(EXISTS(SELECT FilmID from films WHERE FilmImdbID = ImdbID))THEN
SELECT filmID into deleteID from films WHERE FilmImdbID = ImdbID;
DELETE FROM films WHERE deleteID = filmID;
END IF;

IF(EXISTS(SELECT actorID from actors WHERE ActorImdbID = ImdbID))THEN
SELECT actorID into deleteID from actors WHERE ActorImdbID = ImdbID;
DELETE FROM actors WHERE deleteID = filmID;
END IF;

IF(EXISTS(SELECT directorID from directors WHERE DirectorImdbID = ImdbID))THEN
SELECT directorID into deleteID from directors WHERE DirectorImdbID = ImdbID;
DELETE FROM directors WHERE deleteID = filmID;
END IF;

END