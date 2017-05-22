USE `filmdb`;
DROP procedure IF EXISTS `selectAll`;

DELIMITER $$
USE `filmdb`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `selectAll`()
BEGIN
	CREATE TEMPORARY TABLE IF NOT EXISTS results
	SELECT f.FilmImdbID, f.FilmName, f.ImdbRating
	 	, d.DirectorImdbID, d.DirectorName
        , a.ActorImdbID, a.ActorName
        , f.FilmYear
    FROM film_director fd
		JOIN films f ON f.FilmID=fd.FilmID
        JOIN directors d ON d.DirectorID=fd.DirectorID
        JOIN film_actor fa ON fa.FilmID=f.FilmID
        JOIN actors a ON fa.ActorID=a.ActorID
	;

	SELECT * FROM results;
END$$

DELIMITER ;
