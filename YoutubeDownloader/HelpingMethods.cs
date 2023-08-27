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
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloader
{

    public partial class Form1
    {
        // HELPING METHODS //

        private void DownloadIsRunning(bool isRunning)
        {
            downloadIsRunning = !isRunning;
            ChooseButton.Enabled = isRunning;
            ResetButton.Enabled = isRunning;
            FormatBox.Enabled = isRunning;
            ConvertButton.Text = isRunning ? "Start Download" : "Stop Download";

            if ((FormatBox.SelectedIndex == (int)DownloadFormat.mp4) ||
                (FormatBox.SelectedIndex == (int)DownloadFormat.mkv))
            {
                QualitySettings.Enabled = isRunning;
            }
        }

        private string SanitizeFileName(string fileName)
        {
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
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
                if (control is RadioButton radio && radio.Checked)
                {
                    return radio;
                }
            }

            // no radioButton selected
            return null;
        }

        private void HandleException(Exception ex)
        {
            switch (ex)
            {
                case ArgumentNullException _:
                    _ = MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case YoutubeExplode.Exceptions.VideoUnavailableException _:
                    _ = MessageBox.Show("The requested video is unavailable. Please check the URL and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressTextBox.Clear();
                    ProgressTextBox.SelectionColor = Color.Red;
                    ProgressTextBox.AppendText("Download failed!");
                    break;
                case YoutubeExplode.Exceptions.VideoUnplayableException _:
                    _ = MessageBox.Show("The requested video is unplayable. Please check the URL and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressTextBox.Clear();
                    ProgressTextBox.SelectionColor = Color.Red;
                    ProgressTextBox.AppendText("Download failed!");
                    break;
                case YoutubeExplode.Exceptions.RequestLimitExceededException _:
                    _ = MessageBox.Show("Too many requests to YouTube. Please wait a moment and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressTextBox.Clear();
                    ProgressTextBox.SelectionColor = Color.Red;
                    ProgressTextBox.AppendText("Download failed!");
                    break;
                case YoutubeExplode.Exceptions.YoutubeExplodeException _:
                    _ = MessageBox.Show("An error with YoutubeExplode has occurred. Please check your input and internet connection and try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressTextBox.Clear();
                    ProgressTextBox.SelectionColor = Color.Red;
                    ProgressTextBox.AppendText("Download failed!");
                    break;
                case System.Net.Http.HttpRequestException _:
                    _ = MessageBox.Show("Network error. Please check your internet connection and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressTextBox.Clear();
                    ProgressTextBox.SelectionColor = Color.Red;
                    ProgressTextBox.AppendText("Download failed!");
                    break;
                case IOException _:
                    _ = MessageBox.Show("Error accessing files. Please ensure you have sufficient permissions and disk space.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressTextBox.Clear();
                    ProgressTextBox.SelectionColor = Color.Red;
                    ProgressTextBox.AppendText("Download failed!");
                    break;
                case ArgumentException _:
                    _ = MessageBox.Show("The given URL is invalid. Please check the URL and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressTextBox.Clear();
                    ProgressTextBox.SelectionColor = Color.Red;
                    ProgressTextBox.AppendText("Download failed!");
                    break;
                case TaskCanceledException _:
                    CleanUpFiles();
                    if (ProgressTextBox != null && !ProgressTextBox.IsDisposed)
                    {
                        ProgressTextBox.Clear();
                        ProgressTextBox.SelectionColor = Color.Red;
                        ProgressTextBox.AppendText("Download failed!");
                    }
                    break;
                case OperationCanceledException _:
                    CleanUpFiles();
                    if (ProgressTextBox != null && !ProgressTextBox.IsDisposed)
                    {
                        ProgressTextBox.Clear();
                        ProgressTextBox.SelectionColor = Color.Red;
                        ProgressTextBox.AppendText("Download failed!");
                    }
                    break;
                case Exception _:
                    _ = MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (ProgressTextBox != null && !ProgressTextBox.IsDisposed)
                    {
                        ProgressTextBox.Clear();
                        ProgressTextBox.SelectionColor = Color.Red;
                        ProgressTextBox.AppendText("Download failed!");
                    }
                    break;
            }
        }

        private void CleanUpFiles()
        {
            if (File.Exists(videoFileName))
                File.Delete(videoFileName);
            if (File.Exists(audioFileName))
                File.Delete(audioFileName);
            if (File.Exists(mergedFileName))
                File.Delete(mergedFileName);
        }

        private void ResetFileNames()
        {
            videoFileName = null;
            audioFileName = null;
            mergedFileName = null;
        }
    }
} 