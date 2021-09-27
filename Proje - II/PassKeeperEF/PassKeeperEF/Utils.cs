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

namespace PassKeeperEF
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
            try
            {
                (List<Users> users, Exception err) = DBUtils.GetAllUsers();
                if (err != null)
                {
                    MessageBox.Show(err.Message);
                    return;
                }

                (List<Passwords> myPasswords, Exception err2) = DBUtils.GetAllPasswords();
                if (err2 != null)
                {
                    MessageBox.Show(err2.Message);
                    return;
                }

                (List<Categories> categories, Exception err3) = DBUtils.GetAllCategories();
                if (err3 != null)
                {
                    MessageBox.Show(err3.Message);
                    return;
                }

                AllData allData = new AllData() 
                {
                    MyUsers = users,
                    MyPasswords = myPasswords,
                    MyCategories = categories
                };

                var options = new JsonSerializerOptions { 
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.Preserve,
                };

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
            AllData allData = JsonSerializer.Deserialize<AllData>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            
            try
            {
                
                Exception err = DBUtils.AddMultipleUsers(allData.MyUsers);
                if (err != null)
                {
                    MessageBox.Show(err.Message);
                    return;
                }

                MessageBox.Show("İçeri aktarma işlemi başarıyla tamamlandı!");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            
        }
    }
}
