using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTool.Xuly
{
    public class LoginFaceBook : BaseHandler
    {
        public string X_TAIKHOAN = "//div//div//div//input[@id='email']";
        public string X_MAT_KHAU = "//div//div//div//input[@id='pass']";
        public string X_DANG_NHAP = "//div//div//div//input[@value='Đăng nhập']";


        private LoginFaceBook()
        {
        }
        public static LoginFaceBook getInstance { get { return NestedSelenium.instance; } }
        private class NestedSelenium
        {
            static NestedSelenium()
            {

            }
            internal static readonly LoginFaceBook instance = new LoginFaceBook();
        }
        public void Login()
        {
            Delay(1200);
            FindElementWaitSecond(X_TAIKHOAN,1200).SendKeys(TAI_KHOAN);
            Delay(1000);
            FindElementWaitSecond(X_MAT_KHAU,1200).SendKeys(MAT_KHAU);
            Delay(500);
            FindElementWaitSecond(X_DANG_NHAP,1200).Click();
        }


    }
}
