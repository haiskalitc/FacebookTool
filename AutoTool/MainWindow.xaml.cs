using AutoTool.BaseModel;
using AutoTool.Model;
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
        NotifiableCollection<InformatonFacebook> danhSach = new NotifiableCollection<InformatonFacebook>() {
            new InformatonFacebook() { Token ="hẢI"},
            new InformatonFacebook() { Token ="dasdas"},
            new InformatonFacebook() { Token ="hẢI"},

        };
        public MainWindow()
        {
            InitializeComponent();

            dataGridDanhSachTaiKhoan.DataContext = danhSach;

            update(Task.Factory.StartNew(() => {
                Thread.Sleep(2000);
                this.Dispatcher.Invoke(() => {
                    danhSach[1].Token = "Ni";
                });
            }));

        }
        public async void update(Task task)
        {
            await task;
            task.Dispose();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
