using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace backEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetPathController : Controller
    {
        private IHostingEnvironment Environment;
 
        public GetPathController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        [HttpGet]
        [Route("GetPhoto")]
         public string GetPhoto() {

            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;
            Console.WriteLine(wwwPath);
 
        return wwwPath;
         }
    }
}