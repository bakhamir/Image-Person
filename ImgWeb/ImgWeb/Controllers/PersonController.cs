using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ImgWeb.Models;
using System.Data.SqlClient;
namespace ImgWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpPost("/CreateNewUser")]
        public async Task<bool> CreateNewUser(string name,int age,
            string address,string photo,string role,int income)
        {
            try
            {
                string connectionString = @"Data Source=207-3;Initial Catalog = ImageDB;Integrated Security=True";
                string query = $"INSERT INTO PERSON VALUES('{name}',{age},'{address}','{photo}','{role}',{income})";
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand(query, sqlCon);
                    await cmd.ExecuteNonQueryAsync();
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
 
        }

        [HttpGet("/CountUsers")]
        public async Task<object> CountUsers()
        {
            try
            {
                string connectionString = @"Data Source=207-3;Initial Catalog = ImageDB;Integrated Security=True";
                string query = $"Select Count(*) from Person";
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand(query, sqlCon);
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception)
            {

                return 0;
            }

        }
        [HttpGet("/GetUserById")]
        public async Task<List<Person>> GetUserById(int id)
        {
            try
            {
                List<Person> list = new List<Person>();
                string connectionString = @"Data Source=207-3;Initial Catalog = ImageDB;Integrated Security=True";
                string query = $"Select * from person where id = {id}";
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand(query, sqlCon);
                    SqlDataReader sqlDataReader = await cmd.ExecuteReaderAsync();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            Person person = new Person();
                            person.Id = sqlDataReader.GetInt32(0);
                            person.name = sqlDataReader.GetString(1);
                            person.age = sqlDataReader.GetInt32(2);
                            person.address = sqlDataReader.GetString(3);
                            person.photo = sqlDataReader.GetString(4);
                            person.role = sqlDataReader.GetString(5);
                            person.income = sqlDataReader.GetInt32(6);
                            list.Add(person);
                        }
                    }
                    return list;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
