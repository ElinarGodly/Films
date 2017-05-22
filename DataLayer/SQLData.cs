using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using avItems = ApplicationVariables.ApplicationVariables.DataIDs.Items_Movies;
using avCon = ApplicationVariables.ApplicationVariables.SystemSettings.SQLconnection;
using avQuery = ApplicationVariables.ApplicationVariables.SystemValues.SQLqueries;
using mcl = MovieClassLayer.MovieClasses;
using System.Data;

namespace MovieDataLayer
{
    public class SQLData
    {//-- TODO refactor code

        public SQLData() { }

        public mcl.Films GetSQLData()
        {
            mcl.Films films = new mcl.Films();
            DataTable table = getFilmsTable();
            films = dataTableToFilms(table);

            return films;
        }

        private DataTable getFilmsTable()
        {
            string connectionString = String.Format(avCon.connectionString, avCon.server, avCon.database, avCon.uid, avCon.password);
            using (MySqlDataAdapter da = new MySqlDataAdapter(avQuery.allFilms, connectionString))
            {
                DataTable table = new DataTable();
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
                return table;
            }
        }

        private mcl.Films dataTableToFilms(DataTable table)
        {
            mcl.Films films = new mcl.Films();

            DataRow[] dr = table.Select();

            foreach (var row in dr)//TODO try with LINQ
            {
                mcl.Film film = new mcl.Film();
                if (films.Any(item => item.FilmID == row.ItemArray[avItems.FilmID].ToString()) == false)
                {
                    films.Add(getFilm(row));
                }
                else
                {
                    film = films.Find(item => item.FilmID == row.ItemArray[avItems.FilmID].ToString());
                    addActor(film, row);
                    addDirector(film, row);
                }
            }
            return films;
        }

        private mcl.Film getFilm(DataRow row)
        {
            mcl.Film film = new mcl.Film();

            film.FilmID = row.ItemArray[avItems.FilmID].ToString();
            film.FilmName = row.ItemArray[avItems.FilmName].ToString();
            film.FilmYear = row.ItemArray[avItems.FilmYear].ToString();
            film.ImdbRating = row.ItemArray[avItems.ImdbRating].ToString();

            addActor(film, row);
            addDirector(film, row);

            return film;
        }

        private void addActor(mcl.Film film, DataRow row)
        {
            if (film.Actors.Any(item => item.PersonID == row.ItemArray[avItems.ActorID].ToString()) == false)
            {
                mcl.Actor actor = new mcl.Actor(row.ItemArray[avItems.ActorID].ToString()
                                                , row.ItemArray[avItems.ActorName].ToString());
                film.Actors.Add(actor);
            }
        }

        private void addDirector(mcl.Film film, DataRow row)
        {
            if (film.Directors.Any(item => item.PersonID == row.ItemArray[avItems.DirectorID].ToString()) == false)
            {
                mcl.Director director = new mcl.Director(row.ItemArray[avItems.DirectorID].ToString()
                                                        , row.ItemArray[avItems.DirectorName].ToString());
                film.Directors.Add(director);
            }
        }

    }
}