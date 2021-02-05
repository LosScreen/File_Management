using Microsoft.AspNetCore.Mvc;
using backEnd.Models;
using System;
using backEnd.Service;
using System.Data;

namespace backEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        [HttpPost]
        [Route("Register")]
        public IActionResult Register ([FromBody]User data){
            ResponseErr res = new ResponseErr();
            try{
            var db = new ConMySQL();
            string sql = $"INSERT INTO User(UserName, Password) VALUES ('{data.userName}','{data.passWord}')";
            db.executeQuery(sql);
            // Console.WriteLine("API Come");
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

        [HttpPost]
        [Route("checkRegister")]
        public IActionResult checkRegister ([FromBody]User data){
            ResponseErr res = new ResponseErr();
            try{
            var db = new ConMySQL();
            string sql = $"SELECT * FROM User WHERE UserName = '{data.userName}'";
            DataTable dataTable = db.getData(sql);
            Console.WriteLine(dataTable.Rows);
            User obj = new User();
            foreach (DataRow dr in dataTable.Rows)
                {
                    obj.userName = dr["userName"].ToString();
                }
            Console.WriteLine(obj.userName);
            if(obj.userName != null){
                return Ok("notEmpty");
            }
            else {
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
        public IActionResult login ([FromBody]User data){
            ResponseErr res = new ResponseErr();
            try{
            var db = new ConMySQL();
            string sql = $"SELECT * FROM User WHERE UserName = '{data.userName}' and PassWord = '{data.passWord}'";
            DataTable dataTable = db.getData(sql);
            Console.WriteLine(dataTable.Rows);
            User obj = new User();
            foreach (DataRow dr in dataTable.Rows)
                {
                    obj.userName = dr["userName"].ToString();
                    obj.passWord = dr["passWord"].ToString();
                }
            Console.WriteLine(obj.userName);
            if(obj.userName != null){
                res.msg = "okay";
                return Ok(obj);
            }
            else {
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
        
    }
}