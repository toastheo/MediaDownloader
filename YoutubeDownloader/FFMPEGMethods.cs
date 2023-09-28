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

namespace YoutubeDownloader
{
    public partial class Form1
    {
        // FFMPEG METHODS //

        private async Task MergeVideoAndAudio(string videoPath, string audioPath, string outputPath, string format, CancellationToken cancellationToken)
        {
            // merges audio and video together by using ffmpeg.exe
            if (format != "mp4" && format != "mkv" && format != "webm" && format != "flv")
            {
                _ = MessageBox.Show($"Failed to merge: Unkown video format {format}!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ffmpegError = true;
                return;
            }

            if (!File.Exists(ffmpegPath))
            {
                _ = MessageBox.Show("ffmpeg.exe was not found. Please ensure it is installed and available in systempath.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ffmpegError = true;
                return;
            }

            // get the procressInformation for using ffmpeg
            ProcessStartInfo startInfo = null;
            switch (format)
            {
                case "mp4":
                    startInfo = ProcessMP4(videoPath, audioPath, outputPath, reEncodeVideo, reEncodeAudio);
                    break;
                case "mkv":
                    startInfo = ProcessMKV(videoPath, audioPath, outputPath, reEncodeVideo, reEncodeAudio);
                    break;
                case "webm":
                    startInfo = ProcessWEBM(videoPath, audioPath, outputPath, reEncodeVideo, reEncodeAudio);
                    break;
                case "flv":
                    startInfo = ProcessFLV(videoPath, audioPath, outputPath, reEncodeVideo, reEncodeAudio);
                    break;
                default:
                    _ = MessageBox.Show("Failed to Start Process", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            try
            {
                bool wasKilledByCancellationToken = false;      // display no error message if user requested cancellation
                using (Process process = new Process { StartInfo = startInfo })
                {
                    string errorMessage = null;         // to store the errormessage
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            errorMessage = e.Data;
                            Match match = Regex.Match(e.Data, @"time=(\d+:\d+:\d+.\d+)");
                            if (match.Success)
                            {
                                TimeSpan currentTime = TimeSpan.Parse(match.Groups[1].Value);
                                double percentage = currentTime.TotalSeconds / videoDuration.TotalSeconds * 100;

                                // check if percentage is out of range
                                if (percentage > 100) percentage = 100;
                                if (percentage < 0) percentage = 0;

                                // update ProgressBar from the main UI thread
                                if (!ProgressBar.IsDisposed && ProgressBar != null)
                                {
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
                        }
                    };

                    _ = cancellationToken.Register(() =>
                    {
                        if (!process.HasExited)
                        {
                            process.Kill();
                            wasKilledByCancellationToken = true;
                        }
                    });

                    _ = process.Start();
                    process.BeginErrorReadLine();

                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Run(() => process.WaitForExit());

                    if (process.ExitCode != 0)
                    {
                        if (!wasKilledByCancellationToken)
                            _ = MessageBox.Show($"There was an error while merging the video and audio: {errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        ffmpegError = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ffmpegError = true;
            }
        }

        private async Task ConvertIntoAudio(string webmPath, string outputPath, string format, CancellationToken cancellationToken)
        {
            // Converts .webm to the given audioformat using ffmpeg.exe

            if (format != "mp3" && format != "wav" && format != "oga" && format != "m4a" && format != "aac")
            {
                _ = MessageBox.Show($"Failed to convert into {format}: Unkown audio format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ffmpegError = true;
                return;
            }

            if (!File.Exists(ffmpegPath))
            {
                _ = MessageBox.Show("ffmpeg.exe was not found. Please ensure it is installed and available in systempath.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ffmpegError = true;
                return;
            }

            // get the procressInformation for using ffmpeg
            ProcessStartInfo startInfo;
            switch (format)
            {
                case "mp3":
                    startInfo = ProcessMP3(webmPath, outputPath);
                    break;
                case "wav":
                    startInfo = ProcessWAV(webmPath, outputPath);
                    break;
                case "oga":
                    startInfo = ProcessOGG(webmPath, outputPath);
                    break;
                case "m4a":
                    startInfo = ProcessM4A(webmPath, outputPath);
                    break;
                case "aac":
                    startInfo = ProcessAAC(webmPath, outputPath);
                    break;
                default:
                    _ = MessageBox.Show("Failed to Start Process", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }
             
            try
            {
                bool wasKilledByCancellationToken = false;
                using (Process process = new Process { StartInfo = startInfo })
                {
                    string errorMessage = null;
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            errorMessage = e.Data;         // to store the errormessage
                            Match match = Regex.Match(e.Data, @"time=(\d+:\d+:\d+.\d+)");
                            if (match.Success)
                            {
                                TimeSpan currentTime = TimeSpan.Parse(match.Groups[1].Value);
                                double percentage = currentTime.TotalSeconds / videoDuration.TotalSeconds * 100;

                                // update ProgressBar from the main UI thread

                                if (!ProgressBar.IsDisposed && ProgressBar != null)
                                {
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
                        }
                    };

                    _ = cancellationToken.Register(() =>
                    {
                        if (!process.HasExited)
                        {
                            process.Kill();
                            wasKilledByCancellationToken = true;
                        }
                    });

                    _ = process.Start();
                    process.BeginErrorReadLine();

                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Run(() => process.WaitForExit());

                    if (process.ExitCode != 0)
                    {
                        if (!wasKilledByCancellationToken)
                            _ = MessageBox.Show($"There was an error while converting the .webm to {format}: {errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        ffmpegError = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ffmpegError = true;
            }
        }
    }
}