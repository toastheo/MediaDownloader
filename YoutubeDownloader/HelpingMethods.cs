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
        // HELPING METHODS //

        private void EnableButtons(bool enabled)
        {
            ConvertButton.Enabled = enabled;
            ChooseButton.Enabled = enabled;
            ResetButton.Enabled = enabled;
            FormatBox.Enabled = enabled;

            if (FormatBox.SelectedIndex == 0)
            {
                QualitySettings.Enabled = enabled;
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
                RadioButton radio = control as RadioButton;
                if (radio != null && radio.Checked)
                {
                    return radio;
                }
            }

            // no radioButton selected
            return null;
        }

        private void HandleExeption(Exception ex)
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
                case Exception _:
                    _ = MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressTextBox.Clear();
                    ProgressTextBox.SelectionColor = Color.Red;
                    ProgressTextBox.AppendText("Download failed!");
                    break;
            }
        }
    }
} 