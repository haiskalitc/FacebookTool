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
        public string X_ANH_TAG = "//div//div//div//input[@value='Đăng nhập']";
        public string X_ANH_TIME_1_TAG = "//div//a[@aria-controls='pagelet_timeline_app_collection_";
        public string X_ANH_TIME_2_TAG = ":2305272732:5']";
        public string X_ANH_TIME_1 = "//div//div[@id='pagelet_timeline_app_collection_";
        public string X_ANH_TIME_2 = ":2305272732:5']//ul";

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
            var taiKhoan = FindElementWaitSecond(X_TAIKHOAN);
            if (taiKhoan != null)
            {
                Delay(800);
                taiKhoan.SendKeys(TAI_KHOAN);
                var matKhau = FindElementWaitSecond(X_MAT_KHAU);
                if (matKhau != null)
                {
                    Delay(800);
                    matKhau.SendKeys(MAT_KHAU);
                    var buttonDangNhap = FindElementWaitSecond(X_DANG_NHAP);
                    if (buttonDangNhap != null)
                    {
                        Delay(500);
                        buttonDangNhap.Click();
                    }
                }
                else
                {
                    // mật khẩu
                }
            }
            else
            {
                // tài khoản
            }
        }
        public void BackupAnhTag()
        {

        }
        public List<string> BackupAnhDongThoiGian(string id)
        {
            try
            {
                var btnAnhDongThoiGian = FindElementWaitSecond(X_ANH_TIME_1_TAG + id + X_ANH_TIME_2_TAG);
                if (btnAnhDongThoiGian != null)
                {
                    Delay(500);
                    btnAnhDongThoiGian.SendKeys(Keys.Enter);
                    var imageElements = driver.FindElement(By.XPath(X_ANH_TIME_1 + id + X_ANH_TIME_2+ "//li//a//div//i"));
                    if (imageElements != null)
                    {
                        for (int i = 0; i < 10; i++)
                        {

                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                GC.Collect();
            }
            return null;
        }

    }
}
