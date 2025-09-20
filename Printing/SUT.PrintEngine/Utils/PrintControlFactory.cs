using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Practices.Unity;
using SUT.PrintEngine.Extensions;
using SUT.PrintEngine.ViewModels;

namespace SUT.PrintEngine.Utils
{
    public class PrintControlFactory
    {
        private static readonly Lazy<UnityContainer> _cachedContainer = new Lazy<UnityContainer>(() =>
        {
            var container = new UnityContainer();
            PrintEngineModule.Initialize(container);
            return container;
        });

        private static UnityContainer CachedContainer => _cachedContainer.Value;

        public static IPrintControlViewModel Create(Size visualSize, Visual visual)
        {
            var printControlPresenter = (PrintControlViewModel)CachedContainer.Resolve<IPrintControlViewModel>();

            var drawingVisual = BuildGraphVisual(new PageMediaSize(visualSize.Width, visualSize.Height), visual);
            printControlPresenter.DrawingVisual = drawingVisual;

            return printControlPresenter;
        }

        public static IPrintControlViewModel Create(FrameworkElement frameworkElement)
        {
            var size = new Size(frameworkElement.ActualWidth, frameworkElement.ActualHeight);
            return Create(size, frameworkElement);
        }

        public static async Task<IPrintControlViewModel> CreateAsync(DataTable dataTable, List<double> columnWidths)
        {
            var printControlPresenter = (DataTablePrintControlViewModel)CachedContainer.Resolve<IDataTablePrintControlViewModel>();
            await SetupDataTablePrintControlPresenterAsync(dataTable, printControlPresenter, columnWidths, string.Empty).ConfigureAwait(false);
            return printControlPresenter;
        }

        public static async Task<IPrintControlViewModel> CreateAsync(DataTable dataTable, List<double> columnWidths, string headerTemplate)
        {
            var printControlPresenter = (DataTablePrintControlViewModel)CachedContainer.Resolve<IDataTablePrintControlViewModel>();
            await SetupDataTablePrintControlPresenterAsync(dataTable, printControlPresenter, columnWidths, headerTemplate).ConfigureAwait(false);
            return printControlPresenter;
        }

        private static async Task SetupDataTablePrintControlPresenterAsync(DataTable dataTable, DataTablePrintControlViewModel printControlPresenter, List<double> columnWidths, string headerTemplate)
        {

            var fieldNames = new List<string>();
            foreach (DataColumn column in dataTable.Columns)
            {
                fieldNames.Add(column.ColumnName);
            }

            double pageAccrossWidth = 0;

            foreach (var columnsWidth in columnWidths)
            {
                pageAccrossWidth += columnsWidth;
            }

            var customVisualGrid = await CreateDocumentAsync(dataTable, pageAccrossWidth, columnWidths).ConfigureAwait(false);

            var rowHeights = CalculateRowHeights(customVisualGrid);

            var drawingVisual = CreateDrawingVisual(customVisualGrid, pageAccrossWidth, customVisualGrid.ActualHeight);

            var printTableDefination = new PrintTableDefination
            {
                ColumnWidths = columnWidths,
                RowHeights = rowHeights,
                HasFooter = false,
                FooterText = null,
                ColumnNames = fieldNames.ToArray(),
                ColumnHeaderFontSize = 12,
                ColumnHeaderBrush = Brushes.Black,
                ColumnHeaderHeight = 22,
                HeaderTemplate = headerTemplate
            };

            printControlPresenter.PrintTableDefination = printTableDefination;
            printControlPresenter.DrawingVisual = drawingVisual;
            return;
        }

        private static List<double> CalculateRowHeights(Border border)
        {
            var rowHeights = new List<double>();
            var sp = border.Child as StackPanel;
            foreach (Border child in sp.Children)
            {
                rowHeights.Add(child.ActualHeight);
            }
            return rowHeights;
        }

        private static async Task<Border> CreateDocumentAsync(DataTable dataTable, double pageAcrossWidth, List<double> columnWidths)
        {
            await Task.Yield();
            var spBorder = new Border { BorderBrush = Brushes.Gray, BorderThickness = new Thickness(0.25), Background = Brushes.White};
            var stackPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Vertical
            };


            foreach (DataRow dataRow in dataTable.Rows) // loop for all propertyOwners
            {
                await Task.Yield();
                var border = new Border { Margin = new Thickness(0, 0, 0, 0), BorderBrush = Brushes.Gray, BorderThickness = new Thickness(0.25), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
                var sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                var columnIndex = 0;
                foreach (DataColumn dataColumn in dataTable.Columns) //loop for all properties of the owners
                {
                    {

                        var tbBorder = new Border { BorderBrush = Brushes.Gray, BorderThickness = new Thickness(0.25, 0, 0.25, 0) };
                        var columnWidth = columnWidths[columnIndex];
                        var textBlock = CreateCellBlock(dataRow, dataColumn, columnWidth, tbBorder);

                        tbBorder.Child = textBlock;
                        sp.Children.Add(tbBorder);
                        columnIndex++;
                    }
                }
                border.Child = sp;
                stackPanel.Children.Add(border);
            }
            spBorder.Child = stackPanel;

            UiUtil.UpdateSize(spBorder, pageAcrossWidth);

            return spBorder;
        }

        protected static TextBlock CreateCellBlock(DataRow dataRow, DataColumn dataColumn, double columnWidth, Border tbBorder)
        {
            var textBlock = new TextBlock
            {
                FontSize = 12,
                Width = columnWidth - 0.5,
                TextWrapping = TextWrapping.Wrap,
                Padding = new Thickness(5)
            };
            textBlock.Text = dataRow[dataColumn].ToString();
            return textBlock;
        }

        public static DrawingVisual BuildGraphVisual(PageMediaSize pageSize, Visual visual)
        {
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {

                var visualContent = visual;
                var rect = new Rect
                {
                    X = 0,
                    Y = 0,
                    Width = pageSize.Width.Value,
                    Height = pageSize.Height.Value
                };

                var stretch = Stretch.None;
                var visualBrush = new VisualBrush(visualContent) { Stretch = stretch };

                drawingContext.DrawRectangle(visualBrush, null, rect);
                drawingContext.PushOpacityMask(Brushes.White);
            }
            return drawingVisual;
        }

        public static DrawingVisual CreateDrawingVisual(FrameworkElement visual, double width, double height)
        {
            var drawingVisual = new DrawingVisual();
            using (var dc = drawingVisual.RenderOpen())
            {
                var vb = new VisualBrush(visual) { Stretch = Stretch.None };
                var rectangle = new Rect
                {
                    X = 0,
                    Y = 0,
                    Width = width,
                    Height = height,
                };
                dc.DrawRectangle(Brushes.White, null, rectangle);
                dc.DrawRectangle(vb, null, rectangle);
            }
            return drawingVisual;
        }
    }
}
