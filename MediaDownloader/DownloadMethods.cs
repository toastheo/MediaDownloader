using AngleSharp.Io;
using MediaToolkit.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
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

namespace MediaDownloader
{
    public partial class Form1
    {
        private string videoFileName = null;
        private string audioFileName = null;
        private string mergedFileName = null;

        private long totalVideoSize = 0;
        private long totalAudioSize = 0;

        private DateTime lastUpdateTime = DateTime.Now;
        private long previousBytes = 0;
        private const double BytesPerMegabyte = 1048576;
        TimeSpan estimatedRemainingTime;
        private const int speedMeasurementsToKeep = 10;
        Queue<double> speedMeasurements = new Queue<double>(speedMeasurementsToKeep);

        private async Task DownloadVideoAs(string format, CancellationToken cancellationToken)
        {
            bool isDownloadingVideo = true;         // set this to false after downloading the videostream

            if (format != "mp4" && format != "mkv" && format != "webm" && format != "flv")
            {
                _ = MessageBox.Show("Unkown video format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Video video = await youtube.Videos.GetAsync(url);
                videoInformation = StoreInformations(video);
                AdvancedInformationsTextBox.Text = videoInformation;

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
                IEnumerable<VideoOnlyStreamInfo> streams = streamManifest.GetVideoOnlyStreams();
                IVideoStreamInfo videoStreamInfo = null;

                // filter by vp9 if webm is requested
                if (format == "webm")
                {
                    streams = streamManifest.GetVideoOnlyStreams().Where(s => s.VideoCodec.Contains("vp9"));
                    if (!streams.Any())
                        throw new StreamNotAvaibleException("webm");
                }

                // check which quality user requested
                switch (selectedRadio.Name)
                {
                    case "ButtonMax":
                        videoStreamInfo = streams.GetWithHighestVideoQuality();
                        break;
                    case "Button4K":
                        videoStreamInfo = streams.FirstOrDefault(s => s.VideoResolution.Height == 2160);
                        break;
                    case "Button1440p":
                        videoStreamInfo = streams.FirstOrDefault(s => s.VideoResolution.Height == 1440);
                        break;
                    case "Button1080p":
                        videoStreamInfo = streams.FirstOrDefault(s => s.VideoResolution.Height == 1080);
                        break;
                    case "Button720p":
                        videoStreamInfo = streams.FirstOrDefault(s => s.VideoResolution.Height == 720);
                        break;
                    case "Button480p":
                        videoStreamInfo = streams.FirstOrDefault(s => s.VideoResolution.Height == 480);
                        break;
                    case "Button360p":
                        videoStreamInfo = streams.FirstOrDefault(s => s.VideoResolution.Height == 360);
                        break;
                }

                // if requested quality is not available
                if (videoStreamInfo == null)
                {
                    videoStreamInfo = streams.GetWithHighestVideoQuality();
                }

                // get audio
                IAudioStreamInfo audioStreamInfo = null;
                if (format == "webm")
                {
                    IEnumerable<AudioOnlyStreamInfo> opusStreams = streamManifest.GetAudioOnlyStreams().Where(a => a.AudioCodec.Contains("opus"));

                    if (opusStreams.Any())
                        audioStreamInfo = (IAudioStreamInfo)opusStreams.GetWithHighestBitrate();
                }
                else
                {
                    IEnumerable<AudioOnlyStreamInfo> aacStreams = streamManifest.GetAudioOnlyStreams().Where(a => a.AudioCodec.Contains("aac"));

                    if (aacStreams.Any())
                        audioStreamInfo = (IAudioStreamInfo)aacStreams.GetWithHighestBitrate();
                }

                // if requested audio is not available
                if (audioStreamInfo == null)
                {
                    audioStreamInfo = (IAudioStreamInfo)streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                }

                if (videoStreamInfo != null && audioStreamInfo != null)
                {
                    // create video and audio file
                    string sanitizedTitle = SanitizeFileName(video.Title);
                    videoFileName = $"{downloadPath}\\{sanitizedTitle}_video.{videoStreamInfo.Container}";
                    audioFileName = $"{downloadPath}\\{sanitizedTitle}_audio.{audioStreamInfo.Container}";

                    // watch progress
                    string downloadType = "Downloading Video ..." + Environment.NewLine;

                    // get total size
                    totalVideoSize = videoStreamInfo.Size.Bytes;
                    totalAudioSize = audioStreamInfo.Size.Bytes;

                    Progress<double> progressHandler = new Progress<double>(percent =>
                    {
                        // update progressbar
                        ProgressBar.Value = (int)(percent * 100);

                        DateTime currentTime = DateTime.Now;

                        if ((currentTime - lastUpdateTime).TotalSeconds >= 1)
                        {
                            long currentBytes = isDownloadingVideo ? (long)(percent * videoStreamInfo.Size.Bytes) : (long)(percent * audioStreamInfo.Size.Bytes);
                            double timeDiff = (currentTime - lastUpdateTime).TotalSeconds;
                            long bytesDiff = currentBytes - previousBytes;

                            if (timeDiff > 0)
                            {
                                // calculate download speed
                                double speed = bytesDiff / timeDiff / BytesPerMegabyte;  // in MB
                                string speedText = string.Empty;

                                double averageSpeed = 0;
                                if (speed >= 0)
                                {
                                    if (speedMeasurements.Count >= speedMeasurementsToKeep)
                                    {
                                        speedMeasurements.Dequeue();
                                    }
                                    speedMeasurements.Enqueue(speed);

                                    averageSpeed = speedMeasurements.Average();      // calculates average speed

                                    speedText = $", Speed: {speed:F2} MB/s";
                                }

                                // progress
                                string downloadProgress = string.Empty;
                                downloadProgress += isDownloadingVideo ? $"{(currentBytes / BytesPerMegabyte):F2}MB / {(totalVideoSize / BytesPerMegabyte):F2}MB" :
                                                                         $"{(currentBytes / BytesPerMegabyte):F2}MB / {(totalAudioSize / BytesPerMegabyte):F2}MB";
                                // calculate estimated time
                                long remainingBytes = isDownloadingVideo ? totalVideoSize - (long)(percent * totalVideoSize) : totalAudioSize - (long)(percent * totalAudioSize);
                                if (averageSpeed > 0)
                                {
                                    double remainingTimeInSeconds = remainingBytes / (speed * BytesPerMegabyte);
                                    estimatedRemainingTime = TimeSpan.FromSeconds(remainingTimeInSeconds);

                                    // format display
                                    string remainingTimeString = Environment.NewLine + "Estimated remaining time: " +
                                    string.Format("{0:D2}:{1:D2}:{2:D2}",
                                    estimatedRemainingTime.Hours,
                                    estimatedRemainingTime.Minutes,
                                    estimatedRemainingTime.Seconds);

                                    if (AdvancedInformationsCheck.Checked)
                                    {
                                        string progressText = $"Download progress: {downloadProgress} ({(int)(percent * 100)}%){speedText}";
                                        AdvancedInformationsTextBox.Text = videoInformation + downloadType + progressText + remainingTimeString;
                                    }
                                }

                                lastUpdateTime = currentTime;
                                previousBytes = currentBytes;
                            }
                        }
                    });

                    // try to download the video
                    ProgressLabel.Text = "Downloading Video ...";
                    StepLabel.Text = "Step 1 of 3";

                    cancellationToken.ThrowIfCancellationRequested();
                    await youtube.Videos.Streams.DownloadAsync(videoStreamInfo, videoFileName, progressHandler, cancellationToken);

                    // try to download the audio
                    isDownloadingVideo = false;
                    ProgressLabel.Text = "Downloading Audio ...";
                    StepLabel.Text = "Step 2 of 3";
                    downloadType = "Downloading audio ..." + Environment.NewLine;
                    speedMeasurements.Clear();

                    cancellationToken.ThrowIfCancellationRequested();
                    await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, audioFileName, progressHandler, cancellationToken);

                    // merge files together
                    ProgressLabel.Text = $"Merging files to {format} ...";
                    StepLabel.Text = "Step 3 of 3";

                    string mergedFileNameOriginal = $"{downloadPath}\\{sanitizedTitle}.{format}";
                    mergedFileName = GetUniqueFileName(mergedFileNameOriginal);
                    await MergeVideoAndAudio(videoFileName, audioFileName, mergedFileName, format, cancellationToken);

                    // delete the temporary files
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

                // dipose cancelToken
                cts.Dispose();
                cts = null;

                // reset progressbar and steplabel
                if (ProgressBar != null && !ProgressBar.IsDisposed)
                {
                    ProgressBar.Value = 0;
                    ProgressLabel.Text = "";
                    ProgressTextBox.SelectionColor = ProgressTextBox.ForeColor;
                    AdvancedInformationsTextBox.Text = "Waiting for Download...";
                }
                if (StepLabel != null && !StepLabel.IsDisposed)
                    StepLabel.Text = "";
            }
        }

        private async Task DownloadAudioAs(string format, CancellationToken cancellationToken)
        {
            try
            {
                Video video = await youtube.Videos.GetAsync(url);
                videoInformation = StoreInformations(video);
                AdvancedInformationsTextBox.Text = videoInformation;

                videoDuration = (TimeSpan)video.Duration;
                ProgressTextBox.Text = $"Downloading \"{video.Title}\"";

                StreamManifest streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);

                // Get audio
                IAudioStreamInfo audioStreamInfo = (IAudioStreamInfo)streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

                if (audioStreamInfo != null)
                {
                    // create audioFile
                    string sanitizedTitle = SanitizeFileName(video.Title);
                    audioFileName = $"{downloadPath}\\{sanitizedTitle}.webm";

                    // watch progress
                    string downloadType = "Downloading audio ..." + Environment.NewLine;

                    // get total size
                    totalAudioSize = audioStreamInfo.Size.Bytes;

                    Progress<double> progressHandler = new Progress<double>(percent =>
                    {

                        // update progressbar
                        ProgressBar.Value = (int)(percent * 100);

                        // calculate download speed
                        DateTime currentTime = DateTime.Now;

                        if ((currentTime - lastUpdateTime).TotalSeconds >= 1)
                        {
                            // calculate download speed
                            long currentBytes = (long)(percent * audioStreamInfo.Size.Bytes);
                            double timeDiff = (currentTime - lastUpdateTime).TotalSeconds;
                            long bytesDiff = currentBytes - previousBytes;

                            if (timeDiff > 0)
                            {
                                // calculate speed
                                double speed = bytesDiff / timeDiff / BytesPerMegabyte;  // in MB
                                string speedText = string.Empty;

                                if (speedMeasurements.Count >= speedMeasurementsToKeep)
                                {
                                    speedMeasurements.Dequeue();
                                }
                                speedMeasurements.Enqueue(speed);

                                double averageSpeed = speedMeasurements.Average();      // calculates average speed

                                if (speed >= 0)
                                    speedText = $", Speed: {speed:F2} MB/s";

                                // progress
                                string downloadProgress = string.Empty;
                                downloadProgress += $"{(currentBytes / BytesPerMegabyte):F2}MB / {(totalAudioSize / BytesPerMegabyte):F2}MB";

                                // calculate estimated time
                                long remainingBytes = totalAudioSize - (long)(percent * totalAudioSize);
                                if (averageSpeed > 0)
                                {
                                    double remainingTimeInSeconds = remainingBytes / (speed * BytesPerMegabyte);
                                    estimatedRemainingTime = TimeSpan.FromSeconds(remainingTimeInSeconds);

                                    // format display
                                    string remainingTimeString = Environment.NewLine + "Estimated remaining time: " +
                                    string.Format("{0:D2}:{1:D2}:{2:D2}",
                                    estimatedRemainingTime.Hours,
                                    estimatedRemainingTime.Minutes,
                                    estimatedRemainingTime.Seconds);

                                    if (AdvancedInformationsCheck.Checked)
                                    {
                                        string progressText = $"Download progress: {downloadProgress} ({(int)(percent * 100)}%){speedText}";
                                        AdvancedInformationsTextBox.Text = videoInformation + downloadType + progressText + remainingTimeString;
                                    }
                                }
                                lastUpdateTime = currentTime;
                                previousBytes = currentBytes;
                            }
                        }
                    });

                    // try to download the audio
                    ProgressLabel.Text = "Downloading Audio ...";
                    StepLabel.Text = "Step 1 of 2";

                    cancellationToken.ThrowIfCancellationRequested();
                    await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, audioFileName, progressHandler, cancellationToken);

                    // converting into the requested format
                    ProgressLabel.Text = $"Converting into {format} ...";
                    StepLabel.Text = "Step 2 of 2";

                    string mergedFileNameOriginal = $"{downloadPath}\\{sanitizedTitle}.{format}";
                    mergedFileName = GetUniqueFileName(mergedFileNameOriginal);
                    await ConvertIntoAudio(audioFileName, mergedFileName, format, cancellationToken);

                    // delete temporary file
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

                // dipose cancelToken
                cts.Dispose();
                cts = null;

                // reset ProgressBar and Steplabel
                if (ProgressBar != null && !ProgressBar.IsDisposed)
                {
                    ProgressBar.Value = 0;
                    ProgressLabel.Text = "";
                    ProgressTextBox.SelectionColor = ProgressTextBox.ForeColor;
                    AdvancedInformationsTextBox.Text = "Waiting for Download...";
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