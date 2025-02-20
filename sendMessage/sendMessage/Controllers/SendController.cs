using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using sendMessage.Data;
using sendMessage.Models;

namespace sendMessage.Controllers
{
    [EnableCors("*","*","*")]

    [RoutePrefix("api/send")]
    public class SendController : ApiController
    {
        private readonly UserController kontrolcu;

        public SendController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
            kontrolcu = new UserController(connectionString);
        }

        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register(SaveModel user)
        {
            if (kontrolcu.UserExists(user.Username))
            {
                return BadRequest("Kullanıcı adı zaten alınmış.");
            }

            kontrolcu.AddUser(user);
            return Ok("Kullanıcı başarıyla kaydedildi.");
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(SaveModel loginRequest)
        {
            try
            {
                var user = kontrolcu.GetUser(loginRequest.Username);

                if (user == null) 
                {
                    return BadRequest("Kullanıcı adı veya şifre eşleşmiyor.");
                }
                if (!kontrolcu.ValidatePassword(loginRequest.Password, user.Password))
                {
                    // return Unauthorized();
                    return BadRequest("Kullanıcı adı veya şifre eşleşmiyor.");
                }
                return Ok("Giriş başarılı.");

            }
            catch (Exception ex)
            {
                return InternalServerError(ex); 
            }
            
        }
    }
}
