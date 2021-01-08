using System.Data;
using System.Threading.Tasks;
using MySqlConnector;
using Microsoft.AspNetCore.Http;

namespace backEnd.Models
{
    public class DataFile
    {
        public int Id { get; set; }
        public string NameFile { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public dynamic File { get; set; }
        public System.Collections.Generic.List<IFormFile> filedata { get; set; }

    }
}