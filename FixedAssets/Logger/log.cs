using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace FixedAssets.Logger
{
    public class logger
    {
        public void Log(Exception ex)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\LogData\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            using (StreamWriter sm = new StreamWriter(path,true))
            {
                sm.Write(DateTime.Now.ToString() + "/r/n"+ "Message: " +ex.Message + "/r/n" + "Stacktrace: " +ex.StackTrace+"/r/n");
            }
        }
    }
}