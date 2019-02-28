using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoTool.Views
{
    /// <summary>
    /// Interaction logic for InputDataLogin.xaml
    /// </summary>
    public partial class InputDataLogin : Window
    {
        public bool isToken;

        public event EventHandler Chon;
        public InputDataLogin()
        {
            InitializeComponent();
        }

        public InputDataLogin(bool v)
        {
            InitializeComponent();
            this.isToken = v;
        }

        private void btnDong_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Chon(this, new EventArgs());
        }
    }
}
