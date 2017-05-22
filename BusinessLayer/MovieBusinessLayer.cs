using System;
using System.Collections.Generic;
using mcl = MovieClassLayer.MovieClasses;
using csvD = MovieDataLayer.CSVData;
using sqlD = MovieDataLayer.SQLData;

namespace MovieBusinessLayer
{
    public class MovieBusinessLayer : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources if there are any.
        }

        //--------------------------------------------------------------------- FILMS
        public mcl.Films GetFilms(string csvPath)
        {
            sqlD dl1 = new sqlD();
            return dl1.GetSQLData();
            //-- TODO: raise error if needed
        }

        public List<mcl.SimplisticFilm> GetDistinctSimplisticFilmsFromFilms(mcl.Films films)
        {
            return (films == null) ? null : films.ToListSimplisticFilm();
        }

        public mcl.Films GetFilmsSubset(string filmID, string directorID, string actorID, string filmYear,string imdbRating, mcl.Films films)
        {
            return films.GetFilmsFilteredSubset(filmID, directorID, actorID, filmYear, imdbRating);
        }

        //--------------------------------------------------------------------- DIRECTORS
        public List<mcl.Director> GetDistinctDirectorsFromFilms(mcl.Films films)
        {
            return (films == null) ? null : films.ToListDistinctDirector();
        }

        public List<mcl.Director> GetDistinctDirector(mcl.Films films, string directorID)
        {
            return films.GetDistinctDirector(directorID);
        }

        //--------------------------------------------------------------------- ACTORS

        public List<mcl.Actor> GetDistinctActorsFromFilms(mcl.Films films)
        {
            return (films == null) ? null : films.ToListDistinctActor();
        }

        public List<mcl.Actor> GetDistinctActor(mcl.Films films, string actorID)
        {
            return films.GetDistinctActor(actorID);
        }

        //---------------------------------------------------------------------FilmYear

        public List<string> GetDistinctFilmYearFromFilms(mcl.Films films)
        {
            return (films == null) ? null : films.ToListDistinctFilmYear();
        }

        public List<string> GetDistinctFilmYear(mcl.Films films, string filmYear)
        {
            return films.GetDistinctFilmYear(filmYear);
        }

        //-----------------------------------------------------------------------ImdbRating

        public List<string> GetDistinctImdbRatingFromFilms(mcl.Films films)
        {
            return (films == null) ? null : films.ToListDistinctImdbRating();
        }

        public List<string> GetDistinctImdbRating(mcl.Films films, string imdbRating)
        {
            return films.GetDistinctImdbRating(imdbRating);
        }

    }
}
