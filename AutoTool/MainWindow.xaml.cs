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
            // Token
            foreach (InformatonFacebook info in danhSachTaiKhoan)
            {
                //XuLyBackups.getInstance.TaoFile("NguyenMinhHai", (itemFileName) => {
                //});
                Backup(info);
            }
        }
        public async void Backup(InformatonFacebook info)
        {
            //Đăng nhập FB
            LoginFaceBook.getInstance.CreateWebDriver("https://www.facebook.com/");
            LoginFaceBook.getInstance.Login();
            //---------------------------------
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
                danhSachTaiKhoan.Remove(info);
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
                    danhSachTaiKhoan.Remove(info);
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
                            XuLyBackups.getInstance.TaoFile(info.UID);
                            for (int i = 0; i < 1; i++)
                            {
                                if (dataJson_fr["data"][i]["id"] != null && dataJson_fr["data"][i]["name"] != null)
                                {
                                    string _id = dataJson_fr["data"][i]["id"].ToString();
                                    string _name = dataJson_fr["data"][i]["name"].ToString();
                                    try
                                    {
                                        await Task.Run(() =>
                                        {
                                            // Đang nhập facebook
                                            LoginFaceBook.getInstance.Naviga("https://www.facebook.com/" + _id + "/photos");
                                        });
                                    }
                                    catch
                                    {
                                        info.Status = "Token died!";
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
                        danhSachTaiKhoan.Remove(info);
                        GC.Collect();
                        return;
                    }
                }
                else
                {
                    info.Status = "Token died!";
                    danhSachTaiKhoan.Remove(info);
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

        }
    }
}
