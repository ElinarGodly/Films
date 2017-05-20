using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLMovies = ApplicationVariables.ApplicationVariables.DataIDs.Items_Movies;
using mcl = MovieClassLayer.MovieClasses;

namespace MovieDataLayer
{
    public class SQLData : IDisposable
    {//TODO refactor code
        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources if there are any.
        }
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public SQLData()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "filmdb";
            uid = "root";
            password = "blacklila";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                //switch (ex.Number)
                //{
                //    case 0:
                //        MessageBox.Show("Cannot connect to server.  Contact administrator");
                //        break;

                //    case 1045:
                //        MessageBox.Show("Invalid username/password, please try again");
                //        break;
                //}
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert()
        {
            string query = "INSERT INTO tableinfo (name, age) VALUES('John Smith', '33')";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update()
        {
            string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete()
        {
            string query = "DELETE FROM tableinfo WHERE name='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public mcl.Films Select()
        {
            string query = "CALL selectImdb();";
            mcl.Films films = new mcl.Films();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    if (films.Any(item => item.FilmID == dataReader[SQLMovies.FilmID].ToString()))
                    {
                        mcl.Film tmpFilm = films.Find(item => item.FilmID == dataReader[SQLMovies.FilmID].ToString());
                        if (tmpFilm.Directors.Any(item => item.PersonID == dataReader[SQLMovies.DirectorID].ToString()))
                        { }
                        else
                        {
                            mcl.Director director = getDirectorFromSQL(dataReader);
                            tmpFilm.Directors.Add(director);
                        }
                        if (tmpFilm.Actors.Any(item => item.PersonID == dataReader[SQLMovies.ActorID].ToString()))
                        { }
                        else
                        {
                            mcl.Actor actor = getActorFromSQL(dataReader);
                            tmpFilm.Actors.Add(actor);
                        }
                    }
                    else
                    {
                        mcl.Film film = getFilmFromSQL(dataReader);
                        films.Add(film);
                    }
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return films;
            }
            else
            {
                return films;
            }
        }

        private mcl.Director getDirectorFromSQL(MySqlDataReader dr)
        {
            mcl.Director director = new mcl.Director(dr[SQLMovies.DirectorID].ToString()
                                                    , dr[SQLMovies.DirectorName].ToString());
            return director;
        }

        private mcl.Actor getActorFromSQL(MySqlDataReader dr)
        {
            mcl.Actor actor = new mcl.Actor(dr[SQLMovies.ActorID].ToString()
                                            , dr[SQLMovies.ActorName].ToString());
            return actor;
        }

        private mcl.Film getFilmFromSQL(MySqlDataReader dr)
        {
            mcl.Director director = getDirectorFromSQL(dr);
            mcl.Actor actor = getActorFromSQL(dr);
            mcl.Film film = new mcl.Film(dr[SQLMovies.FilmID].ToString()
                                        , dr[SQLMovies.FilmName].ToString()
                                        , dr[SQLMovies.ImdbRating].ToString()
                                        , dr[SQLMovies.FilmYear].ToString());
            film.Directors.Add(director);
            film.Actors.Add(actor);
            return film;
        }

        //Count statement
        public int Count()
        {
            string query = "SELECT Count(*) FROM tableinfo";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        //Backup
        public void Backup()
        {
            try
            {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;
                int millisecond = Time.Millisecond;

                //Save file to C:\ with the current date as a filename
                string path;
                path = "C:\\MySqlBackup" + year + "-" + month + "-" + day +
            "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
                StreamWriter file = new StreamWriter(path);


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysqldump";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
            }
            catch (IOException ex)
            {
                //MessageBox.Show("Error , unable to backup!");
            }
        }

        //Restore
        public void Restore()
        {
            try
            {
                //Read file from C:\
                string path;
                path = "C:\\MySqlBackup.sql";
                StreamReader file = new StreamReader(path);
                string input = file.ReadToEnd();
                file.Close();

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysql";
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;


                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
            }
            catch (IOException ex)
            {
                //MessageBox.Show("Error , unable to Restore!");
            }
        }
    }
}
