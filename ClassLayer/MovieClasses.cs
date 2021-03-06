﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace MovieClassLayer
{
    public class MovieClasses
    {
        // ------------------------------------------------------------------------------------------------------------------- FILMS
        public class Films : List<Film> 
        {
            //--------------------------------------------------------------------- CONSTRUCTORS
            public Films() { }

            public Films(List<Film> films)
            {
                this.AddRange(films);
            }

            //--------------------------------------------------------------------- METHODS

            //----------------------------------------------------------- FILMS
            public Films GetFilmsFilteredSubset(string filmID, string directorID, string actorID, string filmYear, string imdbRating, string rottenRating)
            {
                var tmpFilms = this.Where(f => f.FilmID == ((filmID == null) ? f.FilmID : filmID))
                                    .Where(f => f.Directors.Any(p => p.PersonID == ((directorID == null) ? p.PersonID : directorID)))
                                    .Where(f => f.Actors.Any(p => p.PersonID == ((actorID == null) ? p.PersonID : actorID )))
                                    .Where(f => f.FilmYear == ((filmYear==null) ? f.FilmYear : filmYear))
                                    .Where(f => f.ImdbRating == ((imdbRating==null) ? f.ImdbRating : imdbRating))
                                    .Where(f => f.RottenRating == ((rottenRating==null) ? f.RottenRating : rottenRating))
                                    .OrderBy(f => f.FilmName)
                                    .ThenBy(f => f.FilmID)
                                    .ToList();
                return new Films(tmpFilms);
            }

            public List<SimplisticFilm> ToListSimplisticFilm()
            {
                return this.ToList<SimplisticFilm>().OrderBy(s => s.FilmName)
                                                    .ThenBy(s => s.FilmID)
                                                    .ToList();
            }

            public List<SimplisticFilm> GetDistinctSimplisticFilm(string filmID)
            {
                return this.Select(f => f.GetSimplisticFilm()).Where(f => f.FilmID == filmID)
                                                                .ToList();
            }

            public Film GetDistinctFilm(string filmID)
            {
                var tmp = this.Where(f => f.FilmID == filmID).ToList();
                return tmp[0];
            }
            
            //----------------------------------------------------------- DIRECTORS
            public List<Director> ToListDistinctDirector()
            {
                return this.SelectMany(p => p.Directors, (parent, child) => (Director)(child.GetPerson())).GroupBy(p => p.PersonID)
                                                                                                            .Select(grp => grp.First())
                                                                                                            .OrderBy(p => p.PersonName)
                                                                                                            .ThenBy(p => p.PersonID)
                                                                                                            .ToList();
            }

            public List<Director> GetDistinctDirector(string directorID)
            {
                return this.SelectMany(p => p.Directors, (parent, child) => (Director)(child.GetPerson())).Where(p => p.PersonID == directorID)
                                                                                                            .GroupBy(p => p.PersonID)
                                                                                                            .Select(grp => grp.First())
                                                                                                            .ToList();            
            }

            //----------------------------------------------------------- ACTORS
            public List<Actor> ToListDistinctActor()
            {
                return this.SelectMany(p => p.Actors, (parent, child) => (Actor)(child.GetPerson())).GroupBy(p => p.PersonID)
                                                                                                        .Select(grp => grp.First())
                                                                                                        .OrderBy(p => p.PersonName)
                                                                                                        .ThenBy(p => p.PersonID)
                                                                                                        .ToList();
            }

            public List<Actor> GetDistinctActor(string actorID)
            {
                return this.SelectMany(p => p.Actors, (parent, child) => (Actor)(child.GetPerson())).Where(p => p.PersonID == actorID)
                                                                                                        .GroupBy(p => p.PersonID)
                                                                                                        .Select(grp => grp.First())
                                                                                                        .ToList();
            }

            //--------------------------------------------------------------FilmYear
            public List<string> ToListDistinctFilmYear()
            {
                return this.Select(p => p.FilmYear).GroupBy(p => p).Select(p => p.First())
                                                                                        .OrderByDescending(p => p)
                                                                                        .ToList();
            }

            public List<string> GetDistinctFilmYear(string filmYear)  
            {
                return this.Select(p => p.FilmYear).Where(p => p == filmYear).GroupBy(p => p)
                                                                                           .Select(p => p.First())
                                                                                           .OrderByDescending(p => p)
                                                                                           .ToList();
            }

            //-----------------------------------------------------------------ImdbRating
            public List<string> ToListDistinctImdbRating()
            {
                return this.Select(p => p.ImdbRating).GroupBy(p => p).Select(p => p.First())
                                                                              .OrderByDescending(p => p)
                                                                              .ToList();
            }

            public List<string> GetDistinctImdbRating(string imdbRating)
            {
                return this.Select(p => p.ImdbRating).Where(p => p == imdbRating).GroupBy(p => p)
                                                                                           .Select(p => p.First())
                                                                                           .OrderByDescending(p => p)
                                                                                           .ToList();
            }

            //-----------------------------------------------------------------RottenRating
            public List<string> ToListDistinctRottenRating()
            {
                return this.Select(p => p.RottenRating).GroupBy(p => p).Select(p => p.First())
                                                                              .OrderByDescending(p => p)
                                                                              .ToList();
            }

            public List<string> GetDistinctRottenRating(string rottenRating)
            {
                return this.Select(p => p.RottenRating).Where(p => p == rottenRating).GroupBy(p => p)
                                                                                           .Select(p => p.First())
                                                                                           .OrderByDescending(p => p)
                                                                                           .ToList();
            }

        }

        // ------------------------------------------------------------------------------------------------------------------- FILM
        public class Film : SimplisticFilm
        {
            public string ImdbRating { get; set; }
            public string FilmYear { get; set; }
            public string RottenRating { get; set; }
            public List<Director> Directors { get; set; }
            public List<Actor> Actors { get; set; }

            public Film()
            {
                Directors = new List<Director>();
                Actors = new List<Actor>();
            }

            //------------------------------------------ CONSTRUCTORS
            public Film(string filmID, string filmName, string imdbRating, string filmYear, string rottenRating) : base(filmID, filmName)
            {
                ImdbRating = imdbRating;
                FilmYear = filmYear;
                RottenRating = rottenRating;
                Directors = new List<Director>();
                Actors = new List<Actor>();
            }

            public Film(string filmID, string filmName, string imdbRating, List<Director> directors, List<Actor> actors, string filmYear, string rottenRating)
                : base(filmID, filmName)
            {
                ImdbRating = imdbRating;
                FilmYear = filmYear;
                RottenRating = rottenRating;
                Directors = directors;
                Actors = actors;
            }

            public Film(Film film, List<Actor> actors, List<Director> directors)
            {
                FilmID = film.FilmID;
                FilmName = film.FilmName;
                ImdbRating = film.ImdbRating;
                FilmYear = film.FilmYear;
                RottenRating = film.RottenRating;
                Actors = actors;
                Directors = directors;                     
            }

            //------------------------------------------ METHODS

            public SimplisticFilm GetSimplisticFilm()
            {
                return this;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------- SIMPLISTIC FILM
        public class SimplisticFilm
        {
            public string FilmID { get; set; }
            public string FilmName { get; set; }

            //------------------------------------------ CONSTRUCTORS
            public SimplisticFilm() { }
            public SimplisticFilm(string filmID, string filmName)
            {
                FilmID = filmID;
                FilmName = filmName;
            }

            //------------------------------------------ METHODS

            public bool IsValid()
            {
                //-- valid if has ID and Name
                return (!(string.IsNullOrEmpty(this.FilmID)) && !(string.IsNullOrEmpty(this.FilmName)));
            }
        }

        // ------------------------------------------------------------------------------------------------------------------- DIRECTOR
        public class Director : Person
        {
            //------------------------------------------ CONSTRUCTORS
            public Director() { }
            public Director(string personID, string personName) : base(personID, personName)
            { }
        }

        // ------------------------------------------------------------------------------------------------------------------- ACTOR
        public class Actor : Person
        {
            //------------------------------------------ CONSTRUCTORS
            public Actor() { }
            public Actor(string personID, string personName) : base(personID, personName)
            { }
        }

        // ------------------------------------------------------------------------------------------------------------------- PERSON
        public class Person: IDisposable
        { 
            public string PersonID { get; set; }
            public string PersonName { get; set; }

            //------------------------------------------ CONSTRUCTORS
            public Person() { }
            public Person(string personID, string personName)
            {
                PersonID = personID;
                PersonName = personName;
            }


            //------------------------------------------ METHODS
            public Person GetPerson()
            {
                return this;
            }

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
        }
    }
}
