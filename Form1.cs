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
                string conn = "Data Source = C:\\Users\\itmex\\OneDrive - NIBE\\Escritorio\\database.db;Version = 3;";
                SQLiteConnection connection = new SQLiteConnection(conn);
                connection.Open();

                SQLiteCommand insertCommand = new SQLiteCommand(connection);
                SQLiteCommand selectCommand = new SQLiteCommand(connection);
                selectCommand.CommandText = "SELECT number FROM Numbers WHERE number = @serial";
                insertCommand.CommandText = "INSERT INTO Numbers (number) VALUES (@serial)";

                List<string> duplicates = new List<string>();
                string currentSerial;

                for (int i = 0; i < txtNumbers.Lines.Length; i++)
                {
                    currentSerial = txtNumbers.Lines[i];
                    selectCommand.Parameters.AddWithValue("serial", currentSerial);
                    insertCommand.Parameters.AddWithValue("serial", currentSerial);
                    SQLiteDataReader reader = selectCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string serial = reader.GetString(0);
                            duplicates.Add(serial);
                            Console.WriteLine(serial);
                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        insertCommand.ExecuteNonQuery();
                    }

                    if (currentSerial == null)
                    {
                        throw new Exception();
                    }

                    Bitmap generatedBarcode = GenerateBarcode(currentSerial);
                    XImage barcodeTemp = XImage.FromGdiPlusImage(generatedBarcode);


                }
                connection.Close();
                
                pbQrCode.Image = GenerateQR();
                
                if (duplicates.Count() != 0)
                {
                    string duplicatesString = string.Join("\n", duplicates);
                    MessageBox.Show("Seriales Duplicados:\n" + duplicatesString, "Duplicados Encontrados", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                page.Orientation = PdfSharp.PageOrientation.Landscape;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Verdana", 18, XFontStyleEx.Bold);
                XFont serialFont = new XFont("Verdana", 5, XFontStyleEx.Regular);
                gfx.DrawString("Verificador de Seriales", titleFont, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.TopCenter);

                double xImage = 1;
                double yImage = 0.7;

                for (int i = 0; i < txtNumbers.Lines.Length; i++)
                {
                    currentSerial = txtNumbers.Lines[i];

                    if (currentSerial == null)
                    {
                        throw new Exception();
                    }

                    Bitmap generatedBarcode = GenerateBarcode(currentSerial);
                    XImage barcodeTemp = XImage.FromGdiPlusImage(generatedBarcode);

                    //gfx.DrawString(currentSerial, serialFont, XBrushes.Black, );
                    gfx.DrawImage(barcodeTemp, xImage * 93.5, yImage * 75);

                    yImage += 1.5;

                    Console.WriteLine(barcodeTemp.Size);

                    if (yImage >= 8)
                    {
                        yImage = 0.7;
                        xImage += 1.5;
                    }

                }

                pbQrCode.Image = GenerateQR();

                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.ShowDialog();

                string filename = dialog.SelectedPath + "\\Seriales.pdf";
                Console.WriteLine(filename);
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
            barcodeWriter.Options.PureBarcode = true;
            barcodeWriter.Options.Margin = 1;
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
                Width = 300,
                Height = 300
            };
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            barcodeWriter.Options = options;

            string concatNumbers = string.Join(Environment.NewLine, txtNumbers.Lines);
            var qrCodeBitmap = barcodeWriter.Write(concatNumbers);

            return qrCodeBitmap;
        }

       
    }
}
