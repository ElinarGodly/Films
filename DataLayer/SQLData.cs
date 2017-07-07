using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using avItems = ApplicationVariables.ApplicationVariables.DataIDs.CSV_IDs;
using avCon = ApplicationVariables.ApplicationVariables.SystemSettings.SQLconnection;
using avQuery = ApplicationVariables.ApplicationVariables.SystemValues.SQLqueries;
using mcl = MovieClassLayer.MovieClasses;
using System.Data;

namespace MovieDataLayer
{
    public class SQLData
    {//-- TODO refactor code

        public SQLData() { }

        #region ReadFromDB
        public mcl.Films GetSQLData()
        {
            mcl.Films films = new mcl.Films();
            DataTable table = getFilmsTable();
            //films = dataTableToFilms(table);
            return films;
        }

        private DataTable getFilmsTable()
        {
            string connectionString = String.Format(avCon.connectionString, avCon.server, avCon.database, avCon.uid, avCon.password);
            using (MySqlDataAdapter da = new MySqlDataAdapter(avQuery.selectFilms, connectionString))
            {
                DataTable table = new DataTable();
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
                return table;
            }
        }

        //private mcl.Films dataTableToFilms(DataTable table)
        //{
        //    mcl.Films films = new mcl.Films();

        //    DataRow[] dr = table.Select();

        //    foreach (var row in dr)//TODO try with LINQ
        //    {
        //        mcl.Film film = new mcl.Film();
        //        if (films.Any(item => item.FilmID == row.ItemArray[avItems.FilmID].ToString()) == false)
        //            films.Add(getFilm(row));
        //        else
        //        {
        //            film = films.Find(item => item.FilmID == row.ItemArray[avItems.FilmID].ToString());
        //            addActor(film, row);
        //            addDirector(film, row);
        //        }
        //    }
        //    return films;
        //}

        //private mcl.Film getFilm(DataRow row) // TODO check if it can go into a shared space along with CSV bits
        //{
        //    mcl.Film film = new mcl.Film();

        //    film.FilmID = row.ItemArray[avItems.FilmID].ToString();
        //    film.FilmName = row.ItemArray[avItems.FilmName].ToString();
        //    film.FilmYear = row.ItemArray[avItems.FilmYear].ToString();
        //    film.ImdbRating = row.ItemArray[avItems.ImdbRating].ToString();

        //    addActor(film, row);
        //    addDirector(film, row);

        //    return film;
        //}

        //private void addActor(mcl.Film film, DataRow row)
        //{
        //    if (film.Actors.Any(item => item.PersonID == row.ItemArray[avItems.ActorID].ToString()) == false)
        //    {
        //        mcl.Actor actor = new mcl.Actor(row.ItemArray[avItems.ActorID].ToString()
        //                                        , row.ItemArray[avItems.ActorName].ToString());
        //        film.Actors.Add(actor);
        //    }
        //}

        //private void addDirector(mcl.Film film, DataRow row)
        //{
        //    if (film.Directors.Any(item => item.PersonID == row.ItemArray[avItems.DirectorID].ToString()) == false)
        //    {
        //        mcl.Director director = new mcl.Director(row.ItemArray[avItems.DirectorID].ToString()
        //                                                , row.ItemArray[avItems.DirectorName].ToString());
        //        film.Directors.Add(director);
        //    }
        //}
        #endregion

        #region UpdateDB
        public void UpdateCreateFilm(List<string> inputData)
        {
            MySqlConnection con = new MySqlConnection(String.Format(avCon.connectionString, avCon.server, avCon.database, avCon.uid, avCon.password));
            MySqlCommand cmd = new MySqlCommand(avQuery.updateFilms, con);

            cmd.CommandType = CommandType.StoredProcedure;

            foreach (var item in inputData)
                cmd.Parameters.Add(item);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void UpdateDBFromS3(mcl.Films films)
        {
            for (int i = 0; i < films.Count; i++)
            {
                if (films[i].Directors.Count > 1)
                {
                    UpdateCreateFilm(filmToString(films[i]));
                    films[i].Directors.RemoveAt(0);
                    i--;
                }
                else if (films[i].Actors.Count > 1)
                {
                    UpdateCreateFilm(filmToString(films[i]));
                    films[i].Actors.RemoveAt(0);
                    i--;
                }
                else
                    UpdateCreateFilm(filmToString(films[i]));
            }
        }

        private List<string> filmToString(mcl.Film film)//TODO get order from shared/dynamic method
        {
            List<string> fields = new List<string>
            {
                film.FilmID, film.FilmName, film.ImdbRating, film.Directors[0].PersonID, film.Directors[0].PersonName,
                film.Actors[0].PersonID, film.Actors[0].PersonName, film.FilmYear
            };

            return fields;
        }
        
#endregion
    }
}