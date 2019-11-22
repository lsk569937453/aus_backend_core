using System;
using Microsoft.AspNetCore.Http;

namespace netcoreTest.service
{
    public interface IFileService
    {
        public string uploadFile(IFormFile file);
    }
}
