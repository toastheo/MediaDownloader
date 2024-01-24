# Media Downloader (v.1.3.1)

## Description

MediaDownloader is a Windows Forms application developed in C# using the .NET Framework. It leverages the powerful capabilities of the Youtube.Explode libary to provide an efficient way for downloading media content.

## Disclaimer:

Compliance with Terms of Service: The MediaDownloader software is intended solely for educational and research purposes. Users are responsible for ensuring compliance with the Terms of Service of any platform they interact with using this software, including but not limited to YouTube.

No Endorsement of Misuse: This software is provided with the understanding that it will not be used to violate any copyrights or terms of service. The developer of MediaDownloader do not endorse or condone any misuse of the software for illegal or unethical activities.

Liability: The developer of MediaDownloader shall not be held liable for any misuse of the software. Users assume all responsibility and risk associated with the use of this software.

Changes to the Disclaimer: This disclaimer is subject to change at any time and users are encouraged to review it periodically.

## Features

- Download videos and audio in various formats (mp4, mkv, webm, flv, mp3, wav, oga, m4a, aac)
- User-friendly Windows Form interface
- Freely selectable video quality from 360p to 8K
- Control over conversion of the video or audio

![image](https://github.com/toastheo/MediaDownloader/assets/114708595/1a7dde28-7d9c-4f91-8825-18e960e99bb7)

## Dependencies

- FFmpeg
- .NET Framework 4.8
- Costura.Fody
- Youtube.Explode

##  Installation

To use the Media Downloader, follow these steps:

### Variant 1 (Prebuild):

1. **Download the Application**: Visit the [MediaDownloader Onedrive link](https://1drv.ms/u/s!ArgwWHbVjXCmj6t-L3TAlhkMf144DA?e=SHXBa5 "MediaDownloader Onedrive") to download the application.
2. **Extract the .zip file**: After downloading, locate the .zip file in your Downloads folder and right-click to select 'Extract All...' to unzip the contents.
3. **Run MediaDownloader**: Open the extracted folder and double-click on the 'MediaDownloader' executable to start the application.

### Variant 2:

1. **Install Development Tools**: Download and install Visual Studio 2022 and the .NET Framework 4.8 from the [Visual Studio Website](https://visualstudio.microsoft.com/de/downloads/ "Visual Studio Downloadpage").
2. **Clone the Repository**: Use Git to clone the repository. You can use `git clone https://github.com/toastheo/MediaDownloader`.
3. **Install FFmpeg**: You can download FFmpeg via [this link](https://ffmpeg.org/download.html "Download Page FFmpeg"). Add FFmpeg to your system path for global use or copy it into the project's source file directories. If you need help, follow this guide on [adding FFmpeg to your system path](#how-to-add-ffmpeg-to-the-system-path).
4. **Open the Project**: Launch Visual Studio, select 'Open a project or solution', and navigate to the cloned repository to open the solution file or just double click the .sln file.
5. **Restore NuGet Packages**: Restore the necessary NuGet packages either through the Visual Studio interface or by executing `Update-Package -reinstall` in the Package Manager Console.
6. **Build the Solution**: Build the project in Visual Studio to resolve all dependencies.
7. **Run the Application**: Start the application directly from Visual Studio or by locating and running the executable in the build directory.

For any issues or questions, please feel free to contact me for assistance.

## Usage

After launching the application:

1. Enter the URL of the media you want to download.
2. Select the desired format and quality.
3. Optional: Choose the save location (Default is the default download folder).
4. Optional: You can also select in the advanced options whether the video or audio stream should be re-converted. If you have problems with playback, activating these options can help. The recommended options are activated by default.
5. Click the "Start Download" button and the media will download to the specified location.

## Contributing

Contributions to the Media Downloader are welcome. Please follow these steps to contribute:

1. Fork the repository.
2. Create a new branch for your feature `git checkout -b feature/AmazingFeature`.
3. Commit your changes `git commit -m 'Add some AmazingFeature'`.
4. Push to the branch `git push origin feature/AmazingFeature`.
5. Open a Pull Request.

## How to add ffmpeg to the system path

1. **Download FFmpeg**: First, visit the official [FFmpeg website](https://ffmpeg.org/download.html "Download Page FFmpeg") and download the latest version of FFmpeg for Windows. Usually, this will be a .zip file.
2. **Extract the Files**: After downloading, extract the files from the .zip file. You can store them in any location, but a simple path like C:\FFmpeg is recommended for easy access.
3. **Locate the 'bin' Folder**: In the extracted FFmpeg folder, you'll find a subfolder named bin. This contains the executable files you will need.
4. **Copy the Path**: Open the bin folder, click on the address bar in the file explorer to reveal the full path, and copy this path. It should look something like C:\FFmpeg\bin.
5. **Open System Properties**: Press the Windows key, type “system properties”, and select “Edit the system environment variables”. This will open the System Properties window.
6. **Access Environment Variables**: In the System Properties window, click on the “Environment Variables” button near the bottom.
7. **Edit the Path Variable**: In the Environment Variables window, under the “System variables” section, find and select the Path variable, then click “Edit…”.
8. **Add FFmpeg Path**: In the Edit Environment Variable window, click “New” and paste the path you copied earlier (C:\FFmpeg\bin). Click “OK” to close each window.
9. **Verify the Installation**: To verify that FFmpeg has been added to your system path, open a command prompt (cmd) and type ffmpeg -version. If it displays the version information, FFmpeg is successfully installed and accessible from the command line.

## License

### MediaDownloader License
This project is licensed under the [MIT-License](https://opensource.org/license/mit/).

### FFmpeg License:
Please note that this software uses FFmpeg, which is a separate software not covered by the MIT-License of MediaDownloader. FFmpeg is licensed under the LGPL-2.1 (or later) or GPL-2.0 (or later) licenses, depending on the build configuration. This affects the redistribution of FFmpeg in binary form.

For detailed information on FFmpeg licensing, please visit https://www.ffmpeg.org/legal.html.

## Acknowledgments
Thanks to the developers of Costury.Fody and Youtube.Explode for their fantastic libaries.
