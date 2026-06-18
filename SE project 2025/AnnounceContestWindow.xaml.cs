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
    public partial class AnnounceContestWindow : Window
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT1;Integrated Security=True;";

        public AnnounceContestWindow()
        {
            InitializeComponent();
        }

        private void AnnounceContest_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string theme = txtTheme.Text.Trim();
            DateTime? startDate = dpStartDate.SelectedDate;
            DateTime? endDate = dpEndDate.SelectedDate;

            if (string.IsNullOrWhiteSpace(title) || !startDate.HasValue || !endDate.HasValue)
            {
                MessageBox.Show("Please fill in all required fields (Title, Start Date, End Date).", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (endDate < startDate)
            {
                MessageBox.Show("End Date cannot be before Start Date.", "Invalid Dates", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string insertQuery = "INSERT INTO PoetryContests (Title, Theme, StartDate, EndDate) " +
                                         "VALUES (@Title, @Theme, @StartDate, @EndDate)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Theme", string.IsNullOrEmpty(theme) ? (object)DBNull.Value : theme);
                        cmd.Parameters.AddWithValue("@StartDate", startDate.Value.Date);
                        cmd.Parameters.AddWithValue("@EndDate", endDate.Value.Date);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Contest announced successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error announcing contest: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
