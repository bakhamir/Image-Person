using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ImgWeb.Models;
using System.Data.SqlClient;

namespace ImgWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        [HttpPost("/UploadImages")]
        public async Task<bool> UploadImages([FromBody] Models.Data data)
        {
            try
            {
                bool result = false;
                string connectionString = @"Data Source=207-3;Initial Catalog = ImageDB;Integrated Security=True";
                string query = $"insert into data values('{data.fromUrl}','{data.fromPc}')";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand(query, sqlConnection);
                    await cmd.ExecuteNonQueryAsync();
                    result = true;
                        
                }
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpGet("/DownloadImages")]
        public async Task<List<Data>> DownloadImages()
        {
            try
            {
                List<Data> list = new List<Data>();
                string connectionString = @"Data Source=207-3;Initial Catalog = ImageDB;Integrated Security=True";
                string query = $"select * from data";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand(query, sqlConnection);
                    SqlDataReader sqlDataReader = await cmd.ExecuteReaderAsync();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            Data data = new Data();
                            data.id = sqlDataReader.GetInt32(0);
                            data.fromUrl = sqlDataReader.GetString(1);
                            data.fromPc = sqlDataReader.GetString(2);
                            list.Add(data);
                        }
                    }

                }
                return list;

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
