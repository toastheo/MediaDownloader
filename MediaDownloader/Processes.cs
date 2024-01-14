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

namespace MediaDownloader
{
    public partial class Form1
    {
        private ProcessStartInfo ProcessMP4(string videoPath, string audioPath, string outputPath, bool reVideo = false, bool reAudio = false)
        {
            string videoSettings = reVideo ? "-c:v libx264" : "-c:v copy";
            string audioSettings = reAudio ? "-c:a aac" : "-c:a copy";

            return new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{videoPath}\" -i \"{audioPath}\" {videoSettings} {audioSettings} \"{outputPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
        }

        private ProcessStartInfo ProcessMKV(string videoPath, string audioPath, string outputPath, bool reVideo = false, bool reAudio = false)
        {
            string videoSettings = reVideo ? "-c:v libx264" : "-c:v copy";
            string audioSettings = reAudio ? "-c:a aac" : "-c:a copy";

            return new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{videoPath}\" -i \"{audioPath}\" {videoSettings} {audioSettings} \"{Path.ChangeExtension(outputPath, ".mkv")}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
        }

        private ProcessStartInfo ProcessWEBM(string videoPath, string audioPath, string outputPath, bool reVideo = false, bool reAudio = false)
        {
            string videoSettings = reVideo ? "-c:v libvpx-vp9" : "-c:v copy";
            string audioSettings = reAudio ? "-c:a libopus" : "-c:a copy";

            return new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{videoPath}\" -i \"{audioPath}\" {videoSettings} {audioSettings} \"{Path.ChangeExtension(outputPath, ".webm")}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
        }

        private ProcessStartInfo ProcessFLV(string videoPath, string audioPath, string outputPath, bool reVideo = false, bool reAudio = false)
        {
            string videoSettings = reVideo ? "-c:v libx264" : "-c:v copy";
            string audioSettings = reAudio ? "-c:a aac" : "-c:a copy";

            return new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{videoPath}\" -i \"{audioPath}\" {videoSettings} {audioSettings} \"{Path.ChangeExtension(outputPath, ".flv")}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
        }

        private ProcessStartInfo ProcessMP3(string webmPath, string outputPath)
        {
            return new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{webmPath}\" -b:a 192K -vn \"{outputPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
        }

        private ProcessStartInfo ProcessOGG(string webmPath, string outputPath)
        {
            return new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{webmPath}\" -c:a libvorbis -q:a 4 \"{Path.ChangeExtension(outputPath, ".oga")}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
        }

        private ProcessStartInfo ProcessM4A(string webmPath, string outputPath)
        {
            return new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{webmPath}\" -c:a aac -b:a 192k \"{Path.ChangeExtension(outputPath, ".m4a")}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
        }

        private ProcessStartInfo ProcessAAC(string webmPath, string outputPath)
        {
            return new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{webmPath}\" -c:a aac -strict -2 -b:a 192k \"{Path.ChangeExtension(outputPath, ".aac")}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
        }

        private ProcessStartInfo ProcessWAV(string webmPath, string outputPath)
        {
            return new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{webmPath}\" -vn -c:a pcm_s16le \"{Path.ChangeExtension(outputPath, ".wav")}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
        }
    }
}