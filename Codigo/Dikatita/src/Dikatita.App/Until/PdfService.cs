using Dikatita.App.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;

namespace Dikatita.App.Until
{
    public class PdfService
    {
        public byte[] GerarCatalogoPdf(IEnumerable<ProdutoViewModel> produtos, string baseUrl)
        {
            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4, 36, 36, 36, 36);
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Adicionar título
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            var title = new Paragraph("Catálogo de Produtos", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            document.Add(title);
            document.Add(new Paragraph(" ")); // Espaço

            // Criar tabela para os produtos
            var table = new PdfPTable(3) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 2f, 5f, 3f });

            // Cabeçalho da tabela
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            table.AddCell(new PdfPCell(new Phrase("Imagem", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Produto", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Preço", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

            // Linhas da tabela
            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            foreach (var produto in produtos)
            {
                if (produto.Ativo && produto.QuantidadeEstoque > 0)
                {
                    // Célula da imagem
                    PdfPCell imageCell = new PdfPCell();
                    try
                    {
                        string imagePath = Path.Combine(baseUrl, "uploadImagens", produto.Imagem);
                        if (File.Exists(imagePath))
                        {
                            var image = Image.GetInstance(imagePath);
                            image.ScaleToFit(100, 100);
                            imageCell.AddElement(image);
                            imageCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else
                        {
                            imageCell.Phrase = new Phrase("Imagem não disponível", normalFont);
                        }
                    }
                    catch
                    {
                        imageCell.Phrase = new Phrase("Erro ao carregar imagem", normalFont);
                    }
                    table.AddCell(imageCell);

                    // Células de informações
                    var infoCell = new PdfPCell();
                    infoCell.AddElement(new Paragraph(produto.Nome, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11)));
                    infoCell.AddElement(new Paragraph(produto.Descricao, normalFont));
                    table.AddCell(infoCell);

                    // Célula de preço
                    table.AddCell(new PdfPCell(new Phrase(produto.Valor.ToString("C"), normalFont)) 
                    { 
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }
            }

            document.Add(table);

            // Adicionar rodapé
            var footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.ITALIC);
            var footer = new Paragraph("Catálogo gerado em: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), footerFont)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            document.Add(new Paragraph(" ")); // Espaço
            document.Add(footer);

            document.Close();
            return memoryStream.ToArray();
        }
    }
} 