namespace MyChromaKey
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.InputImagePictureBox = new System.Windows.Forms.PictureBox();
            this.OutImagePictureBox = new System.Windows.Forms.PictureBox();
            this.AddImageButton = new System.Windows.Forms.Button();
            this.imagePathLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.InputImagePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutImagePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // InputImagePictureBox
            // 
            this.InputImagePictureBox.Location = new System.Drawing.Point(12, 47);
            this.InputImagePictureBox.Name = "InputImagePictureBox";
            this.InputImagePictureBox.Size = new System.Drawing.Size(506, 454);
            this.InputImagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.InputImagePictureBox.TabIndex = 0;
            this.InputImagePictureBox.TabStop = false;
            // 
            // OutImagePictureBox
            // 
            this.OutImagePictureBox.Location = new System.Drawing.Point(544, 47);
            this.OutImagePictureBox.Name = "OutImagePictureBox";
            this.OutImagePictureBox.Size = new System.Drawing.Size(506, 454);
            this.OutImagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.OutImagePictureBox.TabIndex = 1;
            this.OutImagePictureBox.TabStop = false;
            // 
            // AddImageButton
            // 
            this.AddImageButton.Location = new System.Drawing.Point(395, 12);
            this.AddImageButton.Name = "AddImageButton";
            this.AddImageButton.Size = new System.Drawing.Size(145, 25);
            this.AddImageButton.TabIndex = 2;
            this.AddImageButton.Text = "Выбрать изображение";
            this.AddImageButton.UseVisualStyleBackColor = true;
            this.AddImageButton.Click += new System.EventHandler(this.AddImageButton_Click);
            // 
            // imagePathLabel
            // 
            this.imagePathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.imagePathLabel.Location = new System.Drawing.Point(12, 12);
            this.imagePathLabel.Name = "imagePathLabel";
            this.imagePathLabel.Size = new System.Drawing.Size(377, 25);
            this.imagePathLabel.TabIndex = 3;
            this.imagePathLabel.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 513);
            this.Controls.Add(this.imagePathLabel);
            this.Controls.Add(this.AddImageButton);
            this.Controls.Add(this.OutImagePictureBox);
            this.Controls.Add(this.InputImagePictureBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.InputImagePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutImagePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox InputImagePictureBox;
        private System.Windows.Forms.PictureBox OutImagePictureBox;
        private System.Windows.Forms.Button AddImageButton;
        private System.Windows.Forms.Label imagePathLabel;
    }
}

