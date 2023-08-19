using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace YoutubeDownloader
{
    public partial class Form1 : Form
    {
        private YoutubeClient youtube;
        private TimeSpan videoDuration;
        private string downloadPath;
        public Form1()
        {
            InitializeComponent();
            youtube = new YoutubeClient();

            // set standart download path
            downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        }

        // BUTTON EVENTS //

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
            PathTextBox.Text = "Standart Downloadfolder";
        }
        private async void ConvertButton_Click(object sender, EventArgs e)
        {
            // disable all Buttons while downloading
            EnableButtons(false);
            string url = textBox1.Text;

            if (string.IsNullOrWhiteSpace(url))
            {
                EnableButtons(true);
                return;
            }

            try
            {
                Video video = await youtube.Videos.GetAsync(url);
                videoDuration = (TimeSpan)video.Duration;
                ProgressTextBox.Text = $"Downloading {video.Title}";

                StreamManifest streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);

                // get selected radioButton
                RadioButton selectedRadio = GetCheckedRadio(QualitySettings);
                if (selectedRadio == null)
                {
                    _ = MessageBox.Show("An unexpected error occurred: Selected radioButton was not found. Please try to restart the programm!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // get video in required quality
                IVideoStreamInfo videoStreamInfo = null;
                switch (selectedRadio.Name)
                {
                    case "ButtonMax":
                        videoStreamInfo = streamManifest.GetVideoOnlyStreams().GetWithHighestVideoQuality();
                        break;
                    case "Button1440p":
                        videoStreamInfo = streamManifest.GetVideoOnlyStreams().FirstOrDefault(s => s.VideoResolution.Height == 1440);
                        break;
                    case "Button1080p":
                        videoStreamInfo = streamManifest.GetVideoOnlyStreams().FirstOrDefault(s => s.VideoResolution.Height == 1080);
                        break;
                    case "Button720p":
                        videoStreamInfo = streamManifest.GetVideoOnlyStreams().FirstOrDefault(s => s.VideoResolution.Height == 720);
                        break;
                    case "Button480p":
                        videoStreamInfo = streamManifest.GetVideoOnlyStreams().FirstOrDefault(s => s.VideoResolution.Height == 480);
                        break;
                    case "Button360p":
                        videoStreamInfo = streamManifest.GetVideoOnlyStreams().FirstOrDefault(s => s.VideoResolution.Height == 360);
                        break;
                }

                // if requested quality is not available
                if (videoStreamInfo == null)
                {
                    videoStreamInfo = streamManifest.GetVideoOnlyStreams().GetWithHighestVideoQuality();
                }
                
                // get audio
                IAudioStreamInfo audioStreamInfo = (IAudioStreamInfo)streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

                if (videoStreamInfo != null && audioStreamInfo != null)
                {
                    string sanitizedTitle = SanitizeFileName(video.Title);
                    string videoFileName = $"{downloadPath}\\{sanitizedTitle}_video.{videoStreamInfo.Container}";
                    string audioFileName = $"{downloadPath}\\{sanitizedTitle}_audio.{audioStreamInfo.Container}";

                    Progress<double> progressHandler = new Progress<double>(percent => ProgressBar.Value = (int)(percent * 100));

                    ProgressLabel.Text = "Downloading Video ...";
                    await youtube.Videos.Streams.DownloadAsync(videoStreamInfo, videoFileName, progressHandler);

                    ProgressLabel.Text = "Downloading Audio ...";
                    await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, audioFileName, progressHandler);

                    // merge files together
                    ProgressLabel.Text = "Merging files together ...";

                    string mergedFileNameOriginal = $"{downloadPath}\\{sanitizedTitle}.mp4";
                    string mergedFileName = GetUniqueFileName(mergedFileNameOriginal);
                    await MergeVideoAndAudio(videoFileName, audioFileName, mergedFileName);
                    File.Delete(videoFileName);
                    File.Delete(audioFileName);

                    System.Media.SystemSounds.Asterisk.Play();
                    ProgressTextBox.Text = $"{video.Title} downloaded successfully!";
                } 
                else
                {
                    _ = MessageBox.Show("An unexpected error occurred when getting the Video or Audio!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (YoutubeExplode.Exceptions.VideoUnavailableException)
            {
                _ = MessageBox.Show("The requested video is unavailable. Please check the URL and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (YoutubeExplode.Exceptions.RequestLimitExceededException)
            {
                _ = MessageBox.Show("Too many requests to YouTube. Please wait a moment and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.Net.Http.HttpRequestException)
            {
                _ = MessageBox.Show("Network error. Please check your internet connection and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.IO.IOException)
            {
                _ = MessageBox.Show("Error accessing files. Please ensure you have sufficient permissions and disk space.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableButtons(true);
                ProgressBar.Value = 0;
                ProgressLabel.Text = "";
            }
        }
        
        // HELPING METHODS //

        private void EnableButtons(bool enabled)
        {
            ConvertButton.Enabled = enabled;
            ChooseButton.Enabled = enabled;
            ResetButton.Enabled = enabled;
            QualitySettings.Enabled = enabled;
        }
        
        private string SanitizeFileName(string fileName)
        {
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            foreach(char c in invalidChars)
            {
                fileName = fileName.Replace(c.ToString(), "_");
            }
            return fileName;
        }

        private string GetUniqueFileName(string path)
        {
            if (!File.Exists(path))
                return path;

            string directory = Path.GetDirectoryName(path);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            int count = 1;

            while (File.Exists(Path.Combine(directory, $"{fileNameWithoutExtension} ({count}){extension}")))
                count++;

            return Path.Combine(directory, $"{fileNameWithoutExtension} ({count}){extension}");
        }

        private RadioButton GetCheckedRadio(GroupBox groupBox)
        {
            foreach (Control control in groupBox.Controls)
            {
                RadioButton radio = control as RadioButton;
                if (radio != null && radio.Checked)
                {
                    return radio;
                }
            }

            // no radioButton selected
            return null;
        }

        // MERGING METHOD //
        private async Task MergeVideoAndAudio(string videoPath, string audioPath, string outputPath)
        {
            // merges audio and video together by using ffmpeg.exe

            if (!File.Exists("ffmpeg.exe"))
            {
                _ = MessageBox.Show("ffmpeg.exe was not found. Please ensure it is installed and available in systempath.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-i \"{videoPath}\" -i \"{audioPath}\" -c:v copy -c:a aac \"{outputPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            try
            {
                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            Match match = Regex.Match(e.Data, @"time=(\d+:\d+:\d+.\d+)");
                            if (match.Success)
                            {
                                var currentTime = TimeSpan.Parse(match.Groups[1].Value);
                                double percentage = currentTime.TotalSeconds / videoDuration.TotalSeconds * 100;

                                // update ProgressBar from the main UI thread
                                if (ProgressBar.InvokeRequired)
                                {
                                    ProgressBar.Invoke(new Action(() => ProgressBar.Value = (int)percentage));
                                }
                                else
                                {
                                    ProgressBar.Value = (int)percentage;
                                }
                            }
                        }
                    };

                    _ = process.Start();
                    process.BeginErrorReadLine();
                    await Task.Run(() => process.WaitForExit());

                    if (process.ExitCode != 0)
                    {
                        _ = MessageBox.Show("There was an error while merging the video and audio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            } catch (Exception ex)
            {
                _ = MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
