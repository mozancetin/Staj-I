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
using System.Windows;

namespace PassKeeperWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IEController : ControllerBase
    {
        public JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };

        [HttpGet("/export")]
        public string Get()
        {
            try
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

                (string jsonString, Exception err2) = Utils.Export(user.UserId);
                if (err2 != null)
                {
                    return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err2.Message, StatusCode = 500 }, options);
                }

                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Başarıyla export edildi!", JsonData = jsonString, StatusCode = 200 }, options);
            }
            catch (Exception error)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = error.Message, StatusCode = 500 }, options);
            }
        }

        [HttpPost("/import")]
        public string Post([FromForm] string jsonString)
        {
            Exception err = Utils.Import(jsonString);
            if (err != null)
            {
                return JsonSerializer.Serialize(new MyJsonFormatter() { Message = err.Message + " Inner: " + err.InnerException.Message, StatusCode = 500 }, options);
            }

            return JsonSerializer.Serialize(new MyJsonFormatter() { Message = "Bilgiler başarıyla import edildi!", StatusCode = 200 }, options);
        }
    }
}
