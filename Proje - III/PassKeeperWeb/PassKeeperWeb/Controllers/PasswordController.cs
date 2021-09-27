using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PassKeeperWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace PassKeeperWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PasswordController : ControllerBase
    {
        public JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };

        [HttpGet("/password")]
        public string Get([FromQuery] string passwordName = null, [FromQuery] int passwordID = -1)
        {
            (Password pass, Exception err) = (new Password(), new Exception());
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Önce giriş yapmalısın.", StatusCode = 400 }, options);
            }
            else
            {
                if (passwordName == null && passwordID == -1)
                {
                    return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Yeterli parametre girilmedi!", StatusCode = 400 }, options);
                }

                (User _user, Exception error) = DBUtils.GetUserByUsername(HttpContext.Session.GetString("username"));
                if (error != null)
                {
                    return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Bir hata oluştu. (Kullanıcı alınamadı.)", StatusCode = 404 }, options);
                }
                if (passwordName != null)
                {
                    (pass, err) = DBUtils.GetPasswordByName(passwordName, _user.UserId);
                    if (err != null)
                    {
                        return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Bir hata oluştu! (Böyle bir şifre olmayabilir.)", StatusCode = 404 }, options);
                    }
                }
                else
                {
                    (pass, err) = DBUtils.GetPasswordByID(passwordID);
                    if (err != null)
                    {
                        return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Bir hata oluştu! (Böyle bir şifre olmayabilir.)", StatusCode = 404 }, options);
                    }
                }
                
                MyPasswords passToGo = new MyPasswords() 
                { 
                    CategoryID = pass.CategoryId,
                    name = pass.Name,
                    password = pass.Password1,
                    PasswordID = pass.PasswordId,
                    UserID = pass.UserId,
                };

                string jsonData = JsonSerializer.Serialize(passToGo, options);

                return JsonSerializer.Serialize(new MyJsonFormatter() { JsonData = jsonData, StatusCode = 200 }, options);
            }
        }
        

        [HttpGet("/mypasswords")]
        public string GetPasswords([FromQuery] string categoryName = null, [FromQuery] string like = null)
        {
            List<MyPasswords> passwords = new List<MyPasswords>();
            Exception err = new Exception();
            Category category = new Category();

            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Önce giriş yapmanız gerekiyor.", StatusCode = 400 }, options);
            }
            else
            {
                (User _user, Exception error) = DBUtils.GetUserByUsername(HttpContext.Session.GetString("username"));
                if (error != null)
                {
                    return JsonSerializer.Serialize(new MyJsonFormatter() { Message = error.Message, StatusCode = 404 }, options);
                }

                if (string.IsNullOrWhiteSpace(categoryName))
                {
                    (List<Password> passwordsList, Exception err2) = DBUtils.GetAllPasswordsByUserID(_user.UserId, like);
                    if (err2 != null)
                    {
                        return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err2.Message, StatusCode = 500 }, options);
                    }

                    passwordsList.ForEach(pass => 
                    {
                        MyPasswords password = new MyPasswords() { 
                            name=pass.Name,
                            password=pass.Password1,
                            CategoryID=pass.CategoryId,
                            PasswordID=pass.PasswordId,
                            UserID=pass.UserId
                        };

                        passwords.Add(password);
                    });
                }
                else
                {
                    (category, err) = DBUtils.GetCategoryByName(categoryName, _user.UserId);
                    if (err != null)
                    {
                        return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 404 }, options);
                    }

                    (List<Password> passwordsList, Exception err2) = DBUtils.GetAllPasswordsByCategoryID(category.CategoryId, _user.UserId, like);
                    if (err2 != null)
                    {
                        return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err2.Message, StatusCode = 500 }, options);
                    }

                    passwordsList.ForEach(pass =>
                    {
                        MyPasswords password = new MyPasswords()
                        {
                            name = pass.Name,
                            password = pass.Password1,
                            CategoryID = pass.CategoryId,
                            PasswordID = pass.PasswordId,
                            UserID = pass.UserId
                        };

                        passwords.Add(password);
                    });
                }
                
                string jsonData = JsonSerializer.Serialize(passwords, options);
                return JsonSerializer.Serialize(new MyJsonFormatter() { JsonData = jsonData, StatusCode = 200 }, options);
            }
        }

        [HttpPost("/password")]
        public string Post([FromForm] string name, [FromForm] string password, [FromForm] string categoryName = "Uncategorized")
        {
            User user = new User();
            Exception err = new Exception();
            Category category = new Category();

            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Önce giriş yapmanız gerekiyor.", StatusCode = 401 }, options);
            }
            (user, err) = DBUtils.GetUserByUsername(HttpContext.Session.GetString("username"));
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 404 }, options);
            }

            if (string.IsNullOrWhiteSpace(categoryName)) { categoryName = "Uncategorized"; }
            (category, err) = DBUtils.GetCategoryByName(categoryName, user.UserId);
            if (err != null)
            {
                err = DBUtils.AddCategory(categoryName, user.UserId);
                if(err != null)
                {
                    return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 500 }, options);
                }

                (category, err) = DBUtils.GetCategoryByName(categoryName, user.UserId);
                if (err != null)
                {
                    return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 404 }, options);
                }
            }

            if (password.ToLower() == "random" || password.ToLower() == "rastgele") { password = Utils.GetRandomPassword(); }
            err = DBUtils.AddPassword(name, password, user.UserId, category.CategoryId);
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 500 }, options);
            }
            return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Şifre başarıyla eklendi!", StatusCode = 200 }, options);
        }

        [HttpDelete("/password")]
        public string Delete([FromForm] string name)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Önce giriş yapmanız gerekiyor.", StatusCode = 400 }, options);
            }

            (User user, Exception err) = DBUtils.GetUserByUsername(HttpContext.Session.GetString("username"));
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 404 }, options);
            }

            err = DBUtils.DeletePasswordByName(name, user.UserId);
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 500 }, options);
            }

            return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Şifre başarıyla silindi.", StatusCode = 200 }, options);
        }

        [HttpPut("/password")]
        public string Put([FromForm] string currentName, [FromForm] string newName = null, [FromForm] string newPassword = null, [FromForm] string newCategoryName = null)
        {
            Category category = new Category();
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Önce giriş yapmanız gerekiyor.", StatusCode = 400 }, options);
            }

            if(string.IsNullOrWhiteSpace(newName) && string.IsNullOrWhiteSpace(newPassword) && string.IsNullOrWhiteSpace(newCategoryName))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Hiçbir değişiklik yapılmadı.", StatusCode = 400 }, options);
            }

            (User user, Exception err) = DBUtils.GetUserByUsername(HttpContext.Session.GetString("username"));
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 404 }, options);
            }
            if (!string.IsNullOrWhiteSpace(newCategoryName))
            {
                (category, err) = DBUtils.GetCategoryByName(newCategoryName, user.UserId);
                if (err != null)
                {
                    err = DBUtils.AddCategory(newCategoryName, user.UserId);
                    if (err != null)
                    {
                        return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 500 }, options);
                    }
                }

                (category, err) = DBUtils.GetCategoryByName(newCategoryName, user.UserId);
                if (err != null)
                {
                    return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 404 }, options);
                }
            }

            if (newPassword.ToLower() == "random" || newPassword.ToLower() == "rastgele") { newPassword = Utils.GetRandomPassword(); }
            err = DBUtils.UpdatePasswordByName(currentName, user.UserId, newName, newPassword, category.CategoryId);
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 500 }, options);
            }
            return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Şifre başarıyla güncellendi!", StatusCode = 200 }, options);
        }
    }
}
