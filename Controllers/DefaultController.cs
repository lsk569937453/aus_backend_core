using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aus_backend_core.entity;
using aus_backend_core.service;
using Microsoft.AspNetCore.Http;
using System.IO;
using aus_backend_core.utils;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace aus_backend_core
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
        public JsonResult saveTemplate([FromBody]SaveTemplateReq req)
        {
            AusBaseResponse ausBaseResponse = saveService.saveTemplate(req.fileCode, req.PostData);
            return Json(ausBaseResponse);
        }
        [HttpPost]
        [Route("api/initPageUseTemplate")]
        public JsonResult initPageUseTemPlate([FromBody]InitPageReq req)
        {
            AusBaseResponse ausBaseResponse = saveService.getTemplate(req.templateCode);
            return Json(ausBaseResponse);
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
            catch (Exception e)
            {

                res.responseCode = -1;
            }

            return Json(res);
        }

        [HttpPost]
        [Route("api/writeAllTemplateData")]

        public async Task<IActionResult> DownloadFile([FromBody] WriteAllTemplateReq req)
        {
            
            var fileCode = req.fileCode;

            MemoryStream pdfMemoryStream=fileService.writeAllTemPlate(fileCode);
            Response.Headers.Add("Content-Disposition", "attachment; filename=" + fileCode);
            return new FileStreamResult(pdfMemoryStream, "application/octet-stream");//文件流方式，指定文件流对应的ContenType。
        }

        [HttpPost]
        [Route("api/saveDataToTemplate")]
        public async Task<IActionResult> saveDataToTemplate([FromBody] AllReqData req)
        {
            
          

            MemoryStream pdfMemoryStream=saveService.saveDataToTemplate(req);
            Response.Headers.Add("Content-Disposition", "attachment; filename=" + req.fileCode);
            return new FileStreamResult(pdfMemoryStream, "application/octet-stream");//文件流方式，指定文件流对应的ContenType。
        }


    }
}