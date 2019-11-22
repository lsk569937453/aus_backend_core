using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BeetleX.FastHttpApi;
using Bytescout.PDF2HTML;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace netcoreTest
{
    [Controller]
    public class Program
    {

        int BUTTON = 1;
        int CHECK_BOX = 2;
        int RADIO_BUTTON = 3;
        int TEXT_FIELD = 4;
        int LIST_BOX = 5;
        int COMBO_BOX = 6;
        private static BeetleX.FastHttpApi.HttpApiServer mApiServer;
        static void Main(string[] args)
        {
            //mApiServer = new BeetleX.FastHttpApi.HttpApiServer();
            //mApiServer.Options.LogLevel = BeetleX.EventArgs.LogType.Trace;
            //mApiServer.Options.LogToConsole = true;
            ////      mApiServer.Options.Debug = true;
            ////      mApiServer.Debug();//set view path with vs project folder
            //mApiServer.Register(typeof(Program).Assembly);
            ////mApiServer.Options.Port=80; set listen port to 80
            //mApiServer.Open();//default listen port 9090  
            //Console.Write(mApiServer.BaseServer);
            //Console.Read();
            var host = new WebHostBuilder()
                .UseUrls("http://*:8889")
          .UseKestrel()
          .UseContentRoot(Directory.GetCurrentDirectory())
          .UseIISIntegration()
          .UseStartup<Startup>()
          .Build();

            host.Run();
        }
        // Get /hello?name=henry 
        // or
        // Get /hello/henry
        [Get(Route = "{name}")]
        public object Hello(string name)
        {
            return $"hello {name} {DateTime.Now}";
        }
        // Get /GetTime  
        public object GetTime()
        {
            return DateTime.Now;
        }
        // Get /GetTime  
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


                AcroFields pdfFormFields = pdfReader.AcroFields;
                foreach (var item in pdfFormFields.Fields)
                {
                    var d = item.Value.GetMerged(0);
                    int type = pdfFormFields.GetFieldType(item.Key);


               //     Console.WriteLine(item.Key+":"+type.ToString);
                    Console.WriteLine("{0},{1}",item.Key,type);

                    if (type == 2) {
                        string[]aaa=pdfFormFields.GetAppearanceStates(item.Key);
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

        public static void PrintProperties(Object obj)
        {
            Type type = obj.GetType();
            foreach (PropertyInfo p in type.GetProperties())
            {

                Console.WriteLine("------" + p.GetValue(obj));
            }
        }
        private static void ConvertPdfStreamToHtml()
        {
            // We need files only for demonstration purposes.
            // The whole conversion process will be done in memory.

            string templateFile = "/Users/user/Downloads/1022out.pdf";
            string outFile = "/Users/user/Downloads/1022out.html";


            // Convert PDF to HTML in memory
            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();

            // This property is necessary only for licensed version.
            //f.Serial = "XXXXXXXXXXX";

            // Let's force the component to store images inside HTML document
            // using base-64 encoding.
            // Thus the component will not use HDD.
            f.HtmlOptions.IncludeImageInHtml = true;
            f.HtmlOptions.Title = "Simple text";

            // Assume that we have a PDF document as Stream.
            using (FileStream fs = File.OpenRead(templateFile))
            {
                f.OpenPdf(fs);

                if (f.PageCount > 0)
                {
                    // Convert PDF to HTML to a MemoryStream.
                    using (MemoryStream msHtml = new MemoryStream())
                    {
                        int res = f.ToHtml(msHtml);
                        // Open the result for demonstation purposes.
                        if (res == 0)
                        {
                            File.WriteAllBytes(outFile, msHtml.ToArray());
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outFile) { UseShellExecute = true });
                        }
                    }
                }
            }
        }
    }
}
