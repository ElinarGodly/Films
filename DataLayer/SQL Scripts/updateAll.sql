DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `updateFilms`(
				IN film_Name varchar(50)
				, IN film_ImdbID varchar(50)
				, IN film_Rating varchar(50)
				, IN film_Year varchar(50)
				, IN actor_Name varchar(50)
				, IN actor_ImdbID varchar(50)
				, IN director_Name varchar(50)
				, IN director_ImdbID varchar(50)
)
BEGIN

DECLARE fID INT;

IF EXISTS (SELECT filmID from films where films.FilmImdbID = film_ImdbID) THEN
	UPDATE films SET FilmName = film_Name
					, ImdbRating = film_rating
					, FilmYear = film_year; 
	SET fID = filmID;
else	
	insert into films (FilmImdbID, FilmName, ImdbRating, FilmYear)
				values (film_ImdbID, film_Name, film_Rating, film_Year);
	SET fID = filmID;
END IF;

IF EXISTS (SELECT actorID from actors where actors.ActorImdbID = actor_ImdbID) THEN
	UPDATE actors SET ActorName = actor_Name;
else
	insert into directors (ActorImdbID, ActorName)
				values (actor_ImdbID, actor_Name);
	insert into film_actor (FilmID,ActorID)
						values(actorID, fID);
END IF;						

IF EXISTS (SELECT directorID from directors where directors.DirectorImdbID = director_ImdbID) THEN
	UPDATE directors SET directors.DirectorName = director_Name;
ELSE
	insert into directors (DirectorImdbID, DirectorName)
					values (director_ImdbID, director_Name);
	insert into film_director (FilmID, DirectorID)
					values (directorID, fID);			
END IF;

END$$
DELIMITER ;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `updateAll`(
				IN film_Name varchar(50)
				, IN film_ImdbID varchar(50)
				, IN film_Rating varchar(50)
				, IN film_Year varchar(50)
				, IN actor_Name varchar(50)
				, IN actor_ImdbID varchar(50)
				, IN director_Name varchar(50)
				, IN director_ImdbID varchar(50)
)
BEGIN

DECLARE fID INT;

IF EXISTS (SELECT filmID from films where films.FilmImdbID = film_ImdbID) THEN
	UPDATE films SET FilmName = film_Name
					, ImdbRating = film_rating
					, FilmYear = film_year; 
	SET fID = filmID;
else	
	insert into films (FilmImdbID, FilmName, ImdbRating, FilmYear)
						values (film_ImdbID, film_Name, film_Rating, film_Year);
	SET fID = filmID;
END IF;

IF EXISTS (SELECT actorID from actors where actors.ActorImdbID = actor_ImdbID) THEN
	UPDATE actors SET ActorName = actor_Name;
else
	insert into directors (ActorImdbID, ActorName)
				values (actor_ImdbID, actor_Name);
	insert into film_actor (FilmID,ActorID)
						values(actorID, fID);
END IF;						

IF EXISTS (SELECT directorID from directors where directors.DirectorImdbID = director_ImdbID) THEN
	UPDATE directors SET directors.DirectorName = director_Name;
ELSE
	insert into directors (DirectorImdbID, DirectorName)
					values (director_ImdbID, director_Name);
	insert into film_director (FilmID, DirectorID)
					values (directorID, fID);			
END IF;

END$$
DELIMITER ;

call updateAll(`Transformers`, `0418279`, `7.1`, `2007`,`Shia LaBeouf`, `0479471`, `Michael Bay`, `0000881`); 