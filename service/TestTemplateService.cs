using System.Collections.Generic;
using System;
using System.IO;
using aus_backend_core.entity;
using aus_backend_core.utils;
using iTextSharp.text.pdf;
namespace aus_backend_core.service
{
    public class TestTemplateService : ITestTemplateService
    {
        public MemoryStream saveDataToTemplate(TestTemplateReq allReq)
        {
            MemoryStream resultStream = new MemoryStream();
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                PDFEntityConfig pDFEntity = CommonConstantsUtil.PDFCONFIG[allReq.fileCode];
                string path = pDFEntity.filePath;


                PdfReader reader = new PdfReader(path, null);

                PdfReader.unethicalreading = true;
                PdfStamper stamper = new PdfStamper(reader, memoryStream, '\0', false);

                stamper.Writer.CloseStream = false;
                AcroFields pdfFormFields = stamper.AcroFields;



                pdfFormFields.SetField(allReq.fieldName, allReq.fieldValue);


                stamper.FormFlattening = true;
                stamper.Writer.CloseStream = false;
                stamper.Close();
                memoryStream.Position = 0;
                reader.Close();




                //You have to rewind the MemoryStream before copying
                // memoryStream.Seek(0, SeekOrigin.Begin);


                memoryStream.CopyTo(resultStream);
                memoryStream.Close();
                resultStream.Position = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resultStream;
        }


        public AusBaseResponse getRadioTemplate(GetTemplateFieldReq getTemplateFieldReq)
        {
            AusBaseResponse ausBaseResponse=new AusBaseResponse();
            List<GetTemplateFieldRes> getTemplateFieldResList = new List<GetTemplateFieldRes>();

            string fileCode = getTemplateFieldReq.fileCode;
            PDFEntityConfig pDFEntityConfig = CommonConstantsUtil.PDFCONFIG[fileCode];

            string path = pDFEntityConfig.filePath;
            PdfReader.unethicalreading = true;
            PdfReader reader = new PdfReader(path);

            MemoryStream memory = new MemoryStream();

            PdfStamper stamper = new PdfStamper(reader, memory, '\0', false);

            stamper.Writer.CloseStream = false;
            AcroFields pdfFormFields = stamper.AcroFields;

            foreach (var item in pdfFormFields.Fields)
            {
                var d = item.Value.GetMerged(0);
                 int type = pdfFormFields.GetFieldType(item.Key);

                if (type == 2)
                {
                    GetTemplateFieldRes getTemplateFieldRes=new GetTemplateFieldRes();
                    string[] aaa = pdfFormFields.GetAppearanceStates(item.Key);
                    getTemplateFieldRes.fieldName=item.Key;
                    getTemplateFieldRes.children=aaa;

                    getTemplateFieldResList.Add(getTemplateFieldRes);

                }
            }
            ausBaseResponse.responseBody=getTemplateFieldResList;
            ausBaseResponse.responseCode=0;

            return ausBaseResponse;
        }
    }
}