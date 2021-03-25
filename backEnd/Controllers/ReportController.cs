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
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "authors.xlsx";
            try
            {
                var db = new ConMySQL();
                db.Open();


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
                            worksheet.Cell(index + 1, 1).Value = dr["path"].ToString() + "/" + dr["namefile"].ToString();


                            index++;
                        }
                        index+=2;
                        worksheet.Cell(index, 1).Value = "File(Share)";
                        worksheet.Cell(index, 2).Value = "File(Share TO)";
                        //  worksheet.Cell(index, 1).Value += "eiei";
                        // index++;

                        // string namefilearr = null;


                        //edit
                        string sqlFileShare1 = $"SELECT * FROM DataFile Where IdUser = '{objUser.id}' and Share = '0'";
                        System.Data.DataTable SqlDataFileShare1 = db.get(sqlFileShare1);

                        string sqlFileShare2 = $"SELECT * FROM DataFile Where IdUser = '{objUser.id}' and Share != '0'";
                        System.Data.DataTable SqlDataFileShare2 = db.get(sqlFileShare2);
                        
                        foreach (DataRow dr in SqlDataFileShare1.Rows)
                        {
                            var x = 0;
                           foreach (DataRow dt in SqlDataFileShare2.Rows){
                               if (Convert.ToInt32(dr["id"]) == Convert.ToInt32(dt["mainfolder"]) && Convert.ToInt32(dr["share"]) != Convert.ToInt32(dt["share"])) {
                                   if(x == 0){
                                    worksheet.Cell(index, 1).Value = dr["path"].ToString() + "/" + dr["namefile"].ToString();
                                    int drNum = Convert.ToInt32(dt["share"]);
                                    string sqlUsernameShareTo = $"SELECT * FROM user Where Id = '{drNum}'";
                                    System.Data.DataTable SqlDataFileShareTo = db.get(sqlUsernameShareTo);
                                    // Console.WriteLine("index");
                                    foreach (DataRow drTo in SqlDataFileShareTo.Rows)
                                    {
                                        worksheet.Cell(index, 2).Value = drTo["username"].ToString();

                                        
                                    }
                                    // return Ok(SqlDataFileShareTo.Rows);
                                    // Console.WriteLine(SqlDataFileShareTo.Rows.length);
                                    // worksheet.Cell(index, 2).Value = Convert.ToInt32(dr["share"]) + ", " + Convert.ToInt32(dt["share"]);
                                   }else{
                                    int drNum = Convert.ToInt32(dt["share"]);
                                    string sqlUsernameShareTo1 = $"SELECT * FROM user Where Id = '{drNum}'";
                                    System.Data.DataTable SqlDataFileShareTo1 = db.get(sqlUsernameShareTo1);
                                    foreach (DataRow dr1 in SqlDataFileShareTo1.Rows)
                                    {
                                        worksheet.Cell(index, 2).Value += ", " + dr1["username"].ToString();
                                    }          
                                    // worksheet.Cell(index, 2).Value += ", " + Convert.ToInt32(dt["share"]);
                                   }
                                   x++;
                               }
                               
                           }
                            //edit2
                        //     int IdUsernameShare = Convert.ToInt32(dr["share"]);
                        //     string sqlUsernameShareTo = $"SELECT * FROM user Where Id = '{IdUsernameShare}'";
                        //     System.Data.DataTable SqlDataFileShareTo = db.get(sqlUsernameShareTo);
                        //     foreach (DataRow datar in SqlDataFileShareTo.Rows)
                        // {
                        //     worksheet.Cell(index, 2).Value = datar["username"].ToString();
                        // }

                            // worksheet.Cell(index + 1, 2).Value = Convert.ToInt32(dr["share"]);

                           index++;
                        }
                        index+=1;
                        worksheet.Cell(index, 1).Value = "File(Receiver Share)";
                        worksheet.Cell(index, 2).Value = "File(Share From)";

                        string sqlFileReceiverShare = $"SELECT * FROM DataFile Where Share = '{objUser.id}'";
                        System.Data.DataTable SqlDataFileReceiverShare = db.get(sqlFileReceiverShare);
                        foreach (DataRow dr in SqlDataFileReceiverShare.Rows)
                        {
                            worksheet.Cell(index + 1, 1).Value = dr["path"].ToString() + "/" + dr["namefile"].ToString();

                            int drNum = Convert.ToInt32(dr["IdUser"]);
                                    string sqlUsernameShareTo1 = $"SELECT * FROM user Where Id = '{drNum}'";
                                    System.Data.DataTable SqlDataFileShareTo1 = db.get(sqlUsernameShareTo1);
                                    foreach (DataRow dr1 in SqlDataFileShareTo1.Rows)
                                    {
                                        worksheet.Cell(index + 1, 2).Value = dr1["username"].ToString();
                                    }          
                            // worksheet.Cell(index + 1, 2).Value = Convert.ToInt32(dr["IdUser"]);


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
                }else if (Username == "All"){
                    // Console.WriteLine("Key");
                   string sqlAllUsername = $"SELECT * FROM user";
                    System.Data.DataTable SqlDataAllUsername = db.get(sqlAllUsername);
                    var workbook = new XLWorkbook();
                    foreach (DataRow drAll in SqlDataAllUsername.Rows){

                    string name = drAll["username"].ToString();
                    // Console.WriteLine(name);
                    string sqlUser = $"SELECT * FROM User Where UserName = '{name}'";

                    System.Data.DataTable SqlData = db.get(sqlUser);
                    User objUser = new User();
                    foreach (DataRow dr in SqlData.Rows)
                    {
                        objUser.id = Convert.ToInt32(dr["id"]);
                    }



                    //edit
                    // var workbook = new XLWorkbook();
                    
                        IXLWorksheet worksheet =
                        workbook.Worksheets.Add(name);
                        worksheet.Cell(1, 1).Value = "File(Owner)";
                        // worksheet.Cell(1, 2).Value = "File(Share)";
                        // worksheet.Cell(1, 3).Value = "File(receiver Share)";

                        int index = 1;


                        
                        string sqlFileOwner = $"SELECT * FROM DataFile Where IdUser = '{objUser.id}' and Share = '0'";
                        System.Data.DataTable SqlDataFileOwner = db.get(sqlFileOwner);
                        foreach (DataRow dr in SqlDataFileOwner.Rows)
                        {
                            worksheet.Cell(index + 1, 1).Value = dr["path"].ToString() + "/" + dr["namefile"].ToString();


                            index++;
                        }
                        index+=2;
                        worksheet.Cell(index, 1).Value = "File(Share)";
                        worksheet.Cell(index, 2).Value = "File(Share TO)";
                        //  worksheet.Cell(index, 1).Value += "eiei";
                        // index++;

                        // string namefilearr = null;


                        //edit
                        string sqlFileShare1 = $"SELECT * FROM DataFile Where IdUser = '{objUser.id}' and Share = '0'";
                        System.Data.DataTable SqlDataFileShare1 = db.get(sqlFileShare1);

                        string sqlFileShare2 = $"SELECT * FROM DataFile Where IdUser = '{objUser.id}' and Share != '0'";
                        System.Data.DataTable SqlDataFileShare2 = db.get(sqlFileShare2);
                        
                        foreach (DataRow dr in SqlDataFileShare1.Rows)
                        {
                            var x = 0;
                           foreach (DataRow dt in SqlDataFileShare2.Rows){
                               if (Convert.ToInt32(dr["id"]) == Convert.ToInt32(dt["mainfolder"]) && Convert.ToInt32(dr["share"]) != Convert.ToInt32(dt["share"])) {
                                   if(x == 0){
                                    worksheet.Cell(index, 1).Value = dr["path"].ToString() + "/" + dr["namefile"].ToString();
                                    int drNum = Convert.ToInt32(dt["share"]);
                                    string sqlUsernameShareTo = $"SELECT * FROM user Where Id = '{drNum}'";
                                    System.Data.DataTable SqlDataFileShareTo = db.get(sqlUsernameShareTo);
                                    // Console.WriteLine("index");
                                    foreach (DataRow drTo in SqlDataFileShareTo.Rows)
                                    {
                                        worksheet.Cell(index, 2).Value = drTo["username"].ToString();

                                        
                                    }
                                    // return Ok(SqlDataFileShareTo.Rows);
                                    // Console.WriteLine(SqlDataFileShareTo.Rows.length);
                                    // worksheet.Cell(index, 2).Value = Convert.ToInt32(dr["share"]) + ", " + Convert.ToInt32(dt["share"]);
                                   }else{
                                    int drNum = Convert.ToInt32(dt["share"]);
                                    string sqlUsernameShareTo1 = $"SELECT * FROM user Where Id = '{drNum}'";
                                    System.Data.DataTable SqlDataFileShareTo1 = db.get(sqlUsernameShareTo1);
                                    foreach (DataRow dr1 in SqlDataFileShareTo1.Rows)
                                    {
                                        worksheet.Cell(index, 2).Value += ", " + dr1["username"].ToString();
                                    }          
                                    // worksheet.Cell(index, 2).Value += ", " + Convert.ToInt32(dt["share"]);
                                   }
                                   x++;
                               }
                               
                           }
                            //edit2
                        //     int IdUsernameShare = Convert.ToInt32(dr["share"]);
                        //     string sqlUsernameShareTo = $"SELECT * FROM user Where Id = '{IdUsernameShare}'";
                        //     System.Data.DataTable SqlDataFileShareTo = db.get(sqlUsernameShareTo);
                        //     foreach (DataRow datar in SqlDataFileShareTo.Rows)
                        // {
                        //     worksheet.Cell(index, 2).Value = datar["username"].ToString();
                        // }

                            // worksheet.Cell(index + 1, 2).Value = Convert.ToInt32(dr["share"]);

                           index++;
                        }
                        index+=1;
                        worksheet.Cell(index, 1).Value = "File(Receiver Share)";
                        worksheet.Cell(index, 2).Value = "File(Share From)";

                        string sqlFileReceiverShare = $"SELECT * FROM DataFile Where Share = '{objUser.id}'";
                        System.Data.DataTable SqlDataFileReceiverShare = db.get(sqlFileReceiverShare);
                        foreach (DataRow dr in SqlDataFileReceiverShare.Rows)
                        {
                            worksheet.Cell(index + 1, 1).Value = dr["path"].ToString() + "/" + dr["namefile"].ToString();

                            int drNum = Convert.ToInt32(dr["IdUser"]);
                                    string sqlUsernameShareTo1 = $"SELECT * FROM user Where Id = '{drNum}'";
                                    System.Data.DataTable SqlDataFileShareTo1 = db.get(sqlUsernameShareTo1);
                                    foreach (DataRow dr1 in SqlDataFileShareTo1.Rows)
                                    {
                                        worksheet.Cell(index + 1, 2).Value = dr1["username"].ToString();
                                    }          
                            // worksheet.Cell(index + 1, 2).Value = Convert.ToInt32(dr["IdUser"]);


                            index++;
                        }





                        //  worksheet.Cell(index + 1, 2).Value =
                        //     authors[index - 1].FirstName;
                        //     worksheet.Cell(index + 1, 3).Value =
                        //     authors[index - 1].LastName;

                        // return Ok(listFileOwner);
                        // using (var stream = new MemoryStream())
                        // {
                        //     workbook.SaveAs(stream);
                        //     var content = stream.ToArray();
                        //     return File(content, contentType, fileName);
                        // }
                    
                    }
                    using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            return File(content, contentType, fileName);
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