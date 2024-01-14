using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace MediaDownloader
{
    public partial class Form1
    {
        Stream fontStream;

        // HELPING METHODS //

        private void DownloadIsRunning(bool isRunning)
        {
            downloadIsRunning = !isRunning;
            ChooseButton.Enabled = isRunning;
            ResetButton.Enabled = isRunning;
            FormatBox.Enabled = isRunning;
            ConvertButton.Text = isRunning ? "Start Download" : "Stop Download";

            reEncodeVideo = ReencodeVideoCheck.Checked;
            reEncodeAudio = ReencodeAudioCheck.Checked;
            
            if ((FormatBox.SelectedIndex == (int)DownloadFormat.mp4) ||
                (FormatBox.SelectedIndex == (int)DownloadFormat.mkv) ||
                (FormatBox.SelectedIndex == (int)DownloadFormat.webm)||
                (FormatBox.SelectedIndex == (int)DownloadFormat.flv))
            {
                QualitySettings.Enabled = isRunning;
                ReencodeVideoCheck.Enabled = isRunning;
                ReencodeAudioCheck.Enabled = isRunning;
            }
        }

        private void LoadFontFromRessources()
        {
            using (fontStream = GetType().Assembly.GetManifestResourceStream("YoutubeDownloader.Montserrat-VariableFont_wght.ttf"))
            {
                if (fontStream == null)
                {
                    _ = MessageBox.Show("Error loading font! Use default font instead.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // create byte array and save data from Montserratfont
                byte[] fontData = new byte[fontStream.Length];
                fontStream.Read(fontData, 0, (int)fontStream.Length);

                // create GCHandle and adding the font
                GCHandle pinnedArray = GCHandle.Alloc(fontData, GCHandleType.Pinned);
                IntPtr fontPointer = pinnedArray.AddrOfPinnedObject();

                // add font to privateFontCollection
                privateFonts.AddMemoryFont(fontPointer, fontData.Length);

                // release GCHandle
                pinnedArray.Free();
            }
        }

        // giving al UI Elements the correct Font
        private void InitFonts()
        {
            if (fontStream == null)
                return;

            Headline.Font = new Font(privateFonts.Families[0], 16, FontStyle.Bold);
            InputBox.Font = new Font(privateFonts.Families[0], 10);
            ConvertButton.Font = new Font(privateFonts.Families[0], 10);
            FormatLabel.Font = new Font(privateFonts.Families[0], 11);
            FormatBox.Font = new Font(privateFonts.Families[0], 10);
            QualitySettings.Font = new Font(privateFonts.Families[0], 8);
            TargetDirLabel.Font = new Font(privateFonts.Families[0], 11);
            PathTextBox.Font = new Font(privateFonts.Families[0], 8);
            ChooseButton.Font = new Font(privateFonts.Families[0], 10);
            ResetButton.Font = new Font(privateFonts.Families[0], 10);
            ProgressTextBox.Font = new Font(privateFonts.Families[0], 10);
            ProgressLabel.Font = new Font(privateFonts.Families[0], 11);
            StepLabel.Font = new Font(privateFonts.Families[0], 8);
            ASettingsLabel.Font = new Font(privateFonts.Families[0], 8);
            ReencodeVideoCheck.Font = new Font(privateFonts.Families[0], 8);
            ReencodeAudioCheck.Font = new Font(privateFonts.Families[0], 8);
            AdvancedInformationsCheck.Font = new Font(privateFonts.Families[0], 8);
            AdvancedInformationsTextBox.Font = new Font(privateFonts.Families[0], 10);
        }

        private static string SanitizeFileName(string fileName)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                fileName = fileName.Replace(c.ToString(), "_");
            }
            return fileName;
        }

        private static string GetUniqueFileName(string path)
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

        private static RadioButton GetCheckedRadio(GroupBox groupBox)
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

        private static string StoreInformations(Video video)
        {
            string title = "Titel: " + video.Title + Environment.NewLine;
            string author = "Autor: " + video.Author.ChannelTitle + Environment.NewLine;
            string uploadDate = "Upload-Date: " + video.UploadDate.ToString() + Environment.NewLine;
            string duration = "Duration: " + video.Duration.ToString() + Environment.NewLine;
            string viewCount = "ViewCount: " + video.Engagement.ViewCount + Environment.NewLine;
            string likeCount = "LikeCount: " + video.Engagement.LikeCount + Environment.NewLine;
            IEnumerable<string> tags = video.Keywords;
            string allTags = "Tags: ";

            if (tags.Any())
            {
                foreach (string tag in tags)
                {
                    allTags += tag + ", ";
                }
                allTags += Environment.NewLine;
            }
            else
            {
                allTags += "No tags" + Environment.NewLine;
            }

            return title + author + uploadDate + duration + viewCount + likeCount + allTags + Environment.NewLine;
        }

        private void HandleException(Exception ex)
        {
            switch (ex)
            {
                case ArgumentNullException _:
                    _ = MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case StreamNotAvaibleException _:
                    _ = MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case YoutubeExplode.Exceptions.VideoUnavailableException _:
                    _ = MessageBox.Show("The requested video is unavailable. Please check the URL and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case YoutubeExplode.Exceptions.VideoUnplayableException _:
                    _ = MessageBox.Show("The requested video is unplayable. Please check the URL and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case YoutubeExplode.Exceptions.RequestLimitExceededException _:
                    _ = MessageBox.Show("Too many requests to YouTube. Please wait a moment and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case YoutubeExplode.Exceptions.YoutubeExplodeException _:
                    _ = MessageBox.Show("An error with YoutubeExplode has occurred. Please check your input and internet connection and try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case System.Net.Http.HttpRequestException _:
                    _ = MessageBox.Show("Network error. Please check your internet connection and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case IOException _:
                    _ = MessageBox.Show("Error accessing files. Please ensure you have sufficient permissions and disk space.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case ArgumentException _:
                    _ = MessageBox.Show("The given URL is invalid. Please check the URL and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case TaskCanceledException _:
                    CleanUpFiles();
                    break;
                case OperationCanceledException _:
                    CleanUpFiles();
                    break;
                case Exception _:
                    _ = MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            if (ProgressTextBox != null && !ProgressTextBox.IsDisposed)
            {
                ProgressTextBox.Clear();
                ProgressTextBox.SelectionColor = Color.Red;
                ProgressTextBox.AppendText("Download failed!");
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