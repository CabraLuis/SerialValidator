namespace SerialValidator
{
    partial class mainForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNumbers = new System.Windows.Forms.RichTextBox();
            this.btnValidate = new System.Windows.Forms.Button();
            this.pbQrCode = new System.Windows.Forms.PictureBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.btnGenerarPDF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbQrCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // txtNumbers
            // 
            this.txtNumbers.Location = new System.Drawing.Point(12, 12);
            this.txtNumbers.Name = "txtNumbers";
            this.txtNumbers.Size = new System.Drawing.Size(150, 426);
            this.txtNumbers.TabIndex = 0;
            this.txtNumbers.Text = "";
            this.txtNumbers.WordWrap = false;
            // 
            // btnValidate
            // 
            this.btnValidate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidate.Location = new System.Drawing.Point(168, 12);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(199, 30);
            this.btnValidate.TabIndex = 1;
            this.btnValidate.Text = "Validar Seriales";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // pbQrCode
            // 
            this.pbQrCode.Location = new System.Drawing.Point(168, 238);
            this.pbQrCode.Name = "pbQrCode";
            this.pbQrCode.Size = new System.Drawing.Size(200, 200);
            this.pbQrCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbQrCode.TabIndex = 3;
            this.pbQrCode.TabStop = false;
            // 
            // pbLogo
            // 
            this.pbLogo.Image = global::SerialValidator.Properties.Resources.Thermx_Logo_No_Background;
            this.pbLogo.Location = new System.Drawing.Point(167, 84);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(200, 148);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 4;
            this.pbLogo.TabStop = false;
            // 
            // btnGenerarPDF
            // 
            this.btnGenerarPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarPDF.Location = new System.Drawing.Point(168, 48);
            this.btnGenerarPDF.Name = "btnGenerarPDF";
            this.btnGenerarPDF.Size = new System.Drawing.Size(199, 30);
            this.btnGenerarPDF.TabIndex = 5;
            this.btnGenerarPDF.Text = "Generar PDF";
            this.btnGenerarPDF.UseVisualStyleBackColor = true;
            this.btnGenerarPDF.Click += new System.EventHandler(this.btnGenerarPDF_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 450);
            this.Controls.Add(this.btnGenerarPDF);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.pbQrCode);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.txtNumbers);
            this.Name = "mainForm";
            this.Text = "Serial Validator";
            ((System.ComponentModel.ISupportInitialize)(this.pbQrCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtNumbers;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.PictureBox pbQrCode;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Button btnGenerarPDF;
    }
}

