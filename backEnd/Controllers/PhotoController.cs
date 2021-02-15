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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace backEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhotoController : ControllerBase
    {
        [Authorize]
        [HttpPost]
        [Route("GetPhoto")]
        public IActionResult GetPhoto (DataFile data){
            var db = new ConMySQL();
            try
            {
                Request.Headers.TryGetValue("Authorization", out var token);
                token = ((string)token).Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken decodedValue = handler.ReadJwtToken(token);
                List<Claim> claimsList = decodedValue.Claims.ToList();
                var id = claimsList.Find(x => x.Type == "unique_name").Value;

                

                string sql = string.Format("SELECT * FROM DataFile WHERE wwwPath like '%{0}'and IdUser = '{1}'", data.Path, id);
                DataTable SqlDataSet = db.getData(sql);
                Console.WriteLine(sql);

                DataFile obj = new DataFile();
                foreach (DataRow dr in SqlDataSet.Rows)
                {
                    obj.wwwPath = dr["wwwpath"].ToString();
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }
        
    }
}