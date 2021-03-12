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

using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Web;

namespace backEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataFileController : ControllerBase
    {
        private readonly ILogger<DataFileController> _logger;


        public DataFileController(ILogger<DataFileController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [Route("GetAllDataFiles")]
        public IEnumerable<DataFile> GetAllDataFiles([FromBody] DataFile GetData)
        {
            ResponseErr res = new ResponseErr();
            DataFile data = new DataFile();
            var db = new ConMySQL();
            List<DataFile> list_result = new List<DataFile>();
            try
            {
                db.Open();
                Request.Headers.TryGetValue("Authorization", out var token);
                token = ((string)token).Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken decodedValue = handler.ReadJwtToken(token);
                List<Claim> claimsList = decodedValue.Claims.ToList();
                var id = claimsList.Find(x => x.Type == "unique_name").Value;

                // Console.WriteLine(claimsList);
                int Share = 0;
                // int id = 23;
                // string sql = $"SELECT * FROM DataFile";
                // string sql = string.Format("SELECT * FROM DataFile WHERE Path = '{0}'and IdUser = '{1}'", datafile.Path, id);
                string sql = string.Format("SELECT * FROM DataFile WHERE Path like '{0}%'and IdUser = '{1}' and Share = '{2}'", GetData.Path, id, Share);
                // Console.WriteLine(sql);
                DataTable SqlDataSet = db.get(sql);

                foreach (DataRow dr in SqlDataSet.Rows)
                {
                    DataFile obj = new DataFile();
                    obj.Id = Convert.ToInt32(dr["id"]);
                    obj.NameFile = dr["namefile"].ToString();
                    obj.Path = dr["path"].ToString();
                    obj.Type = dr["type"].ToString();
                    obj.wwwPath = dr["wwwpath"].ToString();
                    obj.IdUser = Convert.ToInt32(dr["iduser"]);
                    obj.MainFolder = Convert.ToInt32(dr["MainFolder"]);
                    list_result.Add(obj);
                }




                string sqlA = string.Format("SELECT * FROM DataFile WHERE Path like '{0}%'and IdUser != '{1}' and Share = '{2}'", GetData.Path, id, id);
                // Console.WriteLine(sqlA);
                DataTable SqlDataSetA = db.get(sqlA);

                foreach (DataRow dr in SqlDataSetA.Rows)
                {
                    DataFile obj = new DataFile();
                    obj.Id = Convert.ToInt32(dr["id"]);
                    obj.NameFile = dr["namefile"].ToString();
                    obj.Path = dr["path"].ToString();
                    obj.Type = dr["type"].ToString();
                    obj.wwwPath = dr["wwwpath"].ToString();
                    obj.IdUser = Convert.ToInt32(dr["iduser"]);
                    obj.MainFolder = Convert.ToInt32(dr["MainFolder"]);
                    list_result.Add(obj);
                }
                // return Ok(claimsList);
                db.Close();
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                res.listdata = list_result;
                Console.WriteLine(ex.Message);
                // return BadRequest();
            }
            return list_result;
        }
        [Authorize]
        [HttpPost]
        [Route("getData")]
        // IEnumerable<DataFile>
        public IEnumerable<DataFile> GetDataFiles([FromBody] DataFile GetData)
        {
            ResponseErr res = new ResponseErr();
            DataFile data = new DataFile();
            var db = new ConMySQL();
            List<DataFile> list_result = new List<DataFile>();
            try
            {
                Request.Headers.TryGetValue("Authorization", out var token);
                token = ((string)token).Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken decodedValue = handler.ReadJwtToken(token);
                List<Claim> claimsList = decodedValue.Claims.ToList();
                var id = claimsList.Find(x => x.Type == "unique_name").Value;

                // Console.WriteLine(claimsList);


                int share = 0;

                // int id = 23;
                // string sql = $"SELECT * FROM DataFile";
                // string sql = string.Format("SELECT * FROM DataFile WHERE Path = '{0}'and IdUser = '{1}'", datafile.Path, id);
                string sql = string.Format("SELECT * FROM DataFile WHERE Path = '{0}'and IdUser = '{1}' and Share = '{2}'", GetData.Path, id, share);
                // Console.WriteLine(sql);
                DataTable SqlDataSet = db.getData(sql);

                foreach (DataRow dr in SqlDataSet.Rows)
                {
                    DataFile obj = new DataFile();
                    obj.Id = Convert.ToInt32(dr["id"]);
                    obj.NameFile = dr["namefile"].ToString();
                    obj.Path = dr["path"].ToString();
                    obj.Type = dr["type"].ToString();
                    obj.wwwPath = dr["wwwpath"].ToString();
                    obj.IdUser = Convert.ToInt32(dr["iduser"]);
                    obj.MainFolder = Convert.ToInt32(dr["MainFolder"]);
                    list_result.Add(obj);
                }
                // return Ok(claimsList);
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                res.listdata = list_result;
                Console.WriteLine(ex.Message);
                // return BadRequest();
            }

            // Console.WriteLine(JsonConvert.SerializeObject(list_result, Formatting.Indented));

            return list_result;
        }

        [HttpPost]
        [Route("GetPhoto")]
        public IActionResult GetPhoto([FromBody] DataFile datafile)
        {
            ResponseErr res = new ResponseErr();
            try
            {

                static string Convert(string path)
                {
                    return path.Replace(@"D:\Orisma\File_Management\File_Management\backEnd\wwwroot", @"http://localhost:5000").Replace('\\', '/');
                }
                // string filename = "/uploads/Duck.png";
                // string path = Path.GetFullPath(filename);
                // string urlFolder = new Uri(path).AbsoluteUri;
                // Console.WriteLine(urlFolder);
                string url = Convert(@"D:\Orisma\File_Management\File_Management\backEnd\wwwroot" + datafile.Path + "/" + datafile.NameFile);

                return Ok(url);
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                // res.test = url;
                return BadRequest(res);
            }

        }

        [HttpPost]
        [Route("post")]
        public IActionResult PostDataFile([FromBody] DataFile datafile)
        {
            // int test = 3;

            ResponseErr res = new ResponseErr();
            try
            {
                var db = new ConMySQL();
                string sql = $"INSERT INTO DataFile(NameFile, Path, Type) VALUES ('{datafile.NameFile}','{datafile.Path}', '{datafile.Type}')";
                db.executeQuery(sql);
                res.msg = "okay";
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                res.data = datafile;
                // return res;
                return BadRequest(res);
            }
        }

        [HttpDelete]
        [Route("delete")]
        public IEnumerable<ActionResult<ResponseErr>> DeleteDataFile([FromBody] DataFile datafile)
        {
            ResponseErr res = new ResponseErr();
            try
            {
                var db = new ConMySQL();
                db.Open();
                string sql = $"Delete From datafile Where ID = {datafile.Id}";
                db.execute(sql);
                string sqlshare = $"Delete From datafile Where MainFolder = {datafile.Id}";
                db.execute(sqlshare);

                db.Close();
                res.msg = "okay";
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                res.data = datafile.Id;
            }
            yield return res;
        }

        [HttpPatch]
        [Route("update/{Id}")]
        public IEnumerable<ActionResult<ResponseErr>> UpdateDataFile([FromBody] DataFile datafile, int Id)
        {
            ResponseErr res = new ResponseErr();
            try
            {
                var db = new ConMySQL();
                string sql = $"UPDATE datafile SET NameFile='{datafile.NameFile}',Path='{datafile.Path}',Type='{datafile.Type}' WHERE Id='{Id}'";
                db.executeQuery(sql);
                res.msg = "okay";
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                res.data = datafile.Id;
            }
            yield return res;
        }

        [HttpPost]
        [Route("putfile")]
        public IEnumerable<ActionResult<ResponseErr>> UploadDataFile([FromForm] DataFile datafile)
        {
            dynamic rs = new System.Dynamic.ExpandoObject();
            // rs.filedata_0 = Newtonsoft.Json.JsonConvert.SerializeObject(datafile.filedata[0]);
            Console.WriteLine(datafile.filedata.Count);
            ResponseErr res = new ResponseErr();
            try
            {
                var db = new ConMySQL();
                BinaryReader br = new BinaryReader(datafile.filedata[0].OpenReadStream());
                int Type = 1;
                byte[] byte_data = br.ReadBytes((int)datafile.filedata[0].OpenReadStream().Length);
                string sql = $"INSERT INTO DataFile(NameFile, Path, Type, File) VALUES ('{datafile.filedata[0].FileName}','{datafile.Path}', '{Type}', '{datafile.filedata[0].ContentType}')";
                db.executeQuery(sql);
                res.msg = "okay";
            }
            catch (Exception ex)
            {
                res.msg = ex.ToString();
                res.data = datafile.Id;
            }
            yield return res;
        }

        [Authorize]
        [HttpPost]
        [Route("putfile2")]
        public async Task<ResponseErr> UploadDataFile2([FromForm] DataFile datafile)
        {
            ResponseErr res = new ResponseErr();

            Console.WriteLine("Pass");
            try
            {
                string type = null;
                string path = null;
                string uploads = null;
                var db = new ConMySQL();

                Request.Headers.TryGetValue("Authorization", out var token);
                token = ((string)token).Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken decodedValue = handler.ReadJwtToken(token);
                List<Claim> claimsList = decodedValue.Claims.ToList();
                var id = claimsList.Find(x => x.Type == "unique_name").Value;

                Console.WriteLine(id);

                if (datafile.Path != null)
                {
                    uploads = Path.Combine(@"wwwroot", "uploads" + datafile.Path);
                    // Console.WriteLine(uploads);
                }
                else
                {
                    uploads = Path.Combine(@"wwwroot", "uploads");
                }
                // Console.WriteLine(datafile.filedata);
                foreach (IFormFile file in datafile.filedata)
                {
                    if (file.Length > 0)
                    {
                        // Console.WriteLine(datafile.IdUser);
                        string filePath = Path.Combine(uploads, file.FileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        // Console.WriteLine(file.ContentType.Contains("image"));
                        if (file.ContentType.StartsWith("image"))
                        {
                            Console.WriteLine(datafile.Path);
                            if (datafile.Path == null)
                            {
                                path = "/uploads";
                            }
                            else if (datafile.Path != null)
                            {
                                path = "/uploads" + datafile.Path;
                            }
                            Console.WriteLine(path);
                            string wwwPath = "http://localhost:5000/" + path + "/" + file.FileName;
                            type = "image";
                            string sql = $"INSERT INTO DataFile(NameFile, Path, Type, File, wwwPath, IdUser) VALUES ('{file.FileName}','{path}', '{type}', '{file.ContentType}', '{wwwPath}', '{id}')";
                            Console.WriteLine(sql);
                            db.executeQuery(sql);
                            Console.WriteLine("sql");
                        }
                        else if (file.ContentType.StartsWith("application"))
                        {
                            if (datafile.Path == null)
                            {
                                path = "/uploads";
                            }
                            else if (datafile.Path != null)
                            {
                                path = "/uploads" + datafile.Path;
                            }
                            string wwwPath = "http://localhost:5000/" + path + "/" + file.FileName;
                            type = "application";
                            string sql = $"INSERT INTO DataFile(NameFile, Path, Type, File, wwwPath, IdUser) VALUES ('{file.FileName}','{path}', '{type}', '{file.ContentType}', '{wwwPath}', '{id}')";
                            db.executeQuery(sql);
                        }
                        else if (file.ContentType.StartsWith("video"))
                        {
                            if (datafile.Path == null)
                            {
                                path = "/uploads";
                            }
                            else if (datafile.Path != null)
                            {
                                path = "/uploads" + datafile.Path;
                            }
                            string wwwPath = "http://localhost:5000/" + path + "/" + file.FileName;
                            type = "video";
                            string sql = $"INSERT INTO DataFile(NameFile, Path, Type, File, wwwPath, IdUser) VALUES ('{file.FileName}','{path}', '{type}', '{file.ContentType}', '{wwwPath}', '{id}')";
                            db.executeQuery(sql);
                        }
                        else
                        {
                            if (datafile.Path == null)
                            {
                                path = "/uploads";
                            }
                            else if (datafile.Path != null)
                            {
                                path = "/uploads" + datafile.Path;
                            }
                            type = "unknow";
                            string sql = $"INSERT INTO DataFile(NameFile, Path, Type, File , IdUser) VALUES ('{file.FileName}','{path}', '{type}', '{file.ContentType}', '{id}')";
                            db.executeQuery(sql);
                        }

                    }
                }
                res.msg = "okay";
                return res;
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                res.data = datafile.Id;
                return res;
            }
        }

        [HttpGet]
        [Route("download/{nameFile}")]
        public IActionResult downloadFile([FromQuery] string path, string nameFile)
        {
            // FileResult
            string startupPath = Environment.CurrentDirectory;
            // Console.WriteLine(startupPath);
            ResponseErr res = new ResponseErr();
            // return File(@"/FolderData/uploads/duck.png", "image/png", "duck.png");
            // +"/wwwroot" + datafile.Path
            try
            {
                IFileProvider provider = new PhysicalFileProvider(startupPath + "/wwwroot" + path);
                IFileInfo fileInfo = provider.GetFileInfo(nameFile);
                var readStream = fileInfo.CreateReadStream();
                var mimeType = "image/png";
                return File(readStream, mimeType, nameFile);
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;

                return BadRequest(res);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("createFolder")]
        public ResponseErr createFolderDate([FromBody] DataFile datafile)
        {
            string startupPath = Environment.CurrentDirectory;
            var db = new ConMySQL();
            // Console.WriteLine(startupPath);
            ResponseErr res = new ResponseErr();
            try
            {
                Request.Headers.TryGetValue("Authorization", out var token);
                token = ((string)token).Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken decodedValue = handler.ReadJwtToken(token);
                List<Claim> claimsList = decodedValue.Claims.ToList();
                var id = claimsList.Find(x => x.Type == "unique_name").Value;


                string path = startupPath + "/wwwroot" + datafile.Path + "/" + datafile.NameFile;
                // Console.WriteLine(path);
                Directory.CreateDirectory(path);
                // Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
                res.msg = "okay";
                string sql = $"INSERT INTO DataFile(NameFile, Path, Type, IdUser) VALUES ('{datafile.NameFile}','{datafile.Path}', 'Folder', '{id}')";
                db.executeQuery(sql);
                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                res.msg = e.Message;
                return res;

            }
        }

        [Authorize]
        [HttpPost]
        [Route("deleteFolder")]
        public ResponseErr deleteFolder([FromBody] DataFile datafile)
        {
             Request.Headers.TryGetValue("Authorization", out var token);
                token = ((string)token).Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken decodedValue = handler.ReadJwtToken(token);
                List<Claim> claimsList = decodedValue.Claims.ToList();
                var id = claimsList.Find(x => x.Type == "unique_name").Value;


            string startupPath = Environment.CurrentDirectory;
            // Console.WriteLine(startupPath);
            ResponseErr res = new ResponseErr();
            // + datafile.Path + "/" + datafile.NameFile
            string path = startupPath + "/wwwroot" + datafile.Path + "/" + datafile.NameFile;
            string pathdel = datafile.Path + "/" + datafile.NameFile;
            // Console.WriteLine(path);
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            var db = new ConMySQL();
            try
            {

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                //  and IdUser = '{id}'
                string sql = $"Delete From datafile Where Path like '{pathdel}%' and IdUser = '{id}'";
                db.delete(sql);
                Console.WriteLine(sql);

                string sqlId = $"Delete From datafile Where ID = {datafile.Id}";
                db.delete(sqlId);

                string sqlSup = $"Delete From datafile Where MainFolder = {datafile.Id}";
                db.delete(sqlSup);

                Directory.Delete(path);
                Console.WriteLine("The directory was delete successfully at {0}.", Directory.GetCreationTime(path));
                res.msg = "okay";
                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                res.msg = e.Message;
                return res;

                // Directory.Delete(subPath);
            }
        }

        [HttpPost]
        [Route("deleteFile")]
        public ResponseErr deleteFile([FromBody] DataFile datafile)
        {
            var db = new ConMySQL();
            string startupPath = Environment.CurrentDirectory;
            // Console.WriteLine(startupPath);
            ResponseErr res = new ResponseErr();
            // + "FolderData" + datafile.Path + datafile.NameFile
            try
            {
                var fileInfo = new System.IO.FileInfo(startupPath + "/wwwroot" + datafile.Path + "/" + datafile.NameFile);
                fileInfo.Delete();
                db.Open();
                string sqlId = $"Delete From datafile Where ID = {datafile.Id}";
                db.execute(sqlId);
                string sqlsup = $"Delete From datafile Where MainFolder = {datafile.Id}";
                db.execute(sqlsup);
                db.Close(); 
                res.msg = "Okay";
                return res;
            }
            catch (Exception e)
            {
                res.msg = e.Message;
                return res;
            }
        }

        [HttpGet]
        [Route("downloadFolderZip/{nameFile}")]
        public IActionResult downloadFolderZip([FromQuery] string path, string nameFile)
        {
            string startupPath = Environment.CurrentDirectory;
            string startPath = startupPath + "/wwwroot" + path + "/" + nameFile;
            string zipPath = startupPath + "/FolderData/uploads/" + nameFile + ".zip";

            ZipFile.CreateFromDirectory(startPath, zipPath);

            // IFileProvider provider = new PhysicalFileProvider(startupPath + "/FolderData/uploads");
            // IFileInfo fileInfo = provider.GetFileInfo(nameFile  + ".zip");
            // var readStream = fileInfo.CreateReadStream();
            // var mimeType = "application/zip";
            // var eiei = File(readStream, mimeType, nameFile+ ".zip");

            //     HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            // response.Content = new StreamContent(new FileStream(startupPath + "/FolderData/uploads/"+ nameFile  + ".zip", FileMode.Open, FileAccess.Read));
            // response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            // response.Content.Headers.ContentDisposition.FileName = nameFile  + ".zip";
            // response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");



            byte[] bytes;
            using (FileStream file = new FileStream(startupPath + "/FolderData/uploads/" + nameFile + ".zip", FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
            }
            var fileInfodelete = new System.IO.FileInfo(startupPath + "/FolderData/uploads/" + nameFile + ".zip");
            fileInfodelete.Delete();
            return new FileContentResult(bytes, "application/zip")
            {
                FileDownloadName = nameFile + ".zip"
            };

            // var fileInfodelete = new System.IO.FileInfo(startupPath + "/FolderData/uploads/" + nameFile + ".zip");
            // fileInfodelete.Delete();

            // ;       return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("Share")]
        public IActionResult Share([FromBody] DataFile data,[FromQuery]string username){
            var db = new ConMySQL();
            db.Open();

                            int test = 0;


            Request.Headers.TryGetValue("Authorization", out var token);
                token = ((string)token).Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken decodedValue = handler.ReadJwtToken(token);
                List<Claim> claimsList = decodedValue.Claims.ToList();
                var id = claimsList.Find(x => x.Type == "unique_name").Value;
            try
            {
                
                string sqlMainUser = string.Format("SELECT * FROM User WHERE Id = '{0}'", id);
                DataTable SqlMainData = db.get(sqlMainUser);
                User objMainUser = new User();
                foreach (DataRow dr in SqlMainData.Rows)
                    {
                        // objMainUser.id = Convert.ToInt32(dr["id"]);
                        objMainUser.email = dr["email"].ToString();
                    }
                
                // Console.WriteLine(data.Type);
                if(data.Type != "Folder"){
                
                string sqlUser = string.Format("SELECT * FROM User WHERE UserName = '{0}'", username);
                DataTable SqlData = db.get(sqlUser);
                User objUser = new User();
                foreach (DataRow dr in SqlData.Rows)
                    {
                        objUser.id = Convert.ToInt32(dr["id"]);
                        objUser.email = dr["email"].ToString();
                    }

                Console.WriteLine(objUser.id);
                // Console.WriteLine(data.Path);
                // Console.WriteLine(id);
                string sql = string.Format("SELECT * FROM DataFile WHERE NameFile = '{0}'and Path = '{1}' and IdUser = {2} and Share = '0'", data.NameFile, data.Path, id);
                // Console.WriteLine(sql);
                DataTable SqlDataSet = db.get(sql);
                // Console.WriteLine("SqlDataSet");
                DataFile obj = new DataFile();
                // Console.WriteLine("1");
                foreach (DataRow dr in SqlDataSet.Rows)
                    {
                        obj.Id = Convert.ToInt32(dr["id"]);
                        obj.NameFile = dr["namefile"].ToString();
                        obj.Path = dr["path"].ToString();
                        obj.Type = dr["type"].ToString();
                        obj.File = dr["file"].ToString();
                        obj.wwwPath = dr["wwwpath"].ToString();
                        obj.IdUser = Convert.ToInt32(dr["iduser"]);
                    }

                


                Console.WriteLine(obj.Id);
                string sqlz = string.Format("SELECT * FROM DataFile WHERE Share = '{0}' and MainFolder = '{1}'", objUser.id, obj.Id);
                DataTable SqlDataz = db.get(sqlz);
                DataFile objz = new DataFile();
                // List<DataFile> list_result = new List<DataFile>();
                foreach (DataRow dr in SqlDataz.Rows)
                    {
                        objz.Id = Convert.ToInt32(dr["id"]);
                        objz.NameFile = dr["namefile"].ToString();
                        objz.Path = dr["path"].ToString();
                        objz.Type = dr["type"].ToString();
                        objz.File = dr["file"].ToString();
                        objz.wwwPath = dr["wwwpath"].ToString();
                        objz.IdUser = Convert.ToInt32(dr["iduser"]);
                    }
                if(objz.Id == 0){
                    if(obj.NameFile != null){
                        string sqlShare = $"INSERT INTO DataFile(NameFile, Path, Type, wwwPath, IdUser, Share, MainFolder) VALUES ('{obj.NameFile}','{obj.Path}', '{obj.Type}', '{obj.wwwPath}', '{obj.IdUser}', '{objUser.id}',{obj.Id})";
                        db.execute(sqlShare);
                        Console.WriteLine(sqlShare);

                        // string to = objUser.email;
                        // Console.WriteLine("http://localhost:8080/Share/"+ HttpUtility.UrlEncode(obj.Path.Replace("/uploads", "")) + "/"+ obj.NameFile);
                        string linkshare = "http://localhost:8080/Share/"+ HttpUtility.UrlEncode(obj.Path.Replace("/uploads", "")) + "/"+ obj.NameFile;
                        Email mm = new Email();
                        mm.From = objMainUser.email;
                        mm.To = objUser.email;
                        mm.Subject = "มีการแชร์ไฟล์มาถึงคุณ";
                        
                        SendShareEmail(mm,linkshare);


                    }
                db.Close();
                return Ok("โอเค แบบ File");
                }else{
                    return Ok("แชร์ไปแล้ว");
                }


                }
                // Console.WriteLine(.Length);






                else if(data.Type == "Folder")
                {
                    
                string sqlUser = string.Format("SELECT * FROM User WHERE UserName = '{0}'", username);
                DataTable SqlData = db.get(sqlUser);
                User objUser = new User();
                foreach (DataRow dr in SqlData.Rows)
                    {
                        objUser.id = Convert.ToInt32(dr["id"]);
                        objUser.email = dr["email"].ToString();
                    }

                Console.WriteLine(objUser.id);
                if(objUser.id != 0){
                List<DataFile> list_result = new List<DataFile>();

                string sql = string.Format("SELECT * FROM DataFile WHERE NameFile = '{0}'and Path = '{1}' and IdUser = '{2}' and Share = '{3}'", data.NameFile, data.Path, id, objUser.id);
                Console.WriteLine(sql);
                DataTable SqlDataSet = db.get(sql);
                foreach (DataRow dr in SqlDataSet.Rows)
                    {
                        DataFile obj = new DataFile();
                        obj.Id = Convert.ToInt32(dr["id"]);
                        obj.NameFile = dr["namefile"].ToString();
                        obj.Path = dr["path"].ToString();
                        obj.Type = dr["type"].ToString();
                        obj.File = dr["file"].ToString();
                        obj.wwwPath = dr["wwwpath"].ToString();
                        obj.IdUser = Convert.ToInt32(dr["iduser"]);
                        obj.MainFolder = Convert.ToInt32(dr["mainfolder"]);
                        list_result.Add(obj);
                    }

                    string sqlD = string.Format("SELECT * FROM DataFile WHERE Path like '{0}%' and IdUser = {1} and Share = '{2}' ", data.Path+"/"+data.NameFile, id, objUser.id);
                    Console.WriteLine(sqlD);
                DataTable SqlDataSetA = db.get(sqlD);
                foreach (DataRow dr in SqlDataSetA.Rows)
                    {
                        DataFile obj = new DataFile();
                        obj.Id = Convert.ToInt32(dr["id"]);
                        obj.NameFile = dr["namefile"].ToString();
                        obj.Path = dr["path"].ToString();
                        obj.Type = dr["type"].ToString();
                        obj.File = dr["file"].ToString();
                        obj.wwwPath = dr["wwwpath"].ToString();
                        obj.IdUser = Convert.ToInt32(dr["iduser"]);
                        obj.MainFolder = Convert.ToInt32(dr["mainfolder"]);
                        list_result.Add(obj);
                    }
                    
                if(list_result.Count == 0){
                        // Console.WriteLine("0");
                        Console.WriteLine(list_result.Count);


                List<DataFile> list_resultA = new List<DataFile>();
                string sqlFolder = string.Format("SELECT * FROM DataFile WHERE NameFile = '{0}'and Path = '{1}' and IdUser = {2} and Share = '0'", data.NameFile, data.Path, id);
                DataTable SqlDataFolder = db.get(sqlFolder);
                // Console.WriteLine(SqlDataFolder.Length);
                string linkshare = null;
                foreach (DataRow dr in SqlDataFolder.Rows)
                    {
                        DataFile obj = new DataFile();
                        obj.Id = Convert.ToInt32(dr["id"]);
                        obj.NameFile = dr["namefile"].ToString();
                        obj.Path = dr["path"].ToString();
                        obj.Type = dr["type"].ToString();
                        obj.File = dr["file"].ToString();
                        obj.wwwPath = dr["wwwpath"].ToString();
                        obj.IdUser = Convert.ToInt32(dr["iduser"]);
                        obj.MainFolder = Convert.ToInt32(dr["mainfolder"]);
                        linkshare = "http://localhost:8080/Share/"+ HttpUtility.UrlEncode(obj.Path.Replace("/uploads", "")) + "/"+ obj.NameFile;
                        list_resultA.Add(obj);
                    }

                        

                string sqlC = string.Format("SELECT * FROM DataFile WHERE Path like '{0}%' and IdUser = {1} and Share = '0'", data.Path+"/"+data.NameFile, id);
                DataTable SqlDataSetC = db.get(sqlC);
                foreach (DataRow dr in SqlDataSetC.Rows)
                    {
                        DataFile obj = new DataFile();
                        obj.Id = Convert.ToInt32(dr["id"]);
                        obj.NameFile = dr["namefile"].ToString();
                        obj.Path = dr["path"].ToString();
                        obj.Type = dr["type"].ToString();
                        obj.File = dr["file"].ToString();
                        obj.wwwPath = dr["wwwpath"].ToString();
                        obj.IdUser = Convert.ToInt32(dr["iduser"]);
                        obj.MainFolder = Convert.ToInt32(dr["mainfolder"]);
                        list_resultA.Add(obj);
                    }

                if(SqlDataFolder != null){
                    foreach (DataFile item in list_resultA)
                    {
                        // Console.WriteLine(item.NameFile);
                        string sqlShare = $"INSERT INTO DataFile(NameFile, Path, Type, wwwPath, IdUser, Share, MainFolder) VALUES ('{item.NameFile}','{item.Path}', '{item.Type}', '{item.wwwPath}', '{item.IdUser}', '{objUser.id}',{item.Id})";
                        db.execute(sqlShare);

                    }
                    }
                db.Close();
                // List<DataFile> list_result = new List<DataFile>();
                // foreach (DataRow dr in SqlDataSet.Rows)
                // {
                //     DataFile obj = new DataFile();
                //     obj.Id = Convert.ToInt32(dr["id"]);
                //     obj.NameFile = dr["namefile"].ToString();
                //     obj.Path = dr["path"].ToString();
                //     obj.Type = dr["type"].ToString();
                //     obj.wwwPath = dr["wwwpath"].ToString();
                //     list_result.Add(obj);
                // }
                Email mm = new Email();
                mm.From = objMainUser.email;
                mm.To = objUser.email;
                mm.Subject = "มีการแชร์ไฟล์มาถึงคุณ";
                        
                SendShareEmail(mm,linkshare);

                return Ok("โอเค แบบ Folder 1");
                }
                else if (list_result.Count > 0){

                List<DataFile> list_resultB = new List<DataFile>();
                string sqlFolder = string.Format("SELECT * FROM DataFile WHERE NameFile = '{0}'and Path = '{1}' and IdUser = {2} and Share = '0'", data.NameFile, data.Path, id);
                Console.WriteLine(sqlFolder);
                DataTable SqlDataFolder = db.get(sqlFolder);
                // return Ok(SqlDataFolder);
                // Console.WriteLine(SqlDataFolder.Length);
                string linkshare = null;
                foreach (DataRow dr in SqlDataFolder.Rows)
                    {
                        DataFile obj = new DataFile();
                        obj.Id = Convert.ToInt32(dr["id"]);
                        obj.NameFile = dr["namefile"].ToString();
                        obj.Path = dr["path"].ToString();
                        obj.Type = dr["type"].ToString();
                        obj.File = dr["file"].ToString();
                        obj.wwwPath = dr["wwwpath"].ToString();
                        obj.IdUser = Convert.ToInt32(dr["iduser"]);
                        obj.MainFolder = Convert.ToInt32(dr["mainfolder"]);
                        linkshare = "http://localhost:8080/Share/"+ HttpUtility.UrlEncode(obj.Path.Replace("/uploads", "")) + "/"+ obj.NameFile;

                        list_resultB.Add(obj);
                        // Console.WriteLine("========");
                    }



                string sqlA = string.Format("SELECT * FROM DataFile WHERE Path like '{0}%' and IdUser = {1} and Share = '0'", data.Path+"/"+data.NameFile, id);
                Console.WriteLine(sqlA);
                DataTable SqlDataSetB = db.get(sqlA);
                foreach (DataRow dr in SqlDataSetB.Rows)
                    {
                        DataFile obj = new DataFile();
                        obj.Id = Convert.ToInt32(dr["id"]);
                        obj.NameFile = dr["namefile"].ToString();
                        obj.Path = dr["path"].ToString();
                        obj.Type = dr["type"].ToString();
                        obj.File = dr["file"].ToString();
                        obj.wwwPath = dr["wwwpath"].ToString();
                        obj.IdUser = Convert.ToInt32(dr["iduser"]);
                        obj.MainFolder = Convert.ToInt32(dr["mainfolder"]);
                        list_resultB.Add(obj);
                    }

                                    List<DataFile> list_ = new List<DataFile>();

                    // Console.WriteLine(list_resultB.Count);
                    // return Ok(list_result);
                    foreach (var itemA in list_resultB)
                    {
                        int x = 0;
                        foreach (var itemB in list_result)
                        {
                            // Console.WriteLine(itemA.Id);
                            // Console.WriteLine(itemB.MainFolder);
                            if(itemA.Id == itemB.MainFolder){
                                x = 1;
                            }
                        }
                        if(x != 1){
                        string sqlShare = $"INSERT INTO DataFile(NameFile, Path, Type, wwwPath, IdUser, Share, MainFolder) VALUES ('{itemA.NameFile}','{itemA.Path}', '{itemA.Type}', '{itemA.wwwPath}', '{itemA.IdUser}', '{objUser.id}',{itemA.Id})";
                        db.execute(sqlShare);
                        }

                    }

                    Email mm = new Email();
                    mm.From = objMainUser.email;
                    mm.To = objUser.email;
                    mm.Subject = "มีการแชร์ไฟล์มาถึงคุณ";
                        
                    SendShareEmail(mm,linkshare);

                    // Console.WriteLine("asd");
                    db.Close();

                    return Ok("โอเค แบบ Folder 2");
                }
                }else{
                    return Ok("ไม่มี User นี้");
                }
                // Console.WriteLine("Endline");
                
                }return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private IActionResult SendShareEmail(Email mm, string linkShare=null){
            try{
                // Console.WriteLine("linkShare");
                // var email = new MimeMessage();
                // email.From.Add(MailboxAddress.Parse(mm.From));
                // email.To.Add(MailboxAddress.Parse(mm.To));
                // //"Test Email Subject"
                // email.Subject = mm.Subject;
                // email.Body = new TextPart(TextFormat.Html) { Text = "<h3>ลิ้งค์ที่ไฟล์ที่ถูกแชร์มาถึงคุณ :</h3>"+linkShare};
                // Console.WriteLine(linkShare);

                // // send email
                // using var smtp = new SmtpClient();
                // smtp.Connect("smtp.gmail.com", 465);
                // smtp.Authenticate("testemail.2541@gmail.com", "123456789top");
                // smtp.Send(email);
                // smtp.Disconnect(true);


                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(mm.From));
                email.To.Add(MailboxAddress.Parse(mm.To));
                email.Subject = mm.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = "<h3>ลิ้งค์ที่ไฟล์ที่ถูกแชร์มาถึงคุณ :</h3>"+linkShare};


                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 465);
                smtp.Authenticate("testemail.2541@gmail.com", "123456789top");
                smtp.Send(email);
                smtp.Disconnect(true);

                Console.WriteLine("OK");
                return Ok();
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }

        // [Authorize]
        // [HttpPost]
        // [Route("CheckShare")]
        // public IActionResult CheckShare([FromBody] DataFile data,[FromQuery]string username){
        //     var db = new ConMySQL();
        //         db.Open();

        //         Request.Headers.TryGetValue("Authorization", out var token);
        //         token = ((string)token).Replace("Bearer ", "");
        //         var handler = new JwtSecurityTokenHandler();
        //         JwtSecurityToken decodedValue = handler.ReadJwtToken(token);
        //         List<Claim> claimsList = decodedValue.Claims.ToList();
        //         var id = claimsList.Find(x => x.Type == "unique_name").Value;

        //     try{
       
                
        //         string sqlUser = string.Format("SELECT * FROM User WHERE UserName = '{0}'", username);
        //         DataTable SqlData = db.get(sqlUser);
        //         User objUser = new User();
        //         foreach (DataRow dr in SqlData.Rows)
        //             {
        //                 objUser.id = Convert.ToInt32(dr["id"]);
        //             }

        //         List<DataFile> list_result = new List<DataFile>();

        //         string sql = string.Format("SELECT * FROM DataFile WHERE NameFile = '{0}'and Path = '{1}' and IdUser = '{2}' and Share = '{3}'", data.NameFile, data.Path, id, objUser.id);
        //         Console.WriteLine(sql);
        //         DataTable SqlDataSet = db.get(sql);
        //         foreach (DataRow dr in SqlDataSet.Rows)
        //             {
        //                 DataFile obj = new DataFile();
        //                 obj.Id = Convert.ToInt32(dr["id"]);
        //                 obj.NameFile = dr["namefile"].ToString();
        //                 obj.Path = dr["path"].ToString();
        //                 obj.Type = dr["type"].ToString();
        //                 obj.File = dr["file"].ToString();
        //                 obj.wwwPath = dr["wwwpath"].ToString();
        //                 obj.IdUser = Convert.ToInt32(dr["iduser"]);
        //                 list_result.Add(obj);
        //             }

        //             string sql = string.Format("SELECT * FROM DataFile WHERE Path like '{0}%' and IdUser = {1} and Share = '{2}' ", data.Path+"/"+data.NameFile, id, objUser.id);
        //         DataTable SqlDataSetA = db.get(sql);
        //         foreach (DataRow dr in SqlDataSetA.Rows)
        //             {
        //                 DataFile obj = new DataFile();
        //                 obj.Id = Convert.ToInt32(dr["id"]);
        //                 obj.NameFile = dr["namefile"].ToString();
        //                 obj.Path = dr["path"].ToString();
        //                 obj.Type = dr["type"].ToString();
        //                 obj.File = dr["file"].ToString();
        //                 obj.wwwPath = dr["wwwpath"].ToString();
        //                 obj.IdUser = Convert.ToInt32(dr["iduser"]);
        //                 list_result.Add(obj);
        //             }
        //         if(list_result.Count == 0){

        //             return Ok("Good Share");

        //         }else if (list_result > 0){
        //             return Ok(list_result.Count);
        //         }
        //         return BadRequest("Bad");
        //     }catch (Exception ex){
        //         return BadRequest(ex.Message);
        //     }

        // }
    }
}

