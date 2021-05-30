using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alturos.Yolo;
using Alturos.Yolo.Model;

namespace ObjectRecognitionInPhotos
{
    //1971 2021 0

    public partial class Form1 : Form
    {
        private string fileName = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();

            if (res==DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(fileName);
            }
            else
            {
                MessageBox.Show("Картинка не выбрана !","Выберите картинку !",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            YoloWrapper yolo = new YoloWrapper("yolov3.cfg", "yolov3.weights", "coco.names");

            MemoryStream memoryStream = new MemoryStream();
            pictureBox1.Image.Save(memoryStream, ImageFormat.Jpeg);
            List<YoloItem> items = yolo.Detect(memoryStream.ToArray()).ToList<YoloItem>();

            Image finalImage = pictureBox1.Image;
            Graphics graph = Graphics.FromImage(finalImage);
            Font font = new Font("Consolas",16,FontStyle.Bold);
            SolidBrush brush = new SolidBrush(Color.Yellow);

            foreach (YoloItem item in items)
            {
                Point rectPoint = new Point(item.X,item.Y);
                Size rectSize = new Size(item.Width,item.Height);
                Rectangle rect = new Rectangle(rectPoint,rectSize);

                Pen pen = new Pen(Color.Yellow,2);

                graph.DrawRectangle(pen,rect);
                graph.DrawString(item.Type,font,brush,rectPoint);

            }

            pictureBox1.Image = finalImage;

        }
    }
}
