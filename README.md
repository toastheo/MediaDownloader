# Media Downloader (v.1.3.0)

## Description

MediaDownloader is a Windows Forms application developed in C# using the .NET Framework. It leverages the powerful capabilities of the Youtube.Explode libary to provide an efficient way for downloading media content.

## Disclaimer:

Compliance with Terms of Service: The MediaDownloader software is intended solely for educational and research purposes. Users are responsible for ensuring compliance with the Terms of Service of any platform they interact with using this software, including but not limited to YouTube.

No Endorsement of Misuse: This software is provided with the understanding that it will not be used to violate any copyrights or terms of service. The developer of MediaDownloader do not endorse or condone any misuse of the software for illegal or unethical activities.

Liability: The developer of MediaDownloader shall not be held liable for any misuse of the software. Users assume all responsibility and risk associated with the use of this software.

No Warranty: MediaDownloader is provided "as is" without any warranty of any kind, either express or implied, including but not limited to the implied warranties of merchantability, fitness for a particular purpose, or non-infringement.

Changes to the Disclaimer: This disclaimer is subject to change at any time and users are encouraged to review it periodically.

## Features

- Download videos and audio in various formats (mp4, mkv, webm, flv, mp3, wav, oga, m4a, aac)
- User-friendly Windows Form interface
- Freely selectable video quality from 360p to 8K
- Control over conversion of the video or audio

![image](https://github.com/toastheo/MediaDownloader/assets/114708595/1a7dde28-7d9c-4f91-8825-18e960e99bb7)

##  Installation

To use the Media Downloader, follow these steps:

### Variant 1 (Prebuild):

1. Download the application from [Onedrive](https://1drv.ms/u/s!ArgwWHbVjXCmj6ZPER-n-JfY7KYxkA?e=zmfJyV "MediaDownloader Onedrive").
2. Extract the .zip file.
3. Run MediaDownloader.

### Variant 2:

1. Clone the repository. You can use `git clone https://github.com/toastheo/MediaDownloader`.
2. Open the solution file in Visual Studio 2022 or higher.
3. The project uses NuGet packages. You can restore them by either using the Visual Studio interface or by running the following command in the Package Manager Console: `Update-Package -reinstall`.
4. Build the solution to resolve dependencies.
5. Run the application from Visual Studio or the executable in the build directory.

## Usage

After launching the application:

1. Enter the URL of the media you want to download.
2. Select the desired format and quality.
3. Optional: Choose the save location (Default is the default download folder).
4. Optional: You can also select in the advanced options whether the video or audio stream should be re-converted. If you have problems with playback, activating these options can help. The recommended options are activated by default.
5. Click the "Start Download" button and the media will download to the specified location.

## Dependencies

- .NET Framework 4.8
- Costura.Fody
- Youtube.Explode

## Contributing

Contributions to the Media Downloader are welcome. Please follow these steps to contribute:

1. Fork the repository.
2. Create a new branch for your feature `git checkout -b feature/AmazingFeature`.
3. Commit your changes `git commit -m 'Add some AmazingFeature'`.
4. Push to the branch `git push origin feature/AmazingFeature`.
5. Open a Pull Request.

## License

### MediaDownloader License
This project is licensed under the [UNLICENSE](https://unlicense.org).

### ffmpeg License:
Please note that this software uses ffmpeg, which is a separate software not covered by the Unlicense of MediaDownloader. ffmpeg is licensed under the LGPL-2.1 (or later) or GPL-2.0 (or later) licenses, depending on the build configuration. This affects the redistribution of ffmpeg in binary form.

For detailed information on ffmpeg licensing, please visit https://www.ffmpeg.org/legal.html.

### Costura.Fody and Youtube.Explode License:
This software incorporates Costura.Fody and Youtube.Explode, both licensed under the MIT License, a permissive free software license similar to the Unlicense, allowing broad usage and distribution.
For more information on the MIT License, please visit https://opensource.org/licenses/MIT.

### Important Notice:
By using MediaDownloader, you agree to comply with the licensing terms of ffmpeg, Costura.Fody, and Youtube.Explode. It is the user's responsibility to ensure compliance with these terms.

## Acknowledgments
Thanks to the developers of Costury.Fody and Youtube.Explode for their fantastic libaries.
