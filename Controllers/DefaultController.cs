using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using netcoreTest.entity;
using netcoreTest.service;
using Microsoft.AspNetCore.Http;
using System.IO;
using netcoreTest.utils;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace netcoreTest
{
    [Route("/")]
    public class DefaultController : Controller
    {
        private ISaveService saveService;

        private IFileService fileService;
        public DefaultController(ISaveService companyService, IFileService file)
        {
            saveService = companyService;
            fileService = file;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    
        [HttpGet("test")]
        [Route("api/test")]
        public string Submit()
        {
            saveService.ISaveService();
            return "a";
        }

        [HttpPost]
        [Route("api/saveTemplate")]
        public string saveTemplate([FromBody]SaveTemplateReq postData)
        {
            
            Console.WriteLine(postData.PostData);
            saveService.saveTemplate(postData.PostData);
            return "a";
        }
        [HttpPost]
        [Route("api/upload")]
        public JsonResult uploadProject(IFormFile file)
        {
            AusBaseResponse res = new AusBaseResponse();
            try
            {
                string guid = fileService.uploadFile(file);


                res.responseCode = 0;
                res.responseBody = guid;
            }
            catch (Exception e) {

                res.responseCode = -1;
            }

            return Json(res);
        }
    }
}
