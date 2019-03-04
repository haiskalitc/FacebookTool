using AutoTool.BaseModel;
using AutoTool.Login;
using AutoTool.Model;
using AutoTool.Views;
using AutoTool.Xuly;
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
        public void CloseAllTabChrome()
        {
            Process[] chromeInstances = Process.GetProcessesByName("chrome");
            foreach (Process p in chromeInstances)
            {
                p.Kill();
            }
        }
        public void CloseAllTabCMD()
        {
            Process[] chromeInstances = Process.GetProcessesByName("cmd");
            foreach (Process p in chromeInstances)
            {
                p.Kill();
            }
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
        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            FinishHim(driver);
            LoginFaceBook.getInstance.CreateWebDriver("https://www.facebook.com/");
            LoginFaceBook.getInstance.Login();
            // Token
            foreach (InformatonFacebook info in danhSachTaiKhoan)
            {
                Backup(info);
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
        public async void Backup(InformatonFacebook info)
        {

            string respone = "";
            info.Status = "Đang check live .....";
            HttpClient httpRequest = new HttpClient();
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
                            for (int i = 0; i < info.Friends; i++)
                            {
                                if (dataJson_fr["data"][i]["id"] != null && dataJson_fr["data"][i]["name"] != null)
                                {
                                    string _id = dataJson_fr["data"][i]["id"].ToString();
                                    string _name = dataJson_fr["data"][i]["name"].ToString();
                                    try
                                    {
                                        await Task.Run(() =>
                                        {
                                            LoginFaceBook.getInstance.Naviga("https://www.facebook.com/" + _id + "/photos");
                                            var listTimeLine = LoginFaceBook.getInstance.BackupAnhDongThoiGian(_id);
                                            var listTag = LoginFaceBook.getInstance.BackupAnhTag(_id);
                                            listTimeLine.AddRange(listTag);
                                            XuLyBackups.getInstance.TaoFile(info.UID, _name, _id, listTimeLine);
                                        });
                                    }
                                    catch
                                    {
                                        info.Status = "Backup thất bại!";
                                        danhSachTaiKhoan.Remove(info);
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
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        info.Status = "Token died!";
                        GC.Collect();
                        return;
                    }
                }
                else
                {
                    info.Status = "Token died!";
                    GC.Collect();
                    return;
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
            FinishHim(driver);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FinishHim(driver);
        }
    }
}
