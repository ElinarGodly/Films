using System;
using System.Collections.Generic;
using mcl = MovieClassLayer.MovieClasses;
using dl = MovieDataLayer;
using avSys = ApplicationVariables.ApplicationVariables.SystemSettings;

namespace MovieBusinessLayer
{
    public class MovieBusinessLayer
    {
        //--------------------------------------------------------------------- FILMS
        public mcl.Films GetFilms(int accessPoint)
        {
            switch (accessPoint)
            {
                case avSys.DataAccessPoint.CSV:
                    dl.CSVData dl1 = new dl.CSVData();
                    return dl1.GetCsvData();
                case avSys.DataAccessPoint.MySQL:
                    dl.SQLData dl2 = new dl.SQLData();
                    return dl2.GetSQLData();
                default:
                    dl1 = new dl.CSVData();
                    return dl1.GetCsvData();
            }
            //-- TODO: raise error if needed
        }

        public void UpdateFilmInDatabase(List<string> inputData)
        {
            dl.SQLData dl = new dl.SQLData();
            dl.UpdateCreateFilm(inputData);
        }

        public void UpdateCSVandRDB() //--TODO make it return message
        {
            dl.AWS_S3 dl1 = new dl.AWS_S3();
            dl1.UpdateAllFromS3();
        }

        public List<mcl.SimplisticFilm> GetDistinctSimplisticFilmsFromFilms(mcl.Films films)
        {
            return (films == null) ? null : films.ToListSimplisticFilm();
        }

        public mcl.Films GetFilmsSubset(string filmID, string directorID, string actorID, string filmYear,
                                        string imdbRating,string rottenRating, mcl.Films films)
        {
            return films.GetFilmsFilteredSubset(filmID, directorID, actorID, filmYear, imdbRating, rottenRating);
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

        //-----------------------------------------------------------------------RottenRating

        public List<string> GetDistinctRottenRatingFromFilms(mcl.Films films)
        {
            return (films == null) ? null : films.ToListDistinctRottenRating();
        }

        public List<string> GetDistinctRottenRating(mcl.Films films, string rottenRating)
        {
            return films.GetDistinctRottenRating(rottenRating);
        }
    }
}
