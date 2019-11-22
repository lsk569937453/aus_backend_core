using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using netcoreTest.utils;

namespace netcoreTest.service
{
    public class FileService:IFileService
    {
        public FileService()
        {
        }
        public string uploadFile(IFormFile file) {
          
            string currentDirectory =CommonUtils.getCurrentDir();
            string guid = CommonUtils.Getuuid();

            if (file != null)
            {

                //文件名称
                string projectFileName = file.FileName;

                //上传的文件的路径
                string filePath = currentDirectory + $@"/{guid}.pdf";
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
               
            }
            return guid;
          
        }
    }
}
