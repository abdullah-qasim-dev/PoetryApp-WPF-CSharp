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
using static System.Collections.Specialized.BitVector32;

namespace SE_project_2025
{
    public partial class UpdateInfo : Window
    {
        private string fieldType;  // internal field used to decide what to update
        private string dbField;    // actual DB column name

        public UpdateInfo(string updateField)
        {
            InitializeComponent();
            fieldType = updateField.ToLower();

            // Map to real column names
            switch (fieldType)
            {
                
                case "username":
                    dbField = "first_name";
                    break;
                case "email":
                    dbField = "email";
                    break;
                case "password":
                    dbField = "password";
                    break;
                default:
                    MessageBox.Show("Invalid field type.");
                    Close();
                    return;
            }

            fieldType=fieldType.ToUpper();

            lblHeading.Text = $"UPDATE {fieldType}";
            lblHeading1.Text = $"Enter old {fieldType}";
            lblHeading2.Text = $"Enter old {fieldType}";



        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string oldValue = txtOldValue.Text.Trim();
            string newValue = txtNewValue.Text.Trim();

            if (string.IsNullOrWhiteSpace(oldValue) || string.IsNullOrWhiteSpace(newValue))
            {
                MessageBox.Show("Both fields are required.");
                return;
            }

            if(dbField== "first_name" )
            {
                SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;");
                // Check if Username (first_name) exists
                conn.Open();
                string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE first_name = @userName";
                using (SqlCommand checkCmd = new SqlCommand(checkUserQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@userName", newValue);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("This username is already taken. Please use a different one.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }

            if (dbField== "email")
            {
                SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;");
                // Check if Username (first_name) exists
                conn.Open();
                string checkEmailQuery = "SELECT COUNT(*) FROM users WHERE email = @email";
                using (SqlCommand checkCmd2 = new SqlCommand(checkEmailQuery, conn))
                {
                    checkCmd2.Parameters.AddWithValue("@email", newValue);
                    int count = (int)checkCmd2.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("This Email is already registered. Please use a different email.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;"))
            {
                conn.Open();

                string query = $@"
                UPDATE users 
                SET {dbField} = @newVal 
                WHERE user_id = @uid AND {dbField} = @oldVal";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@newVal", newValue);
                    cmd.Parameters.AddWithValue("@oldVal", oldValue);
                    cmd.Parameters.AddWithValue("@uid", session.UserId);  

                    int rowsAffected = cmd.ExecuteNonQuery();



                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"{fieldType} updated successfully!");      
                        ProfilePage profilePage = new ProfilePage();    
                        profilePage.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect old value or update failed.");
                    }
                }
            }
        }
    }


}
