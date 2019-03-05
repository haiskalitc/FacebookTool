using AutoTool.Model;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        // Close google chrome
        public void CloseAllTabChrome()
        {
            Process[] chromeInstances = Process.GetProcessesByName("chrome");
            foreach (Process p in chromeInstances)
            {
                p.Kill();
            }
        }
        // Close CMD
        public void CloseAllTabCMD()
        {
            Process[] chromeInstances = Process.GetProcessesByName("cmd");
            foreach (Process p in chromeInstances)
            {
                p.Kill();
            }
        }
        private static readonly List<string> _processesToCheck =
        new List<string>
        {
                    "opera",
                    "chrome",
                    "firefox",
                    "ie",
                    "gecko",
                    "phantomjs",
                    "edge",
                    "microsoftwebdriver",
                    "webdriver"
        };
        public static void FinishHim(IWebDriver driver)
        {
            driver?.Dispose();
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                try
                {
                    var shouldKill = false;
                    foreach (var processName in _processesToCheck)
                    {
                        if (process.ProcessName.ToLower().Contains(processName))
                        {
                            shouldKill = true;
                            break;
                        }
                    }
                    if (shouldKill)
                    {
                        process.Kill();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }

    }
}
