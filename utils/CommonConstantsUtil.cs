using System.Collections.Concurrent;
using aus_backend_core.entity;
namespace aus_backend_core.utils
{
    public class CommonConstantsUtil
    {
        //key存储文件的key,value存储,key为文件标识，value为文件的存储位置
       public  static ConcurrentDictionary<string,PDFEntityConfig>PDFCONFIG=new ConcurrentDictionary<string, PDFEntityConfig>();


        public  static ConcurrentDictionary<string,string>TEMPLATE_CONFIG=new ConcurrentDictionary<string, string>();
        // static  ConcurrentDictionary<string,string>tez=new ConcurrentDictionary<string, string>();
     
    }
}