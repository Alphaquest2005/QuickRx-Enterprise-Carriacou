using System;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Xps;
using SUT.PrintEngine;
using SUT.PrintEngine.Paginators;
using SUT.PrintEngine.Utils;
using log4netWrapper;
using RMSDataAccessLayer;

namespace SalesRegion
{
    public partial class SalesVM
    {
        private static PrintQueue _cachedPrintQueue;
        private static PrintServer _cachedPrintServer;
        private static DateTime _cacheTime;
        private static readonly TimeSpan CacheTimeout = TimeSpan.FromMinutes(5);
        private static readonly object _printLock = new object();

        private PrintQueue GetOrCreatePrintQueue()
        {
            lock (_printLock)
            {
                if (_cachedPrintQueue == null || DateTime.Now - _cacheTime > CacheTimeout)
                {
                    if (_cachedPrintServer != null)
                    {
                        try { _cachedPrintServer.Dispose(); } catch { }
                    }

                    _cachedPrintServer = Station.PrintServer.StartsWith("\\")
                        ? new PrintServer(Station.PrintServer)
                        : new LocalPrintServer();

                    _cachedPrintQueue = _cachedPrintServer.GetPrintQueue(Station.ReceiptPrinterName);
                    _cacheTime = DateTime.Now;
                }

                return _cachedPrintQueue;
            }
        }

        public void PrintOptimized(ref FrameworkElement fwe, PrescriptionEntry prescriptionEntry = null)
        {
            try
            {
                if (fwe == null) return;

                var printQueue = GetOrCreatePrintQueue();

                Size visualSize = new Size(288, 2 * 96);

                DrawingVisual visual = PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);

                var page = new VisualPaginator(visual, visualSize, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 0));
                page.Initialize(false);

                XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(printQueue);
                writer.Write(page);
            }
            catch (Exception ex)
            {
                if (prescriptionEntry != null)
                {
                    Instance.UpdateTransactionEntry(ex, prescriptionEntry);
                }

                Logger.Log(LoggingLevel.Error, $"Print error (optimized): {ex.Message} {ex.StackTrace}");

                try
                {
                    InvalidatePrintCache();
                    PrintFallback(ref fwe, prescriptionEntry);
                }
                catch (Exception fallbackEx)
                {
                    MessageBox.Show(
                        "Print error! Please check prints and reprint. Error details logged.",
                        "Print Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    Logger.Log(LoggingLevel.Error, $"Fallback print also failed: {fallbackEx.Message}");
                }
            }
        }

        public async Task PrintOptimizedAsync(FrameworkElement fwe, PrescriptionEntry prescriptionEntry = null)
        {
            try
            {
                if (fwe == null) return;

                await Task.Run(() =>
                {
                    var printQueue = GetOrCreatePrintQueue();

                    Size visualSize = new Size(288, 2 * 96);

                    DrawingVisual visual = PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);

                    var page = new VisualPaginator(visual, visualSize, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 0));
                    page.Initialize(false);

                    XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(printQueue);
                    writer.Write(page);
                });
            }
            catch (Exception ex)
            {
                if (prescriptionEntry != null)
                {
                    Instance.UpdateTransactionEntry(ex, prescriptionEntry);
                }

                Logger.Log(LoggingLevel.Error, $"Async print error: {ex.Message} {ex.StackTrace}");

                InvalidatePrintCache();

                await Task.Run(() => PrintFallback(ref fwe, prescriptionEntry));
            }
        }

        private void PrintFallback(ref FrameworkElement fwe, PrescriptionEntry prescriptionEntry)
        {
            try
            {
                PrintServer printserver = Station.PrintServer.StartsWith("\\")
                    ? new PrintServer(Station.PrintServer)
                    : new LocalPrintServer();

                Size visualSize = new Size(288, 2 * 96);

                DrawingVisual visual = PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);

                var page = new VisualPaginator(visual, visualSize, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 0));
                page.Initialize(false);

                PrintDialog pd = new PrintDialog();
                pd.PrintQueue = printserver.GetPrintQueue(Station.ReceiptPrinterName);
                pd.PrintDocument(page, "");
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, $"Fallback print error: {ex.Message}");
                throw;
            }
        }

        public static void InvalidatePrintCache()
        {
            lock (_printLock)
            {
                _cachedPrintQueue = null;
                if (_cachedPrintServer != null)
                {
                    try
                    {
                        _cachedPrintServer.Dispose();
                    }
                    catch { }
                    _cachedPrintServer = null;
                }
                _cacheTime = DateTime.MinValue;
            }
        }

        public static void WarmupPrintCache(Station station)
        {
            Task.Run(() =>
            {
                try
                {
                    lock (_printLock)
                    {
                        _cachedPrintServer = station.PrintServer.StartsWith("\\")
                            ? new PrintServer(station.PrintServer)
                            : new LocalPrintServer();

                        _cachedPrintQueue = _cachedPrintServer.GetPrintQueue(station.ReceiptPrinterName);
                        _cacheTime = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(LoggingLevel.Warning, $"Failed to warmup print cache: {ex.Message}");
                }
            });
        }

        // Simple method to quickly replace the existing slow print call
        public void FastPrint(ref FrameworkElement fwe, PrescriptionEntry prescriptionEntry = null)
        {
            PrintOptimized(ref fwe, prescriptionEntry);
        }
    }
}