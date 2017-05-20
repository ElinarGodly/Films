using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using av = ApplicationVariables.ApplicationVariables;
using avt = ApplicationVariables.ApplicationVariables.SystemValues.TableValues;
using cache = ApplicationVariables.ApplicationVariables.SystemSettings.Cache;
using ddl = ApplicationVariables.ApplicationVariables.SystemValues.DropDownLists;
using mbl = MovieBusinessLayer.MovieBusinessLayer;
using mcl = MovieClassLayer.MovieClasses;


namespace WebMovies
{
    public partial class Default : SharedBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var tmp = Page.Request.Params["__EVENTTARGET"];

            if (this.IsPostBack && isFilteredPageLoad())
            {
                using (mbl bl1 = new mbl())
                {
                    string filmID = (DropDownListFilms.SelectedValue == av.SystemValues.DropDownLists.DefaultValue ? null : DropDownListFilms.SelectedValue);
                    string directorID = (DropDownListDirectors.SelectedValue == av.SystemValues.DropDownLists.DefaultValue ? null : DropDownListDirectors.SelectedValue);
                    string actorID = (DropDownListActors.SelectedValue == av.SystemValues.DropDownLists.DefaultValue ? null : DropDownListActors.SelectedValue);
                    string filmYear = (DropDownListFilmYears.SelectedValue == av.SystemValues.DropDownLists.DefaultValue ? null :
                        DropDownListFilmYears.SelectedValue);
                    string imdbRating = (DropDownListImdbRatings.SelectedValue == av.SystemValues.DropDownLists.DefaultValue ? null :
                        DropDownListImdbRatings.SelectedValue);

                    populateDropDownsWithFilteredData(filmID, directorID, actorID, filmYear, imdbRating);
                }
            }
            else
            {
                populateDropDownsWithOriginalData();
            }
        }

        private bool isFilteredPageLoad()
        { 
            return (Page.Request.Params["__EVENTTARGET"].ToLower() != av.SystemValues.Buttons.BtnResetID_ToLower);
        }

        private void populateDropDowns(bool addBlankItem, List<mcl.SimplisticFilm> sFilms
                                                        , List<mcl.Director> directors
                                                        , List<mcl.Actor> actors
                                                        , List<string> filmYears
                                                        , List<string> imdbRatings)
        {
            populateDropDownList(addBlankItem, ddl.Films.ControlID
                                                        , sFilms
                                                        , ddl.Films.DataTextField
                                                        , ddl.Films.DataValueField);
            populateDropDownList(addBlankItem, ddl.Directors.ControlID
                                                        , directors
                                                        , ddl.Directors.DataTextField
                                                        , ddl.Directors.DataValueField);
            populateDropDownList(addBlankItem, ddl.Actors.ControlID
                                                        , actors
                                                        , ddl.Actors.DataTextField
                                                        , ddl.Actors.DataValueField);
            populateDropDownList(addBlankItem, ddl.FilmYears.ControlID, filmYears);
            populateDropDownList(addBlankItem, ddl.ImdbRatings.ControlID, imdbRatings);
        }

        private mcl.Films getFilms()
        {
            mcl.Films films = new mcl.Films();

            if ((cache.UseCache) && (Cache[cache.FilmCacheName] != null))
            {
                films = Cache[cache.FilmCacheName] as mcl.Films;
            }
            else
            {
                using (mbl bl1 = new mbl())
                {
                    films = bl1.GetFilms(av.CsvPaths.MoviesCSV);
                    if (cache.UseCache) Cache[cache.FilmCacheName] = films;
                }
            }

            return films;
        }

        private void populateDropDownsWithOriginalData()
        {
            using (mbl bl1 = new mbl())
            {
                mcl.Films films = getFilms();

                List<mcl.Director> directors = bl1.GetDistinctDirectorsFromFilms(films);
                List<mcl.Actor> actors = bl1.GetDistinctActorsFromFilms(films);
                List<mcl.SimplisticFilm> sFilms = bl1.GetDistinctSimplisticFilmsFromFilms(films);
                List<string> filmYears = bl1.GetDistinctFilmYearFromFilms(films);
                List<string> imdbRatings = bl1.GetDistinctImdbRatingFromFilms(films);
                                
                populateDropDowns(ddl.UseBlankItem, sFilms, directors, actors, filmYears, imdbRatings);
            }
        }

        private void populateDropDownsWithFilteredData(string filmID, string directorID, string actorID, string filmYear
                                                                                                       , string imdbRating)
        {
            mcl.Films films = getFilms();
            using (mbl bl1 = new mbl())
            {
                mcl.Films tmp = bl1.GetFilmsSubset(filmID, directorID, actorID, filmYear, imdbRating, films);

                List<mcl.Actor> actors = (actorID == null) ? bl1.GetDistinctActorsFromFilms(tmp) : bl1.GetDistinctActor(tmp, actorID);
                List<mcl.Director> directors = (directorID == null) ? bl1.GetDistinctDirectorsFromFilms(tmp) : bl1.GetDistinctDirector(tmp, directorID);
                List<mcl.SimplisticFilm> sFilms = (filmID == null) ? bl1.GetDistinctSimplisticFilmsFromFilms(tmp) : tmp.GetDistinctSimplisticFilm(filmID);
                List<string> filmYears = (filmYear == null) ? bl1.GetDistinctFilmYearFromFilms(tmp) :
                  tmp.GetDistinctFilmYear(filmYear);
                List<string> imdbRatings = (imdbRating == null) ? bl1.GetDistinctImdbRatingFromFilms(tmp) :
                  tmp.GetDistinctImdbRating(imdbRating);

                populateDropDowns(ddl.UseBlankItem, sFilms, directors, actors, filmYears, imdbRatings);

                if (isSelectionComplete(sFilms, actors, directors))
                {
                    mcl.Film film = new mcl.Film(tmp.GetDistinctFilm(sFilms[0].FilmID), actors, directors);
                    selectionComplete(film);
                }
            }
        }
        #region Table Population

        //--------------------------------------------------------------------------Table Population

        private bool isSelectionComplete(List<mcl.SimplisticFilm> sFilms, List<mcl.Actor> actors, List<mcl.Director> directors)
        {
            return (sFilms.Count.Equals(1) && actors.Count.Equals(1) && directors.Count.Equals(1)) ? true : false;
        }

        private void selectionComplete(mcl.Film film)
        {
            CreateFilmsResultsTable(film);

            btnReset.Enabled = true;
        }

        private void CreateFilmsResultsTable (mcl.Film film)
        {
            TableRow row = new TableRow();
            Table table = (Table)Page.FindControl(avt.ResultsTable);
            CreateFilmResultsTableHeader();
            table.Rows.Add(CreateFilmResultRow(film));
            table.Visible = true;
        }

        private TableRow CreateFilmResultRow(mcl.Film film)
        {
            TableRow row = new TableRow();

            List<string> linkValues = new List<string>{ avt.HyperLinkFilm, film.FilmID, film.FilmName };
            row.Cells.Add(CreateFilmInfoCell(avt.HyperLinkTemplate, linkValues));

            linkValues = new List<string>{ avt.HyperLinkPerson, film.Directors[0].PersonID, film.Directors[0].PersonName};
            row.Cells.Add(CreateFilmInfoCell(avt.HyperLinkTemplate, linkValues));

            linkValues = new List<string> { avt.HyperLinkPerson, film.Actors[0].PersonID, film.Actors[0].PersonName };
            row.Cells.Add(CreateFilmInfoCell(avt.HyperLinkTemplate, linkValues));

            row.Cells.Add(CreateFilmInfoCell(film.ImdbRating, null));
            row.Cells.Add(CreateFilmInfoCell(film.FilmYear, null));

            return row;
        }

        private TableCell CreateFilmInfoCell(string filmText, List<string> linkValues)
        {
            TableCell cell = new TableCell();
            
            if(linkValues != null)
                cell.Controls.Add(CreateHyperLink(avt.HyperLinkTemplate, linkValues));
            else
                cell.Text = filmText;
            
            return cell;
        }

        private HyperLink CreateHyperLink(string template, List<string> values)
        {
            var link = new HyperLink();
            
            link.NavigateUrl = String.Format(template, values[0], values[1]);
            link.Text = values[2];

            return link;
        }

        private void CreateFilmResultsTableHeader()
        {
            Table table = (Table)Page.FindControl(avt.ResultsTable);
            TableHeaderRow header = new TableHeaderRow();
            header.Font.Bold = true;

            for (int i = 0; i < avt.HeaderCells.Count; i++)
            {
                TableCell cell = new TableCell();
                if (avt.headerID.Equals(avt.HeaderCells[i].Remove(5,1))) cell.CssClass = avt.headerID;
                cell.Text = avt.HeaderCells[i];
                header.Cells.Add(cell);
            }
           
            table.Rows.Add(header);
        }

        #endregion

        //--------------------------------------------------------------------- EVENTS

        protected void btnReset_Click(object sender, EventArgs e)
        {
            populateDropDownsWithOriginalData();
            Table table = (Table) Page.FindControl(avt.ResultsTable);
            table.Visible = false;
            btnReset.Enabled = false;
            for(int i =0; i< table.Rows.Count;i++)
                table.Rows.RemoveAt(i);
        }
        
    }
}