using AutoTool.BaseModel;
using AutoTool.Model;
using AutoTool.Views;
using AutoTool.Xuly;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
        NotifiableCollection<InformatonFacebook> danhSach = new NotifiableCollection<InformatonFacebook>();
        public MainWindow()
        {
            InitializeComponent();
            dataGridDanhSachTaiKhoan.DataContext = danhSach;
        }
              
        /// <summary>
        /// Khởi tạo danh sách TOKEN
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportToken_Click(object sender, RoutedEventArgs e)
        {
            /// IMPORT TỪ FILE
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Text|*.txt|All|*.*";
            //if (openFileDialog.ShowDialog() == true)
            //{
            //   List<string> ds =  Handlers.ImportDanhSachToken(openFileDialog.FileName);
            //    foreach (var item in ds)
            //    {
            //        danhSach.Add(new InformatonFacebook() { Token = item });
            //    }
            //}
            //------------------------------------------
            InputDataLogin inputDataLogin = new InputDataLogin(true);
            inputDataLogin.Chon += (_sender, _args) =>
            {
                string dataCallback = (_sender as InputDataLogin).txtDataInput.Text;
                bool isToken = (_sender as InputDataLogin).isToken;
                (_sender as InputDataLogin).Close();
                danhSach.Add(new InformatonFacebook() { Token = isToken ? dataCallback : "", Cookie = !isToken ? dataCallback : "" }); 
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

        }
        private void btnImportCookie_Click(object sender, RoutedEventArgs e)
        {
            InputDataLogin inputDataLogin = new InputDataLogin(false);
            inputDataLogin.Chon += (_sender, _args) =>
            {
                string dataCallback = (_sender as InputDataLogin).txtDataInput.Text;
                bool isToken = (_sender as InputDataLogin).isToken;
                (_sender as InputDataLogin).Close();
                danhSach.Add(new InformatonFacebook() { Token = isToken ? dataCallback : "", Cookie = !isToken ? dataCallback : "" });
            };
            inputDataLogin.ShowDialog();
        }

        private void btnGoCheckPoint_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
