using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ScottPlot;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;

using QuestColors = QuestPDF.Helpers.Colors;
using ScottColor = ScottPlot.Color;
using ScottImageFormat = ScottPlot.ImageFormat;

namespace TempestSushi.Application.Services.Implementations
{
    public class ServiceReportePdf : IServiceReportePdf
    {
        public byte[] GenerarReporte(
            ReporteDashboardDTO reporte)
        {
            var graficoPedidos =
                GenerarGraficoPedidosPorDia(
                    reporte.PedidosPorDia);

            var graficoProductos =
                GenerarGraficoProductosMasVendidos(
                    reporte.ProductosMasVendidos);

            var graficoEstados =
                GenerarGraficoPedidosPorEstado(
                    reporte.PedidosPorEstado);

            var totalPedidosSemana =
                reporte.PedidosPorDia
                    .Sum(x => x.CantidadPedidos);

            var pedidosCancelados =
                reporte.PedidosPorEstado
                    .FirstOrDefault(x =>
                        x.Estado.Equals(
                            "Cancelada",
                            StringComparison.OrdinalIgnoreCase))
                    ?.CantidadPedidos ?? 0;

            var productoMasVendido =
                reporte.ProductosMasVendidos
                    .FirstOrDefault();

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);

                    page.Margin(35);

                    page.DefaultTextStyle(style =>
                        style.FontSize(10));

                    page.Header()
                        .Column(header =>
                        {
                            header.Item()
                                .Text("TempestSushi")
                                .FontSize(22)
                                .Bold()
                                .FontColor(
                                    QuestColors.Blue.Darken3);

                            header.Item()
                                .Text("Reporte consolidado")
                                .FontSize(14)
                                .Bold();

                            header.Item()
                                .Text(
                                    $"Fecha de generación: " +
                                    $"{DateTime.Now:dd/MM/yyyy HH:mm}")
                                .FontSize(9)
                                .FontColor(
                                    QuestColors.Grey.Darken1);
                        });

