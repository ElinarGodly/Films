<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addFilm.aspx.cs" Inherits="WebMovies.addFilm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Film</title>
     <link rel="stylesheet" type="text/css" href="addFilm.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label AssociatedControlID="tb_FilmImdbID" ID="lbl_FilmImdb" runat="server">Film Imdb ID</asp:Label>
            <asp:TextBox ID="tb_FilmImdbID" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Label AssociatedControlID="tb_FilmName" ID="lbl_FilmName" runat="server">Film Name</asp:Label>
            <asp:TextBox ID="tb_FilmName" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Label AssociatedControlID="tb_FilmImdbRating" ID="lbl_FilmImdbRating" runat="server">Film Imdb Rating</asp:Label>
            <asp:TextBox ID="tb_FilmImdbRating" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Label AssociatedControlID="tb_DirectorImdbID" ID="lbl_DirectorImdbID" runat="server">Director Imdb ID</asp:Label>
            <asp:TextBox ID="tb_DirectorImdbID" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Label AssociatedControlID="tb_DirectorName" ID="lbl_DirectorName" runat="server">Director Name</asp:Label>
            <asp:TextBox ID="tb_DirectorName" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Label AssociatedControlID="tb_ActorImdbID" ID="lbl_ActorImdbID" runat="server">Actor Imdb ID</asp:Label>
            <asp:TextBox ID="tb_ActorImdbID" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Label AssociatedControlID="tb_ActorName" ID="lbl_ActorName" runat="server">Actor Name</asp:Label>
            <asp:TextBox ID="tb_ActorName" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Label AssociatedControlID="tb_FilmYear" ID="lbl_FilmYear" runat="server">Film Year</asp:Label>
            <asp:TextBox ID="tb_FilmYear" runat="server"></asp:TextBox>
        </div>
        <div>
            <a href="Default.aspx">Main Page</a>
            <asp:Button ID="btn_CreateUpdate" runat="server" OnClick="btn_CreateUpdate_Click" text="Create/Update"/>
        </div>
    </form>
</body>
</html>
