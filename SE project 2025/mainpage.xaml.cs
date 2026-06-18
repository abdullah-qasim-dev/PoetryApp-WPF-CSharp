using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Collections;

namespace SE_project_2025
{
    /// <summary>
    /// Interaction logic for mainpage.xaml
    /// </summary>
    public partial class mainpage : Window
    {
        public mainpage()
        {
            InitializeComponent();
        }
        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {

            ProfilePage profilePage = new ProfilePage();
            profilePage.Show();
            this.Close();
        }

        private void about_btn_click(object sender, RoutedEventArgs e)
        {

           About about = new About();
            about.Show();
            this.Close();
        }
        private void Poet_btn_click(object sender, RoutedEventArgs e)
        {

           poet_from_main poet_From_Main = new poet_from_main();
            poet_From_Main.Show();
            this.Close();
        }




        private void Search_btn_click(object sender, RoutedEventArgs e)
        {
            string poetName = searchtextbox.Text.Trim();

            if (string.IsNullOrEmpty(poetName))
            {
                MessageBox.Show("Please enter a poet's name.");
                return;
            }

            string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
            string query = "SELECT * FROM Poets WHERE Name LIKE @Name";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", "%" + poetName + "%"); // Wildcard search

                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    
                 Poet_page poet_Page = new Poet_page(poetName);
                    session.current_poetID = poetName;

                    poet_Page.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    this.Close();
                }
            }
        }

        //new work from here
        private void btn_phil(object sender, RoutedEventArgs e)
        {
            if (rdo_urdu.IsChecked == true)
            {
                poems_by_theme pt = new poems_by_theme("Philophy", "Urdu");
                pt.Show();
                this.Close();
            }
            if (rdo_punjabi.IsChecked == true)
            {
                poems_by_theme pt = new poems_by_theme("Nazm", "Punjabi");
                pt.Show();
                this.Close();
            }
        }


        private void btn_patriotism(object sender, RoutedEventArgs e)
        {
            if (rdo_urdu.IsChecked == true)
            {
                poems_by_theme pt = new poems_by_theme("Patriotism", "Urdu");
                pt.Show();
                this.Close();
            }
            if (rdo_punjabi.IsChecked == true)
            {
                poems_by_theme pt = new poems_by_theme("Nazm", "Punjabi");
                pt.Show();
                this.Close();
            }
        }

        private void btn_love(object sender, RoutedEventArgs e)
        {
            if (rdo_urdu.IsChecked == true)
            {
                poems_by_theme pt = new poems_by_theme("Love", "Urdu");
                pt.Show();
                this.Close();
            }
            if (rdo_punjabi.IsChecked == true)
            {
                poems_by_theme pt = new poems_by_theme("Love", "Punjabi");
                pt.Show();
                this.Close();
            }
        }

        private void btn_sad(object sender, RoutedEventArgs e)
        {
            if (rdo_urdu.IsChecked == true)
            {
                poems_by_theme pt = new poems_by_theme("Saddness", "Urdu");
                pt.Show();
                this.Close();
            }
            if (rdo_punjabi.IsChecked == true)
            {
                poems_by_theme pt = new poems_by_theme("Saddness", "Punjabi");
                pt.Show();
                this.Close();
            }
        }

        private void btn_hopelessness(object sender, RoutedEventArgs e)
        {
            if (rdo_urdu.IsChecked == true)
            {
                poems_by_theme pt = new poems_by_theme("Hopelessness", "Urdu");
                pt.Show();
                this.Close();
            }
            if (rdo_punjabi.IsChecked == true)
            {
                poems_by_theme pt = new poems_by_theme("hopelessness", "Punjabi");
                pt.Show();
                this.Close();
            }
        }


        //till  here





        private void btn_nazm(object sender, RoutedEventArgs e)
        {
            if (rdo_urdu.IsChecked == true)
            {
                poems_by_type pt = new poems_by_type("Nazm", "Urdu");
                pt.Show();
                this.Close();
            }
            if (rdo_punjabi.IsChecked == true)
            {
                poems_by_type pt = new poems_by_type("Nazm", "Punjabi");
                pt.Show();
                this.Close();
            }
        }


        private void btn_ghazal(object sender, RoutedEventArgs e)
        {
            if (rdo_urdu.IsChecked == true)
            {
                poems_by_type pt = new poems_by_type("Ghazal", "Urdu");
                pt.Show();
                this.Close();
            }
            if (rdo_punjabi.IsChecked == true)
            {
                poems_by_type pt = new poems_by_type("Ghazal", "Punjabi");
                pt.Show();
                this.Close();
            }
        }

        private void btn_qasida(object sender, RoutedEventArgs e)
        {
            if (rdo_urdu.IsChecked == true)
            {
                poems_by_type pt = new poems_by_type("Qasida", "Urdu");
                pt.Show();
                this.Close();
            }
            if (rdo_punjabi.IsChecked == true)
            {
                poems_by_type pt = new poems_by_type("Qasida", "Punjabi");
                pt.Show();
                this.Close();
            }
        }

        private void btn_hamd(object sender, RoutedEventArgs e)
        {
            if (rdo_urdu.IsChecked == true)
            {
                poems_by_type pt = new poems_by_type("Hamd", "Urdu");
                pt.Show();
                this.Close();
            }
            if (rdo_punjabi.IsChecked == true)
            {
                poems_by_type pt = new poems_by_type("Hamd", "Punjabi");
                pt.Show();
                this.Close();
            }
        }

        private void btn_naat(object sender, RoutedEventArgs e)
        {
            if (rdo_urdu.IsChecked == true)
            {
                poems_by_type pt = new poems_by_type("Naat", "Urdu");
                pt.Show();
                this.Close();
            }
            if (rdo_punjabi.IsChecked == true)
            {
                poems_by_type pt = new poems_by_type("Naat", "Punjabi");
                pt.Show();
                this.Close();
            }
        }


      

        private void Ghalib_Click(object sender, RoutedEventArgs e)
        {

            try
            {
           

                Poet_page poet_Page = new Poet_page("Mirza Ghalib");
                poet_Page.Show();
                this.Close();
                session.current_poetID = "Mirza Ghalib";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Faiz_Click(object sender, RoutedEventArgs e)
        {

            try
            {


                Poet_page poet_Page = new Poet_page("Faiz Ahmed Faiz");
                poet_Page.Show();
                this.Close();
                session.current_poetID = "Faiz Ahmed Faiz";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void Jaun_Click(object sender, RoutedEventArgs e)
        {

            try
            {


                Poet_page poet_Page = new Poet_page("Jaun Elia");
                poet_Page.Show();
                this.Close();
                session.current_poetID = "Jaun Elia";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void Mir_Click(object sender, RoutedEventArgs e)
        {

            try
            {


                Poet_page poet_Page = new Poet_page("Mir Taqi Mir");
                poet_Page.Show();
                this.Close();
                session.current_poetID = "Mir Taqi Mir";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Faraz_Click(object sender, RoutedEventArgs e)
        {

            try
            {


                Poet_page poet_Page = new Poet_page("Ahmed Faraz");
                poet_Page.Show();
                this.Close();
                session.current_poetID = "Ahmed Faraz";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



    }
}
