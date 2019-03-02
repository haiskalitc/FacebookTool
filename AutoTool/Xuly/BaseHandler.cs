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
        public string TAI_KHOAN = "100006307869124";
        public string MAT_KHAU = "hoan280219";

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
            }
            driver.Navigate().GoToUrl(url); ;
        }
        public IWebElement FindElementWaitSecond(string PATH,int second)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(second));
            return wait.Until(drv => drv.FindElement(By.XPath(PATH)));
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
