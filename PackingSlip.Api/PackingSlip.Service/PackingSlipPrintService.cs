using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Options;
using PackingSlip.Domain.Entities;
using PackingSlip.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PackingSlip.Service
{
    public interface IPackingSlipPrintService
    {
        ResponseMessage PrintPackingSlip(PackingSlipHeader packingSlipHeader);
    }

    public class PackingSlipPrintService: IPackingSlipPrintService
    {
        private FileConfig _fileConfig = null;

        public PackingSlipPrintService(IOptions<FileConfig> options)
        {
            _fileConfig = options.Value;
        }

        public ResponseMessage PrintPackingSlip(PackingSlipHeader packingSlipHeader)
        {
            ResponseMessage response = new ResponseMessage { IsSuccess = false };
            bool success = false;
            if((packingSlipHeader.HasPhysicalProduct.HasValue && packingSlipHeader.HasPhysicalProduct.Value) ||
                    (packingSlipHeader.HasBook.HasValue && packingSlipHeader.HasBook.Value))
            {
                success = PrintSlip(packingSlipHeader);
            }
            response.IsSuccess = success;
            return response;
        }

        private bool PrintSlip(PackingSlipHeader packingSlipHeader)
        {
            bool success = false;

            using (MemoryStream stream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 2, 2, 2, 2);
                PdfWriter writer = PdfWriter.GetInstance(document, stream);

                document.Open();
                Font bodyFont = new Font(Font.HELVETICA, 10f, Font.NORMAL);

                float[] columnWidth = new float[] { 120f, 50f, 50f };

                PdfPTable table = new PdfPTable(3);
                table.SetWidths(columnWidth);

                PdfPCell header = new PdfPCell(new Phrase($"Packing Slip No: {packingSlipHeader.PackingSlipNumber} - Dated:{DateTime.Now}", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                header.Colspan = 3;
                table.AddCell(header);

                PdfPCell productNameHeader = new PdfPCell(new Phrase($"Prduct Name", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                table.AddCell(productNameHeader);
                productNameHeader = new PdfPCell(new Phrase($"Quantity", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                table.AddCell(productNameHeader);
                productNameHeader = new PdfPCell(new Phrase($"Comments", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                table.AddCell(productNameHeader);
                table.CompleteRow();

                foreach (var item in packingSlipHeader.PackingSlipItems)
                {
                    productNameHeader = new PdfPCell(new Phrase(item.Name, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    table.AddCell(productNameHeader);
                    productNameHeader = new PdfPCell(new Phrase(item.Quantity.ToString("N0"), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    table.AddCell(productNameHeader);
                    productNameHeader = new PdfPCell(new Phrase(string.Empty, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    table.AddCell(productNameHeader);
                    table.CompleteRow();
                }

                document.Add(table);

                string fileName = Path.Combine(_fileConfig.DownloadPath, $"PackingSlip-{packingSlipHeader.PackingSlipNumber}.pdf");
                File.WriteAllBytes(fileName, stream.ToArray());
            }

            success = true;
            return success;
        }
    }
}
