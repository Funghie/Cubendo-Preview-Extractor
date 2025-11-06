using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CprWavExtractor
{
    public partial class Form1 : Form
    {
        // Fixed params
        private const int CH = 2, SR = 48000, BPS = 16, SEC = 5;
        private const int FRAME_BYTES = CH * (BPS / 8);
        private const int DATA_LEN = SR * SEC * FRAME_BYTES;

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Cubase/Nuendo Projects (*.cpr;*.npr)|*.cpr;*.npr|All files|*.*";
            saveFileDialog1.Filter = "WAV audio (*.wav)|*.wav|All files|*.*";
        }

        private void btnBrowseIn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtInput.Text = openFileDialog1.FileName;
                SuggestOutputPath();
            }
        }

        private void btnBrowseOut_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOutput.Text)) SuggestOutputPath();
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK) txtOutput.Text = saveFileDialog1.FileName;
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Working";
            try
            {
                string inPath = txtInput.Text.Trim();
                string outPath = txtOutput.Text.Trim();
                if (!File.Exists(inPath)) throw new FileNotFoundException("Input not found.", inPath);
                if (string.IsNullOrWhiteSpace(outPath)) throw new InvalidOperationException("Output path missing.");

                int pad = ExtractWithAutoOffset(inPath, outPath);
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] pad={pad}, wrote {Path.ChangeExtension(outPath, ".wav")}\r\n");
                lblStatus.Text = "Done";
            }
            catch (Exception ex)
            {
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] ERROR: {ex.Message}\r\n");
                lblStatus.Text = "Error";
            }
        }



        private void SuggestOutputPath()
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text)) return;
            string inPath = txtInput.Text.Trim();
            string baseName = Path.GetFileNameWithoutExtension(inPath);
            string dir = Path.GetDirectoryName(inPath) ?? ".";
            txtOutput.Text = Path.Combine(dir, baseName + "_preview.wav");
        }

        // --- Core ---
        // Make sure these are class-level consts


        public static int ExtractWithAutoOffset(string inPath, string outPath)
        {
            byte[] blob = File.ReadAllBytes(inPath);
            if (blob.LongLength < DATA_LEN) throw new InvalidOperationException("File too small.");

            int pad = FindBestOffset(blob, DATA_LEN, 4096);   // static helper
            long start = blob.LongLength - DATA_LEN - pad;

            byte[] pcmLE = new byte[DATA_LEN];
            Buffer.BlockCopy(blob, (int)start, pcmLE, 0, DATA_LEN);

            string outFile = Path.ChangeExtension(outPath, ".wav");
            WriteRiffWav(outFile, CH, SR, BPS, pcmLE);        // static helper
            return pad;
        }


        // Score a PCM window: prefer non-silent, low DC, decent RMS
        private static double ScoreWindow(byte[] buf, int start, int len)
        {
            int n = Math.Min(len, 16384);
            if (n <= 0) return double.NegativeInfinity;

            long sum = 0, sumSq = 0;
            int zeros = 0;

            for (int i = start; i + 1 < start + n; i += 2)
            {
                short s = (short)(buf[i] | (buf[i + 1] << 8));
                if (s == 0) zeros++;
                int v = s;
                sum += v;
                sumSq += (long)v * v;
            }

            int samples = n / 2;
            double mean = sum / (double)samples;
            double rms = Math.Sqrt(sumSq / (double)samples);
            double dc = Math.Abs(mean);
            double zeroFrac = zeros / (double)samples;

            return rms - 4.0 * dc - 1000.0 * zeroFrac;
        }

        private static int FindBestOffset(byte[] blob, int dataLen, int maxPad)
        {
            double best = double.NegativeInfinity;
            int bestPad = 0;

            for (int pad = 0; pad <= maxPad; pad += 4) // align to 4
            {
                long start = blob.LongLength - dataLen - pad;
                if (start < 0) break;
                double sc = ScoreWindow(blob, (int)start, dataLen);
                if (sc > best) { best = sc; bestPad = pad; }
            }
            return bestPad;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private static void WriteRiffWav(string path, int channels, int sampleRate, int bitsPerSample, byte[] data)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var bw = new BinaryWriter(fs, Encoding.ASCII))
            {
                uint dataLen = (uint)data.Length;
                uint byteRate = (uint)(sampleRate * channels * (bitsPerSample / 8));
                ushort blockAlign = (ushort)(channels * (bitsPerSample / 8));

                bw.Write(Encoding.ASCII.GetBytes("RIFF"));
                bw.Write(36u + dataLen);
                bw.Write(Encoding.ASCII.GetBytes("WAVE"));
                bw.Write(Encoding.ASCII.GetBytes("fmt "));
                bw.Write(16u);
                bw.Write((ushort)1); // PCM
                bw.Write((ushort)channels);
                bw.Write((uint)sampleRate);
                bw.Write(byteRate);
                bw.Write(blockAlign);
                bw.Write((ushort)bitsPerSample);
                bw.Write(Encoding.ASCII.GetBytes("data"));
                bw.Write(dataLen);
                bw.Write(data);
            }
        }

        // Drag & drop
        private void txtInput_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        private void txtInput_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    txtInput.Text = files[0];
                    SuggestOutputPath();
                }
            }
            catch { }
        }

        private static void SwapEndianPerSampleInPlace(byte[] buf, int sampleBytes)
        {
            for (int i = 0; i + sampleBytes <= buf.Length; i += sampleBytes)
            {
                int l = i;
                int r = i + sampleBytes - 1;
                while (l < r)
                {
                    byte tmp = buf[l];
                    buf[l] = buf[r];
                    buf[r] = tmp;
                    l++; r--;
                }
            }
        }
    }
}
