using System.IO;
using System;
using aus_backend_core.entity;
namespace aus_backend_core.service
{
    public interface ISaveService
    {
        public void ISaveService();


        public AusBaseResponse saveTemplate(string fileCode,string template);

        public AusBaseResponse getTemplate(string templateCode);

        public MemoryStream saveDataToTemplate(AllReqData allReq);

    }
}
