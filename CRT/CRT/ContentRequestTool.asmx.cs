using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Net;
using System.Web.Configuration;
using System.Data.SqlClient;
using ScrapySharp.Network;
using CRT.ViewModels;
using ScrapySharp.Extensions;
using System.Globalization;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace CRT
{
    /// <summary>
    /// Summary description for CRT
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CRT : System.Web.Services.WebService
    {
        [WebMethod]
        public string ReadAliasCount()
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter
            {
                ParameterName = "Type",
                Value = "Alias"
            });
            var dt = DataAccess.ExecuteDataTable("spGetTracking", parameters.ToArray());

            var result = "";
            foreach(DataRow row in dt.Rows)
            {
                result += "#" + row["Id"] + "|" + row["Type"] + "|" + row["Modified"] + "|" + row["LanguageId"];
            }

            return result;
        }

        [WebMethod]
        public string ReadGroupsCount()
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter
            {
                ParameterName = "Type",
                Value = "Group"
            });
            var dt = DataAccess.ExecuteDataTable("spGetTracking", parameters.ToArray());

            var result = "";
            foreach (DataRow row in dt.Rows)
            {
                result += "#" + row["Id"] + "|" + row["Type"] + "|" + row["Modified"] + "|" + row["LanguageId"];
            }

            return result;
        }

        [WebMethod]
        public string ReadBrandsCount()
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter
            {
                ParameterName = "Type",
                Value = "Brand"
            });
            var dt = DataAccess.ExecuteDataTable("spGetTracking", parameters.ToArray());

            var result = "";
            foreach (DataRow row in dt.Rows)
            {
                result += "#" + row["Id"] + "|" + row["Type"] + "|" + row["Modified"] + "|" + row["LanguageId"];
            }

            return result;
        }

        [WebMethod]
        public string ReadProductsCount()
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter
            {
                ParameterName = "Type",
                Value = "Product"
            });
            var dt = DataAccess.ExecuteDataTable("spGetTracking", parameters.ToArray());

            var result = "";
            foreach (DataRow row in dt.Rows)
            {
                result += "#" + row["Id"] + "|" + row["Type"] + "|" + row["Modified"] + "|" + row["LanguageId"];
            }

            return result;
        }

        /// <summary>
        /// Get Alias information
        /// </summary>
        /// <param name="language"></param>
        [WebMethod]        
        public void ReadAlias(string language)
        {
            for (int idAlias = 1;
                idAlias < Convert.ToInt32(WebConfigurationManager.AppSettings.Get("TotalAlias"));
                idAlias++)
            {
                var dom = GetInfo(String.Format(
                    WebConfigurationManager.AppSettings.Get("URLRequestAlias"), idAlias), language);

                if (dom == null || dom.RawResponse.StatusCode == 404)
                {
                    var parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter
                    {
                        ParameterName = "Id",
                        Value = idAlias
                    });
                    parameters.Add(new SqlParameter
                    {
                        ParameterName = "Type",
                        Value = "Alias"
                    });
                    parameters.Add(new SqlParameter
                    {
                        ParameterName = "LanguageId",
                        Value = language
                    });

                    DataAccess.InsertDataGeneral("spInsertNotFoundItem", parameters.ToArray());
                }
                else
                {
                    CultureInfo providerEN = CultureInfo.CreateSpecificCulture("en-US");
                    CultureInfo providerES = CultureInfo.CreateSpecificCulture("es-ES");

                    var modifiedDate = DateTime.Now;
                    if (language != WebConfigurationManager.AppSettings.Get("SpanishId"))
                        modifiedDate = DateTime.ParseExact(dom.Html.CssSelect(".span6 p").First().InnerText.Split(':')[1].Trim(), "MMMM d, yyyy", providerEN);
                    else
                        modifiedDate = DateTime.ParseExact(dom.Html.CssSelect(".span6 p").First().InnerText.Split(':')[1].Trim(), "d 'de' MMMM 'de' yyyy", providerES);

                    var alias = new AliasVM
                    {
                        Id = idAlias.ToString(),
                        Name = dom.Html.CssSelect(".span6 h1").First().InnerText,
                        LanguageId = language,
                        Modified = modifiedDate
                    };

                    var links = dom.Html.CssSelect(".col ul li a");

                    foreach (var link in links)
                    {
                        var href = link.Attributes["href"].ToString();
                        var hrefId = href.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[href.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Length - 1];
                        var parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter
                        {
                            ParameterName = "AliasId",
                            Value = idAlias
                        });

                        if (href.Contains("producto"))
                        {
                            alias.ProductId = hrefId;
                            parameters.Add(new SqlParameter
                            {
                                ParameterName = "ProductId",
                                Value = hrefId
                            });

                            if (!DataAccess.HasResults("spGetProductAlias", parameters.ToArray()))
                                DataAccess.InsertAlias(alias);
                            else
                                DataAccess.UpdateAlias(alias);
                        }
                    }
                }

                var param = new List<SqlParameter>();
                param.Add(new SqlParameter
                {
                    ParameterName = "Id",
                    Value = idAlias
                });
                param.Add(new SqlParameter
                {
                    ParameterName = "Type",
                    Value = "Alias"
                });

                var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                modified.Value = DateTime.Now;
                param.Add(modified);
                
                param.Add(new SqlParameter
                {
                    ParameterName = "LanguageId",
                    Value = language
                });

                DataAccess.InsertDataGeneral("spInsertTracking", param.ToArray());
            }
        }

        /// <summary>
        /// Get Trademarks information
        /// </summary>
        /// <param name="language"></param>
        [WebMethod]
        public void ReadBrands(string language)
        {
            for (int idBrand = 1;
                idBrand < Convert.ToInt32(WebConfigurationManager.AppSettings.Get("TotalTrademarks"));
                idBrand++)
            {
                var dom = GetInfo(String.Format(
                    WebConfigurationManager.AppSettings.Get("URLRequestTrademark"), idBrand), language);

                if (dom == null || dom.RawResponse.StatusCode == 404)
                {
                    var parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter
                    {
                        ParameterName = "Id",
                        Value = idBrand
                    });
                    parameters.Add(new SqlParameter
                    {
                        ParameterName = "Type",
                        Value = "Marca"
                    });
                    parameters.Add(new SqlParameter
                    {
                        ParameterName = "LanguageId",
                        Value = language
                    });

                    DataAccess.InsertDataGeneral("spInsertNotFoundItem", parameters.ToArray());
                }
                else
                {
                    CultureInfo providerEN = CultureInfo.CreateSpecificCulture("en-US");
                    CultureInfo providerES = CultureInfo.CreateSpecificCulture("es-ES");

                    var modifiedDate = DateTime.Now;
                    if (language != WebConfigurationManager.AppSettings.Get("SpanishId"))
                        modifiedDate = DateTime.ParseExact(dom.Html.CssSelect(".span6 p").First().InnerText.Split(':')[1].Trim(), "MMMM d, yyyy", providerEN);
                    else
                        modifiedDate = DateTime.ParseExact(dom.Html.CssSelect(".span6 p").First().InnerText.Split(':')[1].Trim(), "d 'de' MMMM 'de' yyyy", providerES);

                    var brand = new BrandVM
                    {
                        Id = idBrand.ToString(),
                        Name = dom.Html.CssSelect(".span6 h1").First().InnerText,
                        LanguageId = language,
                        Modified = modifiedDate
                    };

                    var parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter
                    {
                        ParameterName = "Id",
                        Value = idBrand
                    });

                    if (!DataAccess.HasResults("spGetBrand", parameters.ToArray()))
                        DataAccess.InsertBrand(brand);
                    else
                        DataAccess.UpdateBrand(brand);

                }

                var param = new List<SqlParameter>();
                param.Add(new SqlParameter
                {
                    ParameterName = "Id",
                    Value = idBrand
                });
                param.Add(new SqlParameter
                {
                    ParameterName = "Type",
                    Value = "Brand"
                });

                var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                modified.Value = DateTime.Now;
                param.Add(modified);

                param.Add(new SqlParameter
                {
                    ParameterName = "LanguageId",
                    Value = language
                });

                DataAccess.InsertDataGeneral("spInsertTracking", param.ToArray());
            }
        }

        /// <summary>
        /// Get Groups information
        /// </summary>
        /// <param name="language"></param>
        [WebMethod]
        public void ReadGroups(string language)
        {
            try
            {
                for (int idGroup = 1;
                    idGroup < Convert.ToInt32(WebConfigurationManager.AppSettings.Get("TotalGroups"));
                    idGroup++)
                {
                    var dom = GetInfo(String.Format(
                        WebConfigurationManager.AppSettings.Get("URLRequestGroup"), idGroup), language);

                    if (dom == null || dom.RawResponse.StatusCode == 404)
                    {
                        var parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter
                        {
                            ParameterName = "Id",
                            Value = idGroup
                        });
                        parameters.Add(new SqlParameter
                        {
                            ParameterName = "Type",
                            Value = "Grupo"
                        });
                        parameters.Add(new SqlParameter
                        {
                            ParameterName = "LanguageId",
                            Value = language
                        });

                        DataAccess.InsertDataGeneral("spInsertNotFoundItem", parameters.ToArray());
                    }
                    else
                    {
                        CultureInfo providerEN = CultureInfo.CreateSpecificCulture("en-US");
                        CultureInfo providerES = CultureInfo.CreateSpecificCulture("es-ES");

                        var modifiedDate = DateTime.Now;
                        if (language != WebConfigurationManager.AppSettings.Get("SpanishId"))
                            modifiedDate = DateTime.ParseExact(dom.Html.CssSelect(".span6 p").First().InnerText.Split(':')[1].Trim(), "MMMM d, yyyy", providerEN);
                        else
                            modifiedDate = DateTime.ParseExact(dom.Html.CssSelect(".span6 p").First().InnerText.Split(':')[1].Trim(), "d 'de' MMMM 'de' yyyy", providerES);

                        var group = new GroupVM
                        {
                            Id = idGroup.ToString(),
                            Name = dom.Html.CssSelect(".span6 h1").First().InnerText,
                            LanguageId = language,
                            Modified = modifiedDate
                        };

                        var parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter
                        {
                            ParameterName = "Id",
                            Value = idGroup
                        });

                        if (!DataAccess.HasResults("spGetGroup", parameters.ToArray()))
                            DataAccess.InsertGroup(group);
                        else
                            DataAccess.UpdateGroup(group);
                    }

                    var param = new List<SqlParameter>();
                    param.Add(new SqlParameter
                    {
                        ParameterName = "Id",
                        Value = idGroup
                    });
                    param.Add(new SqlParameter
                    {
                        ParameterName = "Type",
                        Value = "Group"
                    });

                    var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                    modified.Value = DateTime.Now;
                    param.Add(modified);

                    param.Add(new SqlParameter
                    {
                        ParameterName = "LanguageId",
                        Value = language
                    });

                    DataAccess.InsertDataGeneral("spInsertTracking", param.ToArray());
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Get Products information
        /// </summary>
        /// <param name="language"></param>
        [WebMethod]
        public void ReadProducts(string language)
        {
            for (int idProduct = 1;
                idProduct < Convert.ToInt32(WebConfigurationManager.AppSettings.Get("TotalProducts"));
                idProduct++)
            {
                var dom = GetInfo(String.Format(
                    WebConfigurationManager.AppSettings.Get("URLRequestProduct"), idProduct), language);

                if (dom == null || dom.RawResponse.StatusCode == 404)
                {
                    var parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter
                    {
                        ParameterName = "Id",
                        Value = idProduct
                    });
                    parameters.Add(new SqlParameter
                    {
                        ParameterName = "Type",
                        Value = "Producto"
                    });
                    parameters.Add(new SqlParameter
                    {
                        ParameterName = "LanguageId",
                        Value = language
                    });

                    DataAccess.InsertDataGeneral("spInsertNotFoundItem", parameters.ToArray());
                }
                else
                {
                    CultureInfo providerEN = CultureInfo.CreateSpecificCulture("en-US");
                    CultureInfo providerES = CultureInfo.CreateSpecificCulture("es-ES");

                    var modifiedDate = DateTime.Now;
                    if (language != WebConfigurationManager.AppSettings.Get("SpanishId"))
                        modifiedDate = DateTime.ParseExact(dom.Html.CssSelect(".span6 p").First().InnerText.Split(':')[1].Trim(), "MMMM d, yyyy", providerEN);
                    else
                        modifiedDate = DateTime.ParseExact(dom.Html.CssSelect(".span6 p").First().InnerText.Split(':')[1].Trim(), "d 'de' MMMM 'de' yyyy", providerES);

                    var product = new ProductVM
                    {
                        Id = idProduct.ToString(),
                        Name = dom.Html.CssSelect(".span6 h1").First().InnerText,
                        RiskId = DataAccess.GetRiskId(language, dom.Html.CssSelect(".span4 h1").First().InnerText),
                        LanguageId = language,
                        Modified = modifiedDate,
                        Description = HttpUtility.HtmlDecode(dom.Html.CssSelect(".span8 p").First().InnerText)
                    };

                    var parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter
                    {
                        ParameterName = "ProductId",
                        Value = idProduct
                    });

                    if (!DataAccess.HasResults("spGetProduct", parameters.ToArray()))
                        DataAccess.InsertProduct(product);
                    else
                        DataAccess.UpdateProduct(product);

                    var links = dom.Html.CssSelect(".col ul li a");

                    foreach (var link in links)
                    {
                        var href = link.Attributes["href"].Value;
                        var hrefId = href.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[href.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Length - 1];
                        parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter
                        {
                            ParameterName = "ProductId",
                            Value = idProduct
                        });

                        //if (href.Contains("sinonimo"))
                        //    DataAccess.InsertProductAlias();
                        if (href.Contains("grupo"))
                        {
                            parameters.Add(new SqlParameter
                            {
                                ParameterName = "GroupId",
                                Value = hrefId
                            });

                            if (!DataAccess.HasResults("spGetProductGroup", parameters.ToArray()))
                                DataAccess.InsertProductGroup(new ProductGroupVM
                                {
                                    GroupId = hrefId,
                                    ProductId = idProduct.ToString(),
                                    Modified = DateTime.Now,
                                    LanguageId = language
                                });
                        }
                        else if (href.Contains("marca"))
                        {
                            parameters.Add(new SqlParameter
                            {
                                ParameterName = "BrandId",
                                Value = hrefId
                            });

                            if (!DataAccess.HasResults("spGetProductBrand", parameters.ToArray()))
                                DataAccess.InsertProductBrand(new ProductBrandVM
                                {
                                    BrandId = hrefId,
                                    ProductId = idProduct.ToString(),
                                    Modified = DateTime.Now,
                                    LanguageId = language
                                });
                        }
                        else if (href.Contains("producto"))
                        {
                            parameters.Add(new SqlParameter
                            {
                                ParameterName = "ProductAlternativeId",
                                Value = hrefId
                            });

                            if (!DataAccess.HasResults("spGetProductAlternative", parameters.ToArray()))
                                DataAccess.InsertProductAlternative(new ProductAlternativeVM
                                {
                                    ProductAlternativeId = hrefId,
                                    ProductId = idProduct.ToString(),
                                    Modified = DateTime.Now,
                                    LanguageId = language
                                });
                        }
                    }
                }

                var param = new List<SqlParameter>();
                param.Add(new SqlParameter
                {
                    ParameterName = "Id",
                    Value = idProduct
                });
                param.Add(new SqlParameter
                {
                    ParameterName = "Type",
                    Value = "Product"
                });

                var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                modified.Value = DateTime.Now;
                param.Add(modified);
                
                param.Add(new SqlParameter
                {
                    ParameterName = "LanguageId",
                    Value = language
                });

                DataAccess.InsertDataGeneral("spInsertTracking", param.ToArray());
            }
        }

        public WebPage GetInfo(string url, string language)
        {
            try
            {                
                ScrapingBrowser browser = new ScrapingBrowser();
                browser.UserAgent = new FakeUserAgent("userAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
                browser.Headers.Add("Accept-Language", language == WebConfigurationManager.AppSettings.Get("SpanishId") ? "es-ES" : "en-US");
                browser.Headers.Add("Content-Language", language == WebConfigurationManager.AppSettings.Get("SpanishId") ? "es" : "en");
                browser.Encoding = Encoding.UTF8;
                browser.Timeout = TimeSpan.FromSeconds(5);

                var task = Task.Run(() => browser.NavigateToPage(new Uri(url)));
                try
                {
                    task.Wait();
                }
                catch (Exception ex)
                {
                    return null;
                }

                WebPage homePage = task.Result;
                return homePage;
            }
            catch (WebException ex)
            {
                return null;
            }
        }        
    }    
}
