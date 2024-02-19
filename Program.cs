using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Basic_Screenshotter_Form
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Determine which monitor to screenshot
            string[] args = Environment.GetCommandLineArgs();
            int monitorIndex = 0;
            if (args.Length > 1)
            {
                try
                {
                    monitorIndex = Convert.ToInt32(args[1]);
                }
                catch { }
            }

            // Initialize settings and determine directory.
            string directory = "";
            bool playAudio = false, clipboard = false, open = false;
            if (File.Exists("settings.txt"))
            {
                string settings = File.ReadAllText("settings.txt");

                // get directory from settings
                directory = settings.Substring(settings.IndexOf("OutputDirectory=\"") + ("OutputDirectory=\"".Length));
                directory = directory.Substring(0, directory.IndexOf("\""));
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                // play audio?
                string audio = settings.Substring(settings.IndexOf("PlayAudio=") + ("PlayAudio=".Length), 1);
                if (audio.ToLower() == "y")
                    playAudio = true;

                // clipboard?
                string clipboardStr = settings.Substring(settings.IndexOf("CopyToClipboard=") + ("CopyToClipboard=".Length), 1);
                if (clipboardStr.ToLower() == "y")
                    clipboard = true;

                // open?
                string openStr = settings.Substring(settings.IndexOf("OpenOnSuccess=") + ("OpenOnSuccess=".Length), 1);
                if (openStr.ToLower() == "y")
                    open = true;
            }
            else
            {
                if (!Directory.Exists("Screenshots"))
                    Directory.CreateDirectory("Screenshots");

                directory = "Screenshots\\";
            }


            // Take screenshot
            Rectangle rect = Screen.AllScreens[monitorIndex].Bounds;
            Bitmap bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics captureGraphics = Graphics.FromImage(bitmap);
            captureGraphics.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);

            // copy to clipboard
            if (clipboard)
                Clipboard.SetImage(bitmap);

            // save it
            if (directory.Trim() != "" && directory.Substring(directory.Length - 1) != "\\" && directory.Substring(directory.Length - 1) != "/")
                directory += "\\";

            string fileName = DateTime.Now.ToString("yyyy-MM-dd HH mm ss") + " - Screen " + monitorIndex;

            if (File.Exists(fileName + ".png") || File.Exists(directory + fileName + ".png"))
            {
                fileName += " (";
                int i = 1;
                while (File.Exists(fileName + i + ").png") || File.Exists(directory + fileName + i + ").png"))
                    i++;

                fileName += i + ")";
            }

            bitmap.Save(fileName + ".png", ImageFormat.Png);

            // move to directory
            File.Move(fileName + ".png", directory + fileName + ".png");


            // open it
            if (open)
                Process.Start(directory + fileName + ".png");

            // play audio
            if (playAudio)
            {
                SoundPlayer sound = new SoundPlayer(Properties.Resources.screenshot);
                sound.Play();

                var t = Task.Run(async delegate
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    return 42;
                });
                t.Wait();
            }
        }
    }
}
