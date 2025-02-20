using Backend.interfaces;
using MySql.Data.MySqlClient;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Backend.Services
{
    public class DatabaseManagement : IDbManagement
    {
        private string connectionString;
        public DatabaseManagement()
        {
            Initialize();
            connectionString = "Server=localhost;Database=ImageDisplaySystemDb;User ID=root;Password=Qazwsx123!@#;";
        }

        private void Initialize()
        {
            string createDatabaseSql = "CREATE DATABASE IF NOT EXISTS ImageDisplaySystemDb;";

            string createUsersTable = @"
                CREATE TABLE IF NOT EXISTS Users (
                    UserID INT AUTO_INCREMENT PRIMARY KEY,
                    Username VARCHAR(100) NOT NULL UNIQUE,
                    Password VARCHAR(255) NOT NULL
                );
            ";

            string createImagesTable = @"
                CREATE TABLE IF NOT EXISTS Images (
                    ImageID INT AUTO_INCREMENT PRIMARY KEY,
                    ImageURL VARCHAR(255) NOT NULL,
                    Description TEXT,
                    Tags VARCHAR(255)
                );
            ";

            string createReviewsTable = @"
                CREATE TABLE IF NOT EXISTS Reviews (
                    ReviewID INT AUTO_INCREMENT PRIMARY KEY,
                    ImageID INT NOT NULL,
                    Rating INT CHECK (Rating BETWEEN 1 AND 5),
                    Comment TEXT,
                    ReviewDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (ImageID) REFERENCES Images(ImageID) ON DELETE CASCADE
                );
            ";

            string connectionString = "Server=localhost;User ID=root;Password=Qazwsx123!@#;";
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new MySqlCommand(createDatabaseSql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Database 'ImageDisplaySystemDb' has been initialized.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erroe: {ex.Message}");
                }
            }

            connectionString = "Server=localhost;Database=ImageDisplaySystemDb;User ID=root;Password=Qazwsx123!@#;";
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new MySqlCommand(createUsersTable, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Users table has been initialized！");
                    }

                    using (var command = new MySqlCommand(createImagesTable, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Images table has been initialized！");
                    }

                    using (var command = new MySqlCommand(createReviewsTable, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Reviews table has been initialized！");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erroe: {ex.Message}");
                }
            }
        }

        public async Task<bool> QueryUserAsync(string username, string password)
        {
            //Users:
            //UserID Username Password
            string query = $"SELECT * FROM Users WHERE Username = '{username}'";
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return password == reader.GetString(reader.GetOrdinal("Password"));
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        public async Task<bool> RegisterUserAsync(string username, string password)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string checkUsernameQuery = $"SELECT COUNT(*) FROM Users WHERE Username = '{username}'";
                using (var command = new MySqlCommand(checkUsernameQuery, connection))
                {
                    var result = Convert.ToInt32(await command.ExecuteScalarAsync());
                    if (result > 0)
                    {
                        return false;
                    }
                }

                string insertQuery = $"INSERT INTO Users (Username, Password) VALUES ('{username}', '{password}')";
                using (var command = new MySqlCommand(insertQuery, connection))
                {

                    int rowsInserted = await command.ExecuteNonQueryAsync();
                    if (rowsInserted > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

        }

        public async Task<bool> UploadImageAsync(string fileName, string description, string tag)
        {
            //Images:
            // ImageURL VARCHAR(255) NOT NULL,Description TEXT,Tags VARCHAR(255)

            using (var connection = new MySqlConnection(connectionString))
            {
                var query = "INSERT INTO Images (ImageURL, Description, Tags) VALUES (@ImageURL, @Description, @Tags)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ImageURL", fileName);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@Tags", tag);

                    connection.Open();
                    var rowsInserted = await command.ExecuteNonQueryAsync();
                    if (rowsInserted > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
