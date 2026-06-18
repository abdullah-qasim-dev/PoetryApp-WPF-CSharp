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
   
    public partial class poems_by_type : Window
    {

        private string poemtype;
        private string language1;



        public poems_by_type(string type,string lang)
        {
            poemtype = type;
            language1 = lang;
            InitializeComponent();
            load_poem_names();


        }

        private void back_click_poem_type(object sender, RoutedEventArgs e)
        {
            mainpage mp = new mainpage();
            mp.Show();
            this.Close();
          

        }
        private void load_poem_names()
        {
            string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
            string query = "";

            if (language1 == "Urdu" || language1 == "Punjabi")
            {
                query = "SELECT Poetry.Title FROM Poetry WHERE Language = @Language AND PoetryType = @PoemType";
            }
            else
            {
                MessageBox.Show("Please select Urdu or Punjabi", "Language Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                mainpage mp = new mainpage();
                mp.Show();
                this.Close();
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Language", language1);
                cmd.Parameters.AddWithValue("@PoemType", poemtype);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string title = reader.GetString(0);

                    Button poemButton = new Button();
                    poemButton.Content = title;
                    poemButton.Margin = new Thickness(5);
                    poemButton.Padding = new Thickness(10, 5, 10, 5);
                    poemButton.FontSize = 16;
                    poemButton.Width = 200;
                    poemButton.Height = 50;
                    poemButton.Background = Brushes.LightGray;
                    poemButton.Foreground = Brushes.Black;
                    poemButton.Cursor = Cursors.Hand;

                    // Optional: Attach a click event to the button
                    poemButton.Click += (s, e) =>
                    {
                        string name_of_poem = title;
                        string connectionString1 = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
                        string query1 = "SELECT PoetryID FROM Poetry WHERE Title = @name_of_poem";

                        using (SqlConnection conn = new SqlConnection(connectionString1))
                        {
                            SqlCommand cmd1 = new SqlCommand(query1, conn);
                            cmd1.Parameters.AddWithValue("@name_of_poem", name_of_poem);  // ✅ Fix here

                            try
                            {
                                conn.Open();
                                object result = cmd1.ExecuteScalar();  // ✅ Fix here

                                if (result != null)
                                {
                                    int poetryID = Convert.ToInt32(result);
                                    poem p = new poem(poetryID,1,poemtype,language1);
                                    p.Show();
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("No poem found with that title.");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: " + ex.Message);
                            }
                        }
                    };


                    buttonPanel.Children.Add(poemButton);
                }

                reader.Close();
            }
        }

    }
}
