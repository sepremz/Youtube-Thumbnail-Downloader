using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace youtube_thumbnail_downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string videoUrl = textBox1.Text;
            string videoId = GetYouTubeVideoId(videoUrl);
            string thumbnailUrl = $"https://img.youtube.com/vi/{videoId}/maxresdefault.jpg";

            try
            {
                using (WebClient client = new WebClient())
                {
                    byte[] imageData = client.DownloadData(thumbnailUrl);

                    // Show save file dialog to choose the save location
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "JPEG Image|*.jpg";
                    saveFileDialog.Title = "Save Thumbnail Image";
                    saveFileDialog.FileName = "thumbnail.jpg";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Save the image to the selected location
                        string savePath = saveFileDialog.FileName;
                        System.IO.File.WriteAllBytes(savePath, imageData);
                        MessageBox.Show("Thumbnail downloaded successfully and saved.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading thumbnail: {ex.Message}");
            }
        }

        private string GetYouTubeVideoId(string videoUrl)
        {
            // Extract video ID from the YouTube URL
            Regex regex = new Regex(@"(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v(?:i)?=))([^\/\n\s&]+)");
            Match match = regex.Match(videoUrl);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                throw new ArgumentException("Invalid YouTube video URL");
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Don't forget to follow me on social media @sepremz, Thank you!", "Reminder", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result == DialogResult.Cancel)
            {
                e.Cancel = true; // Cancel the closing event to keep the application running
            }
        }
    }
}
