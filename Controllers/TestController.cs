using System.IO;
using Microsoft.AspNetCore.Mvc;
using aus_backend_core.service;
using aus_backend_core.entity;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
namespace aus_backend_core.Controllers
{
    [Route("api/test")]
    public class TestController: Controller
    {
         private ITestTemplateService templateService;
        public TestController(ITestTemplateService templateServiceTemp)
        {
            templateService = templateServiceTemp;
           
        }
        [HttpPost]
        [Route("testTemplate")]
        public async Task<IActionResult> saveTestToTemplate([FromBody] TestTemplateReq req)
        {
            MemoryStream pdfMemoryStream=templateService.saveDataToTemplate(req);
            Response.Headers.Add("Content-Disposition", "attachment; filename=" + req.fileCode);
            return new FileStreamResult(pdfMemoryStream, "application/octet-stream");//文件流方式，指定文件流对应的ContenType。
        }
         [HttpPost]
        [Route("getTemplateField")]
        public JsonResult saveTemplate([FromBody]GetTemplateFieldReq req)
        {
            AusBaseResponse ausBaseResponse = templateService.getRadioTemplate(req);
            return Json(ausBaseResponse);
        }
    }
}