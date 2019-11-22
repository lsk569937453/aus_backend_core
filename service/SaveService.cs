using System;
using System.Text;
using iTextSharp.text.pdf;

namespace netcoreTest.service
{
    public class SaveService:ISaveService
    {
        public void ISaveService()
        {
            save();
        }
        public void saveTemplate(string template) {
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
                //PdfDocument document = new PdfDocument(pdfReader);
                //PdfAcroForm pdfAcroForm = PdfAcroForm.(document, true);
                //Map<String, PdfFormField> formFields = pdfAcroForm.getFormFields();
             PRAcroForm  s=pdfReader.AcroForm;
              


                AcroFields pdfFormFields = pdfReader.AcroFields;
                
                foreach (var item in pdfFormFields.Fields)
                {
                    var d = item.Value.GetMerged(0);
                    int type = pdfFormFields.GetFieldType(item.Key);
                    string aaac = pdfFormFields.GetField(item.Key);
                    PRAcroForm.FieldInformation aaad = s.GetField(item.Key);


                   PdfString aae= aaad.Info.GetAsString(PdfName.TU);

                    if (aae == null) {
                        continue;
                    }
                    //     Console.WriteLine(item.Key+":"+type.ToString);
                    Console.WriteLine("===={0},{1},{2},{3}===", item.Key, type,aaac, aae.ToString());

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
