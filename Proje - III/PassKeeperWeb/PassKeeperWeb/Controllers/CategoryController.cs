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
    public class CategoryController : ControllerBase
    {
        public JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };

        [HttpGet("/mycategories")]
        public string Get([FromQuery] string like = null)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Önce giriş yapmanız gerekiyor.", StatusCode = 400 }, options);
        }

            (User _user, Exception error) = DBUtils.GetUserByUsername(HttpContext.Session.GetString("username"));
            if (error != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = error.Message, StatusCode = 404 }, options);
            }

            (List<Category> categories, Exception err) = DBUtils.GetAllCategoriesByUserID(_user.UserId, like);
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 500 }, options);
            }
            List<MyCategories> sendData = new List<MyCategories>();
            categories.ForEach(c => {
                MyCategories categoryToGo = new MyCategories()
                {
                    CategoryID = c.CategoryId,
                    UserID = c.UserId,
                    name = c.Name,
                };
                sendData.Add(categoryToGo);
            });
            
            string jsonData = JsonSerializer.Serialize(sendData, options);
            return JsonSerializer.Serialize(new MyJsonFormatter() { JsonData = jsonData, StatusCode = 200 }, options);
        }

        [HttpGet("/category")]
        public string GetCategory([FromQuery] string categoryName = null, [FromQuery] int categoryID = -1)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Önce giriş yapmanız gerekiyor.", StatusCode = 400 }, options);
            }

            (User _user, Exception error) = DBUtils.GetUserByUsername(HttpContext.Session.GetString("username"));
            if (error != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = error.Message, StatusCode = 404 }, options);
            }

            List<Password> passwords = new List<Password>();
            Exception err = new Exception();
            Category category = new Category();

            if (categoryName != null)
            {
                (category, err) = DBUtils.GetCategoryByName(categoryName, _user.UserId);
                if (err != null)
                {
                    return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 404 }, options);
                }
            }
            else if(categoryName == null && categoryID != -1)
            {
                (category, err) = DBUtils.GetCategoryByCategoryID(categoryID);
                if (err != null)
                {
                    return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 404 }, options);
                }
            }
            else
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Yeterli parametre girilmedi", StatusCode = 400 }, options);
            }
            
            MyCategories mycategory = new MyCategories()
            {
                CategoryID = category.CategoryId,
                name = category.Name,
                UserID = category.UserId,
            };
            string jsonData = JsonSerializer.Serialize(mycategory, options);
            return JsonSerializer.Serialize(new MyJsonFormatter() { JsonData = jsonData, StatusCode = 200 }, options);
        }

        [HttpPost("/category")]
        public string Post([FromForm] string name)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Önce giriş yapmanız gerekiyor.", StatusCode = 400 }, options);
            }

            (User _user, Exception error) = DBUtils.GetUserByUsername(HttpContext.Session.GetString("username"));
            if (error != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = error.Message, StatusCode = 404 }, options);
            }

            Exception err = DBUtils.AddCategory(name, _user.UserId);
            if(err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 500 }, options);
            }

            return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Kategori başarıyla kaydedildi!", StatusCode = 200 }, options);
        }

        [HttpDelete("/category")]
        public string Delete([FromForm] string name)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Önce giriş yapmanız gerekiyor.", StatusCode = 400 }, options);
            }

            (User _user, Exception error) = DBUtils.GetUserByUsername(HttpContext.Session.GetString("username"));
            if (error != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = error.Message, StatusCode = 404 }, options);
            }

            Exception err = DBUtils.DeleteCategoryByName(name, _user.UserId);
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 500 }, options);
            }

            return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Kategori başarıyla silindi!", StatusCode = 200 }, options);
        }

        [HttpPut("/category")]
        public string Put([FromForm] string currentName, [FromForm] string newName)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Önce giriş yapmanız gerekiyor.", StatusCode = 400 }, options);
            }

            (User _user, Exception error) = DBUtils.GetUserByUsername(HttpContext.Session.GetString("username"));
            if (error != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = error.Message, StatusCode = 404 }, options);
            }

            Exception err = DBUtils.UpdateCategoryByName(currentName, _user.UserId, newName);
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 500 }, options);
            }

            return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Kategori başarıyla güncellendi!", StatusCode = 200 }, options);
        }
    }
}
