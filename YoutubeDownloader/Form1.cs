using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExplode;

namespace YoutubeDownloader
{
    public partial class Form1 : Form
    {
        private YoutubeClient youtube;
        private TimeSpan videoDuration;
        private string downloadPath;
        private string url;

        private string ffmpegPath;
        private bool ffmpegError;

        public Form1()
        {
            InitializeComponent();
            youtube = new YoutubeClient();
            FormatBox.SelectedIndex = 0;

            // set standart download path
            downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // determines the ffmpeg path
            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ffmpegPath = Path.Combine(currentDirectory, "ffmpeg.exe");
        }

        // EVENTS //

        private void ChooseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                downloadPath = folderDialog.SelectedPath;
                PathTextBox.Text = downloadPath;
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            downloadPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            PathTextBox.Text = "Standart Download Folder";
        }

        private void ConvertButton_Click(object sender, EventArgs e)
        {
            // disable all Buttons while downloading
            EnableButtons(false);
            url = textBox1.Text;

            if (string.IsNullOrWhiteSpace(url))
            {
                EnableButtons(true);
                return;
            }

            ffmpegError = false;

            if (FormatBox.SelectedIndex == 0)
                DownloadVideoAsMP4();
            else
                DownloadAudioAsMP3();
        }

        private void FormatBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            QualitySettings.Enabled = FormatBox.SelectedIndex == 0;
        }
    }
}
