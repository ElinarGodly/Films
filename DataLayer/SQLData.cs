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
            using (MySqlDataAdapter da = new MySqlDataAdapter(avQuery.selectFilms, connectionString))
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

        public bool UpdateFilmInDatabase(List<string> inputData)
        {
            bool success = true;

            inputData = new List<string>() {"0499540", "Avatar 2", "9.8", "0000116", "James Comeron", "0757855", "Zoe Saldana", "2009"};

            MySqlConnection con = new MySqlConnection(String.Format(avCon.connectionString, avCon.server, avCon.database, avCon.uid, avCon.password));
            MySqlCommand cmd = new MySqlCommand(avQuery.updateFilms, con);
            

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(@"fImdb", MySqlDbType.VarChar, 7).Value = inputData[0];
            cmd.Parameters.Add(@"fName", MySqlDbType.VarChar, 45).Value = inputData[1];
            cmd.Parameters.Add(@"fRate", MySqlDbType.Decimal, 2).Value = Decimal.Parse(inputData[2]);
            cmd.Parameters.Add(@"dImdb", MySqlDbType.VarChar, 7).Value = inputData[3];
            cmd.Parameters.Add(@"dName", MySqlDbType.VarChar, 45).Value = inputData[4];
            cmd.Parameters.Add(@"aImdb", MySqlDbType.VarChar, 7).Value = inputData[5];
            cmd.Parameters.Add(@"aName", MySqlDbType.VarChar, 45).Value = inputData[6];
            cmd.Parameters.Add(@"fYear", MySqlDbType.VarChar, 4).Value = inputData[7];

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return success;
        }

        public bool addFilmToDatabase(List<string> inputData)
        {
            bool success = true;

            return success;
        }

        //private bool isValid(List<string> inputData)
        //{
        //    bool success = true;

        //    if ((inputData[0].Length != 7) && (inputData[3].Length != 7)
        //        && (inputData[5].Length != 7) && (inputData[7].Length !=4))
        //        success = false;

        //    decimal rate = 0.0m;

        //    if(decimal.TryParse(inputData[2],out rate)



        //    //0 is imdbID = len 7
        //    //1 is Film Name
        //    //2 IMDB Rating = between 0.0 and 10.0
        //    //3 Director ID = len 7
        //    //4 Director
        //    //5 Actor ID = len 7
        //    //6 Actor
        //    //7 Year = len 4

        //    return success;
        //}
    }
}