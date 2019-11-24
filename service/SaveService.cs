using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using iTextSharp.text.pdf;
using aus_backend_core.utils;
using aus_backend_core.entity;
namespace aus_backend_core.service
{
    public class SaveService : ISaveService
    {
        public void ISaveService()
        {
            save();
        }
        public AusBaseResponse getTemplate(string templateCode)
        {
            AusBaseResponse aus = new AusBaseResponse();
            try
            {
                Console.WriteLine("{0}", templateCode);

                string template = CommonConstantsUtil.TEMPLATE_CONFIG[templateCode];
                InitPageRes init = new InitPageRes();
                init.tempalte = template;
                aus.responseBody = init;
                aus.responseCode = 0;
            }
            catch (Exception e)
            {
                aus.responseCode = -1;
            }

            return aus;
        }
        public AusBaseResponse saveTemplate(string fileCode, string template)
        {
            AusBaseResponse aus = new AusBaseResponse();
            PDFEntityConfig pDFEntityConfig = CommonConstantsUtil.PDFCONFIG[fileCode];

            if (pDFEntityConfig == null || pDFEntityConfig.filePath == "")
            {
                aus.responseCode = -1;
                return aus;
            }

            string uuid = CommonUtils.Getuuid();
            CommonConstantsUtil.TEMPLATE_CONFIG.GetOrAdd(uuid, template);
            Console.WriteLine("{0},{1},{2}", fileCode, template, uuid);

            aus.responseBody = uuid;
            aus.responseCode = 0;
            return aus;

        }
        public MemoryStream saveDataToTemplate(AllReqData allReq)
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



                foreach (CardData cardItem in allReq.getAllCards())
                {
                    if (cardItem.radioVList != null && cardItem.radioVList != "")
                    {
                        string[] strArray = cardItem.radioVList.Split("--");
                        pdfFormFields.SetField(strArray[0], strArray[1]);
                    }
                    foreach (RowData rowItem in cardItem.rowList)
                    {
                        if (rowItem.rowType == 2)
                        {
                            foreach (RowItemData childItem in rowItem.rowItemList)
                            {
                                string fieldName = pDFEntity.directory[childItem.rowItemValue];

                                pdfFormFields.SetField(fieldName, childItem.rowItemTextValue);

                            }

                            continue;
                        }
                        if (rowItem.rowType != 3)
                        {
                            string fieldName = pDFEntity.directory[rowItem.rowValue];
                            pdfFormFields.SetField(fieldName, rowItem.inputValue);
                        }
                    }
                }
                //     stamper.SetEncryption(
                // null,
                // null,
                // PdfWriter.ALLOW_PRINTING,
                // PdfWriter.ENCRYPTION_AES_128);

                // foreach (var item in pdfFormFields.Fields)
                // {
                //     string value = i.ToString();

                //     Console.WriteLine("{0}", item.Key);
                //     pdfFormFields.SetField(item.Key, value);
                //     pDFEntity.directory.Add(value, item.Key);
                //     i++;
                // }

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

        public Dictionary<string, string> getWriteMap()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            return dic;
        }

        public object save()
        {
            string templateFile = "/Users/user/Downloads/1022out.pdf";
            string outFile = "/Users/user/Downloads/1022out.html";

            //  ConvertPdfStreamToHtml();

            iTextSharp.text.pdf.PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new iTextSharp.text.pdf.PdfReader(templateFile);
                PRAcroForm s = pdfReader.AcroForm;
                AcroFields pdfFormFields = pdfReader.AcroFields;

                foreach (var item in pdfFormFields.Fields)
                {
                    var d = item.Value.GetMerged(0);
                    int type = pdfFormFields.GetFieldType(item.Key);
                    string aaac = pdfFormFields.GetField(item.Key);
                    PRAcroForm.FieldInformation aaad = s.GetField(item.Key);


                    PdfString aae = aaad.Info.GetAsString(PdfName.TU);

                    if (aae == null)
                    {
                        continue;
                    }
                    //     Console.WriteLine(item.Key+":"+type.ToString);
                    Console.WriteLine("===={0},{1},{2},{3}===", item.Key, type, aaac, aae.ToString());

                    if (type == 2)
                    {
                        string[] aaa = pdfFormFields.GetAppearanceStates(item.Key);
                        Console.WriteLine("{0}", string.Join(",", aaa));

                    }


                    //       PrintProperties(item);
                    var str = d.Get(PdfName.V);
                    if (!string.IsNullOrEmpty(str?.ToString()))
                    {
                        var p = (str.GetBytes()[0] << 8) + str.GetBytes()[1];
                        string code;
                        switch (p)
                        {
                            case 0xefbb:
                                code = "UTF-8";
                                break;
                            case 0xfffe:
                                code = "Unicode";
                                break;
                            case 0xfeff:
                                code = "UTF-16BE";
                                break;
                            default:
                                code = "GBK";
                                break;
                        }
                        var value = Encoding.GetEncoding(code).GetString(str.GetBytes());
                        Console.WriteLine(item.Key + ":" + value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex.Message);
            }
            finally
            {
                if (pdfStamper != null)
                {
                    pdfStamper.Close();
                }
                if (pdfReader != null)
                {
                    pdfReader.Close();
                }
            }



            return DateTime.Now;
        }
    }
}
