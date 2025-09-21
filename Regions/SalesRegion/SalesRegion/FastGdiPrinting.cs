using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using log4netWrapper;
using RMSDataAccessLayer;
using SUT.PrintEngine.Utils;

namespace SalesRegion
{
    public partial class SalesVM
    {
        private static System.Drawing.Printing.PrintDocument _gdiPrintDocument;
        private static Bitmap _printBitmap;
        private static string _currentPrinterName;

        public void FastGdiPrint(ref FrameworkElement fwe, PrescriptionEntry prescriptionEntry = null)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                Logger.Log(LoggingLevel.Info, $"FastGDI Step 1 - Setup: {stopwatch.ElapsedMilliseconds}ms");

                // Step 1: Use EXACT same visual creation as PrintOriginal
                System.Windows.Size visualSize = new System.Windows.Size(288, 2 * 96); // EXACT copy from PrintOriginal
                DrawingVisual visual = PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);

                Logger.Log(LoggingLevel.Info, $"FastGDI Step 2 - CreateDrawingVisual (same as PrintOriginal): {stopwatch.ElapsedMilliseconds}ms");

                // Step 2: Convert the EXACT same visual to bitmap for GDI+
                _printBitmap = ConvertDrawingVisualToBitmap(visual, visualSize, 300);

                Logger.Log(LoggingLevel.Info, $"FastGDI Step 3 - Visual to Bitmap: {stopwatch.ElapsedMilliseconds}ms");

                // Step 3: Setup GDI+ PrintDocument with EXACT same dimensions as PrintOriginal
                if (_gdiPrintDocument == null || _currentPrinterName != Station.ReceiptPrinterName)
                {
                    _gdiPrintDocument?.Dispose();
                    _gdiPrintDocument = new System.Drawing.Printing.PrintDocument();
                    _gdiPrintDocument.PrinterSettings.PrinterName = Station.ReceiptPrinterName;
                    _currentPrinterName = Station.ReceiptPrinterName;

                    // Convert PrintOriginal WPF Size(288,192) to GDI+ hundredths-of-inch
                    // 288 WPF units = 288/96 = 3 inches = 300 hundredths
                    // 192 WPF units = 192/96 = 2 inches = 200 hundredths
                    var paperSize = new PaperSize("3x2 Label", 300, 200);
                    _gdiPrintDocument.DefaultPageSettings.PaperSize = paperSize;
                    _gdiPrintDocument.DefaultPageSettings.Landscape = false; // Same as PrintOriginal
                    _gdiPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0); // Same as PrintOriginal
                }

                Logger.Log(LoggingLevel.Info, $"FastGDI Step 3 - PrintDocument Setup: {stopwatch.ElapsedMilliseconds}ms");

                // Step 3: Set print handler
                _gdiPrintDocument.PrintPage -= GdiPrintDocument_PrintPage; // Remove old handler
                _gdiPrintDocument.PrintPage += GdiPrintDocument_PrintPage;   // Add new handler

                Logger.Log(LoggingLevel.Info, $"FastGDI Step 4 - Event Handler: {stopwatch.ElapsedMilliseconds}ms");

                // Step 4: FAST PRINT - This uses the same API as Word!
                _gdiPrintDocument.Print();

                Logger.Log(LoggingLevel.Info, $"FastGDI Step 5 - PRINT COMPLETE: {stopwatch.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, $"FastGDI Print Error: {ex.Message} | {ex.StackTrace}");

                if (prescriptionEntry != null)
                {
                    Instance.UpdateTransactionEntry(ex, prescriptionEntry);
                }

                // Fallback to original WPF method
                Logger.Log(LoggingLevel.Info, "Falling back to WPF printing...");
                Print(ref fwe, prescriptionEntry);
            }
            finally
            {
                // Clean up bitmap after printing
                _printBitmap?.Dispose();
                _printBitmap = null;
            }
        }

        private static void GdiPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_printBitmap != null)
            {
                // Use PRINTABLE AREA to fill entire label (ignore any system margins)
                var printableArea = e.MarginBounds.IsEmpty ? e.PageBounds : e.MarginBounds;

                // Set MAXIMUM quality rendering
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                // STRETCH bitmap to fill ENTIRE label area - no borders, no wasted space
                e.Graphics.DrawImage(_printBitmap,
                    0, 0,                              // Start at absolute top-left corner
                    printableArea.Width,               // Fill entire width
                    printableArea.Height);             // Fill entire height
            }
            e.HasMorePages = false; // Single page print
        }

        private static Bitmap ConvertDrawingVisualToBitmap(DrawingVisual visual, System.Windows.Size visualSize, double dpi = 300)
        {
            // Use EXACT same dimensions as PrintOriginal visual
            double width = visualSize.Width;   // 288
            double height = visualSize.Height; // 192

            // Calculate pixel dimensions at specified DPI
            int pixelWidth = (int)(width * dpi / 96);
            int pixelHeight = (int)(height * dpi / 96);

            // Create high-resolution bitmap with EXACT same size as PrintOriginal
            var renderBitmap = new RenderTargetBitmap(
                pixelWidth, pixelHeight,
                dpi, dpi, // High DPI for print quality
                PixelFormats.Pbgra32);

            // Render the EXACT same DrawingVisual that PrintOriginal would use
            renderBitmap.Render(visual);

            // Convert to GDI+ bitmap
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                stream.Position = 0;

                var bitmap = new Bitmap(stream);
                bitmap.SetResolution((float)dpi, (float)dpi);
                return bitmap;
            }
        }

        public static void DisposeFastGdiPrinting()
        {
            _gdiPrintDocument?.Dispose();
            _gdiPrintDocument = null;
            _printBitmap?.Dispose();
            _printBitmap = null;
            _currentPrinterName = null;
        }
    }
}