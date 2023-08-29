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
    public class StreamNotAvaibleException : Exception
    {
        public StreamNotAvaibleException()
            : base("The requested stream is not available!")
        {
        }

        public StreamNotAvaibleException(string containertype)
            : base($"The requested stream with the containertype {containertype} is not available!")
        {
        }
    }
}