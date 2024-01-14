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
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace MediaDownloader
{
    public partial class Form1 : Form
    {
        private PrivateFontCollection privateFonts = new PrivateFontCollection();

        private YoutubeClient youtube;
        private TimeSpan videoDuration;
        private string downloadPath;
        private bool downloadIsRunning = false;
        private string url;

        private readonly string ffmpegPath;
        private bool ffmpegError;

        private CancellationTokenSource cts;
        private Task downloadTask = null;
        private SynchronizationContext _uiContext;

        private bool reEncodeAudio = false;
        private bool reEncodeVideo = false;

        private string videoInformation = string.Empty;
        private Size originalSize;
        private Size expandedSize;

        // download formats have to be in the same order like in the FormatBox
        private enum DownloadFormat
        {
            mp4,
            mkv,
            webm,
            flv,
            mp3,
            wav,
            oga,
            m4a,
            aac
        };

        public Form1()
        {
            InitializeComponent();
            LoadFontFromRessources();
            InitFonts();
            Load += Form1_Load;

            youtube = new YoutubeClient();
            FormatBox.SelectedIndex = 0;

            // set standart download path
            downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // determines the ffmpeg path
            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ffmpegPath = Path.Combine(currentDirectory, "ffmpeg.exe");

            // defines original and expanded size
            originalSize = new Size(792, 540);
            expandedSize = new Size(792, 820);
            Size = originalSize;
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

            url = InputBox.Text;
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            cts = new CancellationTokenSource();

            // disable all Buttons while downloading
            DownloadIsRunning(false);
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
                case (int)DownloadFormat.webm:
                    downloadTask = DownloadVideoAs("webm", cts.Token);
                    await downloadTask;
                    break;
                case (int)DownloadFormat.flv:
                    downloadTask = DownloadVideoAs("flv", cts.Token);
                    await downloadTask;
                    break;
                case (int)DownloadFormat.mp3:
                    downloadTask = DownloadAudioAs("mp3", cts.Token);
                    await downloadTask;
                    break;
                case (int)DownloadFormat.wav:
                    downloadTask = DownloadAudioAs("wav", cts.Token);
                    await downloadTask;
                    break;
                case (int)DownloadFormat.oga:
                    downloadTask = DownloadAudioAs("oga", cts.Token);
                    break;
                case (int)DownloadFormat.m4a:
                    downloadTask = DownloadAudioAs("m4a", cts.Token);
                    break;
                case (int)DownloadFormat.aac:
                    downloadTask = DownloadAudioAs("aac", cts.Token);
                    break;
            }

            downloadTask = null;
        }

        // Disable unimportant settings if you user choose an audio format
        private void FormatBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<int> enabledFormats = new List<int>
            {
                (int)DownloadFormat.mp4,
                (int)DownloadFormat.mkv,
                (int)DownloadFormat.webm,
                (int)DownloadFormat.flv
            };

            bool isEnabled = enabledFormats.Contains(FormatBox.SelectedIndex);

            QualitySettings.Enabled = isEnabled;
            ReencodeVideoCheck.Enabled = isEnabled;
            ReencodeAudioCheck.Enabled = isEnabled;
            ASettingsLabel.Enabled = isEnabled;

            // set recommended advanced settings
            if (FormatBox.SelectedIndex == (int)DownloadFormat.webm)
            {
                ReencodeAudioCheck.Checked = false;
                ReencodeVideoCheck.Checked = false;
            }
            else
            {
                ReencodeAudioCheck.Checked = true;
                ReencodeVideoCheck.Checked = false;
            }
        }

        // Expands the window if the user wants to display advanced informations
        private void AdvancedInformationsCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (AdvancedInformationsCheck.Checked)
            {
                Size = expandedSize;
            }
            else
            {
                Size = originalSize;
            }
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
