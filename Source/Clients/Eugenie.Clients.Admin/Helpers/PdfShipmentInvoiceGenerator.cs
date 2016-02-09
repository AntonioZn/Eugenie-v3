namespace Eugenie.Clients.Admin.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Common.WebApiModels;

    using MigraDoc.DocumentObjectModel;
    using MigraDoc.DocumentObjectModel.Tables;
    using MigraDoc.Rendering;

    public class PdfShipmentInvoiceGenerator
    {
        private readonly Document document = new Document();
        private Table table;

        public PdfShipmentInvoiceGenerator(DateTime date, IEnumerable<Shipment> shipments)
        {
            this.DefineStyles();

            this.CreatePage(date);

            this.FillContent(shipments);

            this.SaveDocument();
        }

        private void SaveDocument()
        {
            var pdfRenderer = new PdfDocumentRenderer(true);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = this.document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            // Save the document...
            var filename = "Invoice.pdf";
            pdfRenderer.PdfDocument.Save(filename);

            Process.Start(filename);
        }

        private void DefineStyles()
        {
            // Get the predefined style Normal.
            var style = this.document.Styles["Normal"];

            // Create a new style called Table based on style Normal
            style = this.document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 12;
        }

        private void CreatePage(DateTime date)
        {
            var section = this.document.AddSection();

            var paragraph = section.AddParagraph();
            paragraph.Style = "Table";
            paragraph.Format.SpaceAfter = "0,5cm";
            paragraph.AddFormattedText("Йожени 95 ЕООД", TextFormat.Bold);
            paragraph.AddTab();
            paragraph.AddFormattedText("Стокова разписка", TextFormat.Bold);
            paragraph.AddTab();
            paragraph.AddFormattedText(date.ToShortDateString(), TextFormat.Bold);

            this.table = section.AddTable();
            this.table.Style = "Table";
            this.table.Borders.Visible = true;

            // Before you can add a row, you must define the columns
            var column = this.table.AddColumn("1,5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn("11cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Create the header of the table
            var row = this.table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;

            row.Cells[0].AddParagraph("№");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[1].AddParagraph("Име");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[2].AddParagraph("К-во");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[3].AddParagraph("Цена");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
        }

        private void FillContent(IEnumerable<Shipment> shipments)
        {
            var counter = 1;
            foreach (var shipment in shipments)
            {
                var row = this.table.AddRow();

                row.Cells[0].AddParagraph(counter.ToString());
                row.Cells[1].AddParagraph(shipment.Name);
                row.Cells[2].AddParagraph(shipment.Quantity.ToString());
                row.Cells[3].AddParagraph(shipment.SellingPrice.ToString());

                counter++;
            }
        }
    }
}