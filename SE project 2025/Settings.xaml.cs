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

namespace SE_project_2025
{
    /// <summary>
    /// Interaction logic for settings.xaml
    /// </summary>
    public partial class settings : Window
    {
        public settings()
        {
            InitializeComponent();
        }

        private void btnlogout_click(object sender, RoutedEventArgs e)
        {

           LoginWindow loginPage = new LoginWindow();
            loginPage.Show();
            this.Close();
        }

       

        private void btn_change_username(object sender, RoutedEventArgs e)
        {
            UpdateInfo u = new UpdateInfo("username");
            this.Close();
            u.ShowDialog();
        }

        private void btn_change_email(object sender, RoutedEventArgs e)
        {
            UpdateInfo u = new UpdateInfo("email");
            this.Close();
            u.ShowDialog();
        }

        private void btn_change_pass(object sender, RoutedEventArgs e)
        {
            UpdateInfo u = new UpdateInfo("password");
            this.Close();
            u.ShowDialog();
        }

    }
}
