using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRT.ViewModels;
using CsQuery;
using System.Net;
using System.Text;
using System.IO;

namespace CRT
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlGeneral.DataSource = DataAccess.ExecuteDataTable("", "GetIdiomas", null);
                ddlGeneral.DataTextField = "Idioma";
                ddlGeneral.DataValueField = "ID";
                ddlGeneral.DataBind();

                ddlGeneral.Items.Insert(0, new ListItem("Seleccione un idioma...", "0"));
            }
        }

        protected void DownloadBtn_OnClick(Object sender, EventArgs e)
        {
            if (ddlGeneral.SelectedItem.Value != "0")
            {
                ReadGroups(ddlGeneral.SelectedItem.Value);
                ReadTrademarks(ddlGeneral.SelectedItem.Value);
                ReadAlias(ddlGeneral.SelectedItem.Value);
                ReadProducts(ddlGeneral.SelectedItem.Value);
            }

        }

        protected void Languages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            hdnLanguageSelected.Value = ddlGeneral.SelectedValue;
        }

        /// <summary>
        /// Get Groups information
        /// </summary>
        /// <param name="language"></param>
        private void ReadGroups(string language)
        {
            for (int idGroup = 1;
                idGroup < Convert.ToInt32(WebConfigurationManager.AppSettings.Get("TotalGroups"));
                idGroup++)
            {
                var result = GetInfo(String.Format(
                    WebConfigurationManager.AppSettings.Get("URLRequestGroup" + language), idGroup));
                if (result == "404")
                {
                    var undefined = new GroupVM
                    {
                        Id = idGroup.ToString(),
                        Name = string.Empty,
                        Modified = string.Empty
                    };

                    //TODO: Create Logging module in order to track the scraping
                    DataAccess.InsertGroup(language, "spInsertGroup", undefined);
                }
                else
                {
                    var dom = CQ.Create(result);
                    var group = new GroupVM
                    {
                        Id = idGroup.ToString(),
                        Name = dom[".span8 h1"].Text(),
                        Modified = HttpUtility.HtmlDecode(dom[".span8 p"].FirstOrDefault().InnerText)
                    };

                    //TODO: Create Logging module in order to track the scraping
                    DataAccess.InsertGroup(language, "spInsertGroup", group);
                }
                
            }
        }

        /// <summary>
        /// Get Trademarks information
        /// </summary>
        /// <param name="language"></param>
        private void ReadTrademarks(string language)
        {
            for (int idTrademark = 1;
                idTrademark < Convert.ToInt32(WebConfigurationManager.AppSettings.Get("TotalTrademarks"));
                idTrademark++)
            {
                var result = GetInfo(String.Format(
                    WebConfigurationManager.AppSettings.Get("URLRequestTrademark" + language), idTrademark));
                if (result=="404")
                {
                    var undefined = new BrandVM
                    {
                        Id = idTrademark.ToString(),
                        Name = string.Empty
                    };

                    DataAccess.InsertTrademark(language, "spInsertTrademark", undefined);
                }
                else
                {
                    var dom = CQ.Create(result);
                    var trademark = new BrandVM
                    {
                        Id = idTrademark.ToString(),
                        Name = dom[".span8 h1"].Text()
                    };

                    DataAccess.InsertTrademark(language, "spInsertTrademark", trademark);


                    var products = dom[".span8 li a"];
                    foreach (var product in products)
                    {
                        var productTrademark = new ProductTrademarkVM
                        {
                            IdMarca = idTrademark.ToString(),
                            IdMedicamento = product.Attributes["href"].Split(new [] {'/'}, StringSplitOptions.RemoveEmptyEntries)[1]
                        };

                        DataAccess.InsertProductTrademark(language, "spInsertProductTrademark", productTrademark);
                    }
                }
                
            }
        }

        /// <summary>
        /// Get Alias information
        /// </summary>
        /// <param name="language"></param>
        private void ReadAlias(string language)
        {
            for (int idAlias = 1;
                idAlias < Convert.ToInt32(WebConfigurationManager.AppSettings.Get("TotalAlias"));
                idAlias++)
            {
                var result = GetInfo(String.Format(
                    WebConfigurationManager.AppSettings.Get("URLRequestAlias" + language), idAlias));
                if (result == "404")
                {
                    var undefined = new AliasVM
                    {
                        Id = idAlias.ToString(),
                        Nombre = string.Empty,
                        ProductId = string.Empty
                    };

                    DataAccess.InsertAlias(language, "spInsertAlias", undefined);
                }
                else
                {
                    var dom = CQ.Create(result);
                    var alias = new AliasVM
                    {
                        Id = idAlias.ToString(),
                        Nombre = (String.IsNullOrEmpty(dom[".span8 h1"].Text())) ? dom[".span9 .span8 p"].Text().Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries)[0] : dom[".span8 h1"].Text(),
                        ProductId = dom[".col p a"].FirstOrDefault() == null ? "" : dom[".col p a"].FirstOrDefault().Attributes["href"].Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1]
                    };

                    DataAccess.InsertAlias(language, "spInsertAlias", alias);
                }
            }
        }

        /// <summary>
        /// Get Products information
        /// </summary>
        /// <param name="language"></param>
        private void ReadProducts(string language)
        {
            for (int idProduct = 1;
                idProduct < Convert.ToInt32(WebConfigurationManager.AppSettings.Get("TotalProducts"));
                idProduct++)
            {
                var result = GetInfo(String.Format(
                    WebConfigurationManager.AppSettings.Get("URLRequestProduct" + language), idProduct));
                if (result == "404")
                {
                    var undefined = new ProductVM
                    {
                        Id = idProduct.ToString(),
                        Name = string.Empty,
                        RiskId = string.Empty,
                        DescripcionRiesgo = string.Empty,
                        Modified = string.Empty,
                        Description = string.Empty,
                        IdGrupo = string.Empty
                    };

                    DataAccess.InsertProduct(language, "spInsertProduct", undefined);
                }
                else
                {
                    var dom = CQ.Create(result);
                    var product = new ProductVM
                    {
                        Id = idProduct.ToString(),
                        Name = dom[".span8 h1"].Text(),
                        RiskId = dom[".span4 h1"].Text(),
                        DescripcionRiesgo = dom[".span4 h4"].Text(),
                        Modified = HttpUtility.HtmlDecode(dom[".span8 p"].FirstOrDefault().InnerText),
                        Description = dom[".span9 .span8 p"].Text(),
                        IdGrupo = dom[".col ul li a"].LastOrDefault().Attributes["href"].Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1]
                    };

                    DataAccess.InsertProduct(language, "spInsertProduct", product);

                    var alternatives = dom[".span3 li a"];
                    foreach (var alternative in alternatives)
                    {
                        var productAlternative = new ProductAlternativeVM
                        {
                            ProductId = idProduct.ToString(),
                            ProductAlternativeId = alternative.Attributes["href"].Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1]
                        };

                        DataAccess.InsertProductAlternative(language, "spInsertProductAlternative", productAlternative);
                    }
                }
            }
        }

        //TODO: ALERTS MODULE USING JOB

        public string GetInfo(string url)
        {
            try
            {
                // Read web page HTML to byte array
                Byte[] PageHTMLBytes;
                if (url != "")
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.125 Safari/537.36";
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8,es;q=0.6,nl;q=0.4,pt;q=0.2");
                    request.Credentials = CredentialCache.DefaultCredentials;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream data = response.GetResponseStream();
                    PageHTMLBytes = ReadFully(data);

                    // Convert result from byte array to string
                    // and display it in TextBox txtPageHTML
                    UTF8Encoding oUTF8 = new UTF8Encoding();
                    return oUTF8.GetString(PageHTMLBytes);
                }
                else
                    return "";
            }
            catch (WebException ex)
            {
                HttpWebResponse errorResponse = ex.Response as HttpWebResponse;
                if (errorResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return "404";
                }
                throw;
            }
            
        }

        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}