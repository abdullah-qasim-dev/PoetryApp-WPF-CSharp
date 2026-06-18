using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SE_project_2025
{
    
    public partial class Poet_page : Window
    {
        private int poetId; // Will store the dynamically retrieved PoetID
        private string poetName;
        bool back;

        public Poet_page(string poetName,bool which_page=false)
        {
            InitializeComponent();
            this.poetName = poetName;
            LoadPoetData(poetName); // This will also call LoadPoetThemes
            back = which_page;
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            mainpage main = new mainpage();
            main.Show();
            this.Close();

        }

        private void LoadPoetData(string poetName)
        {
            string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
            string query = "SELECT * FROM Poets WHERE Name = @Name";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", poetName);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // ✅ Store the PoetID for later use
                    poetId = Convert.ToInt32(reader["PoetID"]);

                    // ✅ Populate UI Elements
                    PoetName.Text = reader["Name"].ToString();
                    PoetEra.Text = reader["Era"].ToString();
                    PoetBio.Text = reader["Bio"].ToString();
                    PoetTimeLine.Text = reader["Timeline"].ToString();

                    // ✅ Load image
                    string imagePath = reader["ImagePath"].ToString();
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        PoetImage.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                    }

                    // ✅ Load Themes AFTER loading poet info
                    LoadPoetThemes(poetId);
                    LoadPoetryNames(poetId);
                    top_5(poetId);
                }
                else
                {
                    MessageBox.Show("Can't find poet :(");
                    mainpage mp = new mainpage();
                    mp.Show();
                    this.Close();

                }
            }
        }

        private void LoadPoetThemes(int poetId)
        {
            List<string> themes = new List<string>();
            string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
            string query = "SELECT DISTINCT Theme FROM Poetry WHERE PoetID = @PoetID AND Theme IS NOT NULL";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PoetID", poetId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string theme = reader["Theme"].ToString();
                    if (!string.IsNullOrWhiteSpace(theme))
                    {
                        themes.Add(theme);
                    }
                }
            }

            // ✅ Bind list to ComboBox (make sure this exists in your XAML)
            ThemeDropdown.ItemsSource = themes;
        }

        private void LoadPoetryNames(int poetId)
        {
            List<string> titles = new List<string>();
            string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
            string query = "SELECT DISTINCT Title FROM Poetry WHERE PoetID = @PoetID AND Title IS NOT NULL";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PoetID", poetId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string title = reader["Title"].ToString();
                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        titles.Add(title);
                    }
                }
            }

            // ✅ Populate correct ComboBox
            Namedropdown.ItemsSource = titles;
        }

        private void top_5(int poetId)
        {
            List<string> top = new List<string>();
            string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
            string query = "SELECT TOP 5 Title FROM Poetry p WHERE p.PoetID = @poetId ORDER BY p.top5 DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PoetID", poetId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string title = reader["Title"].ToString();
                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        top.Add(title);
                    }
                }
            }


            // ✅ Update button names dynamically
            if (top.Count >= 5)
            {
                Button1.Content = top[0];
                Button2.Content = top[1];
                Button3.Content = top[2];
                Button4.Content = top[3];
                Button5.Content = top[4];
            }
            else
            {
                // If there are fewer than 5 titles, clear button names for the remaining buttons
                Button1.Content = top.ElementAtOrDefault(0);
                Button2.Content = top.ElementAtOrDefault(1);
                Button3.Content = top.ElementAtOrDefault(2);
                Button4.Content = top.ElementAtOrDefault(3);
                Button5.Content = top.ElementAtOrDefault(4);
            }
        }


        private void btn_top1(object sender, RoutedEventArgs e)
        {
            string name_of_poem = "";
            if (Button1.Content.ToString() != null)
            {
                name_of_poem = Button1.Content.ToString();


                string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
                string query = "SELECT PoetryID FROM Poetry WHERE Title = @name_of_poem";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name_of_poem", name_of_poem);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar(); // Use ExecuteScalar for a single value

                        if (result != null)
                        {
                            int poetryID = Convert.ToInt32(result);
                            poem p = new poem(poetryID,0, "", "");
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
            }
            else
            {
                MessageBox.Show("Sorry no Poem here.");

            }


        }

        private void btn_top2(object sender, RoutedEventArgs e)
        {

            string name_of_poem = Button2.Content.ToString();
            if (string.IsNullOrEmpty(name_of_poem))
            {
                MessageBox.Show("UserName should be betwwen 2 and 25 characters", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
            string query = "SELECT PoetryID FROM Poetry WHERE Title = @name_of_poem";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name_of_poem", name_of_poem);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar(); // Use ExecuteScalar for a single value

                    if (result != null)
                    {
                        int poetryID = Convert.ToInt32(result);
                        poem p = new poem(poetryID, 0, "", "");
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
        }



        private void btn_top3(object sender, RoutedEventArgs e)
        {
            string name_of_poem = Button3.Content.ToString();
            if (string.IsNullOrEmpty(name_of_poem))
            {
                MessageBox.Show("UserName should be betwwen 2 and 25 characters", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
            string query = "SELECT PoetryID FROM Poetry WHERE Title = @name_of_poem";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name_of_poem", name_of_poem);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar(); // Use ExecuteScalar for a single value

                    if (result != null)
                    {
                        int poetryID = Convert.ToInt32(result);
                        poem p = new poem(poetryID, 0, "", "");
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
        }


        private void btn_top4(object sender, RoutedEventArgs e)
        {
            string name_of_poem = Button4.Content.ToString();
            if (string.IsNullOrEmpty(name_of_poem))
            {
                MessageBox.Show("UserName should be betwwen 2 and 25 characters", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
            string query = "SELECT PoetryID FROM Poetry WHERE Title = @name_of_poem";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name_of_poem", name_of_poem);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar(); // Use ExecuteScalar for a single value

                    if (result != null)
                    {
                        int poetryID = Convert.ToInt32(result);
                        poem p = new poem(poetryID,0, "", "");
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
        }






        private void btn_top5(object sender, RoutedEventArgs e)
        {
            string name_of_poem = Button5.Content.ToString();
            string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
            string query = "SELECT PoetryID FROM Poetry WHERE Title = @name_of_poem";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name_of_poem", name_of_poem);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar(); // Use ExecuteScalar for a single value

                    if (result != null)
                    {

                       
                        int poetryID = Convert.ToInt32(result);
                        poem p = new poem(poetryID,0, "", "");
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

        }

        private void back_click_poem(object sender,RoutedEventArgs e)
        {
            if (back == false)
            {
                mainpage mp = new mainpage();
                mp.Show();
                this.Close();
            }
            else
            {
                poet_from_main pt = new poet_from_main();
                pt.Show();
                this.Close();
            }
        }

       
    }
}