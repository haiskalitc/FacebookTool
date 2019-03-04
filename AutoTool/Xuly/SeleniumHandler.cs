using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoTool.Xuly
{
    public class SeleniumHandler
    {
        private SeleniumHandler()
        {
        }
        public static SeleniumHandler getInstance { get { return NestedSelenium.instance; } }
        private class NestedSelenium
        {
            static NestedSelenium()
            {
            }
            internal static readonly SeleniumHandler instance = new SeleniumHandler();
        }

        public IWebElement FindElementWaitSecond(IWebDriver driver, By by, int timeoutInSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(drv => drv.FindElement(by));
        }

        /// <summary>
        /// Đóng một web element
        /// </summary>
        /// <param name="driver"></param>
        public void CloseWebPage(IWebDriver driver)
        {
            if (driver != null)
            {
                try
                {
                    driver.Quit();
                    driver = null;
                }
                catch
                {
                    GC.Collect();
                }
                finally
                {
                    GC.Collect();
                }
            }
        }
        /// <summary>
        /// Focus trang trang vừa mở
        /// </summary>
        public void SwitchtoCurrentWindow(IWebDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }
        /// <summary>
        /// Lấy tiêu đề trang web
        /// </summary>
        /// <returns></returns>
        public string getTitle(IWebDriver drive)
        {
            string title = drive.Title;
            return title;
        }
        /// <summary>
        /// Hover tới và click menu
        /// </summary>
        /// <param name="PrimaryMenu"></param>
        /// <param name="SubMenu"></param>
        public void MouseHover_SubMenuClick(IWebDriver drive, By hover, By itemClick)
        {
            //Doing a MouseHover  
            var wait = new WebDriverWait(drive, TimeSpan.FromSeconds(10));
            var element = wait.Until(find => find.FindElement(hover));
            Actions action = new Actions(drive);
            action.MoveToElement(element).Perform();
            //Clicking the SubMenu on MouseHover   
            var menuelement = drive.FindElement(itemClick);
            menuelement.Click();
        }
        /// <summary>
        /// GET SOURCE CỦA WEBSITE
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetFullWebPage(String url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            return responseFromServer;
            // 
        }
        /// <summary>
        /// GET SOURCE CỦA WEBSITE
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetFullWebPage(IWebDriver driver)
        {
            return driver.PageSource;
            // 
        }
        /// <summary>
        /// Add cokie
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="cookie"></param>
        public void AddCookie(IWebDriver driver, string cookieName, string cookie)
        {
            OpenQA.Selenium.Cookie ck = new OpenQA.Selenium.Cookie(cookieName, cookie);
            driver.Manage().Cookies.AddCookie(ck);
        }
        public void CreateWebDriver(IWebDriver driver, string url)
        {
            if (driver == null)
            {
                var chromeOption = new ChromeOptions();
                chromeOption.AddArguments("chrome.switches", "--disable-extensions --disable-extensions-file-access-check --disable-extensions-http-throttling --disable-infobars --enable-automation --start-maximized");
                chromeOption.AddUserProfilePreference("credentials_enable_service", false);
                chromeOption.AddUserProfilePreference("profile.password_manager_enabled", false);
                chromeOption.AddArgument("disable-infobars");
                driver = new ChromeDriver(chromeOption);
            }
            driver.Url = url;
            driver.Navigate();
            driver.FindElement(By.XPath("//div//div//div//input[@id='email']")).SendKeys("100006307869124");
            driver.FindElement(By.XPath("//div//div//div//input[@id='pass']")).SendKeys("hoan280219");
            driver.FindElement(By.XPath("//div//div//div//input[@value='Đăng nhập']")).Click();

        }
    }
}
