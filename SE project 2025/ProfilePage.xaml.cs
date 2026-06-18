using SE_project_2025.Properties;
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

namespace SE_project_2025
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ProfilePage : Window
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT1;Integrated Security=True;";
        private int userId = session.UserId; // Using session ID to identify the user

        public ProfilePage()
        {
            InitializeComponent();
            LoadUserDetails();
            LoadContests();
        }

        private void back_button_Click(object sender, RoutedEventArgs e)
        {
            mainpage main = new mainpage();
            main.Show();
            this.Close();
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            settings set = new settings();
            set.Show();
            this.Close();
        }

        private void LoadUserDetails()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT first_name, email FROM users WHERE user_id = @userId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblUserName.Text = reader["first_name"].ToString();
                        lblUserEmail.Text = reader["email"].ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user details: " + ex.Message);
            }
        }

        private void LoadContests()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ContestID, Title, Description, StartDate, EndDate FROM PoetryContests";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        StackPanel contestPanel = new StackPanel
                        {
                            Margin = new Thickness(10),
                            Background = Brushes.White,
                            Orientation = Orientation.Vertical
                        };

                        contestPanel.Children.Add(new TextBlock { Text = row["Title"].ToString(), FontSize = 18, FontWeight = FontWeights.Bold });
                        contestPanel.Children.Add(new TextBlock { Text = row["Description"].ToString(), TextWrapping = TextWrapping.Wrap });
                        contestPanel.Children.Add(new TextBlock { Text = "Start: " + Convert.ToDateTime(row["StartDate"]).ToShortDateString() });
                        contestPanel.Children.Add(new TextBlock { Text = "End: " + Convert.ToDateTime(row["EndDate"]).ToShortDateString() });

                        Button joinButton = new Button
                        {
                            Content = "Join Contest",
                            Tag = row["ContestID"],
                            Margin = new Thickness(5),
                            Background = Brushes.SaddleBrown,
                            Foreground = Brushes.White,
                            Padding = new Thickness(5)
                        };
                        joinButton.Click += JoinContest_Click;

                        contestPanel.Children.Add(joinButton);

                        // Assume there's a StackPanel in XAML with x:Name="contestContainer"
                        contestContainer.Children.Add(contestPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading contests: " + ex.Message);
            }
        }

        private void JoinContest_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && int.TryParse(button.Tag.ToString(), out int contestId))
            {
                SubmitPoetryWindow submitWindow = new SubmitPoetryWindow(userId, contestId);
                submitWindow.Show();
            }
        }
    }

}
