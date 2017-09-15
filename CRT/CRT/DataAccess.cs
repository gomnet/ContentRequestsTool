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

namespace CRT
{
    public class DataAccess
    {
        public static DataTable ExecuteDataTable(string language, string storedProcedureName, params SqlParameter[] arrParam)
        {
            DataTable dt = new DataTable();

            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection" + language)))
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

        public static void InsertGroup(string language, string storedProcedureName, GroupVM group)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection" + language)))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("ID", group.Id));
                    cmd.Parameters.Add(new SqlParameter("Fecha", group.Modified));
                    cmd.Parameters.Add(new SqlParameter("Nombre", group.Name));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertTrademark(string language, string storedProcedureName, BrandVM trademark)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection" + language)))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("ID", trademark.Id));
                    cmd.Parameters.Add(new SqlParameter("Nombre", trademark.Name));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertAlias(string language, string storedProcedureName, AliasVM alias)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection" + language)))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("ID", alias.Id));
                    cmd.Parameters.Add(new SqlParameter("Nombre", alias.Nombre));
                    cmd.Parameters.Add(new SqlParameter("IdMedicamento", alias.ProductId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertProduct(string language, string storedProcedureName, ProductVM product)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection" + language)))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("ID", product.Id));
                    cmd.Parameters.Add(new SqlParameter("Nombre", product.Name));
                    cmd.Parameters.Add(new SqlParameter("Riesgo", product.RiskId));
                    cmd.Parameters.Add(new SqlParameter("DescripcionRiesgo", product.DescripcionRiesgo));
                    cmd.Parameters.Add(new SqlParameter("Fecha", product.Modified));
                    cmd.Parameters.Add(new SqlParameter("Comentario", product.Description));
                    cmd.Parameters.Add(new SqlParameter("IdGrupo", product.IdGrupo));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertProductAlternative(string language, string storedProcedureName, ProductAlternativeVM product)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection" + language)))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("IdMedicamento", product.ProductId));
                    cmd.Parameters.Add(new SqlParameter("IdAlternativaMedicamento", product.ProductAlternativeId));
                   
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertProductTrademark(string language, string storedProcedureName, ProductTrademarkVM product)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection" + language)))
            {
                cnn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    // Handle the parameters
                    cmd.Parameters.Add(new SqlParameter("IdMarca", product.IdMarca));
                    cmd.Parameters.Add(new SqlParameter("IdMedicamento", product.IdMedicamento));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertDataGeneral(string language, string storedProcedureName, params SqlParameter[] arrParam)
        {
            // Open the connection
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.AppSettings.Get("StringConnection" + language)))
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