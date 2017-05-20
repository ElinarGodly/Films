using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ddl = ApplicationVariables.ApplicationVariables.SystemValues.DropDownLists;

namespace WebMovies
{
    public class SharedBase : Page
    {
        protected void populateDropDownList<T>(bool addBlankItem, string controlID, List<T> datasource, string dataTextField, string dataValueField)
        {
            DropDownList ddl = Page.FindControl(controlID) as DropDownList;
            ddl.DataTextField = dataTextField;
            ddl.DataValueField = dataValueField;
            ddl.DataSource = datasource;
            ddl.DataBind();
            if (datasource.Count > 1 && addBlankItem) { addBlankItemToDropDownList(ref ddl); }
        }

        protected void populateDropDownList<T>(bool addBlankItem, string controlID, List<T> datasource)
        {
            DropDownList ddl = Page.FindControl(controlID) as DropDownList;
            ddl.DataSource = datasource;
            ddl.DataBind();
            if (datasource.Count > 1 && addBlankItem) { addBlankItemToDropDownList(ref ddl); }
        }

        protected void addBlankItemToDropDownList(ref DropDownList ddlist)
        {
            ddlist.Items.Insert(0, new ListItem(ddl.DefaultText, ddl.DefaultValue));
            ddlist.SelectedIndex = 0;
        }

    }
}