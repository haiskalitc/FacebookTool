using AutoTool.BaseModel;
using AutoTool.DataDum;
using AutoTool.Login;
using AutoTool.Model;
using AutoTool.Views;
using AutoTool.Xuly;
using Leaf.xNet;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IWebDriver driver;
        NotifiableCollection<InformatonFacebook> danhSachTaiKhoan = new NotifiableCollection<InformatonFacebook>();
        public MainWindow()
        {
            InitializeComponent();
            dataGridDanhSachTaiKhoan.DataContext = danhSachTaiKhoan;

        }
        /// <summary>
        /// Khởi tạo danh sách TOKEN
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportToken_Click(object sender, RoutedEventArgs e)
        {
            InputDataLogin inputDataLogin = new InputDataLogin(true);
            inputDataLogin.Chon += (_sender, _args) =>
            {
                string dataCallback = (_sender as InputDataLogin).txtDataInput.Text;
                bool isToken = (_sender as InputDataLogin).isToken;
                (_sender as InputDataLogin).Close();
                danhSachTaiKhoan.Add(new InformatonFacebook() { Token = isToken ? dataCallback : "", Cookie = !isToken ? dataCallback : "" }); 
            };
            inputDataLogin.ShowDialog();
        }
        // Checked
        private void dataGridDanhSachTaiKhoan_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = (InformatonFacebook)dataGridDanhSachTaiKhoan.SelectedItem;
            if (item != null)
            {
                Handlers.UpdateChecked(item, delegate {
                    item.IsCheck = !item.IsCheck;
                });
            }
        }
        // Click Backup
        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            //FinishHim(driver);
            //LoginFaceBook.getInstance.CreateWebDriver("https://www.facebook.com/");
            //LoginFaceBook.getInstance.Login();
            // Token
            foreach (InformatonFacebook info in danhSachTaiKhoan)
            {
                Backup(info);
            }
        }
        public string GoodString(string input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if ((input[i] >= 'a' && input[i] <= 'z') ||
                    (input[i] >= 'A' && input[i] <= 'Z') ||
                    (input[i] >= '0' && input[i] <= '9'))
                {
                    stringBuilder.Append(input[i]);
                }
                else
                {
                    stringBuilder.AppendFormat("%{0:X2}", (int)input[i]);
                }
            }

            return stringBuilder.ToString();
        }

        public async void Backup(InformatonFacebook info)
        {

            string respone = "";
            info.Status = "Đang check live .....";
            HttpRequest httpRequest = new HttpRequest();
            string http_url = "https://z-m-graph.facebook.com/me?fields=id,name,birthday,groups.limit(9999){name},accounts.limit(9999){name}&access_token=";
            try
            {
                await XuLyBackups.getInstance.GetDataJson(http_url, info.Token, (responce) => {
                    respone = responce;
                });
            }
            catch
            {
                info.Status = "Token died!";
                info.IsCheck = false;
                info.IsBackup = false;
                GC.Collect();
                return;
            }
            JObject dataJson = JObject.Parse(respone);
            if (dataJson != null)
            {               
                info.Status = "Token live!";
                if (dataJson["id"] != null)
                {
                    info.UID = dataJson["id"].ToString();
                }
                if (dataJson["name"] != null)
                {
                    info.Name = dataJson["name"].ToString();
                }
                if (dataJson["groups"] != null)
                {
                    info.Groups = JArray.Parse(dataJson["groups"]["data"].ToString()).Count<JToken>();
                }
                if (dataJson["accounts"] != null)
                {
                    info.Friends = JArray.Parse(dataJson["accounts"]["data"].ToString()).Count<JToken>();
                }
                if (dataJson["birthday"] != null)
                {
                    info.BirthDay = dataJson["birthday"].ToString();
                }
                string http_url_fr = "https://z-m-graph.facebook.com/me/friends?limit=5000&access_token=";
                string respone_fr = "";
                try
                {
                    await XuLyBackups.getInstance.GetDataJson(http_url_fr, info.Token, (responce) => {
                        respone_fr = responce;
                    });
                }
                catch
                {
                    info.Status = "Token died!";
                    info.IsCheck = false;
                    info.IsBackup = false;
                    GC.Collect();
                    return;
                }
                JObject dataJson_fr = JObject.Parse(respone_fr);
                if (dataJson_fr != null)
                {
                    if (dataJson_fr["data"] != null)
                    {
                        info.Friends = dataJson_fr["data"].Count<JToken>();
                        info.Status = "Tải thông tin thành công";
                        if (info.IsCheck)
                        {
                            info.Status = "Đang backup...";
                            List<string> danhSachId = new List<string>();
                            string URL = "";
                            string responce = "";
                            for (int i = 0; i < info.Friends; i++)
                            {
                                if (dataJson_fr["data"][i]["id"] != null && dataJson_fr["data"][i]["name"] != null)
                                {
                                    string _id = dataJson_fr["data"][i]["id"].ToString();
                                    string _name = dataJson_fr["data"][i]["name"].ToString();
                                    try
                                    {   
                                        danhSachId.Add(_id);
                                        URL = string.Concat(new string[] { URL, "{\"method\":\"GET\",\"relative_url\":\"?ids=", _id,
                                            "&fields=id,name,picture,photos.limit(", Constance.SO_ANH_BACKUP.ToString(), "){source,width,height}\"}," });
                                        if (danhSachId.Count == 50 ? true : i == info.Friends)
                                        {
                                            URL = string.Concat("[", URL, "]");
                                            string BODY  = string.Concat(new string[] { "access_token=", (Uri.EscapeDataString(info.Token)), "&batch=", (Uri.EscapeDataString(URL)) });
                                            try
                                            {
                                                responce = httpRequest.Post("https://graph.facebook.com", BODY , "application/x-www-form-urlencoded").ToString();
                                            }
                                            catch(Exception ex)
                                            {
      
                                            }
                                            if (string.IsNullOrEmpty(responce))
                                            {
                                            }
                                            else
                                            {
                                                await Task.Run(()=> {
                                                    JArray jArrays = JArray.Parse(responce);
                                                    for (int j = 0; j < jArrays.Count; j++)
                                                    {
                                                        string id_ = danhSachId[j];
                                                        try
                                                        {
                                                            JObject jObjects2 = JObject.Parse(jArrays[j]["body"].ToString());
                                                            string arr = jObjects2[id_]["photos"]["data"].ToString();
                                                            JArray jArrayDanhSach = JArray.Parse(arr);
                                                            List<string> danhSachAnh = new List<string>();
                                                            for (int k = 0; k < jArrayDanhSach.Count; k++)
                                                            {
                                                                danhSachAnh.Add(jArrayDanhSach[k]["source"].ToString());//, "|", jArrays1[k]["width"], "|", jArrays1[k]["height"].ToString())
                                                            }
                                                            XuLyBackups.getInstance.TaoFile(info.UID, jObjects2[danhSachId[j]]["name"].ToString(), danhSachId[j], danhSachAnh);
                                                        }
                                                        catch(Exception ex)
                                                        {
                                                            Console.Write(ex);
                                                        }
                                                    }
                                                });
                                            }
                                            URL = "";
                                            danhSachId.Clear();
                                        }
                                        //await Task.Run(() =>
                                        //{
                                        //    LoginFaceBook.getInstance.Naviga("https://www.facebook.com/" + _id + "/photos");
                                        //    var listTimeLine = LoginFaceBook.getInstance.BackupAnhDongThoiGian(_id);
                                        //    var listTag = LoginFaceBook.getInstance.BackupAnhTag(_id);
                                        //    listTimeLine.AddRange(listTag);
                                        //    XuLyBackups.getInstance.TaoFile(info.UID, _name, _id, listTimeLine);
                                        //});
                                    }
                                    catch(Exception ex)
                                    {
                                        info.Status = "Backup thất bại!";
                                        info.IsCheck = false;
                                        info.IsBackup = false;
                                        //danhSachTaiKhoan.Remove(info);
                                        GC.Collect();
                                        return;
                                    } 
                                    finally
                                    {
                                        GC.Collect();
                                    }
                                }
                            }
                            info.Status = "Backup thành công";
                            info.IsCheck = false;
                            info.IsBackup = true;
                        }
                    }
                    else
                    {
                        info.Status = "Token died!";
                        info.IsCheck = false;
                        info.IsBackup = false;
                        GC.Collect();
                        return;
                    }
                }
                else
                {
                    info.Status = "Token died!";
                    info.IsCheck = false;
                    info.IsBackup = false;
                    GC.Collect();
                    return;
                }
            }
        }

        public async void UnlockCheckPoint(InformatonFacebook info)
        {
            if (info.IsCheck)
            {
                if (info.IsBackup)
                {
                    info.Status = "Login....";
                    Checkpoint unlCheckpoint = new Checkpoint(Constance.AN_CHROME, Constance.LUU_CHROME, info.UID);
                    string login = unlCheckpoint.loginFB(info.UID, info.Password);
                    if (!login.Equals("OK"))
                    {
                        info.Status = login;
                        unlCheckpoint.chromeDriver.Quit();
                    }
                    else
                    {
                        info.Status = "Đang checkpoint";
                        string status = "";
                        int num = 0;
                        int num1 = 1;
                        while (num1 <= 5)
                        {
                            string checkPoint = unlCheckpoint.GetCheckPoint();
                            if (!checkPoint.Equals("OK"))
                            {
                                info.Status = checkPoint;
                                unlCheckpoint.chromeDriver.Quit();
                                return;
                            }
                            else
                            {
                                if (!unlCheckpoint.runingCheckpoint(info.UID, num1).Equals("OK"))
                                {
                                    num++;
                                    if (num >= 3)
                                    {
                                        unlCheckpoint.chonRandom();
                                        status = string.Concat(status, "'Random'|");
                                        info.Status = status;
                                    }
                                    else if (!unlCheckpoint.clickToiKhongBiet())
                                    {
                                        info.Status = "Lỗi click tôi không biết";
                                        unlCheckpoint.chromeDriver.Quit();
                                        return;
                                    }
                                    else
                                    {
                                        status = string.Concat(status, "'Next'|");
                                        info.Status = status;
                                        num1--;
                                    }
                                }
                                else
                                {
                                    status = string.Concat(status, "'OK'|");
                                    info.Status = status;
                                }
                                num1++;
                            }
                        }
                        // đổi mk
                        string matKhauMoi = Constance.MAT_KHAU_MOI;
   
                        string statusChangePass = unlCheckpoint.continueDoneCp(info.UID, matKhauMoi);
                        if (statusChangePass.Equals("false"))
                        {
                            info.Status = "Unlock checkpoint thất bại";
                            unlCheckpoint.chromeDriver.Quit();
                        }
                        else
                        {
                            unlCheckpoint.chromeDriver.Quit();
                            info.Status = "Unlock checkpoint thành công";
                            info.Status = "Get cookie mới...";
                            info.Status = "Get cookie thành công";
                            info.Status = "Get token mới...";
                            info.Cookie = statusChangePass;

                            //(new Thread(this.threadCheckLiveToken(index))
                            //{
                            //    IsBackground = true
                            //}).Start();
                        }
                    }
                }
                else
                {
                    info.Status = "Chưa backup";
                }
            }
        }

        private void btnImportCookie_Click(object sender, RoutedEventArgs e)
        {
            InputDataLogin inputDataLogin = new InputDataLogin(false);
            inputDataLogin.Chon += (_sender, _args) =>
            {
                string dataCallback = (_sender as InputDataLogin).txtDataInput.Text;
                bool isToken = (_sender as InputDataLogin).isToken;
                (_sender as InputDataLogin).Close();
                danhSachTaiKhoan.Add(new InformatonFacebook() { Token = isToken ? dataCallback : "", Cookie = !isToken ? dataCallback : "" });
            };
            inputDataLogin.ShowDialog();
        }

        private void btnGoCheckPoint_Click(object sender, RoutedEventArgs e)
        {
            //FinishHim(driver);
            foreach (InformatonFacebook info in danhSachTaiKhoan)
            {
                UnlockCheckPoint(info);
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //FinishHim(driver);
        }
    }
}
