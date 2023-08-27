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
using System.Threading;
using System.Globalization;
using YoutubeExplode;


namespace YoutubeDownloader
{
    public partial class Form1 : Form
    {
        private YoutubeClient youtube;
        private TimeSpan videoDuration;
        private string downloadPath;
        private bool downloadIsRunning = false;
        private string url;

        private string ffmpegPath;
        private bool ffmpegError;

        private CancellationTokenSource cts;
        private Task downloadTask = null;
        private SynchronizationContext _uiContext;

        private enum DownloadFormat
        {
            mp4,
            mkv,
            mp3,
        };

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;

            youtube = new YoutubeClient();
            FormatBox.SelectedIndex = 0;

            // set standart download path
            downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // determines the ffmpeg path
            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ffmpegPath = Path.Combine(currentDirectory, "ffmpeg.exe");
        }

        // EVENTS //

        private void Form1_Load(object sender, EventArgs e)
        {
            _uiContext = SynchronizationContext.Current;
        }

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

        private async void ConvertButton_Click(object sender, EventArgs e)
        {
            if (downloadIsRunning)
            {
                CancelDownload();
                return;
            }

            cts = new CancellationTokenSource();

            // disable all Buttons while downloading
            DownloadIsRunning(false);
            url = textBox1.Text;

            if (string.IsNullOrWhiteSpace(url))
            {
                DownloadIsRunning(true);
                return;
            }

            ffmpegError = false;

            switch (FormatBox.SelectedIndex)
            {
                case (int)DownloadFormat.mp4:
                    downloadTask = DownloadVideoAs("mp4", cts.Token);
                    await downloadTask;
                    break;
                case (int)DownloadFormat.mkv:
                    downloadTask = DownloadVideoAs("mkv", cts.Token);
                    await downloadTask;
                    break;
                case (int)DownloadFormat.mp3:
                    downloadTask = DownloadAudioAsMP3(cts.Token);
                    await downloadTask;
                    break;
            }

            downloadTask = null;
        }

        private void FormatBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            QualitySettings.Enabled = (FormatBox.SelectedIndex == (int)DownloadFormat.mp4) ||
                                      (FormatBox.SelectedIndex == (int)DownloadFormat.mkv);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (downloadIsRunning)
            {
                DialogResult result = MessageBox.Show("The download will be cancelled if you close the program now. Are you sure you want to quit?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                e.Cancel = true;

                if (result == DialogResult.Cancel)
                {
                    return;
                }

                // cancel the downloadTask
                if (cts == null)
                {
                    Close();
                }
                else
                {
                    cts?.Cancel();

                    // wait asynchronously for the downloadTask to complete
                    _uiContext.Post(async _ =>
                    {
                        try
                        {
                            if (downloadTask != null)
                                await downloadTask;
                            Close();
                        }
                        catch (Exception ex)
                        {
                            HandleException(ex);
                        }
                    }, null);
                }
            }
        }
    }
}
