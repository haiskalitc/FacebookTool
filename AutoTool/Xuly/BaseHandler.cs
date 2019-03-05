using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTool.Xuly
{
   public class BaseHandler
   {
        protected IWebDriver driver = null;
        // khoi tao
        public void CreateWebDriver(string url)
        {
            if (driver == null)
            {
                var chromeOption = new ChromeOptions();
                chromeOption.AddArguments("chrome.switches", "--disable-extensions --disable-extensions-file-access-check --disable-extensions-http-throttling --disable-infobars --enable-automation --start-maximized");
                chromeOption.AddUserProfilePreference("credentials_enable_service", false);
                chromeOption.AddUserProfilePreference("profile.password_manager_enabled", false);
                chromeOption.AddArgument("disable-infobars");
                driver = new ChromeDriver(chromeOption);
                driver.Manage().Window.Size = new System.Drawing.Size(300, 600);
            }
            driver.Navigate().GoToUrl(url);
            
        }
        public IWebElement FindElementWaitSecond(string PATH)
        {
            try
            {
                return driver.FindElement(By.XPath(PATH));
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            finally
            {
                GC.Collect();
            }
        }
        public List<IWebElement> GetByTagNames(IWebElement iweb, string tagName)
        {
            try
            {
                var result = iweb.FindElements(By.XPath(tagName)).ToList();
                return result;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            finally
            {
                GC.Collect();
            }
        }
        public void Naviga(string navi)
        {
            Thread.Sleep(400);
            driver.Navigate().GoToUrl(navi);
        }
        public void Delay(int miliSecond)
        {
            Thread.Sleep(miliSecond);
        }
    }
}
