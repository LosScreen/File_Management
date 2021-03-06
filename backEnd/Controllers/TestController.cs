using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backEnd.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using backEnd.Models;
// using Send_Mail_To_gMail.Models;
// using System.Net.Mail;

using MailKit.Net.Smtp;
using MailKit.Security;

using MailKit.Net.Pop3;
using MailKit;
using MimeKit;
using MimeKit.Text;

// int drNum = Convert.ToInt32(dr["share"]);
//                                     string sqlUsernameShareTo1 = $"SELECT * FROM user Where Id = '{drNum}'";
//                                     System.Data.DataTable SqlDataFileShareTo1 = db.get(sqlUsernameShareTo1);
//                                     foreach (DataRow dr1 in SqlDataFileShareTo1.Rows)
//                                     {
//                                         worksheet.Cell(index, 2).Value = dr1["username"].ToString();
//                                     }            


//                                     worksheet.Cell(index, 2).Value += ", ";

//                                     int dtNum = Convert.ToInt32(dt["share"]);
//                                     string sqlUsernameShareTo2 = $"SELECT * FROM user Where Id = '{dtNum}'";
//                                     System.Data.DataTable SqlDataFileShareTo2 = db.get(sqlUsernameShareTo2);
//                                     foreach (DataRow dr2 in SqlDataFileShareTo2.Rows)
//                                     {
//                                         worksheet.Cell(index, 2).Value += dr2["username"].ToString();
//                                     }            



// worksheet.Cell(index, 2).Value += dr1["username"].ToString();

// string sqlAllUsername = $"SELECT * FROM user";
//                     System.Data.DataTable SqlDataAllUsername = db.get(sqlAllUsername);
//                     foreach (DataRow drAll in SqlDataAllUsername.Rows){

//                     string name = drAll["username"].ToString();
//                     string sqlUser = $"SELECT * FROM User Where UserName = '{name}'";
namespace backEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        private IConfiguration _config;

        public TestController(IConfiguration config)
        {
            _config = config;
        }
        
        
        [HttpPost]
        [Route("Email")]
        public IActionResult Email(){
            try
            {
                // password 123456789top
                // string to = em.To;
                // string subject = em.Subject;
                // string body = em.Body;
                // MailMessage mm = new MailMessage();
                // mm.To.Add(to);
                // mm.Subject = subject;
                // mm.Body = body;
                // mm.From = new MailMessage("testemail.2541@hotmail.com");
                // mm.IsBodyHtml = false;
                // SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                // smtp.Port = 587;
                // smtp.UseDefualtCreadentials = true;
                // smtp.EnableSsl = true;
                // smtp.Credentials = new System.Net.NetworkCredential("testemail.2541@hotmail.com", "123456789top");
                // smtp.Send(mm);


                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("testemail.2541@gmail.com"));
                email.To.Add(MailboxAddress.Parse("gogorotop5@gmail.com"));
                email.Subject = "Test Email Subject";
                email.Body = new TextPart(TextFormat.Html) { Text = "<h3>Example HTML Message Body</h3>" };


                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 465);
                smtp.Authenticate("testemail.2541@gmail.com", "123456789top");
                smtp.Send(email);
                smtp.Disconnect(true);

                return Ok();
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] User login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("Test")]
        public IActionResult Test()
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = ((string)token).Replace("Bearer ","");
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken decodedValue = handler.ReadJwtToken(token);
            List<Claim> claimsList = decodedValue.Claims.ToList();
            var id = claimsList.Find(x => x.Type == "unique_name").Value;
            return Ok(id);
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.UniqueName , "26"),
                new Claim(JwtRegisteredClaimNames.Sub , "Piyapon")
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User AuthenticateUser(User login)
        {
            User user = null;

            //Validate the User Credentials    
            //Demo Purpose, I have Passed HardCoded User Information    
            if (login.userName == "Test")
            {
                user = new User { userName = "Jignesh Trivedi", passWord = "test.btest@gmail.com" };
            }
            return user;
        }
    }
}