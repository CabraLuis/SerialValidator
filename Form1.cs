using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.UniversalAccessibility.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.QrCode;

namespace SerialValidator
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                //Define route and connection to SQLite DB
                string path = "L:\\IT\\SerialValidatorDBPlanner";
                //string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string conn = $"Data Source = {path}\\database.db; Version = 3;";
                SQLiteConnection connection = new SQLiteConnection(conn);
                connection.Open();

                //Commands templates
                SQLiteCommand insertCommand = new SQLiteCommand(connection);
                SQLiteCommand selectCommand = new SQLiteCommand(connection);
                selectCommand.CommandText = "SELECT number FROM Numbers WHERE number = @serial";
                insertCommand.CommandText = "INSERT INTO Numbers (number) VALUES (@serial)";

                //Aux variables
                List<string> duplicates = new List<string>();
                List<string> validated = new List<string>();
                string currentSerial;

                //Change txtNumbers text to black
                txtNumbers.SelectAll();
                txtNumbers.SelectionColor = Color.Black;
                txtNumbers.DeselectAll();

                //Flag to skip insertion of data
                bool detectedDuplicate = false;

                for (int i = 0; i < txtNumbers.Lines.Length; i++)
                {
                    currentSerial = txtNumbers.Lines[i];
                    
                    //Skip if there's an empty line
                    if (currentSerial == "")
                    {
                        continue;
                    }

                    //Search in DB for duplicates and add them to aux variable
                    selectCommand.Parameters.AddWithValue("serial", currentSerial);
                    SQLiteDataReader reader = selectCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        string serial;
                        while (reader.Read())
                        {
                            serial = reader.GetString(0);
                            duplicates.Add(serial);

                            //Highlight duplicate serials
                            txtNumbers.Select(txtNumbers.GetFirstCharIndexFromLine(i), currentSerial.Length);
                            txtNumbers.SelectionColor = Color.Red;
                            txtNumbers.DeselectAll();
                        }
                        detectedDuplicate = true;
                    }
                    else
                    {
                        validated.Add(currentSerial);
                    }
                    reader.Close();
                }
                //If there's no more duplicates, insert serials to DB
                if (!detectedDuplicate)
                {
                    foreach (string serial in validated){
                        insertCommand.Parameters.AddWithValue("serial", serial);
                        insertCommand.ExecuteNonQuery();
                    }
                }
                detectedDuplicate = false;
                connection.Close();

                //Generate auxiliary QR
                Bitmap qrCode = GenerateQR();
                pbQrCode.Image = qrCode;

                if (duplicates.Count() != 0)
                {
                    string duplicatesString = string.Join("\n", duplicates);
                    MessageBox.Show("Seriales Inválidos:\n" + duplicatesString, "Seriales Inválidos Encontrados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Seriales válidos", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnGenerarPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string currentSerial;

                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Verdana", 18, XFontStyleEx.Bold);
                page.Orientation = PdfSharp.PageOrientation.Landscape;
                int pageNum = 1;
                gfx.DrawString("Verificador de Seriales", titleFont, XBrushes   .Black, new XRect(0, 0, page.Width.Point, page.Height.Point), XStringFormats.TopCenter);
                gfx.DrawString(pageNum.ToString(), titleFont, XBrushes.Black, new XRect(0, 0, page.Width.Point, page.Height.Point), XStringFormats.TopRight);

                double xImage = 20;
                double yImage = 75;

                for (int i = 0; i < txtNumbers.Lines.Length; i++)
                {
                    currentSerial = txtNumbers.Lines[i];

                    if (currentSerial == "" )
                    {
                        continue;
                    }

                    Bitmap generatedBarcode = GenerateBarcode(currentSerial);
                    XImage barcodeTemp = XImage.FromGdiPlusImage(generatedBarcode);

                    gfx.DrawImage(barcodeTemp, xImage, yImage);

                    yImage += 113;

                    if (XUnit.FromPoint(yImage) + XUnit.FromPoint(barcodeTemp.Size.Height) >= page.Height)
                    {
                        yImage = 75;
                        xImage += barcodeTemp.Size.Width * 2.4;
                    }

                    if (XUnit.FromPoint(xImage) + XUnit.FromPoint(barcodeTemp.Size.Width) >= page.Width && i + 1 < txtNumbers.Lines.Length)
                    {
                        pageNum++;
                        page = document.AddPage();
                        page.Orientation = PdfSharp.PageOrientation.Landscape;
                        gfx = XGraphics.FromPdfPage(page);
                        gfx.DrawString(pageNum.ToString(), titleFont, XBrushes.Black, new XRect(0, 0, page.Width.Point, page.Height.Point), XStringFormats.TopRight);
                        xImage = 20;
                        yImage = 75;
                    }
                }

                Bitmap qrCode = GenerateQR();
                pbQrCode.Image = qrCode;

                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.ShowDialog();

                string filename = dialog.SelectedPath + "\\Seriales.pdf";
                document.Save(filename);
                ProcessStartInfo startInfo = new ProcessStartInfo(filename);
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Bitmap GenerateBarcode(string serial)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.CODE_128;
            barcodeWriter.Options.PureBarcode = false;
            barcodeWriter.Options.Margin = 0;
            barcodeWriter.Options.Height = 40;
            Bitmap barcodeBitmap = barcodeWriter.Write(serial);
            return barcodeBitmap;
        }

        private Bitmap GenerateQR()
        {
            var barcodeWriter = new BarcodeWriter();

            QrCodeEncodingOptions options = new QrCodeEncodingOptions()
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = 200,
                Height = 200,
                Margin = 0,
            };
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            barcodeWriter.Options = options;

            string concatNumbers = string.Join(Environment.NewLine, txtNumbers.Lines);
            var qrCodeBitmap = barcodeWriter.Write(concatNumbers);

            return qrCodeBitmap;
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.logo_ico;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(txtNumbers.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
