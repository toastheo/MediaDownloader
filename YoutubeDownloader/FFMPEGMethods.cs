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
            if (format != "mp4" && format != "mkv")
            {
                _ = MessageBox.Show("Failed to merge: Unkown video format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(ffmpegPath))
            {
                _ = MessageBox.Show("ffmpeg.exe was not found. Please ensure it is installed and available in systempath.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ffmpegError = true;
                return;
            }

            ProcessStartInfo startInfo = null;
            switch (format)
            {
                case "mp4":
                    startInfo = ProcessMP4(videoPath, audioPath, outputPath);
                    break;
                case "mkv":
                    startInfo = ProcessMKV(videoPath, audioPath, outputPath);
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
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            Match match = Regex.Match(e.Data, @"time=(\d+:\d+:\d+.\d+)");
                            if (match.Success)
                            {
                                TimeSpan currentTime = TimeSpan.Parse(match.Groups[1].Value);
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
                            _ = MessageBox.Show("There was an error while merging the video and audio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

        private async Task ConvertIntoMP3(string webmPath, string outputPath, CancellationToken cancellationToken)
        {
            // Converts .webm to .mp3 using ffmpeg.exe

            if (!File.Exists(ffmpegPath))
            {
                _ = MessageBox.Show("ffmpeg.exe was not found. Please ensure it is installed and available in systempath.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ffmpegError = true;
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{webmPath}\" -b:a 192K -vn \"{outputPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            try
            {
                bool wasKilledByCancellationToken = false;
                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            Match match = Regex.Match(e.Data, @"time=(\d+:\d+:\d+.\d+)");
                            if (match.Success)
                            {
                                TimeSpan currentTime = TimeSpan.Parse(match.Groups[1].Value);
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
                            _ = MessageBox.Show("There was an error while converting the .webm to .mp3.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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