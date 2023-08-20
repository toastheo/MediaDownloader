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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoutubeDownloader
{
    public partial class Form1
    {
        // FFMPEG METHODS //

        private async Task MergeVideoAndAudio(string videoPath, string audioPath, string outputPath)
        {
            // merges audio and video together by using ffmpeg.exe

            if (!File.Exists(ffmpegPath))
            {
                _ = MessageBox.Show("ffmpeg.exe was not found. Please ensure it is installed and available in systempath.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ffmpegError = true;
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
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

                    _ = process.Start();
                    process.BeginErrorReadLine();
                    await Task.Run(() => process.WaitForExit());

                    if (process.ExitCode != 0)
                    {
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

        private async Task ConvertIntoMP3(string webmPath, string outputPath)
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

                    _ = process.Start();
                    process.BeginErrorReadLine();
                    await Task.Run(() => process.WaitForExit());

                    if (process.ExitCode != 0)
                    {
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