using System.Collections.Generic;
using System.IO;
namespace aus_backend_core.entity
{
    public class PDFEntityConfig
    {
        public PDFEntityConfig(){}
        public string filePath{get;set;}
        public Dictionary<string,string>directory=new Dictionary<string,string>();
    }
}