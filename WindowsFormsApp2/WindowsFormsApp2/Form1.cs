using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;
        private Bitmap overlayImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    originalImage = new Bitmap(openFileDialog.FileName);
                    pictureBox1.Image = originalImage;
                }
            }
        }

        private void btnLoadOverlay_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    overlayImage = new Bitmap(openFileDialog.FileName);
                    pictureBox2.Image = overlayImage;
                }
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (originalImage == null || overlayImage == null)
            {
                MessageBox.Show("Please load both images first.");
                return;
            }

            Bitmap processedImage = (Bitmap)originalImage.Clone();
            AdjustContrast(processedImage, 50, 90);
            OverlayImages(processedImage, overlayImage);
            pictureBox3.Image = processedImage;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pictureBox3.Image != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        pictureBox3.Image.Save(saveFileDialog.FileName);
                    }
                }
            }
        }

        private void AdjustContrast(Bitmap bmp, int minPercent, int maxPercent)
        {
            float min = minPercent / 100.0f;
            float max = maxPercent / 100.0f;

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color pixel = bmp.GetPixel(x, y);

                    int r = Clamp((int)(min * 255 + (pixel.R / 255.0) * (max - min) * 255), 0, 255);
                    int g = Clamp((int)(min * 255 + (pixel.G / 255.0) * (max - min) * 255), 0, 255);
                    int b = Clamp((int)(min * 255 + (pixel.B / 255.0) * (max - min) * 255), 0, 255);

                    bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
        }

        private void OverlayImages(Bitmap baseImage, Bitmap overlay)
        {
            for (int y = 0; y < baseImage.Height && y < overlay.Height; y++)
            {
                for (int x = 0; x < baseImage.Width && x < overlay.Width; x++)
                {
                    Color basePixel = baseImage.GetPixel(x, y);
                    Color overlayPixel = overlay.GetPixel(x, y);

                    int r = (basePixel.R + overlayPixel.R) / 2;
                    int g = (basePixel.G + overlayPixel.G) / 2;
                    int b = (basePixel.B + overlayPixel.B) / 2;

                    baseImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
        }

        private int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }
}