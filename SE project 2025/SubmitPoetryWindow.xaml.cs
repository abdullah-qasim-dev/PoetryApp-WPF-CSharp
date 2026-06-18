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
    public partial class SubmitPoetryWindow : Window
    {
        private int contestId;
        private int userId;

        private string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT1;Integrated Security=True;";

        public SubmitPoetryWindow(int contestId, int userId)
        {
            InitializeComponent();
            this.contestId = contestId;
            this.userId = userId;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string content = txtPoetry.Text.Trim();
            string genre = (comboGenre.SelectedItem as ComboBoxItem)?.Content.ToString();
            string theme = txtTheme.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(genre))
            {
                MessageBox.Show("Please fill in all required fields (Title, Content, Genre).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Insert poetry into Poetry table
                    string insertPoetryQuery = @"
                        INSERT INTO Poetry (UserID, Title, Content, Language, PoetryType, Theme)
                        VALUES (@UserID, @Title, @Content, 'Urdu', @PoetryType, @Theme);
                        SELECT SCOPE_IDENTITY();";

                    SqlCommand poetryCmd = new SqlCommand(insertPoetryQuery, conn);
                    poetryCmd.Parameters.AddWithValue("@UserID", userId);
                    poetryCmd.Parameters.AddWithValue("@Title", title);
                    poetryCmd.Parameters.AddWithValue("@Content", content);
                    poetryCmd.Parameters.AddWithValue("@PoetryType", genre);
                    poetryCmd.Parameters.AddWithValue("@Theme", theme ?? (object)DBNull.Value);

                    int poetryId = Convert.ToInt32(poetryCmd.ExecuteScalar());

                    // Insert into ContestEntries table
                    string insertEntryQuery = @"
                        INSERT INTO ContestEntries (ContestID, UserID, PoetryID)
                        VALUES (@ContestID, @UserID, @PoetryID);";

                    SqlCommand entryCmd = new SqlCommand(insertEntryQuery, conn);
                    entryCmd.Parameters.AddWithValue("@ContestID", contestId);
                    entryCmd.Parameters.AddWithValue("@UserID", userId);
                    entryCmd.Parameters.AddWithValue("@PoetryID", poetryId);
                    entryCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Your poetry has been submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error submitting poetry: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
