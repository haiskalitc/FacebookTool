using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AutoTool.Xuly
{
    public class Checkpoint
    {
        public ChromeDriver chromeDriver;

        public IWait<IWebDriver> wait;

        public List<string> names = new List<string>();

        public List<Bitmap> bitmaps = new List<Bitmap>();

        public Checkpoint(bool anChrome, bool luuChrome, string uid)
        {
            try
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(XuLyBackups.PATH_FOLDER_2);
                chromeDriverService.HideCommandPromptWindow = true;
                ChromeOptions chromeOption = new ChromeOptions();
                string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string str = string.Concat(directoryName, "/chrome/", uid);
                string[] strArrays = new string[] { "disable-infobars", "--window-size=300,700" };
                string[] strArrays1 = new string[] { "disable-infobars", "headless" };
                chromeOption.AddArguments(strArrays);
                if (anChrome)
                {
                    chromeOption.AddArguments(strArrays1);
                }
                if (Directory.Exists(str))
                {
                    chromeOption.AddArguments(new string[] { string.Concat("user-data-dir=", directoryName, "/chrome/", uid) });
                    chromeOption.AddArgument("--profile-directory=Default");
                }
                if (luuChrome)
                {
                    chromeOption.AddArguments(new string[] { string.Concat("user-data-dir=", directoryName, "/chrome/", uid) });
                    chromeOption.AddArgument("--profile-directory=Default");
                }
                chromeOption.AddUserProfilePreference("disable-popup-blocking", "true");
                chromeOption.AddArguments(new string[] { "--disable-notifications" });
                chromeOption.AddArgument("--user-agent=Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1");
                this.chromeDriver = new ChromeDriver(chromeDriverService, chromeOption);
            }
            catch(Exception ex)
            {
            }
        }

        public string checkCheckpoint()
        {
            string str;
            string str1 = "CP:";
            try
            {
                this.wait.Until<bool>((IWebDriver _driver) => _driver.FindElements(By.Name("verification_method")).Count > 0);
            }
            catch
            {
                str = str1;
                return str;
            }
            try
            {
                this.chromeDriver.FindElementByXPath("//input[@value=\"3\"]");
                str1 = string.Concat(str1, "Ảnh Bạn Bè|");
            }
            catch
            {
            }
            try
            {
                this.chromeDriver.FindElementByXPath("//input[@value=\"14\"]");
                str1 = string.Concat(str1, "Phê duyệt ĐN|");
            }
            catch
            {
            }
            try
            {
                this.chromeDriver.FindElementByXPath("//input[@value=\"20\"]");
                str1 = string.Concat(str1, "Tin Nhắn|");
            }
            catch
            {
            }
            try
            {
                this.chromeDriver.FindElementByXPath("//input[@value=\"34\"]");
                str1 = string.Concat(str1, "SMS|");
            }
            catch
            {
            }
            try
            {
                this.chromeDriver.FindElementByXPath("//input[@value=\"16\"]");
                str1 = string.Concat(str1, "LH tin cậy|");
            }
            catch
            {
            }
            str = str1;
            return str;
        }

        public string checkNoCheckpoint()
        {
            string str;
            string str1 = "";
            try
            {
                this.chromeDriver.FindElementByXPath("//button[@value=\"OK\"]").Click();
                str1 = "Live";
                str = str1;
                return str;
            }
            catch
            {
            }
            str = str1;
            return str;
        }

        public bool chonName(string name)
        {
            bool flag;
            try
            {
                this.chromeDriver.FindElementByXPath("//fieldset").FindElement(By.XPath(string.Concat("//span[. = '", name.Trim(), "']"))).Click();
                ((IJavaScriptExecutor)this.chromeDriver).ExecuteScript("document.getElementById('checkpointSubmitButton-actual-button').scrollIntoView(true)", new object[0]);
                this.chromeDriver.FindElementById("checkpointSubmitButton-actual-button").Click();
                this.WaitForJqueryAjax();
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public bool chonRandom()
        {
            bool flag;
            try
            {
                int num = (new Random()).Next(1, 6);
                this.chromeDriver.FindElementByXPath(string.Concat("//fieldset/label[", num, "]")).Click();
                ((IJavaScriptExecutor)this.chromeDriver).ExecuteScript("document.getElementById('checkpointSubmitButton-actual-button').scrollIntoView(true)", new object[0]);
                this.chromeDriver.FindElementById("checkpointSubmitButton-actual-button").Click();
                this.WaitForJqueryAjax();
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public bool clickToiKhongBiet()
        {
            bool flag;
            try
            {
                int count = this.names.Count;
                this.chromeDriver.FindElementByXPath(string.Concat("//fieldset/label[", count, "]")).Click();
                ((IJavaScriptExecutor)this.chromeDriver).ExecuteScript("document.getElementById('checkpointSubmitButton-actual-button').scrollIntoView(true)", new object[0]);
                this.chromeDriver.FindElementById("checkpointSubmitButton-actual-button").Click();
                this.WaitForJqueryAjax();
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public string continueDoneCp(string uid, string password_new)
        {
            string cookie;
            try
            {
                bool flag = true;
                int num = 0;
                this.WaitForJqueryAjax();
                ((IJavaScriptExecutor)this.chromeDriver).ExecuteScript("document.getElementById('checkpointSubmitButton-actual-button').scrollIntoView(true)", new object[0]);
                this.chromeDriver.FindElementById("checkpointSubmitButton-actual-button").Click();
                this.WaitForJqueryAjax();
                while (true)
                {
                    if (flag)
                    {
                        num++;
                        if (num == 3)
                        {
                            flag = false;
                            break;
                        }
                        else
                        {
                            try
                            {
                                this.wait.Until<bool>((IWebDriver _driver) => _driver.FindElements(By.Id("checkpointSubmitButton-actual-button")).Count > 0);
                            }
                            catch
                            {
                                flag = false;
                                break;
                            }
                            ((IJavaScriptExecutor)this.chromeDriver).ExecuteScript("document.getElementById('checkpointSubmitButton-actual-button').scrollIntoView(true)", new object[0]);
                            this.chromeDriver.FindElementById("checkpointSubmitButton-actual-button").Click();
                            this.WaitForJqueryAjax();
                            try
                            {
                                this.wait.Until<bool>((IWebDriver _driver) => _driver.FindElements(By.XPath("//input[@value=\"3\"]")).Count == 0);
                            }
                            catch
                            {
                                cookie = "false";
                                return cookie;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                try
                {
                    this.wait.Until<bool>((IWebDriver _driver) => _driver.FindElements(By.Id("checkpointSubmitButton-actual-button")).Count > 0);
                }
                catch
                {
                    cookie = this.getCookie();
                    return cookie;
                }
                ((IJavaScriptExecutor)this.chromeDriver).ExecuteScript("document.getElementById('checkpointSubmitButton-actual-button').scrollIntoView(true)", new object[0]);
                this.chromeDriver.FindElementById("checkpointSubmitButton-actual-button").Click();
                try
                {
                    this.wait.Until<bool>((IWebDriver _driver) => (_driver.FindElements(By.Name("password_new")).Count <= 0 ? false : _driver.FindElements(By.Name("password_confirm")).Count > 0));
                }
                catch
                {
                    cookie = this.getCookie();
                    return cookie;
                }
                this.chromeDriver.FindElement(By.Name("password_new")).SendKeys(password_new);
                this.chromeDriver.FindElement(By.Name("password_confirm")).SendKeys(password_new);
                ((IJavaScriptExecutor)this.chromeDriver).ExecuteScript("document.getElementById('checkpointSubmitButton-actual-button').scrollIntoView(true)", new object[0]);
                this.chromeDriver.FindElementById("checkpointSubmitButton-actual-button").Click();
                this.WaitForJqueryAjax();
                flag = true;
                while (flag)
                {
                    try
                    {
                        this.wait.Until<bool>((IWebDriver _driver) => _driver.FindElements(By.Id("checkpointButtonContinue-actual-button")).Count > 0);
                    }
                    catch
                    {
                        flag = false;
                        cookie = this.getCookie();
                        return cookie;
                    }
                    ((IJavaScriptExecutor)this.chromeDriver).ExecuteScript("document.getElementById('checkpointSubmitButton-actual-button').scrollIntoView(true)", new object[0]);
                    this.chromeDriver.FindElementById("checkpointSubmitButton-actual-button").Click();
                }
                this.WaitForJqueryAjax();
                string str = this.getCookie();
                cookie = (string.IsNullOrEmpty(str) ? "false" : str);
            }
            catch
            {
                cookie = this.getCookie();
            }
            return cookie;
        }

        public Bitmap Download_Picture(string link)
        {
            Bitmap bitmap;
            try
            {
                if (string.IsNullOrEmpty(link))
                {
                    bitmap = null;
                }
                else
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(link);
                    httpWebRequest.AllowWriteStreamBuffering = true;
                    WebResponse response = httpWebRequest.GetResponse();
                    Bitmap bitmap1 = new Bitmap(Image.FromStream(response.GetResponseStream()));
                    response.Close();
                    bitmap = bitmap1;
                }
            }
            catch
            {
                bitmap = null;
            }
            return bitmap;
        }

        public string GetCheckPoint()
        {
            string str;
            this.names.Clear();
            this.bitmaps.Clear();
            this.names = new List<string>();
            this.bitmaps = new List<Bitmap>();
            string str1 = "";
            try
            {
                this.wait.Until<bool>((IWebDriver _driver) => ((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState", new object[0]).Equals("complete"));
            }
            catch
            {
                str = "Không get được ảnh";
                return str;
            }
            try
            {
                this.wait.Until<bool>((IWebDriver _driver) => _driver.FindElements(By.XPath("//img")).Count > 3);
            }
            catch
            {
                str = "Không get được ảnh";
                return str;
            }
            try
            {
                ReadOnlyCollection<IWebElement> webElements = this.chromeDriver.FindElements(By.XPath("//img"));
                if (webElements.Count != 0)
                {
                    int num = 0;
                    for (int i = 1; i < webElements.Count - 1; i++)
                    {
                        try
                        {
                            string attribute = webElements[i].GetAttribute("src");
                            attribute = attribute.Remove(attribute.Length - "&thumbnail_version=2".Length);
                            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(attribute);
                            httpWebRequest.AllowWriteStreamBuffering = true;
                            WebResponse response = httpWebRequest.GetResponse();
                            Bitmap bitmap = new Bitmap(Image.FromStream(response.GetResponseStream()));
                            response.Close();
                            this.bitmaps.Add(bitmap);
                        }
                        catch
                        {
                            num++;
                            if (num != 3)
                            {
                                webElements = this.chromeDriver.FindElements(By.XPath("//img"));
                                this.bitmaps.Clear();
                                i = 0;
                            }
                            else
                            {
                                str = "Lỗi!!! Không get được ảnh CP";
                                return str;
                            }
                        }
                    }
                    ReadOnlyCollection<IWebElement> webElements1 = this.chromeDriver.FindElementsByTagName("label");
                    for (int j = 0; j < webElements1.Count; j++)
                    {
                        this.names.Add(webElements1[j].Text);
                    }
                    str1 = "OK";
                    str = "OK";
                }
                else
                {
                    str = "Không tìm thấy ảnh CP";
                }
            }
            catch
            {
                str = "Lỗi!!! Get CP";
            }
            return str;
        }

        public string getCookie()
        {
            string str;
            try
            {
                this.WaitForJqueryAjax();
                string str1 = null;
                foreach (OpenQA.Selenium.Cookie allCooky in this.chromeDriver.Manage().Cookies.AllCookies)
                {
                    str1 = string.Concat(str1, allCooky.ToString());
                }
                string str2 = Regex.Match(str1, "c_user=(.*?);").Value.ToString();
                string str3 = Regex.Match(str1, "xs=(.*?);").Value.ToString();
                str1 = string.Concat(str2, Uri.UnescapeDataString(str3));
                str = str1;
            }
            catch
            {
                str = null;
            }
            return str;
        }

        private async Task Ghi_Log(string filePath, string messaage, bool append = true)
        {
            using (FileStream stream = new FileStream(filePath, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    await sw.WriteLineAsync(messaage);
                }
                //StreamWriter sw = null;
            }
            //FileStream stream = null;
        }

        public string loginFB(string username, string password)
        {
            string str;
            string str1;
            try
            {
                this.chromeDriver.Navigate().GoToUrl("https://m.facebook.com/");
                this.chromeDriver.FindElementById("m_login_email").SendKeys(username);
                this.chromeDriver.FindElementById("m_login_password").SendKeys(password);
                this.chromeDriver.FindElementByName("login").Click();
                this.wait = new WebDriverWait(this.chromeDriver, TimeSpan.FromSeconds(15));
                try
                {
                    this.wait.Until<bool>((IWebDriver _driver) => _driver.FindElements(By.Id("checkpointSubmitButton-actual-button")).Count > 0);
                }
                catch
                {
                    str = "Không bị checkpoint hoặc sai mật khẩu";
                    str1 = str;
                    return str1;
                }
                this.chromeDriver.FindElementById("checkpointSubmitButton-actual-button").Click();
                try
                {
                    this.wait.Until<bool>((IWebDriver _driver) => _driver.FindElements(By.XPath("//input[@value=\"3\"]")).Count > 0);
                }
                catch
                {
                    str = this.checkCheckpoint();
                    str1 = str;
                    return str1;
                }
                this.chromeDriver.FindElementByXPath("//input[@value=\"3\"]").FindElement(By.XPath("..")).FindElement(By.XPath("..")).FindElement(By.XPath("..")).Click();
                this.chromeDriver.FindElementById("checkpointSubmitButton-actual-button").Click();
                try
                {
                    this.wait.Until<bool>((IWebDriver _driver) => (_driver.FindElements(By.Id("checkpointSubmitButton-actual-button")).Count <= 0 ? false : _driver.FindElements(By.XPath("//input[@value=\"3\"]")).Count == 0));
                }
                catch
                {
                    str = this.checkCheckpoint();
                    str1 = str;
                    return str1;
                }
                this.chromeDriver.FindElementById("checkpointSubmitButton-actual-button").Click();
                try
                {
                    this.wait.Until<bool>((IWebDriver _driver) => _driver.FindElements(By.XPath("//img")).Count > 3);
                }
                catch
                {
                    str = "Lỗi!!! Không load được ảnh";
                    str1 = str;
                    return str1;
                }
                str = "OK";
                str1 = str;
                return str1;
            }
            catch
            {
                str = "Lỗi!!!";
                str1 = str;
                return str1;
            }
            return str1;
        }

        public Bitmap Resize(Bitmap old, int newW, int newH)
        {
            Bitmap bitmap;
            try
            {
                Rectangle rectangle = new Rectangle(0, 0, newW, newH);
                Bitmap bitmap1 = new Bitmap(newW, newH);
                bitmap1.SetResolution(old.HorizontalResolution, old.VerticalResolution);
                using (Graphics graphic = Graphics.FromImage(bitmap1))
                {
                    graphic.CompositingMode = CompositingMode.SourceCopy;
                    graphic.CompositingQuality = CompositingQuality.HighQuality;
                    graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphic.SmoothingMode = SmoothingMode.HighQuality;
                    graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    using (ImageAttributes imageAttribute = new ImageAttributes())
                    {
                        imageAttribute.SetWrapMode(WrapMode.TileFlipXY);
                        graphic.DrawImage(old, rectangle, 0, 0, old.Width, old.Height, GraphicsUnit.Pixel, imageAttribute);
                    }
                }
                bitmap = bitmap1;
            }
            catch
            {
                bitmap = null;
            }
            return bitmap;
        }

        public string runingCheckpoint(string uid, int key)
        {
            string str;
            try
            {
                Function function = new Function();
                foreach (Bitmap bitmap in this.bitmaps)
                {
                    for (int i = 0; i < this.names.Count; i++)
                    {
                        string photo = function.GetPhoto(uid, this.names[i], bitmap);
                        if (!string.IsNullOrEmpty(photo))
                        {
                            string[] strArrays = photo.Split(new char[] { '\n' });
                            int num1 = 0;
                            while (num1 < (int)strArrays.Length)
                            {
                                if (!this.xuLyData(strArrays[num1], bitmap, key))
                                {
                                    num1++;
                                }
                                else if (!this.chonName(this.names[i]))
                                {
                                    str = "Không tìm thấy";
                                    return str;
                                }
                                else
                                {
                                    str = "OK";
                                    return str;
                                }
                            }
                        }
                    }
                }
                str = "Không tìm thấy";
            }
            catch
            {
                str = "Lỗi runing checkpoint";
            }
            return str;
        }

        public double SimilarImg(Bitmap img1, Bitmap img2)
        {
            double width;
            try
            {
                double num = 0;
                for (int i = 0; i < img1.Height; i++)
                {
                    for (int j = 0; j < img1.Width; j++)
                    {
                        Color pixel = img1.GetPixel(j, i);
                        byte r = pixel.R;
                        pixel = img2.GetPixel(j, i);
                        num = num + (double)Math.Abs((int)(r - pixel.R)) / 255;
                        pixel = img1.GetPixel(j, i);
                        byte g = pixel.G;
                        pixel = img2.GetPixel(j, i);
                        num = num + (double)Math.Abs((int)(g - pixel.G)) / 255;
                        pixel = img1.GetPixel(j, i);
                        byte b = pixel.B;
                        pixel = img2.GetPixel(j, i);
                        num = num + (double)Math.Abs((int)(b - pixel.B)) / 255;
                    }
                }
                width = 100 - 100 * num / (double)(img1.Width * img1.Height * 3);
            }
            catch
            {
                width = 0;
            }
            return width;
        }

        public void WaitForJqueryAjax()
        {
            try
            {
                for (int i = 300; i > 0; i--)
                {
                    Thread.Sleep(1000);
                    if ((bool)((IJavaScriptExecutor)this.chromeDriver).ExecuteScript("return window.jQuery == undefined", new object[0]) || (bool)((IJavaScriptExecutor)this.chromeDriver).ExecuteScript("return window.jQuery.active == 0", new object[0]))
                    {
                        break;
                    }
                }
            }
            catch
            {
            }
        }

        public bool xuLyData(string link_photo, Bitmap checkpoint, int key)
        {
            bool flag;
            try
            {
                Bitmap bitmap = this.Download_Picture(link_photo);
                if (bitmap != null)
                {
                    int width = checkpoint.Width;
                    int height = checkpoint.Height;
                    int num = bitmap.Width;
                    int height1 = bitmap.Height;
                    if ((width != num ? true : height != height1))
                    {
                        bitmap = this.Resize(bitmap, width, height);
                    }
                    if (this.SimilarImg(checkpoint, bitmap) > (double)key)
                    {
                        flag = true;
                        return flag;
                    }
                }
                flag = false;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
    }
}
