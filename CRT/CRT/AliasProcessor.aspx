<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductProcessor.aspx.cs" Inherits="CRT.ProductProcessor" %>

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
    <div>
        <h1 style="color: #2A5099; font-size: 2em; height: 55px; margin-bottom: 0; margin-left: auto;
            margin-right: auto; position: relative; text-decoration: underline; width: 425px;
            z-index: 1;">
            CONTENT REQUEST TOOL</h1>
        <div style="border-color: #2A5099; border-style: double; border-width: 3px; margin: 0 auto;
            min-height: 400px; position: relative; width: 960px; height:960px; z-index: 1; background: #ffffff url('/images/Alias.png') no-repeat right top;">
            <select id="languages" style="float: right; margin-right: 10px; margin-top: 10px;">
                <option value="#" selected="selected">Seleccione un idioma...</option>
                <option value="7C0AFE0E-0B25-4AEA-8AAE-51CBDDE1B134">es-ES</option>
                <option value="458AD052-C8AC-486B-A945-FB3A85219448">en-GB</option>
            </select>   
                     
            <img id="loading" alt="loading" src="/images/loading.gif" style="margin-left: 284px; margin-top: 493px;  position: absolute;  width: 100px; display:none;" />
            
            <img id="btnDetalle" data-method="ReadAlias" class="btnDetalle" style="margin-left: 263px; cursor:pointer;
                margin-top: 473px; width:150px; position: absolute;" alt="Pincha aquí para descargar la información detallada" src="/images/download.png" />                                           

            <input type="hidden" id="hdnLastId" />
        </div>
    </div>
</body>
</html>
