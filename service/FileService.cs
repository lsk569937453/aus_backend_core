using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using aus_backend_core.utils;
using iTextSharp.text.pdf;
using aus_backend_core.entity;
namespace aus_backend_core.service
{
    public class FileService : IFileService
    {
        public FileService()
        {
        }
        public string uploadFile(IFormFile file)
        {

            string currentDirectory = CommonUtils.getCurrentDir();
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
                PDFEntityConfig pDFEntity=new PDFEntityConfig();
                pDFEntity.filePath=filePath;

                CommonConstantsUtil.PDFCONFIG.GetOrAdd(guid, pDFEntity);

            }


            return guid;

        }

        public MemoryStream writeAllTemPlate(string fileCode)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                PDFEntityConfig pDFEntity = CommonConstantsUtil.PDFCONFIG[fileCode];
                string path=pDFEntity.filePath;
                PdfReader.unethicalreading = true;
                PdfReader reader = new PdfReader(path);


                PdfStamper stamper = new PdfStamper(reader, memoryStream,'\0', false);

                stamper.Writer.CloseStream = false;
                AcroFields pdfFormFields = stamper.AcroFields;

                int i = 0;

                foreach (var item in pdfFormFields.Fields)
                {
                   string value=i.ToString();

                    Console.WriteLine("{0}", item.Key);
                    pdfFormFields.SetField(item.Key, value);
                    pDFEntity.directory.Add(value,item.Key);
                    i++;
                }

                
                stamper.FormFlattening = true;
                stamper.FormFlattening  = true;
                stamper.Writer.CloseStream = false;
                stamper.Close();
                memoryStream.Position = 0;
                reader.Close();
         



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return memoryStream;
        }
    }
}
