using App.DbAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace App.DbAccess.Services
{
    public class ProductReport : IProductReport
    {
        private readonly string _connectionString;

        public ProductReport(string connectionString) => _connectionString = connectionString;

        public async Task<IEnumerable<ProductView>> GetProductsAsync(string request)
        {
            var lstProductView = new List<ProductView>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetProducts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@request", request));
                con.Open();
                SqlDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    var productView = new ProductView();
                    productView.Id = Convert.ToInt32(rdr["Id"]);
                    productView.Name = rdr["Name"].ToString();
                    productView.Cost = Convert.ToDecimal(rdr["Cost"]);
                    productView.BuyCount = Convert.ToInt32(rdr["BuyCount"]);

                    lstProductView.Add(productView);
                }
                con.Close();
            }
            return lstProductView;
        }
    }
}
