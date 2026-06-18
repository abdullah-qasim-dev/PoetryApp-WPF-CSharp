using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace SE_project_2025
{
    public partial class AdminDashboard : Window
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT1;Integrated Security=True;";

        public AdminDashboard()
        {
            InitializeComponent();
        }

        // Load Users Button Click
        private void btnLoadUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT namee AS FullName, first_name AS Username, email AS Email, password AS Password FROM users";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        usersDataGrid.ItemsSource = dt.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Load Admins Button Click
        private void btnLoadAdmins_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT namee AS [FullName], first_name AS [Username], email AS [Email], password AS [Password] FROM admin";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        usersDataGrid.ItemsSource = dt.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading admins: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Display User Details When Selected
        private void usersDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (usersDataGrid.SelectedItem != null)
            {
                DataRowView row = usersDataGrid.SelectedItem as DataRowView;
                if (row != null)
                {
                    txtFirstName.Text = row["FullName"].ToString();
                    txtUsername.Text = row["Username"].ToString();
                    txtPassword.Text = row["Password"].ToString();
                }
            }
        }

        // Announce Contest Button Click: Show Contest Form
        private void btnAnnounceContest_Click(object sender, RoutedEventArgs e)
        {
            AnnounceContestWindow an =new AnnounceContestWindow();
            an.Show();

        }

        // Submit Contest Button Click: Insert Contest into Database
        private void btnSubmitContest_Click(object sender, RoutedEventArgs e)
        {
            string title = txtContestTitle.Text.Trim();
            DateTime? startDate = dpStartDate.SelectedDate;
            DateTime? endDate = dpEndDate.SelectedDate;

            // Validate the input fields
            if (string.IsNullOrEmpty(title) || !startDate.HasValue || !endDate.HasValue)
            {
                MessageBox.Show("Please fill all the fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO PoetryContests (Title, StartDate, EndDate) VALUES (@Title, @StartDate, @EndDate)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@StartDate", startDate.Value);
                        cmd.Parameters.AddWithValue("@EndDate", endDate.Value);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Contest announced successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            // Hide the form after success
                            ContestFormPanel.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            MessageBox.Show("Failed to announce the contest.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error announcing contest: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}