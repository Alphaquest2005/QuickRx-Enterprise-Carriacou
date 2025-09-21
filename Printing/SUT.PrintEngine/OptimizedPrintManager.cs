using System;
using System.Collections.Concurrent;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Xps;
using SUT.PrintEngine.Utils;
using SUT.PrintEngine.Paginators;

namespace SUT.PrintEngine
{
    public class OptimizedPrintManager : IDisposable
    {
        private static readonly ConcurrentDictionary<string, PrintQueueCache> _printQueueCache =
            new ConcurrentDictionary<string, PrintQueueCache>();

        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);
        private static LocalPrintServer _localPrintServer;
        private static readonly object _lockObject = new object();

        private class PrintQueueCache
        {
            public PrintQueue Queue { get; set; }
            public PrintServer Server { get; set; }
            public DateTime LastAccessed { get; set; }
            public PrintTicket CachedTicket { get; set; }
        }

        static OptimizedPrintManager()
        {
            _localPrintServer = new LocalPrintServer();
        }

        public static PrintQueue GetCachedPrintQueue(string printerName, string serverName = null)
        {
            string cacheKey = $"{serverName ?? "local"}:{printerName}";

            return _printQueueCache.AddOrUpdate(cacheKey,
                key =>
                {
                    PrintServer server = string.IsNullOrEmpty(serverName) || !serverName.StartsWith("\\\\")
                        ? _localPrintServer
                        : new PrintServer(serverName);

                    var queue = server.GetPrintQueue(printerName);

                    return new PrintQueueCache
                    {
                        Queue = queue,
                        Server = server,
                        LastAccessed = DateTime.Now,
                        CachedTicket = queue.DefaultPrintTicket
                    };
                },
                (key, existing) =>
                {
                    if (DateTime.Now - existing.LastAccessed > CacheExpiration)
                    {
                        try
                        {
                            existing.Queue.Refresh();
                            existing.CachedTicket = existing.Queue.DefaultPrintTicket;
                        }
                        catch
                        {
                            PrintServer server = string.IsNullOrEmpty(serverName) || !serverName.StartsWith("\\\\")
                                ? _localPrintServer
                                : new PrintServer(serverName);

                            existing.Queue = server.GetPrintQueue(printerName);
                            existing.Server = server;
                            existing.CachedTicket = existing.Queue.DefaultPrintTicket;
                        }
                    }
                    existing.LastAccessed = DateTime.Now;
                    return existing;
                }).Queue;
        }

        public static void PrintOptimized(Grid fwe, string printerName, string serverName = null)
        {
            if (fwe == null) return;

            try
            {
                PrintQueue printQueue = GetCachedPrintQueue(printerName, serverName);

                Size visualSize = new Size(fwe.ActualWidth, fwe.ActualHeight);
                DrawingVisual visual = PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);

                VisualPaginator page = new VisualPaginator(visual, visualSize,
                    new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 0));
                page.Initialize(false);

                XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(printQueue);
                writer.Write(page);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to print to {printerName}: {ex.Message}", ex);
            }
        }

        public static async Task PrintOptimizedAsync(Grid fwe, string printerName, string serverName = null)
        {
            if (fwe == null) return;

            await Task.Run(() =>
            {
                try
                {
                    PrintQueue printQueue = GetCachedPrintQueue(printerName, serverName);

                    Size visualSize = new Size(fwe.ActualWidth, fwe.ActualHeight);
                    DrawingVisual visual = PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);

                    VisualPaginator page = new VisualPaginator(visual, visualSize,
                        new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 0));
                    page.Initialize(false);

                    XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(printQueue);
                    writer.Write(page);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to print to {printerName}: {ex.Message}", ex);
                }
            });
        }

        public static void PrintWithDialog(Grid fwe, string printerName, string serverName = null)
        {
            if (fwe == null) return;

            try
            {
                PrintQueue printQueue = GetCachedPrintQueue(printerName, serverName);

                PrintDialog pd = new PrintDialog();
                pd.PrintQueue = printQueue;

                if (pd.ShowDialog() == true)
                {
                    Size visualSize = new Size(fwe.ActualWidth, fwe.ActualHeight);
                    DrawingVisual visual = PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);

                    VisualPaginator page = new VisualPaginator(visual, visualSize,
                        new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 0));
                    page.Initialize(false);

                    pd.PrintDocument(page, "");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to print to {printerName}: {ex.Message}", ex);
            }
        }

        public static void PrintUsingAddJob(Grid fwe, string printerName, string serverName = null)
        {
            if (fwe == null) return;

            try
            {
                PrintQueue printQueue = GetCachedPrintQueue(printerName, serverName);

                Size visualSize = new Size(fwe.ActualWidth, fwe.ActualHeight);
                DrawingVisual visual = PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);

                VisualPaginator page = new VisualPaginator(visual, visualSize,
                    new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 0));
                page.Initialize(false);

                // Use direct XPS writer instead of XpsDocument for simpler implementation
                XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(printQueue);
                writer.Write(page);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to print to {printerName}: {ex.Message}", ex);
            }
        }

        public static void ClearCache()
        {
            foreach (var item in _printQueueCache.Values)
            {
                try
                {
                    if (item.Server != _localPrintServer)
                    {
                        item.Server?.Dispose();
                    }
                }
                catch { }
            }
            _printQueueCache.Clear();
        }

        public static void InvalidateCacheEntry(string printerName, string serverName = null)
        {
            string cacheKey = $"{serverName ?? "local"}:{printerName}";
            _printQueueCache.TryRemove(cacheKey, out _);
        }

        public void Dispose()
        {
            ClearCache();
            _localPrintServer?.Dispose();
        }
    }
}