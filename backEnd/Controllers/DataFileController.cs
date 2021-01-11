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
        [HttpGet]
        [Route("getData/{path}")]
        public IEnumerable<DataFile> GetDataFiles(string path)
        {
            ResponseErr res = new ResponseErr();
            DataFile data = new DataFile();
            var db = new ConMySQL();
            List<DataFile> list_result = new List<DataFile>();
            try{
            // string sql = $"SELECT * FROM DataFile WHERE Path={path}";
            string sql = string.Format("SELECT * FROM DataFile WHERE Path = '{0}'", path);
            Console.WriteLine(sql);
            DataTable SqlDataSet = db.getData(sql);
            
            foreach(DataRow dr in SqlDataSet.Rows){
                DataFile obj = new DataFile();
                obj.Id = Convert.ToInt32(dr["id"]);
                obj.NameFile = dr["namefile"].ToString();
                obj.Path = dr["path"].ToString();
                obj.Type = dr["type"].ToString();
                list_result.Add(obj);
            }
            }catch(Exception ex){
                res.msg = ex.Message;
                res.listdata = list_result;
                Console.WriteLine(res);
            }
            
            // Console.WriteLine(JsonConvert.SerializeObject(list_result, Formatting.Indented));
            
            return list_result;
        }

        [HttpPost]
        [Route("post")]
        public IEnumerable<ActionResult<ResponseErr>> PostDataFile([FromBody] DataFile datafile)
        {
            // int test = 3;

            ResponseErr res = new ResponseErr();
            try{
                var db = new ConMySQL();
                string sql = $"INSERT INTO DataFile(NameFile, Path, Type) VALUES ('{datafile.NameFile}','{datafile.Path}', '{datafile.Type}')";
                db.executeQuery(sql);
                res.msg = "okay";
            }catch(Exception ex){
                res.msg = ex.Message;
                res.data = datafile;
                // return res;
            }
                yield return res;
        }

        [HttpDelete]
        [Route("delete")]
        public IEnumerable<ActionResult<ResponseErr>> DeleteDataFile([FromBody] DataFile datafile)
        {
            ResponseErr res = new ResponseErr();
            try{
                var db = new ConMySQL();
                string sql = $"Delete From datafile Where ID = {datafile.Id}";
                db.delete(sql);
                res.msg = "okay";
            }catch(Exception ex){
                res.msg = ex.Message;
                res.data = datafile.Id;
            }
            yield return res;
        }

        [HttpPatch]
        [Route("update/{Id}")]
        public IEnumerable<ActionResult<ResponseErr>> UpdateDataFile([FromBody] DataFile datafile,int Id)
        {
            ResponseErr res = new ResponseErr();
            try{
                var db = new ConMySQL();
                string sql = $"UPDATE datafile SET NameFile='{datafile.NameFile}',Path='{datafile.Path}',Type='{datafile.Type}' WHERE Id='{Id}'";
                db.executeQuery(sql);
                res.msg = "okay";
            }catch(Exception ex){
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
            try{
                var db = new ConMySQL();
                BinaryReader br = new BinaryReader(datafile.filedata[0].OpenReadStream());
                int Type = 1;
                byte[] byte_data = br.ReadBytes((int)datafile.filedata[0].OpenReadStream().Length);
                string sql = $"INSERT INTO DataFile(NameFile, Path, Type, File) VALUES ('{datafile.filedata[0].FileName}','{datafile.Path}', '{Type}', '{datafile.filedata[0].ContentType}')";
                db.executeQuery(sql);
                res.msg = "okay";
            }catch(Exception ex){
                res.msg = ex.ToString();
                res.data = datafile.Id;
            }
            yield return res;
        }

        [HttpPost]
        [Route("putfile2")]
        public async Task<ResponseErr> UploadDataFile2([FromForm] DataFile datafile)
        {
            ResponseErr res = new ResponseErr();
            try{
                string type = null;
                string path = null;
                var db = new ConMySQL();
                string uploads = Path.Combine(@"FolderData", "uploads");
                foreach (IFormFile file in datafile.filedata)
                {
                    if (file.Length > 0) 
                    {
                        string filePath = Path.Combine(uploads, file.FileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create)) {
                        await file.CopyToAsync(fileStream);
                        }
                        // Console.WriteLine(file.ContentType.Contains("image"));
                        if (file.ContentType.StartsWith("image"))
                        {
                            path = "/" + file.FileName;
                            type = "image";
                            string sql = $"INSERT INTO DataFile(NameFile, Path, Type, File) VALUES ('{file.FileName}','{path}', 'image', '{file.ContentType}')";
                            db.executeQuery(sql);
                        }else if (file.ContentType.StartsWith("application"))
                        {
                            path = "/" + file.FileName;
                            type = "application";
                            string sql = $"INSERT INTO DataFile(NameFile, Path, Type, File) VALUES ('{file.FileName}','{path}', 'application', '{file.ContentType}')";
                            db.executeQuery(sql);
                        }else{
                            path = "/" + file.FileName;
                            type = "unknow";
                            string sql = $"INSERT INTO DataFile(NameFile, Path, Type, File) VALUES ('{file.FileName}','{path}', 'application', '{file.ContentType}')";
                            db.executeQuery(sql);
                        }
                        
                    }
                }
                res.msg = "okay";
                return res;
            }catch(Exception ex){
                res.msg = ex.ToString();
                res.data = datafile.Id;
                return res;
            }
        }

        [HttpGet]
        [Route("download/{nameFile}")]
        public FileResult downloadFile([FromQuery] DataFile datafile, string nameFile)
        {
            string startupPath = Environment.CurrentDirectory;
            Console.WriteLine(startupPath);
            // return File(@"/FolderData/uploads/duck.png", "image/png", "duck.png");
            
            IFileProvider provider = new PhysicalFileProvider(startupPath +"/FolderData/uploads" + datafile.Path);
            IFileInfo fileInfo = provider.GetFileInfo(nameFile);
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "image/png";
            return File(readStream, mimeType, nameFile);
        }

        [HttpPost]
        [Route("createFolder")]
        public ResponseErr createFolderDate([FromBody] DataFile datafile)
        {
            string startupPath = Environment.CurrentDirectory;
            // Console.WriteLine(startupPath);
            ResponseErr res = new ResponseErr();
            string path = startupPath+"/FolderData/uploads" + datafile.Path;
            Console.WriteLine(path);
            try
        {
            Directory.CreateDirectory(path);
            Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
            res.msg = "okay";
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
            string path = startupPath + "/FolderData/uploads" + datafile.Path;
            Console.WriteLine(path);
            try
        {
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
            string startupPath = Environment.CurrentDirectory;
            // Console.WriteLine(startupPath);
            ResponseErr res = new ResponseErr();
            // + "FolderData" + datafile.Path + datafile.NameFile
            try{
                var fileInfo = new System.IO.FileInfo(startupPath + "/FolderData" + datafile.Path + datafile.NameFile);
                fileInfo.Delete();
                res.msg = "Okay";
                return res;
            }catch(Exception e){
                res.msg = e.Message;
                return res;
            }
        }




        // public async Task<ResponseErr> DownloadDataFileAsync()
        // {
        //     ResponseErr res = new ResponseErr();
        //     var client = new HttpClient();
        //     var response = await client.GetAsync("http://localhost:5000/backEnd/FolderData/uploads/duck.png");

        //     using (var stream = await response.Content.ReadAsStreamAsync())
        // {
        //     var fileInfo = new FileInfo("duckA.png");
        //     using (var fileStream = fileInfo.OpenWrite())
        //     {
        //         await stream.CopyToAsync(fileStream);
        //     }
        // }
        // return res;
        // }




        // public HttpResponseMessage DownloadDataFile(string fileName)
        // {
        //     var fileDownloadName = "duck.pnga";

        //     // Build the file contents.
        //     var fileContents = "FolderData/uploads";

        //     // Set the headers to indicate we are returning a file.
        //     var downloadContent = new StringContent(fileContents);
        //     downloadContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //     downloadContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //     {
        //         FileName = fileDownloadName
        //     };

        //     // Respond with the file.
        //     return new HttpResponseMessage(HttpStatusCode.OK)
        //     {
        //         Content = downloadContent
        //     };
        // }
    }

}