using System;
using System.IO;

namespace aus_backend_core.utils
{
    public class CommonUtils
    {
        public CommonUtils()
        {
        }

        public static string Getuuid()
        {
            return Guid.NewGuid().ToString();

            //strguid = Guid.NewGuid().ToString("D");//57d99d89-caab-482a-a0e9-a0a803eed3ba 同上，也是标准的标识符 (36位标准)            
            //strguid = Guid.NewGuid().ToString("N");//38bddf48f43c48588e0d78761eaa1ce6  生成32位无符号标识符            
            //strguid = Guid.NewGuid().ToString("B");//{09f140d5-af72-44ba-a763-c861304b46f8}  生成(38位:含大括号)            
            //strguid = Guid.NewGuid().ToString("N");//(778406c2-efff-4262-ab03-70a77d09c2b5)   生成(38位:含小括号)


        }

        public static string getCurrentDir() {
            dynamic type = (new Program()).GetType();
            string currentDirectory = Path.GetDirectoryName(type.Assembly.Location);
            return currentDirectory;
        }


    }
}
