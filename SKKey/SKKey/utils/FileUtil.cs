using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SKKey.utils
{
    /**
     * 
     * 文件工具类
     * 
     * */
    class FileUtil
    {
        public static string getAppPath()
        {
            FileInfo fi = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return fi.Directory.FullName;
        }
    }
}
