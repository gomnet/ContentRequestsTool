using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using CRT.ViewModels;
using System.Collections.Generic;

namespace CRT
{
    public class DataAccess
    {
        public static bool HasResults(string storedProcedureName, params SqlParameter[] arrParam)
        {
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    // Handle the parameters
                    if (arrParam != null)
                    {
                        foreach (SqlParameter param in arrParam)
                            cmd.Parameters.Add(param);
                    }

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        return dataReader.HasRows;
                    }

                }
            }
        }

        public static DataTable ExecuteDataTable(string storedProcedureName, params SqlParameter[] arrParam)
        {
            DataTable dt = new DataTable();

            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    // Handle the parameters
                    if (arrParam != null)
                    {
                        foreach (SqlParameter param in arrParam)
                            cmd.Parameters.Add(param);
                    }

                    // Define the data adapter and fill the dataset
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static string GetRiskId(string languageId, string riskName)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    ParameterName = "RiskName",
                    Value = riskName
                },
                new SqlParameter
                {
                    ParameterName = "LanguageId",
                    Value = languageId
                }
            };

            var table = ExecuteDataTable("spGetRiskId", parameters.ToArray());

            if (table.Rows.Count > 0)
                return table.Rows[0][0].ToString();
            else
                return "";
        }

        public static void InsertGroup(GroupVM group)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spInsertGroup";

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("Id", group.Id));
                    var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                    modified.Value = group.Modified;

                    cmd.Parameters.Add(modified);
                    cmd.Parameters.Add(new SqlParameter("Name", group.Name));
                    cmd.Parameters.Add(new SqlParameter("LanguageId", group.LanguageId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertBrand(BrandVM trademark)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spInsertBrand";

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("Id", trademark.Id));
                    var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                    modified.Value = trademark.Modified;

                    cmd.Parameters.Add(modified);
                    cmd.Parameters.Add(new SqlParameter("Name", trademark.Name));
                    cmd.Parameters.Add(new SqlParameter("LanguageId", trademark.LanguageId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertAlias(AliasVM alias)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spInsertAlias";

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("ID", alias.Id));
                    var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                    modified.Value = alias.Modified;

                    cmd.Parameters.Add(modified);
                    cmd.Parameters.Add(new SqlParameter("ProductId", alias.ProductId));
                    cmd.Parameters.Add(new SqlParameter("Name", alias.Name));
                    cmd.Parameters.Add(new SqlParameter("LanguageId", alias.LanguageId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertProduct(ProductVM product)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spInsertProduct";

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("Id", product.Id));
                    var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                    modified.Value = product.Modified;

                    cmd.Parameters.Add(modified);
                    cmd.Parameters.Add(new SqlParameter("RiskId", product.RiskId));
                    cmd.Parameters.Add(new SqlParameter("Name", product.Name));
                    cmd.Parameters.Add(new SqlParameter("Description", product.Description));
                    cmd.Parameters.Add(new SqlParameter("LanguageId", product.LanguageId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertProductAlternative(ProductAlternativeVM product)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spInsertProductAlternative";

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("ProductId", product.ProductId));
                    cmd.Parameters.Add(new SqlParameter("ProductAlternativeId", product.ProductAlternativeId));
                    cmd.Parameters.Add(new SqlParameter("LanguageId", product.LanguageId));
                    var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                    modified.Value = product.Modified;

                    cmd.Parameters.Add(modified);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertProductBrand(ProductBrandVM product)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spInsertProductBrand";

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("ProductId", product.ProductId));
                    cmd.Parameters.Add(new SqlParameter("BrandId", product.BrandId));
                    cmd.Parameters.Add(new SqlParameter("LanguageId", product.LanguageId));
                    var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                    modified.Value = product.Modified;

                    cmd.Parameters.Add(modified);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void InsertProductGroup(ProductGroupVM product)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spInsertProductGroup";

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("ProductId", product.ProductId));
                    cmd.Parameters.Add(new SqlParameter("GroupId", product.GroupId));
                    cmd.Parameters.Add(new SqlParameter("LanguageId", product.LanguageId));
                    var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                    modified.Value = product.Modified;

                    cmd.Parameters.Add(modified);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateAlias(AliasVM alias)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateAlias";

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("AliasId", alias.Id));
                    var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                    modified.Value = alias.Modified;

                    cmd.Parameters.Add(modified);
                    cmd.Parameters.Add(new SqlParameter("Name", alias.Name));
                    cmd.Parameters.Add(new SqlParameter("LanguageId", alias.LanguageId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateBrand(BrandVM brand)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateBrand";

                    // Handle the parameters
                    var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                    modified.Value = brand.Modified;

                    cmd.Parameters.Add(modified);
                    cmd.Parameters.Add(new SqlParameter("BrandId", brand.Id));
                    cmd.Parameters.Add(new SqlParameter("LanguageId", brand.LanguageId));
                    cmd.Parameters.Add(new SqlParameter("Name", brand.Name));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateGroup(GroupVM group)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateGroup";

                    // Handle the parameters
                    var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                    modified.Value = group.Modified;

                    cmd.Parameters.Add(modified);
                    cmd.Parameters.Add(new SqlParameter("GroupId", group.Id));
                    cmd.Parameters.Add(new SqlParameter("LanguageId", group.LanguageId));
                    cmd.Parameters.Add(new SqlParameter("Name", group.Name));

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static void UpdateProduct(ProductVM product)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateProduct";

                    // Handle the parameters
                    var modified = new SqlParameter("Modified", SqlDbType.DateTime);
                    modified.Value = product.Modified;

                    cmd.Parameters.Add(modified);
                    cmd.Parameters.Add(new SqlParameter("ProductId", product.Id));
                    cmd.Parameters.Add(new SqlParameter("RiskId", product.RiskId));
                    cmd.Parameters.Add(new SqlParameter("Name", product.Name));
                    cmd.Parameters.Add(new SqlParameter("Description", product.Description));
                    cmd.Parameters.Add(new SqlParameter("LanguageId", product.LanguageId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertDataGeneral(string storedProcedureName, params SqlParameter[] arrParam)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    // Handle the parameters
                    if (arrParam != null)
                    {
                        foreach (SqlParameter param in arrParam)
                            cmd.Parameters.Add(param);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateDataGeneral(string storedProcedureName, params SqlParameter[] arrParam)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection")))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    // Handle the parameters
                    if (arrParam != null)
                    {
                        foreach (SqlParameter param in arrParam)
                            cmd.Parameters.Add(param);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}