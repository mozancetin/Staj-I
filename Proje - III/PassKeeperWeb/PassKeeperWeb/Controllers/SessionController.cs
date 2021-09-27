using Microsoft.AspNetCore.Mvc;
using PassKeeperWeb.Models;
using System;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace PassKeeperWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionController : ControllerBase
    {
        public JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };

        [HttpGet("/login")]
        public string Get([FromQuery] string username, [FromQuery] string password)
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Zaten giriş yapmışsın.", StatusCode = 400 }, options);
            }

            (User _user, Exception err) = DBUtils.GetUserByUsername(username);
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Böyle bir kullanıcı yok!", StatusCode = 404 }, options);
            }

            if (Utils.Decrypt(_user.Password) == password)
            {
                HttpContext.Session.SetString("username", username);
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Giriş Yapıldı.", StatusCode = 200 }, options);
            }

            return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Giriş bilgilerin yanlış.", StatusCode = 400 }, options);

        }

        [HttpGet("/logout")]
        public string Get()
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("username")))
            {
                HttpContext.Session.Remove("username");
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Çıkış yapıldı.", StatusCode = 200 }, options);
            }

            return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Halihazırda açık bir oturum yok.", StatusCode = 400 }, options);
        }
    }
}
