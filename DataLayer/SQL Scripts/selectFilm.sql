-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `selectFilm`(IN ImdbID char(7))
BEGIN
	
	SELECT f.FilmImdbID, f.FilmName, f.ImdbRating
	 	, d.DirectorImdbID, d.DirectorName
        , a.ActorImdbID, a.ActorName
        , f.FilmYear
    FROM film_director fd
		JOIN films f ON f.FilmID=fd.FilmID
        JOIN directors d ON d.DirectorID=fd.DirectorID
        JOIN film_actor fa ON fa.FilmID=f.FilmID
        JOIN actors a ON fa.ActorID=a.ActorID
	WHERE f.FilmImdbID = ImdbID
	;

END