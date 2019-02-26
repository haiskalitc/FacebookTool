using AutoTool.Model;
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
        /// <summary>
        /// Lấy danh sách token từ file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> ImportDanhSachToken(string path)
        {
            StreamReader streamReader = new StreamReader(path);
            string result = streamReader.ReadToEnd();
            streamReader.Dispose();
            return result.Split(new char[] { '\n' }).ToList();
        }

        public static void UpdateChecked(InformatonFacebook dataSource, Action<InformatonFacebook> callback)
        {
            callback.Invoke(dataSource);
        }
    }
}
