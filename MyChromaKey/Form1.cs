using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyChromaKey
{
    public partial class Form1 : Form, IView
    {
        private Presenter presenter;
        private delegate void ImageUpdate(Bitmap image);
        public Form1()
        {
            InitializeComponent();

            presenter = new Presenter(this);
        }

        public void SetImage(Bitmap image)
        {
            OutImagePictureBox.Image = image;

            image.Save("output.jpg");
        }

        private void AddImageButton_Click(object sender, EventArgs e)
        {
            AddImageButton.Enabled = false;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        imagePathLabel.Text = openFileDialog.FileName;

                        System.IO.FileInfo file = new System.IO.FileInfo(imagePathLabel.Text); //запрещаем открывать большие файлы
                        long size = file.Length;
                        if (size > 20000000)
                        {
                            MessageBox.Show("Не надо открывать большие файлы!!!");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Big terrible error.\n\nError message: {ex.Message}\n\n" +
                        $"Details:\n\n{ex.StackTrace}");
                    }
                    InputImagePictureBox.Image = Image.FromFile(imagePathLabel.Text);
                }
            }

            presenter.CropImage(InputImagePictureBox.Image);

            AddImageButton.Enabled = true;
        }
    }
}
