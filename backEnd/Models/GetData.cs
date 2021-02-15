using System.Data;
using System.Threading.Tasks;
using MySqlConnector;
using Microsoft.AspNetCore.Http;

namespace backEnd.Models
{
    public class GetData
    {
        public string Path { get; set; }
        public int IdUser { get; set; }

    }
}