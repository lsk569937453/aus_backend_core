using System.IO;
using System;
using Microsoft.AspNetCore.Http;

namespace aus_backend_core.service
{
    public interface IFileService
    {
        public string uploadFile(IFormFile file);

        public MemoryStream writeAllTemPlate(string fileCode);
    }
}
