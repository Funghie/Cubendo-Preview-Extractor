using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace CprWavExtractor
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            // Headless context-menu mode: a file path is passed
            if (args != null && args.Length > 0)
            {
                string inPath = args[0];
                if (!File.Exists(inPath)) return;

                string tempWav = Path.Combine(
                    Path.GetTempPath(),
                    Path.GetFileNameWithoutExtension(inPath) + "_preview.wav"
                );

                try
                {
                    // Static core extractor on Form1 (Option A)
                    Form1.ExtractWithAutoOffset(inPath, tempWav);

                    if (File.Exists(tempWav))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = tempWav,
                            UseShellExecute = true
                        });
                    }
                }
                catch
                {
                    // Silent in headless mode
                }
                return;
            }

            // Normal UI mode
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
