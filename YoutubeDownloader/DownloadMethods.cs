using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace YoutubeDownloader
{
    public partial class Form1
    {
        private string videoFileName = null;
        private string audioFileName = null;
        private string mergedFileName = null;

        private async Task DownloadVideoAs(string format, CancellationToken cancellationToken)
        {
            if (format != "mp4" && format != "mkv")
            {
                _ = MessageBox.Show("Unkown video format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Video video = await youtube.Videos.GetAsync(url);
                videoDuration = (TimeSpan)video.Duration;
                ProgressTextBox.Text = $"Downloading \"{video.Title}\"" + Environment.NewLine + "Please wait!";

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
                    case "Button4K":
                        videoStreamInfo = streamManifest.GetVideoOnlyStreams().FirstOrDefault(s => s.VideoResolution.Height == 2160);
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
                    videoFileName = $"{downloadPath}\\{sanitizedTitle}_video.{videoStreamInfo.Container}";
                    audioFileName = $"{downloadPath}\\{sanitizedTitle}_audio.{audioStreamInfo.Container}";

                    Progress<double> progressHandler = new Progress<double>(percent => ProgressBar.Value = (int)(percent * 100));

                    ProgressLabel.Text = "Downloading Video ...";
                    StepLabel.Text = "Step 1 of 3";
                    cancellationToken.ThrowIfCancellationRequested();
                    await youtube.Videos.Streams.DownloadAsync(videoStreamInfo, videoFileName, progressHandler, cancellationToken);

                    ProgressLabel.Text = "Downloading Audio ...";
                    StepLabel.Text = "Step 2 of 3";
                    cancellationToken.ThrowIfCancellationRequested();
                    await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, audioFileName, progressHandler, cancellationToken);

                    // merge files together
                    ProgressLabel.Text = "Merging files together ...";
                    StepLabel.Text = "Step 3 of 3";

                    string mergedFileNameOriginal = $"{downloadPath}\\{sanitizedTitle}.{format}";
                    mergedFileName = GetUniqueFileName(mergedFileNameOriginal);
                    await MergeVideoAndAudio(videoFileName, audioFileName, mergedFileName, format, cancellationToken);
                    File.Delete(videoFileName);
                    File.Delete(audioFileName);

                    if (!ffmpegError)
                    {
                        System.Media.SystemSounds.Asterisk.Play();
                        ProgressTextBox.Clear();
                        ProgressTextBox.SelectionColor = Color.Green;
                        ProgressTextBox.AppendText($"\"{video.Title}\" downloaded successfully!");
                    }
                    else
                    {
                        if (ProgressTextBox != null && !ProgressTextBox.IsDisposed)
                        {
                            ProgressTextBox.Clear();
                            ProgressTextBox.SelectionColor = Color.Red;
                            ProgressTextBox.AppendText("Download failed!");
                        }
                        CleanUpFiles();
                    }
                }
                else
                {
                    _ = MessageBox.Show("An unexpected error occurred when getting the Video or Audio!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressTextBox.Clear();
                    ProgressTextBox.SelectionColor = Color.Red;
                    ProgressTextBox.AppendText("Download failed!");
                    CleanUpFiles();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                DownloadIsRunning(true);
                ResetFileNames();
                cts.Dispose();
                cts = null;
                if (ProgressBar != null && !ProgressBar.IsDisposed)
                {
                    ProgressBar.Value = 0;
                    ProgressLabel.Text = "";
                    ProgressTextBox.SelectionColor = ProgressTextBox.ForeColor;
                }
                if (StepLabel != null && !StepLabel.IsDisposed)
                    StepLabel.Text = "";
            }
        }

        private async Task DownloadAudioAsMP3(CancellationToken cancellationToken)
        {
            try
            {
                Video video = await youtube.Videos.GetAsync(url);
                videoDuration = (TimeSpan)video.Duration;
                ProgressTextBox.Text = $"Downloading \"{video.Title}\"";

                StreamManifest streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);

                // Get audio
                IAudioStreamInfo audioStreamInfo = (IAudioStreamInfo)streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

                if (audioStreamInfo != null)
                {
                    string sanitizedTitle = SanitizeFileName(video.Title);
                    audioFileName = $"{downloadPath}\\{sanitizedTitle}.webm";

                    Progress<double> progressHandler = new Progress<double>(percent => ProgressBar.Value = (int)(percent * 100));

                    ProgressLabel.Text = "Downloading Audio ...";
                    StepLabel.Text = "Step 1 of 2";

                    cancellationToken.ThrowIfCancellationRequested();
                    await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, audioFileName, progressHandler, cancellationToken);

                    ProgressLabel.Text = "Converting into mp3 ...";
                    StepLabel.Text = "Step 2 of 2";

                    string mergedFileNameOriginal = $"{downloadPath}\\{sanitizedTitle}.mp3";
                    mergedFileName = GetUniqueFileName(mergedFileNameOriginal);
                    await ConvertIntoMP3(audioFileName, mergedFileName, cancellationToken);
                    File.Delete(audioFileName);

                    if (!ffmpegError)
                    {
                        System.Media.SystemSounds.Asterisk.Play();
                        ProgressTextBox.Clear();
                        ProgressTextBox.SelectionColor = Color.Green;
                        ProgressTextBox.AppendText($"\"{video.Title}\" downloaded successfully!");
                    }
                    else
                    {
                        if (ProgressTextBox != null && !ProgressTextBox.IsDisposed)
                        {
                            ProgressTextBox.Clear();
                            ProgressTextBox.SelectionColor = Color.Red;
                            ProgressTextBox.AppendText("Download failed!");
                        }
                        CleanUpFiles();
                    }
                }
                else
                {
                    _ = MessageBox.Show("An unexpected error occurred when getting the Audio!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressTextBox.Clear();
                    ProgressTextBox.SelectionColor = Color.Red;
                    ProgressTextBox.AppendText("Download failed!");
                    CleanUpFiles();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                DownloadIsRunning(true);
                ResetFileNames();
                cts.Dispose();
                cts = null;
                if (ProgressBar != null && !ProgressBar.IsDisposed)
                {
                    ProgressBar.Value = 0;
                    ProgressLabel.Text = "";
                    ProgressTextBox.SelectionColor = ProgressTextBox.ForeColor;
                }
                if (StepLabel != null && !StepLabel.IsDisposed)
                    StepLabel.Text = "";
            }
        }

        private async void CancelDownload()
        {
            if (downloadIsRunning)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to cancel the download?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Cancel || cts == null)
                {
                    return;
                }

                try
                {
                    cts?.Cancel();
                    if (downloadTask != null)
                        await downloadTask;
                } 
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }
    }
}