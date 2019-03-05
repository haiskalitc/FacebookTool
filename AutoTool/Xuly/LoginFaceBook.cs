using AutoTool.DataDum;
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
        public string X_ANH_TIME_2_TAG = ":2305272732:";
        public string X_ANH_TIME_1 = "//div//div[@id='pagelet_timeline_app_collection_";
        public string X_ANH_TIME_2 = ":2305272732:";



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
                taiKhoan.SendKeys(Constance.TAI_KHOAN);
                var matKhau = FindElementWaitSecond(X_MAT_KHAU);
                if (matKhau != null)
                {
                    matKhau.SendKeys(Constance.MAT_KHAU);
                    var buttonDangNhap = FindElementWaitSecond(X_DANG_NHAP);
                    if (buttonDangNhap != null)
                    {
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
        public List<string> BackupAnhTag(string id)
        {
            List<string> ds = new List<string>();
            try
            {
                var btnAnhTag = FindElementWaitSecond(X_ANH_TIME_1_TAG + id + X_ANH_TIME_2_TAG + "4']");
                if (btnAnhTag != null)
                {
                    btnAnhTag.SendKeys(Keys.Enter);
                    var imageElements = driver.FindElements(By.XPath(X_ANH_TIME_1 + id + X_ANH_TIME_2 + "4']//ul//li"));////a//div//i
                    if (imageElements != null)
                    {
                        // backup 5 ảnh
                        for (int i = 0; i < 5; i++)
                        {
                            if (imageElements[i].GetAttribute("data-non-starred-src") != null)
                            {
                                ds.Add(imageElements[i].GetAttribute("data-non-starred-src"));
                            }
                        }
                        return ds;
                    }
                    else
                    {
                        return ds;
                    }
                }
                else
                {
                    return ds;
                }
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                GC.Collect();
            }
            return ds;
        }
        public List<string> BackupAnhDongThoiGian(string id)
        {
            List<string> ds = new List<string>();
            try
            {
                var btnAnhDongThoiGian = FindElementWaitSecond(X_ANH_TIME_1_TAG + id + X_ANH_TIME_2_TAG + "5']");
                if (btnAnhDongThoiGian != null)
                {
                    btnAnhDongThoiGian.SendKeys(Keys.Enter);
                    var imageElements = driver.FindElements(By.XPath(X_ANH_TIME_1 + id + X_ANH_TIME_2  + "5']//ul//li"));////a//div//i
                    if (imageElements != null)
                    {
                        // backup 10 ảnh
                        for (int i = 0; i < 10 ; i++)
                        {
                            if (imageElements[i].GetAttribute("data-non-starred-src") != null)
                            {
                                ds.Add(imageElements[i].GetAttribute("data-non-starred-src"));
                            }
                        }
                        return ds;
                    }
                }
                else
                {
                    return ds;
                }
            }
            catch(Exception ex)
            {
                return ds;
            }
            finally
            {
                GC.Collect();
            }
            return ds;
        }

    }
}
