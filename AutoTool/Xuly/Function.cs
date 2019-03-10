using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTool.Xuly
{
    public class Function
    {
        public string GetPhoto(string uid, string friend_name, Bitmap checkpoint)
        {
            string str = "";
            try
            {
                int width = checkpoint.Width;
                int height = checkpoint.Height;
                double num = (double)Math.Round((double)width / (double)height, 1);
                string[] files = Directory.GetFiles(XuLyBackups.PATH_FOLDER + uid, "*.txt", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < (int)files.Length; i++)
                {
                    if (friend_name == Path.GetFileNameWithoutExtension(files[i]).Split(new char[] { '\u005F' })[1])
                    {
                        string str1 = File.ReadAllText(files[i]);
                        string[] strArrays = str1.Split(new char[] { '\n' });
                        for (int j = 0; j < (int)strArrays.Length; j++)
                        {
                            try
                            {
                                int num1 = Convert.ToInt32(strArrays[j].Split(new char[] { '|' })[1]);
                                int num2 = Convert.ToInt32(strArrays[j].Split(new char[] { '|' })[2]);
                                if (num == (double)Math.Round((double)num1 / (double)num2, 1))
                                {
                                    str = string.Concat(str, strArrays[j].Split(new char[] { '|' })[0], "\n");
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return str;
        }
    }
}
