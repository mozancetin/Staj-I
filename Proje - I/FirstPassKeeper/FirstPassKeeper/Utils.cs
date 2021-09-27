using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstPassKeeper
{
    public static class Utils
    {
        public static string GetRandomPassword(int length = 12)
        {
            string[] alphabet = new string[24] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "X", "Y", "Z" };
            int[] numbers = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            string[] chars = new string[11] { ".", "-", "!", "?", "#", "$", "%", "&", "*", "+", "=" };

            int numberCount = (int)(length / 4);
            int charCount = (int)(length / 4);

            string password = "";
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {

                int choice = r.Next(1, 11);
                if (choice <= 3 && numberCount >= 1)
                {
                    password += numbers[r.Next(0, numbers.Length)].ToString();
                    numberCount -= 1;
                }
                else if (choice <= 5 && choice > 3 && charCount >= 1 && i != 0)
                {
                    password += chars[r.Next(0, chars.Length)].ToString();
                    charCount -= 1;
                }
                else
                {
                    if (r.Next(1, 11) <= 5)
                    {
                        password += alphabet[r.Next(0, alphabet.Length)].ToString().ToLower();
                    }
                    else
                    {
                        password += alphabet[r.Next(0, alphabet.Length)].ToString();
                    }
                }
            }
            return password;
        }

        // Caesar Cipher Algorithm From: https://www.c-sharpcorner.com/article/caesar-cipher-in-c-sharp/
        public static char Cipher(char ch, int key = 10)
        {
            if (!char.IsLetter(ch))
            {
                return ch;
            }
            if ((new char[11] { 'Ç', 'Ğ', 'İ', 'Ş', 'Ü', 'Ö', 'ç', 'ğ', 'ş', 'ü', 'ö' }).Contains(ch))
            {
                return ch;
            }
            char d = char.IsUpper(ch) ? 'A' : 'a';
            return (char)((((ch + key) - d) % 26) + d);
        }


        public static string Encrypt(string text, int key = 10)
        {
            string output = string.Empty;

            foreach (char ch in text)
                output += Cipher(ch, key);

            return output;
        }

        public static string Decrypt(string text, int key = 10)
        {
            return Encrypt(text, 26 - key);
        }

        public static void ExportAsJSON(string path)
        {
            List<string> users = new List<string>();
            AllData allData = new AllData();
            List<User> data = new List<User>();
            string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
            try
            {
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string getUserSql = "SELECT * FROM users";
                SqlCommand getUsers = new SqlCommand(getUserSql, connection);
                SqlDataReader reader = getUsers.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(reader.GetString(1));
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

            try
            {
                foreach (string username in users)
                {
                    User u = CreateUserObject(username);
                    if (u == null)
                    {
                        return;
                    }
                    data.Add(u);
                }

                allData.Users = data;
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(allData, options);
                File.WriteAllText(path, jsonString);
                MessageBox.Show("JSON dosyası başarıyla kaydedildi!");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        public static void ImportFromJSON(string path)
        {
            string jsonString = File.ReadAllText(path);
            AllData allData = JsonSerializer.Deserialize<AllData>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            try
            {
                string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string openUsers = string.Format("IF OBJECT_ID('users', 'U') IS NULL CREATE TABLE users (id INT NOT NULL IDENTITY(1,1) PRIMARY KEY, username nvarchar(255), password nvarchar(255))");
                SqlCommand openUsersCmd = new SqlCommand(openUsers, connection);
                openUsersCmd.ExecuteNonQuery();

                foreach (User userData in allData.Users)
                {

                    // Genel bilgiler
                    string registerSql = "INSERT INTO users (username, password) VALUES(@username, @password)";
                    SqlCommand registerCmd = new SqlCommand(registerSql, connection);
                    registerCmd.Parameters.AddWithValue("@username", userData.Username);
                    registerCmd.Parameters.AddWithValue("@password", userData.Password);
                    registerCmd.ExecuteNonQuery();

                    string openTable = string.Format("IF OBJECT_ID('{0}', 'U') IS NULL CREATE TABLE {0} (name nvarchar(255), password nvarchar(255), category nvarchar(255))", userData.Username);
                    SqlCommand openTableCmd = new SqlCommand(openTable, connection);
                    openTableCmd.ExecuteNonQuery();

                    string categoryTable = string.Format("IF OBJECT_ID('{0}C', 'U') IS NULL CREATE TABLE {0}C (categories nvarchar(255))", userData.Username);
                    SqlCommand categoryTableCmd = new SqlCommand(categoryTable, connection);
                    categoryTableCmd.ExecuteNonQuery();

                    // Kategoriler
                    foreach (string category in userData.Categories)
                    {
                        string addCategory = string.Format("INSERT INTO {0}C VALUES(@category)", userData.Username);
                        SqlCommand addCategoryCmd = new SqlCommand(addCategory, connection);
                        addCategoryCmd.Parameters.AddWithValue("@category", category);
                        addCategoryCmd.ExecuteNonQuery();
                    }

                    // Şifreler
                    foreach (List<string> password in userData.AllPasswords)
                    {
                        string name = password[0];
                        string pass = password[1];
                        string category = password[2];

                        string addPassword = string.Format("INSERT INTO {0} (name, password, category) VALUES(@name, @password, @category)", userData.Username);
                        SqlCommand addPasswordCmd = new SqlCommand(addPassword, connection);
                        addPasswordCmd.Parameters.AddWithValue("@name", name);
                        addPasswordCmd.Parameters.AddWithValue("@password", pass);
                        addPasswordCmd.Parameters.AddWithValue("@category", category);
                        addPasswordCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("İçeri aktarma işlemi başarıyla tamamlandı!");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        public static User CreateUserObject(string user)
        {
            string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
            try
            {
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                // Create a new User Object
                User u = new User { Username = user };

                // Get User Data From Table 'users'
                string userData = "SELECT * FROM users WHERE username = @username";
                SqlCommand userDataCmd = new SqlCommand(userData, connection);
                userDataCmd.Parameters.AddWithValue("@username", user);
                SqlDataReader reader = userDataCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    string password = reader.GetString(2);
                    u.Password = password;
                }
                else
                {
                    MessageBox.Show("Has no rows");
                }
                reader.Close();

                // Get User Categories
                string categoryData = string.Format("SELECT * FROM {0}C", user);
                SqlCommand categoryDataCmd = new SqlCommand(categoryData, connection);
                reader = categoryDataCmd.ExecuteReader();
                List<string> categories = new List<string>();
                while (reader.Read())
                {
                    categories.Add(reader.GetString(0));
                }

                u.Categories = categories;
                reader.Close();

                // Get User Passwords
                string passwordData = string.Format("SELECT * FROM {0}", user);
                SqlCommand passwordDataCmd = new SqlCommand(passwordData, connection);
                reader = passwordDataCmd.ExecuteReader();
                List<List<string>> pass = new List<List<string>>();
                while (reader.Read())
                {
                    List<string> collection = new List<string> { reader.GetString(0), reader.GetString(1), reader.GetString(2) };

                    pass.Add(collection);
                }

                u.AllPasswords = pass;
                reader.Close();
                return u;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                return null;
            }
        }
    }
}
