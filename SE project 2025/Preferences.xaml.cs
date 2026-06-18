using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;

namespace SE_project_2025
{
    public partial class Preferences : Window
    {
        private List<CheckBox> selectedCheckBoxes = new List<CheckBox>();
        private string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";
        private int userId;

        private List<Poet> poets = new List<Poet>
        {
            new Poet { PoetID = 1, Name = "Allama Iqbal", ImagePath = "iqbal.png" },
            new Poet { PoetID = 2, Name = "Faiz Ahmed Faiz", ImagePath = "faiz.jpg" },
            new Poet { PoetID = 3, Name = "Mirza Ghalib", ImagePath = "ghalib.png" },
            new Poet { PoetID = 4, Name = "Ahmed Faraz", ImagePath = "fraz.png" },
            new Poet { PoetID = 5, Name = "John Elia", ImagePath = "jaun.jpeg" },
            new Poet { PoetID = 6, Name = "Parveen Shakir", ImagePath = "parveen.jpeg" },
            new Poet { PoetID = 7, Name = "Mir Taqi Mir", ImagePath = "mir.jpg" },
            new Poet { PoetID = 8, Name = "Habib Jalib", ImagePath = "poet_icon.jpeg" },
            new Poet { PoetID = 9, Name = "Nasir Kazmi", ImagePath = "poet_icon.jpeg" },
            new Poet { PoetID = 10, Name = "Amrita", ImagePath = "amrita.jpeg" }
        };

        public Preferences(int userId)
        {
            InitializeComponent();
            this.userId = userId; // Store the user ID
            LoadPoets();
            ProceedButton.Click += ProceedButton_Click;
        }

        private void LoadPoets()
        {
            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
            string defaultImagePath = Path.Combine(imagesFolder, "poet_icon.j");

            foreach (var poet in poets)
            {
                StackPanel poetCard = CreatePoetCard(poet, imagesFolder, defaultImagePath);
                PoetsWrapPanel.Children.Add(poetCard);
            }
        }

        private StackPanel CreatePoetCard(Poet poet, string imagesFolder, string defaultImagePath)
        {
            StackPanel poetCard = new StackPanel
            {
                Width = 160,
                Margin = new Thickness(10),
                Background = new SolidColorBrush(Colors.White),
                Orientation = Orientation.Vertical
            };

            Image poetImage = new Image
            {
                Width = 140,
                Height = 140,
                Stretch = Stretch.UniformToFill
            };

            string fullImagePath = Path.Combine(imagesFolder, poet.ImagePath);
            if (!File.Exists(fullImagePath))
            {
                fullImagePath = defaultImagePath;
            }

            try
            {
                poetImage.Source = new BitmapImage(new Uri(fullImagePath, UriKind.Absolute));
            }
            catch (Exception)
            {
                poetImage.Source = new BitmapImage(new Uri(defaultImagePath, UriKind.Absolute));
            }

            Border imageBorder = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(5),
                Child = poetImage
            };

            CheckBox poetCheckBox = new CheckBox
            {
                Content = poet.Name,
                Tag = poet.PoetID,
                Margin = new Thickness(5),
                Foreground = Brushes.Black,
                FontWeight = FontWeights.Bold
            };

            poetCheckBox.Checked += PoetCheckBox_Checked;
            poetCheckBox.Unchecked += PoetCheckBox_Unchecked;

            poetCard.Children.Add(imageBorder);
            poetCard.Children.Add(poetCheckBox);

            return poetCard;
        }

        private void PoetCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (selectedCheckBoxes.Count >= 3)
            {
                MessageBox.Show("You can only select up to 3 poets.");
                checkBox.IsChecked = false;
            }
            else
            {
                selectedCheckBoxes.Add(checkBox);
            }
        }

        private void PoetCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            selectedCheckBoxes.Remove(checkBox);
        }

        private void ProceedButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCheckBoxes.Count != 3)
            {
                MessageBox.Show("Please select exactly 3 poets before proceeding.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Delete existing user preferences to prevent duplicates
                string deleteQuery = "DELETE FROM UserPreferences WHERE UserID = @UserID";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection))
                {
                    deleteCmd.Parameters.AddWithValue("@UserID", userId);
                    deleteCmd.ExecuteNonQuery();
                }

                foreach (CheckBox cb in selectedCheckBoxes)
                {
                    int poetId = (int)cb.Tag;
                    string insertQuery = @"
                INSERT INTO UserPreferences (UserID, PoetID) 
                VALUES (@UserID, @PoetID);
                SELECT SCOPE_IDENTITY();";  // Retrieves the last inserted PreferenceID

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@UserID", userId);
                        insertCmd.Parameters.AddWithValue("@PoetID", poetId);

                        // ExecuteScalar returns the newly inserted ID
                        int newPreferenceID = Convert.ToInt32(insertCmd.ExecuteScalar());
                        Console.WriteLine($"Inserted Preference ID: {newPreferenceID}");
                    }
                }
            }

            MessageBox.Show("Preferences saved successfully!");

            //Open Login window
            LoginWindow l = new LoginWindow();
            l.Show();

            // Close the current window
            this.Close();
        }

    }

    public class Poet
    {
        public int PoetID { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}