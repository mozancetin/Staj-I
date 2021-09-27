using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace PassKeeperEFWeb
{
    public static class WebUtils
    {
        public static CookieContainer cookies = new CookieContainer();
        public static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };

        public static JsonSerializerOptions optionsWithoutPreserve = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = null,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };
        public static Exception Login(string username, string password)
        {
            try
            {
                string data = "username=" + username + "&password=" + password;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:44315/login?" + data);
                request.Method = "GET";
                request.AllowAutoRedirect = true;
                request.CookieContainer = cookies;

                request.Credentials = CredentialCache.DefaultCredentials;

                WebResponse response = request.GetResponse();

                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(responseFromServer, options);
                    if (json.StatusCode == 200)
                    {
                        MessageBox.Show(json.Message);
                    }
                    else
                    {
                        return new Exception(json.Message);
                    }
                }
                response.Close();

                return null;
            }
            catch(Exception err)
            {
                return err;
            }
        }

        public static Exception SignIn(string username, string password)
        {
            try
            {
                using (var wb = new MyWebClient())
                {
                    wb.Cookies = cookies;
                    var data = new NameValueCollection();
                    data["username"] = username;
                    data["password"] = password;

                    var response = wb.UploadValues("https://localhost:44315/signin", "POST", data);
                    string responseString = Encoding.UTF8.GetString(response);

                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(responseString, options);
                    if (json.StatusCode != 200)
                    {
                        return new Exception(json.Message);
                    }

                    MessageBox.Show(json.Message);
                    return null;
                }
            }
            catch(Exception error)
            {
                return error;
            }
        }

        public static (List<Categories>, Exception) GetAllCategories(string like = null)
        {
            try
            {
                using (var wb = new MyWebClient())
                {
                    string url = "https://localhost:44315/mycategories";
                    if (like != null)
                    {
                        url += "?like=" + like;
                    }
                    wb.Cookies = cookies;
                    var response = wb.DownloadData(url);
                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(response, options);
                    if (json.StatusCode != 200)
                    {
                        return (new List<Categories>(), new Exception(json.Message));
                    }
                    List<Categories> categories = JsonSerializer.Deserialize<List<Categories>>(json.JsonData, options);
                    return (categories, null);
                }
            }
            catch (Exception error) 
            {
                return (new List<Categories>(), new Exception(error.Message));
            }
        }

        public static (List<Passwords>, Exception) GetAllPasswords(string categoryName = null, string like = null)
        {
            try
            {
                using (var wb = new MyWebClient())
                {
                    string url = "";
                    if (!string.IsNullOrWhiteSpace(categoryName) && !string.IsNullOrWhiteSpace(like))
                    {
                        url = "https://localhost:44315/mypasswords?";
                        url += "like=" + like;
                        url += "&categoryName=" + categoryName;
                    }
                    else if (!string.IsNullOrWhiteSpace(categoryName) || !string.IsNullOrWhiteSpace(like))
                    {
                        url = "https://localhost:44315/mypasswords?";
                        if (like != null)
                        {
                            url += "like=" + like;
                        }
                        else
                        {
                            url += "categoryName=" + categoryName;
                        }
                    }
                    else
                    {
                        url = "https://localhost:44315/mypasswords";
                    }

                    wb.Cookies = cookies;
                    var response = wb.DownloadData(url);
                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(response, options);
                    if (json.StatusCode != 200)
                    {
                        return (new List<Passwords>(), new Exception(json.Message));
                    }
                    List<Passwords> passwords = JsonSerializer.Deserialize<List<Passwords>>(json.JsonData, options);
                    return (passwords, null);
                }
            }
            catch (Exception error)
            {
                return (new List<Passwords>(), new Exception(error.Message));
            }
        }

        public static (Passwords, Exception) GetPassword(string PasswordName = null, int PasswordID = -1)
        {
            try
            {
                using (var wb = new MyWebClient())
                {
                    wb.Cookies = cookies;
                    string url = "https://localhost:44315/password";
                    if (PasswordName != null)
                    {
                        url += "?passwordName=" + PasswordName;
                    }
                    else if (PasswordName == null && PasswordID != -1)
                    {
                        url += "?passwordID=" + PasswordID;
                    }
                    else
                    {
                        return (new Passwords(), new Exception("Yeterince parametre girilmedi!"));
                    }

                    var response = wb.DownloadData(url);
                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(response, options);
                    if (json.StatusCode != 200)
                    {
                        return (new Passwords(), new Exception(json.Message));
                    }
                    Passwords pass = JsonSerializer.Deserialize<Passwords>(json.JsonData, options);
                    return (pass, null);
                }
            }
            catch (Exception error)
            {
                return (new Passwords(), new Exception(error.Message));
            }

        }

        public static (Categories, Exception) GetCategory(string categoryName = null, int categoryID = -1)
        {
            try
            {
                using (var wb = new MyWebClient())
                {
                    wb.Cookies = cookies;
                    string url = "https://localhost:44315/category";
                    if (categoryName != null)
                    {
                        url += "?categoryName=" + categoryName;
                    }
                    else if (categoryName == null && categoryID != -1)
                    {
                        url += "?categoryID=" + categoryID.ToString();
                    }
                    else
                    {
                        return (new Categories(), new Exception("Yeterince parametre girilmedi!"));
                    }
                    var response = wb.DownloadData(url);
                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(response, options);
                    if (json.StatusCode != 200)
                    {
                        return (new Categories(), new Exception(json.Message));
                    }
                    Categories category = JsonSerializer.Deserialize<Categories>(json.JsonData, optionsWithoutPreserve);
                    return (category, null);
                }
            }
            catch (Exception error)
            {
                return (new Categories(), new Exception(error.Message));
            }
        }

        public static Exception AddCategory(string name)
        {
            try
            {
                using (var wb = new MyWebClient())
                {
                    wb.Cookies = cookies;
                    var data = new NameValueCollection();
                    data["name"] = name;

                    var response = wb.UploadValues("https://localhost:44315/category", "POST", data);
                    string responseString = Encoding.UTF8.GetString(response);

                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(responseString, options);
                    if (json.StatusCode != 200)
                    {
                        return new Exception(json.Message);
                    }

                    MessageBox.Show(json.Message);
                    return null;
                }
            }
            catch (Exception error)
            {
                return error;
            }
        }

        public static Exception AddPassword(string name, string password, string categoryName = "Uncategorized")
        {
            try
            {
                using (var wb = new MyWebClient())
                {
                    wb.Cookies = cookies;
                    var data = new NameValueCollection();
                    data["name"] = name;
                    data["password"] = password;
                    data["categoryName"] = categoryName;

                    var response = wb.UploadValues("https://localhost:44315/password", "POST", data);
                    string responseString = Encoding.UTF8.GetString(response);

                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(responseString, options);
                    if (json.StatusCode != 200)
                    {
                        return new Exception(json.Message);
                    }

                    MessageBox.Show(json.Message);
                    return null;
                }
            }
            catch (Exception error)
            {
                return error;
            }
        }

        public static Exception UpdatePassword(string currentName, string newName = null, string newPassword = null, string newCategoryName = null)
        {
            try
            {
                using (var wb = new MyWebClient())
                {
                    wb.Cookies = cookies;
                    var data = new NameValueCollection();
                    data["currentName"] = currentName;
                    data["newName"] = newName;
                    data["newPassword"] = newPassword;
                    data["newCategoryName"] = newCategoryName;

                    var response = wb.UploadValues("https://localhost:44315/password", "PUT", data);
                    string responseString = Encoding.UTF8.GetString(response);

                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(responseString, options);
                    if (json.StatusCode != 200)
                    {
                        return new Exception(json.Message);
                    }

                    MessageBox.Show(json.Message);
                    return null;
                }
            }
            catch (Exception error)
            {
                return error;
            }
        }

        public static Exception DeleteCategory(string name)
        {
            try
            {
                using (var wb = new MyWebClient())
                {
                    wb.Cookies = cookies;
                    var data = new NameValueCollection();
                    data["name"] = name;

                    var response = wb.UploadValues("https://localhost:44315/category", "DELETE", data);
                    string responseString = Encoding.UTF8.GetString(response);

                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(responseString, options);
                    if (json.StatusCode != 200)
                    {
                        return new Exception(json.Message);
                    }

                    MessageBox.Show(json.Message);
                    return null;
                }
            }
            catch (Exception error)
            {
                return error;
            }
        }

        public static Exception UpdateCategory(string currentName, string newName)
        {
            try
            {
                using (var wb = new MyWebClient())
                {
                    wb.Cookies = cookies;
                    var data = new NameValueCollection();
                    data["currentName"] = currentName;
                    data["newName"] = newName;

                    var response = wb.UploadValues("https://localhost:44315/category", "PUT", data);
                    string responseString = Encoding.UTF8.GetString(response);

                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(responseString, options);
                    if (json.StatusCode != 200)
                    {
                        return new Exception(json.Message);
                    }

                    MessageBox.Show(json.Message);
                    return null;
                }
            }
            catch (Exception error)
            {
                return error;
            }
        }

        public static Exception DeletePassword(string name)
        {
            try
            {
                using (var wb = new MyWebClient())
                {
                    wb.Cookies = cookies;
                    var data = new NameValueCollection();
                    data["name"] = name;

                    var response = wb.UploadValues("https://localhost:44315/password", "DELETE", data);
                    string responseString = Encoding.UTF8.GetString(response);

                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(responseString, options);
                    if (json.StatusCode != 200)
                    {
                        return new Exception(json.Message);
                    }

                    MessageBox.Show(json.Message);
                    return null;
                }
            }
            catch (Exception error)
            {
                return error;
            }
        }

        public static Exception Import(string path)
        {
            try
            {
                string jsonString = File.ReadAllText(path);
                using (var wb = new MyWebClient())
                {
                    wb.Cookies = cookies;
                    var data = new NameValueCollection();
                    data["jsonString"] = jsonString;

                    var response = wb.UploadValues("https://localhost:44315/import", "POST", data);
                    string responseString = Encoding.UTF8.GetString(response);

                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(responseString, options);
                    if (json.StatusCode != 200)
                    {
                        return new Exception(json.Message);
                    }

                    MessageBox.Show(json.Message);
                    return null;
                }
            }
            catch (Exception error)
            {
                return error;
            }
        }

        public static Exception Export(string path)
        {
            try
            {
                using (var wb = new MyWebClient())
                {
                    wb.Cookies = cookies;
                    var response = wb.DownloadData("https://localhost:44315/export");
                    MyJsonFormatter json = JsonSerializer.Deserialize<MyJsonFormatter>(response, options);
                    if (json.StatusCode != 200)
                    {
                        return new Exception(json.Message);
                    }
                    string jsonString = json.JsonData;
                    File.WriteAllText(path, jsonString);

                    MessageBox.Show(json.Message);
                    return null;
                }
            }
            catch (Exception error)
            {
                return error;
            }
        }
    }
}
