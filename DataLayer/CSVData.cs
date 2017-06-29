using LumenWorks.Framework.IO.Csv;
using System;
using System.IO;
using System.Linq;
using csvMovies = ApplicationVariables.ApplicationVariables.DataIDs.Items_Movies;
using mcl = MovieClassLayer.MovieClasses;
using CsvPath = ApplicationVariables.ApplicationVariables.SystemSettings.CsvPaths;

namespace MovieDataLayer
{
    public class CSVData
    {

        public mcl.Films GetCsvData()
        {
            mcl.Films films = new mcl.Films();

            AWS_S3.DownloadLatestFile();

            using (CsvReader csv = new CsvReader(new StreamReader(CsvPath.MoviesCSV), true))
            {
                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    if (films.Any(item => item.FilmID == csv[csvMovies.FilmID]))
                    {
                        mcl.Film tmpFilm = films.Find(item => item.FilmID == csv[csvMovies.FilmID]);
                        if (tmpFilm.Directors.Any(item => item.PersonID == csv[csvMovies.DirectorID]))
                        { }
                        else
                        {
                            mcl.Director director = getDirectorFromCSV(csv);
                            tmpFilm.Directors.Add(director);
                        }
                        if (tmpFilm.Actors.Any(item => item.PersonID == csv[csvMovies.ActorID]))
                        { }
                        else
                        {
                            mcl.Actor actor = getActorFromCSV(csv);
                            tmpFilm.Actors.Add(actor);
                        }
                    }
                    else
                    {
                        mcl.Film film = getFilmFromCSV(csv);
                        films.Add(film);
                    }
                }
            }
            return films;
        }

        private mcl.Director getDirectorFromCSV(CsvReader csv)
        {
            mcl.Director director = new mcl.Director(csv[csvMovies.DirectorID]
                                                    , csv[csvMovies.DirectorName]);
            return director;
        }

        private mcl.Actor getActorFromCSV(CsvReader csv)
        {
            mcl.Actor actor = new mcl.Actor(csv[csvMovies.ActorID]
                                            , csv[csvMovies.ActorName]);
            return actor;
        }

        private mcl.Film getFilmFromCSV(CsvReader csv)
        {
            mcl.Director director = getDirectorFromCSV(csv);
            mcl.Actor actor = getActorFromCSV(csv);
            mcl.Film film = new mcl.Film(csv[csvMovies.FilmID]
                                        , csv[csvMovies.FilmName]
                                        , csv[csvMovies.ImdbRating]
                                        , csv[csvMovies.FilmYear]);
            film.Directors.Add(director);
            film.Actors.Add(actor);
            return film;
        }
    }
}