DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `updateAllTest`()
BEGIN
declare fID INT;

IF EXISTS (SELECT filmID from films where films.FilmImdbID = '0287978') THEN
	UPDATE films SET FilmName = 'Daredevil2'
					, ImdbRating = '9'
					, FilmYear = '2003';
	SET fID = filmID;
else	
	insert into films (FilmImdbID, FilmName, ImdbRating, FilmYear)
						values ('0287978', 'Daredevil2', '9', '2003');
	SET fID = filmID;
END IF;
SELECT fID;
END$$
DELIMITER ;