                    page.Content()
                        .PaddingVertical(15)
                        .Column(column =>
                        {
                            column.Spacing(14);

                            column.Item()
                                .LineHorizontal(1)
                                .LineColor(
                                    QuestColors.Grey.Lighten2);

                            column.Item()
                                .Text("Resumen")
                                .FontSize(15)
                                .Bold()
                                .FontColor(
                                    QuestColors.Blue.Darken3);

                            column.Item()
                                .Row(row =>
                                {
                                    row.RelativeItem()
                                        .Border(1)
                                        .BorderColor(
                                            QuestColors.Grey.Lighten2)
                                        .Padding(10)
                                        .Column(card =>
                                        {
                                            card.Item()
                                                .Text(
                                                    "Pedidos últimos 7 días")
                                                .FontSize(9)
                                                .FontColor(
                                                    QuestColors.Grey.Darken1);

                                            card.Item()
                                                .Text(
                                                    totalPedidosSemana
                                                        .ToString())
                                                .FontSize(20)
                                                .Bold()
                                                .FontColor(
                                                    QuestColors.Blue.Darken3);
                                        });

                                    row.ConstantItem(10);

                                    row.RelativeItem()
                                        .Border(1)
                                        .BorderColor(
                                            QuestColors.Grey.Lighten2)
                                        .Padding(10)
                                        .Column(card =>
                                        {
                                            card.Item()
                                                .Text(
                                                    "Pedidos cancelados")
                                                .FontSize(9)
                                                .FontColor(
                                                    QuestColors.Grey.Darken1);

                                            card.Item()
                                                .Text(
                                                    pedidosCancelados
                                                        .ToString())
                                                .FontSize(20)
                                                .Bold()
                                                .FontColor(
                                                    QuestColors.Red.Darken2);
                                        });

                                    row.ConstantItem(10);

                                    row.RelativeItem()
                                        .Border(1)
                                        .BorderColor(
                                            QuestColors.Grey.Lighten2)
                                        .Padding(10)
                                        .Column(card =>
                                        {
                                            card.Item()
                                                .Text(
                                                    "Producto más vendido")
                                                .FontSize(9)
                                                .FontColor(
                                                    QuestColors.Grey.Darken1);

                                            card.Item()
                                                .Text(
                                                    productoMasVendido
                                                        ?.NombreProducto
                                                    ?? "Sin datos")
                                                .FontSize(12)
                                                .Bold()
                                                .FontColor(
                                                    QuestColors.Blue.Darken3);

                                            if (productoMasVendido != null)
                                            {
                                                card.Item()
                                                    .Text(
                                                        $"{productoMasVendido.CantidadVendida} unidades")
                                                    .FontSize(9);
                                            }
                                        });
                                });

                            column.Item()
                                .PaddingTop(5)
                                .Text(
                                    "Pedidos de los últimos 7 días")
                                .FontSize(14)
                                .Bold()
                                .FontColor(
                                    QuestColors.Blue.Darken3);

                            column.Item()
                                .Text(
                                    "Cantidad de pedidos registrados por día.")
                                .FontSize(9)
                                .FontColor(
                                    QuestColors.Grey.Darken1);

                            column.Item()
                                .Height(210)
                                .Image(graficoPedidos)
                                .FitArea();

                            column.Item()
                                .PageBreak();

                            column.Item()
                                .Text(
                                    "Top 5 productos más vendidos")
                                .FontSize(14)
                                .Bold()
                                .FontColor(
                                    QuestColors.Blue.Darken3);

                            column.Item()
                                .Text(
                                    "Unidades vendidas, excluyendo pedidos cancelados.")
                                .FontSize(9)
                                .FontColor(
                                    QuestColors.Grey.Darken1);

                            column.Item()
                                .Height(230)
                                .Image(graficoProductos)
                                .FitArea();

                            column.Item()
                                .PaddingTop(10)
                                .Text(
                                    "Pedidos por estado")
                                .FontSize(14)
                                .Bold()
                                .FontColor(
                                    QuestColors.Blue.Darken3);

                            column.Item()
                                .Text(
                                    "Cantidad total de pedidos según su estado actual.")
                                .FontSize(9)
                                .FontColor(
                                    QuestColors.Grey.Darken1);

                            column.Item()
                                .Height(230)
                                .Image(graficoEstados)
                                .FitArea();

                            column.Item()
                                .PaddingTop(10)
                                .Text(
                                    "Generado automáticamente por TempestSushi.")
                                .FontSize(8)
                                .FontColor(
                                    QuestColors.Grey.Darken1);
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.DefaultTextStyle(
                                style =>
                                    style.FontSize(9));

                            text.Span("Página ");

                            text.CurrentPageNumber();

                            text.Span(" de ");

                            text.TotalPages();
                        });
                });
            });

            return documento.GeneratePdf();
        }

        private static byte[] GenerarGraficoPedidosPorDia(
            ICollection<PedidoPorDiaDTO> pedidos)
        {
            var lista =
                pedidos
                    .OrderBy(x => x.Fecha)
                    .ToList();

            var valores =
                lista
                    .Select(x =>
                        (double)x.CantidadPedidos)
                    .ToArray();

            var posiciones =
                Enumerable.Range(
                        0,
                        lista.Count)
                    .Select(x =>
                        (double)x)
                    .ToArray();

            var plot = new Plot();

            var barras =
                plot.Add.Bars(
                    posiciones,
                    valores);

            foreach (var barra in barras.Bars)
            {
                barra.FillColor =
                    ScottColor.FromHex(
                        "#1A3A6E");

                barra.LineColor =
                    ScottColor.FromHex(
                        "#1A3A6E");

                barra.Size = 0.65;
            }

            plot.Title(
                "Pedidos por día");

            plot.YLabel(
                "Cantidad de pedidos");

            plot.Axes.Bottom.TickGenerator =
                new ScottPlot.TickGenerators.NumericManual(
                    posiciones,
                    lista
                        .Select(x =>
                            x.Fecha.ToString("dd/MM"))
                        .ToArray());

            plot.Axes.Left.Min = 0;

            plot.HideGrid();

            return plot.GetImageBytes(
                900,
                420,
                ScottImageFormat.Png);
        }

        private static byte[]
            GenerarGraficoProductosMasVendidos(
                ICollection<ProductoVendidoDTO> productos)
        {
            var lista =
                productos
                    .OrderByDescending(
                        x => x.CantidadVendida)
                    .ToList();

            if (!lista.Any())
            {
                return GenerarGraficoSinDatos(
                    "No existen ventas de productos.");
            }

            var valores =
                lista
                    .Select(x =>
                        (double)x.CantidadVendida)
                    .ToArray();

            var posiciones =
                Enumerable.Range(
                        0,
                        lista.Count)
                    .Select(x =>
                        (double)x)
                    .ToArray();

            var plot = new Plot();

            var barras =
                plot.Add.Bars(
                    posiciones,
                    valores);

            foreach (var barra in barras.Bars)
            {
                barra.FillColor =
                    ScottColor.FromHex(
                        "#4FA8E8");

                barra.LineColor =
                    ScottColor.FromHex(
                        "#1A3A6E");

                barra.Size = 0.65;
            }

            plot.Title(
                "Top 5 productos");

            plot.YLabel(
                "Unidades vendidas");

            plot.Axes.Bottom.TickGenerator =
                new ScottPlot.TickGenerators.NumericManual(
                    posiciones,
                    lista
                        .Select(x =>
                            x.NombreProducto)
                        .ToArray());

            plot.Axes.Left.Min = 0;

            plot.HideGrid();

            return plot.GetImageBytes(
                900,
                450,
                ScottImageFormat.Png);
        }

        private static byte[]
            GenerarGraficoPedidosPorEstado(
                ICollection<PedidoPorEstadoDTO> estados)
        {
            var lista =
                estados
                    .OrderByDescending(
                        x => x.CantidadPedidos)
                    .ToList();

            if (!lista.Any())
            {
                return GenerarGraficoSinDatos(
                    "No existen pedidos registrados.");
            }

            var colores = new[]
            {
                ScottColor.FromHex("#1A3A6E"),
                ScottColor.FromHex("#4FA8E8"),
                ScottColor.FromHex("#27AE60"),
                ScottColor.FromHex("#D68910"),
                ScottColor.FromHex("#C0392B"),
                ScottColor.FromHex("#8E44AD")
            };

            var slices =
                lista
                    .Select((x, indice) =>
                        new PieSlice
                        {
                            Value =
                                x.CantidadPedidos,

                            Label =
                                $"{x.Estado} ({x.CantidadPedidos})",

                            FillColor =
                                colores[
                                    indice %
                                    colores.Length]
                        })
                    .ToList();

            var plot = new Plot();

            var pie =
                plot.Add.Pie(
                    slices);

            pie.DonutFraction = 0.55;

            plot.Title(
                "Pedidos por estado");

            plot.ShowLegend();

            plot.Axes.Frameless();

            plot.HideGrid();

            return plot.GetImageBytes(
                900,
                450,
                ScottImageFormat.Png);
        }

        private static byte[] GenerarGraficoSinDatos(
            string mensaje)
        {
            var plot = new Plot();

            plot.Add.Text(
                mensaje,
                0,
                0);

            plot.Axes.SetLimits(
                -1,
                1,
                -1,
                1);

            plot.HideGrid();

            return plot.GetImageBytes(
                900,
                350,
                ScottImageFormat.Png);
        }
    }
}