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
    public class DataFileController : ControllerBase
    {
        private readonly ILogger<DataFileController> _logger;


        public DataFileController(ILogger<DataFileController> logger)
        {
            _logger = logger;
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
                string sql = $"Delete From datafile Where ID = {datafile.Id}";
                db.delete(sql);
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

        [HttpPost]
        [Route("deleteFolder")]
        public ResponseErr deleteFolder([FromBody] DataFile datafile)
        {
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

                string sql = $"Delete From datafile Where Path like '{pathdel}%'";
                db.delete(sql);
                Console.WriteLine(sql);

                string sqlId = $"Delete From datafile Where ID = {datafile.Id}";
                db.delete(sqlId);

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
                string sqlId = $"Delete From datafile Where ID = {datafile.Id}";
                db.delete(sqlId);
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

            Request.Headers.TryGetValue("Authorization", out var token);
                token = ((string)token).Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken decodedValue = handler.ReadJwtToken(token);
                List<Claim> claimsList = decodedValue.Claims.ToList();
                var id = claimsList.Find(x => x.Type == "unique_name").Value;
            try
            {
                if(data.Type != "Folder"){
                
                string sqlUser = string.Format("SELECT * FROM User WHERE UserName = '{0}'", username);
                DataTable SqlData = db.get(sqlUser);
                User objUser = new User();
                foreach (DataRow dr in SqlData.Rows)
                    {
                        objUser.id = Convert.ToInt32(dr["id"]);
                    }

                // Console.WriteLine(data.NameFile);
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
                        obj.NameFile = dr["namefile"].ToString();
                        obj.Path = dr["path"].ToString();
                        obj.Type = dr["type"].ToString();
                        obj.File = dr["file"].ToString();
                        obj.wwwPath = dr["wwwpath"].ToString();
                        obj.IdUser = Convert.ToInt32(dr["iduser"]);
                    }
                // Console.WriteLine("2");
                db.Close();
                    if(obj.NameFile != null){
                        string sqlShare = $"INSERT INTO DataFile(NameFile, Path, Type, wwwPath, IdUser, Share) VALUES ('{obj.NameFile}','{obj.Path}', '{obj.Type}', '{obj.wwwPath}', '{obj.IdUser}', '{objUser.id}')";
                        db.executeQuery(sqlShare);
                    }

                return Ok("Eiei");
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
                    }
                List<DataFile> list_result = new List<DataFile>();

                string sqlFolder = string.Format("SELECT * FROM DataFile WHERE NameFile = '{0}'and Path = '{1}' and IdUser = {2} and Share = '0'", data.NameFile, data.Path, id);
                DataTable SqlDataFolder = db.get(sqlFolder);
                // Console.WriteLine(SqlDataFolder.Length);
                foreach (DataRow dr in SqlDataFolder.Rows)
                    {
                        DataFile obj = new DataFile();
                        obj.NameFile = dr["namefile"].ToString();
                        obj.Path = dr["path"].ToString();
                        obj.Type = dr["type"].ToString();
                        obj.File = dr["file"].ToString();
                        obj.wwwPath = dr["wwwpath"].ToString();
                        obj.IdUser = Convert.ToInt32(dr["iduser"]);
                        list_result.Add(obj);
                    }

                string sql = string.Format("SELECT * FROM DataFile WHERE Path like '{0}%' and IdUser = {1} and Share = '0'", data.Path+"/"+data.NameFile, id);
                DataTable SqlDataSet = db.get(sql);
                foreach (DataRow dr in SqlDataSet.Rows)
                    {
                        DataFile obj = new DataFile();
                        obj.NameFile = dr["namefile"].ToString();
                        obj.Path = dr["path"].ToString();
                        obj.Type = dr["type"].ToString();
                        obj.File = dr["file"].ToString();
                        obj.wwwPath = dr["wwwpath"].ToString();
                        obj.IdUser = Convert.ToInt32(dr["iduser"]);
                        list_result.Add(obj);
                    }

                if(SqlDataFolder != null){
                    foreach (DataFile item in list_result)
                    {
                        Console.WriteLine(item.NameFile);
                        string sqlShare = $"INSERT INTO DataFile(NameFile, Path, Type, wwwPath, IdUser, Share) VALUES ('{item.NameFile}','{item.Path}', '{item.Type}', '{item.wwwPath}', '{item.IdUser}', '{objUser.id}')";
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
                return Ok(list_result);
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

