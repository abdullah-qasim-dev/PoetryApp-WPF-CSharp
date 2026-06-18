using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SE_project_2025
{
    public partial class LoginWindow : Window
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string firstName = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both First Name and Password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(firstName.Length>30 || firstName.Length<2)
            {
                MessageBox.Show("Please enter valid lenght for username", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "";

                    if (rdoUser.IsChecked == true)
                    {
                        query = "SELECT COUNT(*) FROM users WHERE first_name = @firstName AND password = @password";
                    }
                    else if (rdoAdmin.IsChecked == true)
                    {
                        query = "SELECT COUNT(*) FROM admin WHERE first_name = @firstName AND password = @password";
                    }
                    else
                    {
                        MessageBox.Show("Please select User or Admin!", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", firstName);
                        cmd.Parameters.AddWithValue("@password", password);

                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0)
                        {
                            if (rdoUser.IsChecked == true)
                            {
                                string query1 = "SELECT user_id FROM users WHERE first_name = @firstName ";

                                using (SqlCommand cmd1 = new SqlCommand(query1, conn))
                                {
                                    cmd1.Parameters.AddWithValue("@firstName", firstName);  


                                    object result = cmd1.ExecuteScalar(); 

                                    if (result != null)
                                    {
                                        int userId = Convert.ToInt32(result);

                                        session.UserId = userId;
                                       

                                       
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid username or password.");
                                    }
                                }
                                
                                mainpage mp = new mainpage();
                                mp.Show();


                            }
                            else if (rdoAdmin.IsChecked == true)
                            {
                                AdminDashboard adminPage = new AdminDashboard();
                                adminPage.Show();
                            }
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Invalid First Name or Password!", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registerPage = new RegistrationWindow();
            registerPage.Show();
            this.Close();
        }
    }
}