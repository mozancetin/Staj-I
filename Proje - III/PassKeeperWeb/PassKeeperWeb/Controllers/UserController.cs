using Microsoft.AspNetCore.Mvc;
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
    public class UserController : ControllerBase
    {
        public JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };

        [HttpPost("/signin")]
        public string Post([FromForm] string username, [FromForm] string password)
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Zaten bir hesapla giriş yapmışsın.", StatusCode = 400 }, options);
            }
            Exception err = DBUtils.AddUser(username, password);
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 500 }, options);
            }

            return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Başarıyla kayıt oldun!", StatusCode = 200 }, options);
        }

        [HttpDelete("/user")]
        public string Delete()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Önce giriş yapmalısın!", StatusCode = 400 }, options);
            }

            (User user, Exception err) = DBUtils.GetUserByUsername(HttpContext.Session.GetString("username"));
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 404 }, options);
            }

            err = DBUtils.DeleteUserByID(user.UserId);
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message, StatusCode = 500 }, options);
        }

            HttpContext.Session.Remove("username");
            return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Kullanıcı Başarıyla Silindi!", StatusCode = 200 }, options);
        }
    }
}
