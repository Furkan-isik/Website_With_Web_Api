using System;
using System.Net;
using System.Data.SqlClient;
using Dapper;
using sendMessage.Models;
using BCrypt.Net;
using System.Configuration;
using System.Web.Http;
using System.Net.Http;
using Serilog;

namespace sendMessage.Data
{
    public class UserController
    {

        private readonly string baglanti;

        public UserController(string connectionString)
        {
            baglanti = connectionString; 
        }
         
        public bool UserExists(string username)
        {
            try
            {
                using (var connection = new SqlConnection(baglanti))
                {
                    connection.Open();
                    var query = "SELECT COUNT(1) FROM Users WHERE Username=@Username";
                    return connection.ExecuteScalar<int>(query, new { Username = username }) > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("UserExists Hata: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);

                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Kontrol yapılırken hata oluştu."),
                        ReasonPhrase="Veritabanı Hatası."
                    });
            }
           
        }

        public void AddUser(SaveModel user)
        {
            try
            {
                using (var connection = new SqlConnection(baglanti))
                {
                    connection.Open();
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    var query = "INSERT INTO Users (Name, Username, Email, Password) VALUES (@Name, @Username, @Email, @Password)";
                    connection.Execute(query, new { user.Name, user.Username, user.Email, user.Password });
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine("AddUser Hata: " + ex.Message);

                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Veritabanı kayıt oluşturma hatası"),
                        ReasonPhrase = "Veritabanı Hatası."
                    });
            }
            
        }

        public SaveModel GetUser(string username)
        {
            try
            {
                using (var connection = new SqlConnection(baglanti))
                {
                    connection.Open();
                    var query = "SELECT * FROM Users WHERE Username = @Username";
                    return connection.QueryFirstOrDefault<SaveModel>(query, new { Username = username });
                }
            }
            catch (Exception ex)
            {  

                Console.WriteLine("GetUser Hata: " + ex.Message);

                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Veritabanı kayıt kontrol hatası"),
                        ReasonPhrase = "Veritabanı Hatası."
                    });
            }
            
        }

        public bool ValidatePassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }
    }
}