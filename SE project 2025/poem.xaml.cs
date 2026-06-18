using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SE_project_2025
{
    public partial class poem : Window
    {
        private readonly int poetryId;
        private MediaPlayer mediaPlayer;
        private bool isPlaying = false;
        private string audioFilePath = string.Empty;
        private int check_which_page;
        private string poem_name_for_type_page;
        private string language = string.Empty;


        string connectionString = "Data Source=localhost;Initial Catalog=SE_PROJECT_2025;Integrated Security=True;";

        public poem(int poetryId,int is_poem_page=0,string p="",string l = "")
        {
            InitializeComponent();
            this.poetryId = poetryId;
            mediaPlayer = new MediaPlayer();
            LoadPoemData();
            check_which_page = is_poem_page;
            poem_name_for_type_page = p;
            language = l;
        }

        private void LoadPoemData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1. Load poem info
                string query = "SELECT Title, Content, Language, PoetryType, Theme FROM Poetry WHERE PoetryID = @PoetryID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PoetryID", poetryId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtPoemTitle.Text = reader["Title"].ToString();
                        txtPoemContent.Text = reader["Content"].ToString();
                        txtLanguage.Text = reader["Language"].ToString();
                        txtGenre.Text = reader["PoetryType"].ToString();
                        txtTheme.Text = reader["Theme"].ToString();
                    }
                    reader.Close();
                }

                // 2. Load poet name
                string poetQuery = @"
                    SELECT p.Name 
                    FROM Poets p 
                    INNER JOIN Poetry py ON p.PoetID = py.PoetID 
                    WHERE py.PoetryID = @PoetryID";

                using (SqlCommand cmd = new SqlCommand(poetQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@PoetryID", poetryId);
                    object result = cmd.ExecuteScalar();
                    txtPoetName.Text = (result?.ToString() ?? "Unknown");
                }


                

                // 3. Load audio path
                string audioQuery = "SELECT AudioFilePath FROM PoetryAudio WHERE PoetryID = @PoetryID";
                using (SqlCommand cmd = new SqlCommand(audioQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@PoetryID", poetryId);
                    object pathResult = cmd.ExecuteScalar();
                    if (pathResult != null)
                    {
                        audioFilePath = pathResult.ToString();
                    }
                    else
                    {
                        audioFilePath = string.Empty;
                    }
                }
            }
        }

        private void btnPlayPauseAudio_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(audioFilePath))
            {
                MessageBox.Show("Audio not available for this poem.", "Audio Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (!isPlaying)
                {
                    mediaPlayer.Open(new Uri(audioFilePath, UriKind.Absolute));
                    mediaPlayer.Play();
                    btnPlayPauseAudio.Content = "⏸ Pause Audio";
                    isPlaying = true;
                }
                else
                {
                    mediaPlayer.Pause();
                    btnPlayPauseAudio.Content = "▶ Play Audio";
                    isPlaying = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing audio: " + ex.Message);
            }
        }


        private void back_click(object sender, RoutedEventArgs e)
        {
            if (check_which_page == 0)
            {
                string poet_id = session.current_poetID;
                Poet_page pg = new Poet_page(poet_id);
                pg.Show();
                this.Close();
            }
            else if(check_which_page==1)
            {
                poems_by_type pt = new poems_by_type(poem_name_for_type_page,language);
                pt.Show();
                this.Close();
            }
            else
            {
                poems_by_theme pt = new poems_by_theme(poem_name_for_type_page,language);
                pt.Show();
                this.Close();
            }
        }

    }
}