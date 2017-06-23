using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using av = ApplicationVariables.ApplicationVariables;
using cache = ApplicationVariables.ApplicationVariables.SystemSettings.Cache;
using avSV = ApplicationVariables.ApplicationVariables.SystemValues;
using mbl = MovieBusinessLayer.MovieBusinessLayer;
using mcl = MovieClassLayer.MovieClasses;


namespace WebMovies
{
    public partial class Default : SharedBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack && isFilteredPageLoad())
            {
                mbl bl1 = new mbl();

                string filmID = (DropDownListFilms.SelectedValue == av.SystemValues.DropDownLists.DefaultValue ? null : DropDownListFilms.SelectedValue);
                string directorID = (DropDownListDirectors.SelectedValue == av.SystemValues.DropDownLists.DefaultValue ? null : DropDownListDirectors.SelectedValue);
                string actorID = (DropDownListActors.SelectedValue == av.SystemValues.DropDownLists.DefaultValue ? null : DropDownListActors.SelectedValue);
                string filmYear = (DropDownListFilmYears.SelectedValue == av.SystemValues.DropDownLists.DefaultValue ? null :
                    DropDownListFilmYears.SelectedValue);
                string imdbRating = (DropDownListImdbRatings.SelectedValue == av.SystemValues.DropDownLists.DefaultValue ? null :
                    DropDownListImdbRatings.SelectedValue);

                populateDropDownsWithFilteredData(filmID, directorID, actorID, filmYear, imdbRating);

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
            populateDropDownList(addBlankItem, avSV.DropDownLists.Films.ControlID
                                                        , sFilms
                                                        , avSV.DropDownLists.Films.DataTextField
                                                        , avSV.DropDownLists.Films.DataValueField);
            populateDropDownList(addBlankItem, avSV.DropDownLists.Directors.ControlID
                                                        , directors
                                                        , avSV.DropDownLists.Directors.DataTextField
                                                        , avSV.DropDownLists.Directors.DataValueField);
            populateDropDownList(addBlankItem, avSV.DropDownLists.Actors.ControlID
                                                        , actors
                                                        , avSV.DropDownLists.Actors.DataTextField
                                                        , avSV.DropDownLists.Actors.DataValueField);
            populateDropDownList(addBlankItem, avSV.DropDownLists.FilmYears.ControlID, filmYears);
            populateDropDownList(addBlankItem, avSV.DropDownLists.ImdbRatings.ControlID, imdbRatings);
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
                mbl bl1 = new mbl();
                films = bl1.GetFilms(av.SystemSettings.DataAccessPoint.Current);
                if (cache.UseCache) Cache[cache.FilmCacheName] = films;
            }

            return films;
        }

        private void populateDropDownsWithOriginalData()
        {
            mbl bl1 = new mbl();

            mcl.Films films = getFilms();

            List<mcl.Director> directors = bl1.GetDistinctDirectorsFromFilms(films);
            List<mcl.Actor> actors = bl1.GetDistinctActorsFromFilms(films);
            List<mcl.SimplisticFilm> sFilms = bl1.GetDistinctSimplisticFilmsFromFilms(films);
            List<string> filmYears = bl1.GetDistinctFilmYearFromFilms(films);
            List<string> imdbRatings = bl1.GetDistinctImdbRatingFromFilms(films);

            populateDropDowns(avSV.DropDownLists.UseBlankItem, sFilms, directors, actors, filmYears, imdbRatings);

        }

        private void populateDropDownsWithFilteredData(string filmID, string directorID, string actorID, string filmYear
                                                                                                       , string imdbRating)
        {
            mcl.Films films = getFilms();
            mbl bl1 = new mbl();

            mcl.Films tmp = bl1.GetFilmsSubset(filmID, directorID, actorID, filmYear, imdbRating, films);

            List<mcl.Actor> actors = (actorID == null) ? bl1.GetDistinctActorsFromFilms(tmp) : bl1.GetDistinctActor(tmp, actorID);
            List<mcl.Director> directors = (directorID == null) ? bl1.GetDistinctDirectorsFromFilms(tmp) : bl1.GetDistinctDirector(tmp, directorID);
            List<mcl.SimplisticFilm> sFilms = (filmID == null) ? bl1.GetDistinctSimplisticFilmsFromFilms(tmp) : tmp.GetDistinctSimplisticFilm(filmID);
            List<string> filmYears = (filmYear == null) ? bl1.GetDistinctFilmYearFromFilms(tmp) :
              tmp.GetDistinctFilmYear(filmYear);
            List<string> imdbRatings = (imdbRating == null) ? bl1.GetDistinctImdbRatingFromFilms(tmp) :
              tmp.GetDistinctImdbRating(imdbRating);

            populateDropDowns(avSV.DropDownLists.UseBlankItem, sFilms, directors, actors, filmYears, imdbRatings);

            if (isSelectionComplete(sFilms, actors, directors))
            {
                mcl.Film film = new mcl.Film(tmp.GetDistinctFilm(sFilms[0].FilmID), actors, directors);
                selectionComplete(film);
            }
        }
        #region Table Population

        //--------------------------------------------------------------------------Table Population
        //-- TODO check if it works for more than 1 row. Maybe 
        private bool isSelectionComplete(List<mcl.SimplisticFilm> sFilms, List<mcl.Actor> actors, List<mcl.Director> directors)
        {
            return (sFilms.Count.Equals(1) && actors.Count.Equals(1) && directors.Count.Equals(1)) ? true : false;
        }

        private void selectionComplete(mcl.Film film)
        {
            CreateFilmsResultsTable(film);
            btnReset.Enabled = true;
        }

        private void CreateFilmsResultsTable(mcl.Film film)
        {
            TableRow row = new TableRow();
            Table table = (Table)Page.FindControl(avSV.TableValues.ResultsTable);
            CreateFilmResultsTableHeader();
            table.Rows.Add(CreateFilmResultRow(film));
            table.Visible = true;
        }

        private TableRow CreateFilmResultRow(mcl.Film film)
        {
            TableRow row = new TableRow();

            List<string> linkValues = new List<string> { avSV.TableValues.HyperLinkFilm, film.FilmID, film.FilmName };
            row.Cells.Add(CreateFilmInfoCell(avSV.TableValues.HyperLinkTemplate, linkValues));

            linkValues = new List<string> { avSV.TableValues.HyperLinkPerson, film.Actors[0].PersonName, film.Directors[0].PersonName };
            row.Cells.Add(CreateFilmInfoCell(avSV.TableValues.HyperLinkTemplate, linkValues));

            linkValues = new List<string> { avSV.TableValues.HyperLinkPerson, film.Actors[0].PersonID, film.Actors[0].PersonName };
            row.Cells.Add(CreateFilmInfoCell(avSV.TableValues.HyperLinkTemplate, linkValues));

            row.Cells.Add(CreateFilmInfoCell(film.ImdbRating, null));
            row.Cells.Add(CreateFilmInfoCell(film.FilmYear, null));

            return row;
        }

        private TableCell CreateFilmInfoCell(string filmText, List<string> linkValues)
        {
            TableCell cell = new TableCell();

            if (linkValues != null)
                cell.Controls.Add(CreateHyperLink(avSV.TableValues.HyperLinkTemplate, linkValues));
            else
                cell.Text = filmText;

            return cell;
        }

        private HyperLink CreateHyperLink(string template, List<string> values)
        {
            var link = new HyperLink();

            link.NavigateUrl = String.Format(template, values[0], values[1]);
            link.Text = values[2];
            link.Target = avSV.TableValues.newWindow;

            return link;
        }

        private void CreateFilmResultsTableHeader()
        {
            Table table = (Table)Page.FindControl(avSV.TableValues.ResultsTable);
            TableHeaderRow header = new TableHeaderRow();
            header.Font.Bold = true;

            for (int i = 0; i < avSV.TableValues.HeaderCells.Count; i++)
            {
                TableCell cell = new TableCell();
                if (avSV.TableValues.headerID.Equals(avSV.TableValues.HeaderCells[i].Remove(5, 1))) cell.CssClass = avSV.TableValues.headerID;
                cell.Text = avSV.TableValues.HeaderCells[i];
                header.Cells.Add(cell);
            }

            table.Rows.Add(header);
        }

        #endregion

        //--------------------------------------------------------------------- EVENTS

        protected void btnReset_Click(object sender, EventArgs e)
        {
            populateDropDownsWithOriginalData();
            Table table = (Table)Page.FindControl(avSV.TableValues.ResultsTable);
            table.Visible = false;
            btnReset.Enabled = false;
            for (int i = 0; i < table.Rows.Count; i++)
                table.Rows.RemoveAt(i);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            mbl bl = new mbl();
            List<string> inputData = new List<string>();
            bl.UpdateFilmInDatabase(inputData);
        }
    }
}