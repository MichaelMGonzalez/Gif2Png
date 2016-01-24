using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace GifToSpriteSheet
{
    public partial class Form1 : Form
    {
        Stream sourceStream;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GifBitmapDecoder decoder = openFileDialog();
            if (decoder == null)
                return;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int i = 0;
                    foreach (BitmapFrame f in decoder.Frames)
                    {
                        Console.WriteLine("Entering foreach loop");
                        //                        FileStream stream = File.Create(saveFileDialog1.FileName + i + ".png");
                        Stream stream = new FileStream(saveFileDialog1.FileName + i + ".png", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                        Console.WriteLine("Opening stream: " + stream);
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        Console.WriteLine("Creating encoder");
                        encoder.Frames.Add(f);
                        Console.WriteLine("Added frame");
                        Console.WriteLine("Saving stream");
                        encoder.Save(stream);
                        stream.Close();
                        i++;
                    }
                    sourceStream.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
        private GifBitmapDecoder openFileDialog()
        {
            sourceStream = null;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Console.WriteLine(openFileDialog1.FileName);
                    if ((sourceStream = openFileDialog1.OpenFile()) != null)
                    {
                        GifBitmapDecoder decoder = new GifBitmapDecoder(sourceStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        return decoder;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            return null;
        }
    }
}
