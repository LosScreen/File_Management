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
using System.Web;

using Microsoft.Office.Interop.Excel;
using ClosedXML.Excel;
//   Excel.Application xlApp;



namespace backEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        [HttpGet]
        [Route("GetAllUser")]
        public IActionResult Register()
        {
            try
            {
                var db = new ConMySQL();
                db.Open();
                string sql = $"SELECT * FROM User";
                System.Data.DataTable SqlData = db.get(sql);
                List<User> list_result = new List<User>();
                foreach (DataRow dr in SqlData.Rows)
                {
                    User objUser = new User();
                    objUser.userName = dr["username"].ToString();
                    list_result.Add(objUser);
                }

                return Ok(list_result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet]
        [Route("ReportExcel/{Username}")]
        public IActionResult ReportExcel(string Username)
        {
            List<Author> authors = new List<Author>
{
    new Author { Id = 1, FirstName = "Joydip", LastName = "Kanjilal" },
    new Author { Id = 2, FirstName = "Steve", LastName = "Smith" },
    new Author { Id = 3, FirstName = "Anand", LastName = "Narayaswamy"}
};
            try
            {
                var db = new ConMySQL();
                db.Open();





                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "authors.xlsx";

                if (Username != "All")
                {
                    string sqlUser = $"SELECT * FROM User Where UserName = '{Username}'";

                    System.Data.DataTable SqlData = db.get(sqlUser);
                    User objUser = new User();
                    foreach (DataRow dr in SqlData.Rows)
                    {
                        objUser.id = Convert.ToInt32(dr["id"]);
                    }




                    using (var workbook = new XLWorkbook())
                    {
                        IXLWorksheet worksheet =
                        workbook.Worksheets.Add(Username);
                        worksheet.Cell(1, 1).Value = "File(Owner)";
                        // worksheet.Cell(1, 2).Value = "File(Share)";
                        // worksheet.Cell(1, 3).Value = "File(receiver Share)";

                        int index = 1;

                        string sqlFileOwner = $"SELECT * FROM DataFile Where IdUser = '{objUser.id}' and Share = '0'";
                        System.Data.DataTable SqlDataFileOwner = db.get(sqlFileOwner);
                        foreach (DataRow dr in SqlDataFileOwner.Rows)
                        {
                            worksheet.Cell(index + 1, 1).Value = dr["namefile"].ToString();


                            index++;
                        }
                        index+=2;
                        worksheet.Cell(index, 1).Value = "File(Share)";
                        worksheet.Cell(index, 2).Value = "File(Share TO)";
                         worksheet.Cell(index, 1).Value += "eiei";

                        string sqlFileShare = $"SELECT * FROM DataFile Where IdUser = '{objUser.id}' and Share != '0'";
                        System.Data.DataTable SqlDataFileShare = db.get(sqlFileShare);
                        foreach (DataRow dr in SqlDataFileShare.Rows)
                        {
                            var x = 0;
                           foreach (DataRow dt in SqlDataFileShare.Rows){
                               if (Convert.ToInt32(dr["mainfolder"]) == Convert.ToInt32(dt["mainfolder"]) && Convert.ToInt32(dr["share"]) != Convert.ToInt32(dt["share"])) {
                                   
                                   if(x == 0){
                                    worksheet.Cell(index, 2).Value = Convert.ToInt32(dr["share"]) + ", " + Convert.ToInt32(dt["share"]);
                                   }else{
                                    worksheet.Cell(index, 2).Value += ", " + Convert.ToInt32(dt["share"]);
                                   }
                                   x++;
                               }
                               
                           }
                            worksheet.Cell(index + 1, 1).Value = dr["namefile"].ToString();
                            worksheet.Cell(index + 1, 2).Value = Convert.ToInt32(dr["share"]);

                           index++;
                        }
                        index+=2;
                        worksheet.Cell(index, 1).Value = "File(Receiver Share)";
                        worksheet.Cell(index, 2).Value = "File(Share From)";

                        string sqlFileReceiverShare = $"SELECT * FROM DataFile Where Share = '{objUser.id}'";
                        System.Data.DataTable SqlDataFileReceiverShare = db.get(sqlFileReceiverShare);
                        foreach (DataRow dr in SqlDataFileReceiverShare.Rows)
                        {
                            worksheet.Cell(index + 1, 1).Value = dr["namefile"].ToString();
                            worksheet.Cell(index + 1, 2).Value = Convert.ToInt32(dr["IdUser"]);


                            index++;
                        }





                        //  worksheet.Cell(index + 1, 2).Value =
                        //     authors[index - 1].FirstName;
                        //     worksheet.Cell(index + 1, 3).Value =
                        //     authors[index - 1].LastName;

                        // return Ok(listFileOwner);
                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            return File(content, contentType, fileName);
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}