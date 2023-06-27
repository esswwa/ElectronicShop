using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using Paragraph = iText.Layout.Element.Paragraph;
using Table = iText.Layout.Element.Table;

namespace ElectronicShop.Services
{
    public class DocumentService
    {
        public async Task GetCheck(int OrderCode, int OrderNumber, List<Product> Products)
        {
            PdfWriter writer = new($"Товарный чек №{OrderNumber}.pdf");
            PdfDocument pdf = new(writer);
            Document document = new(pdf);

            PdfFont comic = PdfFontFactory.CreateFont(@"C:\Windows\Fonts\Arial.ttf", PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_NOT_EMBEDDED);

            var content = new Paragraph($"ELEISSIS")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                .SetFont(comic)
                .SetFontSize(12);

            document.Add(content);

            content = new Paragraph($"Товарный чек № {string.Format("{0}", OrderNumber)} от {DateOnly.FromDateTime(DateTime.Now).ToString("D")}")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(16);
            document.Add(content);

            var table = new Table(6)
                .SetWidth(UnitValue.CreatePercentValue(100))
                .SetHeight(UnitValue.CreatePercentValue(100))
                .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

            table.AddCell(new Paragraph("№ п/п")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph("Артикул")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph("Наименование")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph("Количество")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph("Цена")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph("Сумма")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            for (int i = 0; i < Products.Count; i++)
            {
                table.AddCell(new Paragraph(i.ToString())
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                   .SetFont(comic)
                   .SetFontSize(14));

                table.AddCell(new Paragraph(Products[i].Article)
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                   .SetFont(comic)
                   .SetFontSize(14));
                table.AddCell(new Paragraph(Products[i].NameProduct)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFont(comic)
                    .SetFontSize(14));
             
                table.AddCell(new Paragraph(ListProduct.counts[i].ToString())
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFont(comic)
                    .SetFontSize(14));

                table.AddCell(new Paragraph(Products[i].CostProduct.ToString() + " ₽")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFont(comic)
                    .SetFontSize(14));

                table.AddCell(new Paragraph((Products[i].CostProduct * ListProduct.counts[i]).ToString() + " ₽")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFont(comic)
                    .SetFontSize(14));
            }

            table.AddCell(new Paragraph(" ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph(" ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph(" ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph(" ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph("Итого")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                .SetFont(comic)
                .SetFontSize(14));

            table.AddCell(new Paragraph(string.Format("{0:C2}", ListProduct.cost + " ₽"))
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(14));

            document.Add(table);

            content = new Paragraph(string.Format("Адрес доставки: {0}",
                Settings.Default.Adress))
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                .SetFont(comic)
                .SetFontSize(14);

            document.Add(content);

           

            content = new Paragraph(string.Format("Код получения:\n {0}", OrderCode))
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(16);

            document.Add(content);

            content = new Paragraph($"Большое спасибо за покупку, дорогой {Settings.Default.login}!")
              .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
              .SetFont(comic)
              .SetFontSize(16);

            document.Add(content);
            document.Close();

            await Task.CompletedTask;
        }


    }
}
