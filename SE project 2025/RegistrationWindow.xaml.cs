using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Media;

namespace SE_project_2025
{
    public partial class RegistrationWindow : Window
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";

        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string firstName = txtFirstName.Text.Trim(); 
            string userName = txtUserName.Text.Trim();    
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();
            string confirmPassword = txtConfirmPassword.Password.Trim();

            if(firstName.Length<2 || firstName.Length>30)
            {
                MessageBox.Show("Name should be between 2 to 50 characters", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (userName.Length<2 || userName.Length>30)
            {
                MessageBox.Show("UserName should be betwwen 2 and 25 characters", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }



            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(userName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("All fields are required!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!email.Contains("@gmail.com"))
            {
                MessageBox.Show("Email is not correct", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (email.Length<11)
            {
                MessageBox.Show("Email should be complete", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (password.Length < 6)
                {
                    MessageBox.Show("Password must be at atleast 6 characters long.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    passBlc.Text = "At least 6 characters required!";
                    passBlc.Visibility = Visibility.Visible;
                    return;
                }
                else if (password.Length > 12)
                {
                    MessageBox.Show("Password must be max 12 characters long.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    passBlc.Text = "At least 6 characters required!";
                    passBlc.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    passBlc.Visibility = Visibility.Collapsed;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if Username (first_name) exists
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE first_name = @userName";
                    using (SqlCommand checkCmd = new SqlCommand(checkUserQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@userName", userName);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("This username is already taken. Please use a different one.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    // Check if Email exists
                    string checkEmailQuery = "SELECT COUNT(*) FROM users WHERE email = @email";
                    using (SqlCommand checkCmd2 = new SqlCommand(checkEmailQuery, conn))
                    {
                        checkCmd2.Parameters.AddWithValue("@email", email);
                        int count = (int)checkCmd2.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("This Email is already registered. Please use a different email.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    // Insert new user and get the User ID
                    string query = "INSERT INTO users (first_name, namee, email, password) OUTPUT INSERTED.user_id VALUES (@userName, @firstName, @email, @password)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userName", userName);   // Stored in "first_name"
                        cmd.Parameters.AddWithValue("@firstName", firstName); // Stored in "namee"
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);

                        int userId = (int)cmd.ExecuteScalar();  // Retrieve inserted user ID

                        if (userId > 0)
                        {
                            MessageBox.Show("Registration successful! Please select your preferences.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Open Preferences Window
                            Preferences preferencesWindow = new Preferences(userId); // Pass userId to preferences
                            preferencesWindow.Show();

                            this.Close(); // Close Registration Window
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error: " + sqlEx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Login_click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginPage = new LoginWindow();
            loginPage.Show();
            this.Close();
        }
    }
}
