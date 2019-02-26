using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTool.Xuly
{
    public class Handlers
    {
        public List<string> ImportDanhSachToken(string path)
        {
            StreamReader streamReader = new StreamReader(path);
            string result = streamReader.ReadToEnd();
            streamReader.Dispose();
            return result.Split(new char[] { '\n' }).ToList();
        }

    }
}
