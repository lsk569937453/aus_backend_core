using System.IO;
using aus_backend_core.entity;
namespace aus_backend_core.service
{
    public interface ITestTemplateService
    {
        public MemoryStream saveDataToTemplate(TestTemplateReq allReq);

        public AusBaseResponse getRadioTemplate(GetTemplateFieldReq get);

    }
}