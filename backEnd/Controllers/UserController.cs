using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backEnd.Models;
using System;

using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySqlConnector;
using backEnd.Service;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.IO.Compression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace backEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] User data)
        {
            ResponseErr res = new ResponseErr();
            try
            {
                var db = new ConMySQL();
                var pathsql = "/uploads/" + data.userName;
                string sqluser = $"INSERT INTO User(UserName, Password) VALUES ('{data.userName}','{data.passWord}')";
                db.executeQuery(sqluser);

                string sqlgetuser = $"SELECT * FROM User WHERE UserName = '{data.userName}'";
                DataTable dataTableGet = db.getData(sqlgetuser);
                User obj = new User();
                foreach (DataRow dr in dataTableGet.Rows)
                {
                    obj.id = Convert.ToInt32(dr["id"]);
                }
                Console.WriteLine(obj.id);

                // Console.WriteLine(pathsql);
                // string sqlfile = $"INSERT INTO DataFile(NameFile, Path, Type ,IdUser) VALUES ('{data.userName}','{pathsql}', 'Folder', '{obj.id}')";
                // Console.WriteLine(sqlfile);
                // db.executeQuery(sqlfile);
                var pathFristFolder = "/" + data.userName;
                fristFolder(data.userName, obj.id);

                // Console.WriteLine(data.userName);
                string startupPath = Environment.CurrentDirectory;
                string path = startupPath + "/wwwroot/uploads/" + data.userName;
                Directory.CreateDirectory(path);

                res.msg = "okay";
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                res.data = data;
                // return res;
                return BadRequest(res);
            }
        }

        public IActionResult fristFolder(string userName, int id)
        {
            try
            {
                var db = new ConMySQL();
                var pathfrist = "/uploads";
                string sqlfile = $"INSERT INTO DataFile(NameFile, Path, Type ,IdUser) VALUES ('{userName}','{pathfrist}', 'Folder', '{id}')";
                db.executeQuery(sqlfile);
                // Console.WriteLine(sqlfile);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("checkRegister")]
        public IActionResult checkRegister([FromBody] User data)
        {
            ResponseErr res = new ResponseErr();
            try
            {
                var db = new ConMySQL();
                string sql = $"SELECT * FROM User WHERE UserName = '{data.userName}'";
                DataTable dataTable = db.getData(sql);
                // Console.WriteLine(dataTable.Rows);
                User obj = new User();
                foreach (DataRow dr in dataTable.Rows)
                {
                    obj.userName = dr["userName"].ToString();
                }
                // Console.WriteLine(obj.userName);
                if (obj.userName != null)
                {
                    return Ok("notEmpty");
                }
                else
                {
                    res.msg = "empty";
                    return Ok("empty");
                }
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                res.data = data;
                // return res;
                return BadRequest(res);
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult login([FromBody] User data)
        {
            ResponseErr res = new ResponseErr();
            try
            {
                var db = new ConMySQL();
                string sql = $"SELECT * FROM User WHERE UserName = '{data.userName}' and PassWord = '{data.passWord}'";
                DataTable dataTable = db.getData(sql);
                // Console.WriteLine(dataTable.Rows);
                User obj = new User();
                foreach (DataRow dr in dataTable.Rows)
                {
                    obj.id = Convert.ToInt32(dr["id"]);
                    obj.userName = dr["userName"].ToString();
                    // obj.passWord = dr["passWord"].ToString();
                }

                // foreach (DataRow dataRow in dataTable.Rows)
                // {
                //     foreach(var item in dataRow.ItemArray)
                //     {
                //         Console.WriteLine(item);
                //     }
                // }

                data.id = obj.id;
                // Console.WriteLine(obj.userName);
                IActionResult response = Unauthorized();


                if (obj.userName != null)
                {
                    res.msg = "okay";
                    var tokenString = GenerateJSONWebToken(data);
                    response = Ok(new { token = tokenString,
                    Username = obj.userName });


                    return Ok(response);
                }
                else
                {
                    res.msg = "empty";
                    return Ok("empty");
                }
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                res.data = data;
                // return res;
                return BadRequest(res);
            }
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.UniqueName , Convert.ToString(userInfo.id)),
                // new Claim(JwtRegisteredClaimNames.Sub , "Piyapon")
            };

            Console.WriteLine(Convert.ToString(userInfo.id));
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        [Route("GetUser")]
        public IActionResult GetUser([FromBody] User data){
            try{
            var db = new ConMySQL();
            string sqlUser = string.Format("SELECT * FROM User WHERE id = '{0}'", data.id);
            DataTable SqlData = db.get(sqlUser);
            User objUser = new User();
                foreach (DataRow dr in SqlData.Rows)
                    {
                        objUser.userName = dr["username"].ToString();
                    }
                return Ok(objUser);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

    }
}