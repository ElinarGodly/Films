using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using avd = ApplicationVariables.ApplicationVariables.SystemSettings.SQLconnection;

namespace ApplicationVariables
{
    public class ApplicationVariables
    {
        public ApplicationVariables()
        { }

        

        public struct SystemSettings
        {
            public struct Cache
            {
                public static bool UseCache = true;
                public static string FilmCacheName = @"Cache_Film";
            }

            public struct DataAccessPoint
            {
                public const int CSV = 0;
                public const int MySQL = 1;
                public const int Current = CSV;//the one picked in default.aspx.cs
            }

            public struct CsvPaths
            {
                //--for work
                public static string MoviesCSV = @"C:\Users\Novus\Desktop\Lari_C#\Project\Repos\Films\DataLayer\CSV\films.csv";
                //--for home
                //public static string MoviesCSV = @"D:\Programming\Repos\Films\DataLayer\CSV\films.csv";
            }

            public struct SQLconnection
            {
                public static string server = @"localhost";
                public static string database = @"filmdb";
                public static string uid = @"root";
                public static string password = @"";
                public static string connectionString =String.Format(@"SERVER={0}; DATABASE={1}; UID={2}; PASSWORD={3};"
                                                                        ,server,database,uid,password);
            }

            public struct AWS
            {
                public static string AccessKey = @"AKIAIJ4Q4573BWXABETQ";
                public static string SecretKey = @"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";
            }

        }

        public struct SystemValues
        {
            public struct S3_Storage
            {
                public struct LocalPaths
                {
                    public static string download = @"C:\Users\Novus\Desktop\Lari_C#\Project\Repos\Films\DataLayer\CSV\Download\films.csv";
                    public static string archive = @"C:\Users\Novus\Desktop\Lari_C#\Project\Repos\Films\DataLayer\CSV\Archive\films-{0}.csv";
                    public static string active = @"C:\Users\Novus\Desktop\Lari_C#\Project\Repos\Films\DataLayer\CSV\films.csv";
                }
                public struct S3Paths
                {
                    public static string bucket = "films-csvs";
                    public static string fileKey = "Other/ExtendedTestData.csv";
                }
                public static string timeFormat = "yyMMddHHmmss";
            }

            public struct Buttons
            {
                public static string BtnResetID_ToLower = "@btnreset";
            }

            public struct CheckBoxes
            {
                public static string DataPickSQL = @"cbDataPick";
            }

            public struct DropDownLists
            {
                public static string DefaultValue = @"NOT SELECTED";
                public static string DefaultText = @"<----- SELECT ----->";
                public static bool UseBlankItem = true;

                public struct Films
                {
                    public static string ControlID = @"DropDownListFilms";
                    public static string DataTextField = @"FilmName";
                    public static string DataValueField = @"FilmID";
                }

                public struct Directors
                {
                    public static string ControlID = @"DropDownListDirectors";
                    public static string DataTextField = @"PersonName";
                    public static string DataValueField = @"PersonID";
                }

                public struct Actors
                {
                    public static string ControlID = @"DropDownListActors";
                    public static string DataTextField = @"PersonName";
                    public static string DataValueField = @"PersonID";
                }

                public struct FilmYears
                {
                    public static string ControlID = @"DropDownListFilmYears";
                }

                public struct ImdbRatings
                {
                    public static string ControlID = @"DropDownListImdbRatings";
                }
            }

            public struct TableValues
            {
                public static string ResultsTable = @"ResultsTable";
                public static string HyperLinkTemplate = @"http://www.imdb.com/{0}{1}/";
                public static string HyperLinkFilm = @"title/tt";
                public static string HyperLinkPerson = @"name/nm";
                public static string newWindow = "_blank";
                public static List<string> HeaderCells = new List<string>
                                    { @"Film Name", @"Director Name", @"Actor Name", @"IMDb Rating", @"Film Year"};
                public static string headerID = @"ActorName";
            }

            public struct SQLqueries
            {
                public static string selectFilms = String.Format(@"{0}.selectAllFilms",avd.database);
                public static string updateFilms = String.Format(@"{0}.updateCreateFilmRecord",avd.database);
                public static string createFilms = String.Format(@"{0}.updateCreateFilmRecord", avd.database);
            }

            public static int firstItem = 0;
        }

        public struct DataIDs
        {
            public struct Items_Movies
            {
                public static int FilmID = 0;
                public static int FilmName = 1;
                public static int ImdbRating = 2;
                public static int FilmYear = 7;
                public static int DirectorID = 3;
                public static int DirectorName = 4;
                public static int ActorID = 5;
                public static int ActorName = 6;
            }
        }
    }

}
