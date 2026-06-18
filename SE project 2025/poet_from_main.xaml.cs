using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public partial class poet_from_main : Window
    {



        public poet_from_main()
        {      
            InitializeComponent();
            load_poem_names();
        }



        private void back_click_poet(object sender, RoutedEventArgs e)
        {
            mainpage mp = new mainpage();
            mp.Show();
            this.Close();
        }


        private void load_poem_names()
        {
            string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
            string query = "SELECT PoetID, Name FROM Poets";  // No need for DISTINCT if you're using primary key

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int poetID = reader.GetInt32(0);
                    string poetName = reader.GetString(1);
                    int poetIDCopy = poetID;  // capture variable to use in lambda

                    Button poetButton = new Button();
                    poetButton.Content = poetName;
                    poetButton.Margin = new Thickness(5);
                    poetButton.Padding = new Thickness(10, 5, 10, 5);
                    poetButton.FontSize = 16;
                    poetButton.Width = 200;
                    poetButton.Height = 50;
                    poetButton.Background = Brushes.LightGray;
                    poetButton.Foreground = Brushes.Black;
                    poetButton.Cursor = Cursors.Hand;

                    poetButton.Click += (s, e) =>
                    {
                        try
                        {
                            // Assuming a window named `poet` that takes PoetID
                            Poet_page poetWindow = new Poet_page(poetName,true);
                            session.current_poetID = poetName;
                            poetWindow.Show();
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    };

                    buttonPanel.Children.Add(poetButton);
                }

                reader.Close();
            }
        }



    }
}
