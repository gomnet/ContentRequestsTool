<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CRT.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script src="http://code.jquery.com/jquery-1.6.1.min.js" type="text/javascript">
    </script>

    <script src="scripts/CRT.js" type="text/javascript">		
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1 style="color: #2A5099; font-size: 2em; height: 55px; margin-bottom: 0; margin-left: auto;
            margin-right: auto; position: relative; text-decoration: underline; width: 425px;
            z-index: 1;">
            CONTENT REQUEST TOOL</h1>
        <div style="border-color: #2A5099; border-style: double; border-width: 3px; margin: 0 auto;
            min-height: 400px; position: relative; width: 645px; z-index: 1;">
            <asp:DropDownList ID="ddlGeneral" AutoPostBack="true" runat="server" CssClass="ddlGeneral" onselectedindexchanged="Languages_SelectedIndexChanged" Style="float: right;
                margin-right: 10px; margin-top: 10px;">
            </asp:DropDownList>
            <asp:HiddenField runat="server" ID="hdnLanguageSelected"/>
            <asp:ImageButton ID="btnInsertCSV" runat="server" CssClass="btnInsertCSV" ImageUrl="~/images/csv.gif"
                Style="margin-left: 115px; margin-top: 33px; position: absolute;" />
            <asp:ImageButton ID="btnInsertSQL" runat="server" CssClass="btnInsertSQL" ImageUrl="~/images/sql.png"
                Style="margin-left: 290px; margin-top: 30px; position: absolute; width: 60px;" />
                <asp:ImageButton ID="btnInsertSP" runat="server" CssClass="btnInsertSP" ImageUrl="~/images/icon_sharepoint.jpg"
                Style="margin-left: 290px; margin-top: 30px; position: absolute; width: 60px;" />
            <asp:Image ID="lupa" runat="server" ImageUrl="~/images/Main.PNG" CssClass="lupa">
            </asp:Image>
            <asp:Image ID="personaje" runat="server" ImageUrl="~/images/personaje.PNG" Style="margin-left: -475px;
                margin-top: 94px; position: absolute;"></asp:Image>
            <asp:Image ID="loading" runat="server" ImageUrl="~/images/loading.gif" Style="margin-left: -435px;
                margin-top: 270px; position: absolute;"></asp:Image>
            <asp:ImageButton ID="btnListadoGeneral" runat="server" CssClass="btnListadoGeneral" Style="margin-left: -454px;
                margin-top: 264px; position: absolute; width: 90px;" ImageUrl="~/images/download.png"
                ToolTip="Pincha aquí para descargar la información" />
            <asp:ImageButton ID="btnDetalle" runat="server" CssClass="btnDetalle" Style="margin-left: -454px;
                margin-top: 264px; position: absolute; width: 90px;" ImageUrl="~/images/DownloadIcon.png"
                ToolTip="Pincha aquí para descargar la información detallada" OnClick="DownloadBtn_OnClick" />
        </div>
        <input type="hidden" class="downloadedContentGeneral" id="downloadedContentGeneral" />
        <input type="hidden" class="downloadedContent" id="downloadedContent" />
    </div>
    </form>
</body>
</html>
