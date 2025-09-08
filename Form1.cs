using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CprWavExtractor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Cubase Project (*.cpr)|*.cpr|All files|*.*";
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
                var inPath = txtInput.Text.Trim();
                var outPath = txtOutput.Text.Trim();
                if (!File.Exists(inPath)) throw new FileNotFoundException("Input not found.", inPath);
                if (string.IsNullOrWhiteSpace(outPath)) throw new InvalidOperationException("Output path missing.");

                const int ch = 2, sr = 48000, bps = 16, seconds = 5;
                int frameBytes = ch * (bps / 8);                 // 4
                int dataLen = sr * seconds * frameBytes;         // 960,000

                byte[] blob = File.ReadAllBytes(inPath);
                if (blob.Length < dataLen) throw new InvalidOperationException("File too small.");

                long start = blob.LongLength - dataLen;          // carve last 5s
                var pcmLE = new byte[dataLen];
                Buffer.BlockCopy(blob, (int)start, pcmLE, 0, dataLen);

                string outFile = Path.ChangeExtension(outPath, ".wav");
                WriteRiffWav(outFile, ch, sr, bps, pcmLE);

                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] OK: wrote {outFile} (data={dataLen} bytes)\r\n");
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
            var inPath = txtInput.Text.Trim();
            var baseName = Path.GetFileNameWithoutExtension(inPath);
            var dir = Path.GetDirectoryName(inPath) ?? ".";
            txtOutput.Text = Path.Combine(dir, baseName + "_preview.wav");
        }

        // ---- Helpers ----
        private static void SwapEndianPerSampleInPlace(byte[] buf, int sampleBytes)
        {
            // sampleBytes = 2 for 16-bit
            for (int i = 0; i + sampleBytes <= buf.Length; i += sampleBytes)
            {
                byte t = buf[i];
                buf[i] = buf[i + 1];
                buf[i + 1] = t;
            }
        }

        private void ExtractTailSweep(string inPath, string outPathBase)
        {
            const int ch = 2, sr = 48000, bps = 16, seconds = 5;
            int bytesPerSample = bps / 8;              // 2
            int frameBytes = ch * bytesPerSample;      // 4
            int dataLen = sr * seconds * frameBytes;   // 960,000

            byte[] blob = File.ReadAllBytes(inPath);
            if (blob.LongLength < dataLen) throw new InvalidOperationException("File too small.");

            // Create output folder
            string baseDir = Path.GetDirectoryName(outPathBase) ?? ".";
            string stem = Path.GetFileNameWithoutExtension(outPathBase);
            string outDir = Path.Combine(baseDir, stem + "_candidates");
            Directory.CreateDirectory(outDir);

            // Try paddings 0..2048 stepping 64 bytes
            for (int pad = 0; pad <= 2048; pad += 64)
            {
                long start = blob.LongLength - dataLen - pad;
                if (start < 0) break;

                // 1) Big-endian→Little-endian (expected)
                byte[] pcmLE = new byte[dataLen];
                Buffer.BlockCopy(blob, (int)start, pcmLE, 0, dataLen);
                // swap 16-bit samples only (do not touch headers; this is raw PCM window)
                for (int i = 0; i + 1 < pcmLE.Length; i += 2)
                { byte t = pcmLE[i]; pcmLE[i] = pcmLE[i + 1]; pcmLE[i + 1] = t; }

                string fLE = Path.Combine(outDir, $"{stem}_off{pad}_LE.wav");
                WriteRiffWav(fLE, ch, sr, bps, pcmLE);

                // 2) No swap (in case window already LE)
                byte[] pcmRaw = new byte[dataLen];
                Buffer.BlockCopy(blob, (int)start, pcmRaw, 0, dataLen);
                string fRaw = Path.Combine(outDir, $"{stem}_off{pad}_RAW.wav");
                WriteRiffWav(fRaw, ch, sr, bps, pcmRaw);

                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] start={start} pad={pad} → {fLE} and {fRaw}\r\n");
            }

            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] Wrote candidates in {outDir}\r\n");
        }

        private static void WriteRiffWav(string path, int channels, int sampleRate, int bitsPerSample, byte[] data)
        {
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

        private void ExtractTailWithOffsets(string inPath, string outPathBase)
        {
            const int ch = 2, sr = 48000, bps = 16, seconds = 5;
            int bytesPerSample = bps / 8;              // 2
            int frameBytes = ch * bytesPerSample;      // 4
            int dataLen = sr * seconds * frameBytes;   // 960,000

            byte[] blob = File.ReadAllBytes(inPath);
            int[] offsets = { 0, 512, 1024 };

            foreach (int pad in offsets)
            {
                long start = blob.LongLength - dataLen - pad;
                if (start < 0) continue;

                var pcm = new byte[dataLen];
                Buffer.BlockCopy(blob, (int)start, pcm, 0, dataLen);

                // BE → LE on PCM only
                for (int i = 0; i + 1 < pcm.Length; i += 2)
                {
                    byte t = pcm[i]; pcm[i] = pcm[i + 1]; pcm[i + 1] = t;
                }

                string path = Path.Combine(
                    Path.GetDirectoryName(outPathBase) ?? ".",
                    Path.GetFileNameWithoutExtension(outPathBase) + $"_stereo16_off{pad}.wav"
                );
                WriteRiffWav(path, ch, sr, bps, pcm);
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] Wrote {path} (start={start}, data={dataLen})\r\n");
            }
        }



        // Drag & drop on input box
        private void txtInput_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        private void txtInput_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    txtInput.Text = files[0];
                    SuggestOutputPath();
                }
            }
            catch { }
        }
    }
}
